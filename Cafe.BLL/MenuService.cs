using Microsoft.EntityFrameworkCore;
using Cafe.DAL;
using Cafe.Entity;

namespace Cafe.BLL
{
    public class MenuService
    {
        public async Task<List<Menu>> GetAllMenusAsync(bool onlyActive = true)
        {
            using var context = new CafeContext();
            var query = context.Menus.AsQueryable();
            if (onlyActive)
                query = query.Where(m => m.TrangThai);
            return await query.OrderBy(m => m.TenMon).ToListAsync();
        }

        public async Task<Menu?> GetMonByIdAsync(int maMon)
        {
            using var context = new CafeContext();
            return await context.Menus.FindAsync(maMon);
        }

        public async Task AddMonAsync(Menu mon)
        {
            using var context = new CafeContext();
            context.Menus.Add(mon);
            await context.SaveChangesAsync();
        }

        public async Task UpdateMonAsync(Menu mon)
        {
            using var context = new CafeContext();
            context.Menus.Update(mon);
            await context.SaveChangesAsync();
        }

        public async Task DeleteMonAsync(int maMon)
        {
            using var context = new CafeContext();
            var mon = await context.Menus.FindAsync(maMon);
            if (mon != null)
            {
                mon.TrangThai = false; // Soft delete - ngưng bán
                await context.SaveChangesAsync();
            }
        }
    }
}