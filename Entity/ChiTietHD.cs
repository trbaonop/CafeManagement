using System.ComponentModel.DataAnnotations;
namespace TEST.Entity
{
    public class ChiTietHD
    {
        [Key]
        public int MaCT { get; set; }
        public int MaHD { get; set; }
        public int MaMon { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int ThanhTien { get; set; }

        // Navigation properties
        public HoaDon HoaDon { get; set; } = null!;
        public Menu Menu { get; set; } = null!;
    }
}