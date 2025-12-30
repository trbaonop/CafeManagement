using System.ComponentModel.DataAnnotations;
namespace TEST.Entity
{
    public class HoaDon
    {
        [Key]
        public int MaHD { get; set; }
        public int MaBan { get; set; }
        public int MaCa { get; set; }
        public DateTime NgayLap { get; set; } = DateTime.Now;
        public int TongTien { get; set; } = 0;
        public string TrangThai { get; set; } = "Unpaid"; // Unpaid hoặc Paid

        // Navigation properties
        public Ban Ban { get; set; } = null!;
        public Ca Ca { get; set; } = null!;
        public ICollection<ChiTietHD> ChiTietHDs { get; set; } = new List<ChiTietHD>();
    }
}