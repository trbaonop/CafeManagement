using System;

namespace TEST.Common
{
    /// <summary>
    /// Lớp tĩnh lưu thông tin người dùng đang đăng nhập
    /// Dùng để kiểm tra phân quyền ở mọi nơi trong ứng dụng
    /// </summary>
    public static class CurrentUser
    {
        public static int MaND { get; private set; } = 0;
        public static string TenDangNhap { get; private set; } = string.Empty;
        public static string HoTen { get; private set; } = string.Empty;
        public static string TenVaiTro { get; private set; } = string.Empty;

        /// <summary>
        /// Kiểm tra người dùng đã đăng nhập chưa
        /// </summary>
        public static bool IsLoggedIn => MaND > 0;

        /// <summary>
        /// Kiểm tra có phải Admin không
        /// </summary>
        public static bool IsAdmin => TenVaiTro == "Admin";

        /// <summary>
        /// Kiểm tra có phải Quản lý không
        /// </summary>
        public static bool IsQuanLy => TenVaiTro == "QuanLy";

        /// <summary>
        /// Kiểm tra có phải Nhân viên không
        /// </summary>
        public static bool IsNhanVien => TenVaiTro == "NhanVien";

        /// <summary>
        /// Đăng nhập - lưu thông tin người dùng
        /// </summary>
        public static void Login(int maND, string tenDangNhap, string? hoTen, string? tenVaiTro)
        {
            MaND = maND;
            TenDangNhap = tenDangNhap ?? string.Empty;
            HoTen = string.IsNullOrWhiteSpace(hoTen) ? tenDangNhap : hoTen;
            TenVaiTro = tenVaiTro ?? string.Empty;
        }

        /// <summary>
        /// Đăng xuất - xóa thông tin người dùng
        /// </summary>
        public static void Logout()
        {
            MaND = 0;
            TenDangNhap = string.Empty;
            HoTen = string.Empty;
            TenVaiTro = string.Empty;
        }

        /// <summary>
        /// Lấy thông tin người dùng hiện tại (dùng để hiển thị trên form)
        /// </summary>
        public static string GetInfo()
        {
            if (!IsLoggedIn)
                return "Chưa đăng nhập";

            return $"{HoTen} ({TenVaiTro})";
        }
    }
}