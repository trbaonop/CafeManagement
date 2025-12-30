using Microsoft.EntityFrameworkCore;
using TEST.DAL;
using TEST.Entity;

namespace TEST.BLL
{
    public class AuthService
    {
        /// <summary>
        /// Kiểm tra đăng nhập và trả về người dùng nếu hợp lệ
        /// </summary>
        public async Task<NguoiDung?> LoginAsync(string tenDangNhap, string matKhau)
        {
            using var context = new CafeContext();
            return await context.NguoiDungs
                .Include(u => u.VaiTro)
                .FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap
                                       && u.MatKhau == matKhau
                                       && u.TrangThai);
        }
    }
}