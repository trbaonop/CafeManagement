using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cafe.DAL;
using Cafe.Entity;

namespace Cafe.BLL
{
    public class NguoiDungService
    {
        private readonly CafeContext _context;

        public NguoiDungService(CafeContext context)
        {
            _context = context;
        }

        public async Task<List<NguoiDung>> GetAllUsersAsync()
        {
            return await _context.NguoiDungs
                .Include(u => u.VaiTro)
                .ToListAsync();
        }

        public async Task KhoaTaiKhoanAsync(int maND)
        {
            var user = await _context.NguoiDungs.FindAsync(maND);
            if (user != null)
            {
                user.TrangThai = false;
                await _context.SaveChangesAsync();
            }
        }

        // Nếu cần thêm: GetUserByIdAsync, AddUserAsync, UpdateUserAsync...
    }
}