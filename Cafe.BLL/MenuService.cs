using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cafe.DAL;
using Cafe.Entity;

namespace Cafe.BLL
{
    public class MenuService
    {
        private readonly CafeContext _context;

        public MenuService(CafeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Menu>> GetAllMenusAsync(bool onlyActive = true)
        {
            var query = _context.Menus.AsQueryable();
            if (onlyActive)
                query = query.Where(m => m.TrangThai);

            return await query.OrderBy(m => m.TenMon).ToListAsync();
        }

        public async Task<Menu?> GetMonByIdAsync(int maMon)
        {
            return await _context.Menus.FindAsync(maMon);
        }

        public async Task AddMonAsync(Menu mon)
        {
            _context.Menus.Add(mon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMonAsync(Menu mon)
        {
            _context.Menus.Update(mon);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMonAsync(int maMon)
        {
            var mon = await _context.Menus.FindAsync(maMon);
            if (mon != null)
            {
                mon.TrangThai = false; // Soft delete
                await _context.SaveChangesAsync();
            }
        }
    }
}