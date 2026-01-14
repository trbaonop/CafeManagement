using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cafe.DAL;
using Cafe.Entity;

namespace Cafe.BLL
{
    public class AuthService
    {
        private readonly CafeContext _context;

        public AuthService(CafeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Đăng nhập người dùng
        /// </summary>
        /// <param name="tenDangNhap">Tên đăng nhập</param>
        /// <param name="matKhau">Mật khẩu (chưa hash)</param>
        /// <returns>Thông tin người dùng nếu đúng, null nếu sai</returns>
        public async Task<NguoiDung?> LoginAsync(string tenDangNhap, string matKhau)
        {
            return await _context.NguoiDungs
                .Include(u => u.VaiTro)
                .FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap
                                       && u.MatKhau == matKhau
                                       && u.TrangThai);
        }
    }
}