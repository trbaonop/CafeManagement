using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cafe.DAL;
using Cafe.Entity;

namespace Cafe.BLL
{
    public class HoaDonService
    {
        private readonly CafeContext _context;
        private readonly BanService _banService;
        private readonly CaService _caService;
        private static readonly Dictionary<int, List<ChiTietHD>> _pendingOrders = new();

        public HoaDonService(CafeContext context, BanService banService, CaService caService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _banService = banService ?? throw new ArgumentNullException(nameof(banService));
            _caService = caService ?? throw new ArgumentNullException(nameof(caService));
        }

        /// <summary>
        /// Tạo hóa đơn khi bắt đầu order (Unpaid)
        /// </summary>
        public async Task<HoaDon> TaoHoaDonAsync(int maBan, int maCa, List<ChiTietHD> chiTiets)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var hoaDon = new HoaDon
                {
                    MaBan = maBan,
                    MaCa = maCa,
                    NgayLap = DateTime.Now,
                    TrangThai = "Unpaid",
                    TongTien = chiTiets.Sum(ct => ct.ThanhTien)
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                foreach (var ct in chiTiets)
                {
                    ct.MaHD = hoaDon.MaHD;
                    _context.ChiTietHDs.Add(ct);
                }

                await _context.SaveChangesAsync();

                // Đổi trạng thái bàn sang "DangPhucVu" khi có order đầu tiên
                await _banService.UpdateTrangThaiAsync(maBan, "DangPhucVu");

                await transaction.CommitAsync();
                return hoaDon;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Thanh toán hóa đơn (Paid), cộng tiền vào ca, giải phóng bàn
        /// </summary>
        public async Task ThanhToanAsync(int maHD)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var hd = await _context.HoaDons
                    .Include(h => h.Ban)
                    .FirstOrDefaultAsync(h => h.MaHD == maHD);

                if (hd == null || hd.TrangThai == "Paid")
                    throw new Exception("Hóa đơn không tồn tại hoặc đã thanh toán");

                hd.TrangThai = "Paid";
                await _context.SaveChangesAsync();

                // Cộng doanh thu ca từ tất cả HD Paid trong ca (tính lại chính xác)
                if (hd.MaCa.HasValue)
                {
                    var doanhThu = await _context.HoaDons
                        .Where(h => h.MaCa == hd.MaCa && h.TrangThai == "Paid")
                        .SumAsync(h => h.TongTien);

                    var ca = await _context.Cas.FindAsync(hd.MaCa);
                    if (ca != null)
                    {
                        ca.DoanhThu = doanhThu;
                        await _context.SaveChangesAsync();
                    }
                }

                // Giải phóng bàn → trạng thái về "Trong"
                if (hd.MaBan.HasValue)
                {
                    var ban = await _context.Bans.FindAsync(hd.MaBan);
                    if (ban != null)
                    {
                        ban.TrangThai = "Trong";
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<HoaDon>> GetHoaDonByCaAsync(int maCa)
        {
            return await _context.HoaDons
                .Include(h => h.Ban)
                .Where(h => h.MaCa == maCa)
                .ToListAsync();
        }
        public void SavePendingOrder(int maBan, List<ChiTietHD> chiTiets)
        {
            _pendingOrders[maBan] = new List<ChiTietHD>(chiTiets); // Copy để an toàn
        }

        /// <summary>
        /// Lấy lịch sử order tạm của bàn (khi mở lại bàn cũ)
        /// </summary>
        public List<ChiTietHD>? GetPendingOrder(int maBan)
        {
            _pendingOrders.TryGetValue(maBan, out var order);
            return order;
        }

        /// <summary>
        /// Xóa lịch sử tạm sau khi thanh toán thành công
        /// </summary>
        public void ClearPendingOrder(int maBan)
        {
            _pendingOrders.Remove(maBan);
        }
    }
}