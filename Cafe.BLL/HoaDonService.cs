using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Cafe.DAL;
using Cafe.Entity;

namespace Cafe.BLL
{
    public class HoaDonService
    {
        public async Task<HoaDon> TaoHoaDonAsync(int maBan, int maCa, List<ChiTietHD> chiTiets)
        {
            using var context = new CafeContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Tạo hóa đơn
                var hoaDon = new HoaDon
                {
                    MaBan = maBan,
                    MaCa = maCa,
                    NgayLap = DateTime.Now,
                    TrangThai = "Unpaid"
                };

                context.HoaDons.Add(hoaDon);
                await context.SaveChangesAsync(); // Lưu để lấy MaHD

                int tongTien = 0;
                foreach (var ct in chiTiets)
                {
                    var mon = await context.Menus.FindAsync(ct.MaMon);
                    if (mon == null || !mon.TrangThai)
                        throw new Exception($"Món {ct.MaMon} không tồn tại hoặc đã ngưng bán");

                    ct.MaHD = hoaDon.MaHD;
                    ct.DonGia = mon.DonGia;
                    ct.ThanhTien = ct.SoLuong * mon.DonGia;
                    tongTien += ct.ThanhTien;

                    context.ChiTietHDs.Add(ct);
                }

                hoaDon.TongTien = tongTien;
                await context.SaveChangesAsync();

                // Cập nhật trạng thái bàn
                var banService = new BanService();
                await banService.UpdateTrangThaiAsync(maBan, "DangPhucVu");

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
            using var context = new CafeContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var hd = await context.HoaDons.Include(h => h.Ban).FirstOrDefaultAsync(h => h.MaHD == maHD);
                if (hd == null || hd.TrangThai == "Paid")
                    throw new Exception("Hóa đơn không tồn tại hoặc đã thanh toán");

                hd.TrangThai = "Paid";

                // Cập nhật doanh thu ca
                var ca = await context.Cas.FindAsync(maCa);
                if (ca != null)
                    ca.DoanhThu += hd.TongTien;

                // Giải phóng bàn
                if (hd.Ban != null)
                    hd.Ban.TrangThai = "Trong";

                await context.SaveChangesAsync();
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
            using var context = new CafeContext();
            return await context.HoaDons
                .Include(h => h.Ban)
                .Where(h => h.MaCa == maCa)
                .ToListAsync();
        }
    }
}