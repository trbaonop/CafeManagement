using System.ComponentModel.DataAnnotations;
namespace Cafe.Entity
{
    public class NguoiDung
    {
        [Key]
        public int MaND { get; set; }
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string? HoTen { get; set; }
        public int? MaVaiTro { get; set; }
        public bool TrangThai { get; set; } = true;

        public VaiTro? VaiTro { get; set; }
    }
}