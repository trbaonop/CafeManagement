using System;

namespace Cafe.Common
{
  
    public static class CurrentUser
    {
        public static int MaND { get; private set; } = 0;
        public static string TenDangNhap { get; private set; } = string.Empty;
        public static string HoTen { get; private set; } = string.Empty;
        public static string TenVaiTro { get; private set; } = string.Empty;


        public static bool IsLoggedIn => MaND > 0;


        public static bool IsAdmin => TenVaiTro == "Admin";

   
        public static bool IsQuanLy => TenVaiTro == "QuanLy";

    
        public static bool IsNhanVien => TenVaiTro == "NhanVien";

  
        public static void Login(int maND, string tenDangNhap, string? hoTen, string? tenVaiTro)
        {
            MaND = maND;
            TenDangNhap = tenDangNhap ?? string.Empty;
            HoTen = string.IsNullOrWhiteSpace(hoTen) ? tenDangNhap : hoTen;
            TenVaiTro = tenVaiTro ?? string.Empty;
        }

     
        public static void Logout()
        {
            MaND = 0;
            TenDangNhap = string.Empty;
            HoTen = string.Empty;
            TenVaiTro = string.Empty;
        }

    
        public static string GetInfo()
        {
            if (!IsLoggedIn)
                return "Chưa đăng nhập";

            return $"{HoTen} ({TenVaiTro})";
        }
    }
}