using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cafe.DAL;
using Cafe.Entity;

namespace Cafe.BLL
{
    public class HoaDonService
    {
        private readonly CafeContext _context;

        public HoaDonService(CafeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<HoaDon> TaoHoaDonAsync(int maBan, int maCa, List<ChiTietHD> chiTiets)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var hoaDon = new HoaDon
                {
                    MaBan = maBan,
                    MaCa = maCa,
                    NgayLap = DateTime.Now,
                    TrangThai = "Unpaid"
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync(); // Lấy MaHD

                int tongTien = 0;
                foreach (var ct in chiTiets)
                {
                    var mon = await _context.Menus.FindAsync(ct.MaMon);
                    if (mon == null || !mon.TrangThai)
                        throw new Exception($"Món {ct.MaMon} không tồn tại hoặc đã ngưng bán");

                    ct.MaHD = hoaDon.MaHD;
                    ct.DonGia = mon.DonGia;
                    ct.ThanhTien = ct.SoLuong * mon.DonGia;
                    tongTien += ct.ThanhTien;

                    _context.ChiTietHDs.Add(ct);
                }

                hoaDon.TongTien = tongTien;
                await _context.SaveChangesAsync();

                // Cập nhật trạng thái bàn (nếu cần)
                var ban = await _context.Bans.FindAsync(maBan);
                if (ban != null)
                {
                    ban.TrangThai = "DangPhucVu";
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return hoaDon;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ThanhToanAsync(int maHD, int maCa)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var hd = await _context.HoaDons
                    .Include(h => h.Ban)
                    .FirstOrDefaultAsync(h => h.MaHD == maHD);

                if (hd == null || hd.TrangThai == "Paid")
                    throw new Exception("Hóa đơn không tồn tại hoặc đã thanh toán");

                hd.TrangThai = "Paid";

                // Cập nhật doanh thu ca
                var ca = await _context.Cas.FindAsync(maCa);
                if (ca != null)
                    ca.DoanhThu += hd.TongTien;

                // Giải phóng bàn
                if (hd.Ban != null)
                    hd.Ban.TrangThai = "Trong";

                await _context.SaveChangesAsync();
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
    }
}