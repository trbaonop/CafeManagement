using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cafe.DAL;     // CafeContext
using Cafe.Entity;  // Ban

namespace Cafe.BLL
{
    public class BanService
    {
        private readonly CafeContext _context;

        public BanService(CafeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Ban>> GetAllBansAsync()
        {
            return await _context.Bans
                .OrderBy(b => b.MaBan)
                .ToListAsync();
        }

        public async Task<Ban?> GetBanByIdAsync(int maBan)
        {
            return await _context.Bans
                .FirstOrDefaultAsync(b => b.MaBan == maBan);
        }

        public async Task DeleteBanAsync(int maBan)
        {
            var ban = await _context.Bans.FindAsync(maBan);
            if (ban != null)
            {
                _context.Bans.Remove(ban);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateTrangThaiAsync(int maBan, string trangThaiMoi)
        {
            var ban = await _context.Bans.FindAsync(maBan);
            if (ban != null)
            {
                ban.TrangThai = trangThaiMoi;
                await _context.SaveChangesAsync();
            }
        }
    }
}