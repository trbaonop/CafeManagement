using Microsoft.EntityFrameworkCore;
using TEST.DAL;
using TEST.Entity;

namespace TEST.BLL
{
    public class CaService
    {
        public async Task<Ca> TaoCaMoiAsync()
        {
            using var context = new CafeContext();
            var caMoi = new Ca
            {
                GioBD = DateTime.Now,
                DoanhThu = 0
            };
            context.Cas.Add(caMoi);
            await context.SaveChangesAsync();
            return caMoi;
        }

        public async Task DongCaAsync(int maCa)
        {
            using var context = new CafeContext();
            var ca = await context.Cas.FindAsync(maCa);
            if (ca != null)
            {
                ca.GioKT = DateTime.Now;
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Ca>> GetAllCaAsync()
        {
            using var context = new CafeContext();
            return await context.Cas.OrderByDescending(c => c.GioBD).ToListAsync();
        }

        public async Task<int> GetDoanhThuCaAsync(int maCa)
        {
            using var context = new CafeContext();
            var hdService = new HoaDonService();
            var hoaDons = await hdService.GetHoaDonByCaAsync(maCa);
            return hoaDons.Where(h => h.TrangThai == "Paid").Sum(h => h.TongTien);
        }
    }
}