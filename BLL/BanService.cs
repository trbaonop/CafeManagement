using Microsoft.EntityFrameworkCore;
using TEST.DAL;
using TEST.Entity;

namespace TEST.BLL
{
    public class BanService
    {
        public async Task<List<Ban>> GetAllBansAsync()
        {
            using var context = new CafeContext();
            return await context.Bans.ToListAsync();
        }

        public async Task<Ban?> GetBanByIdAsync(int maBan)
        {
            using var context = new CafeContext();
            return await context.Bans.FindAsync(maBan);
        }

        public async Task UpdateTrangThaiAsync(int maBan, string trangThaiMoi)
        {
            using var context = new CafeContext();
            var ban = await context.Bans.FindAsync(maBan);
            if (ban != null)
            {
                ban.TrangThai = trangThaiMoi;
                await context.SaveChangesAsync();
            }
        }
    }
}