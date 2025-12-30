using System.ComponentModel.DataAnnotations;
namespace TEST.Entity
{
    public class Menu
    {
        [Key]
        public int MaMon { get; set; }
        public string TenMon { get; set; } = string.Empty;
        public int DonGia { get; set; }
        public bool TrangThai { get; set; } = true;

        // Navigation property
        public ICollection<ChiTietHD> ChiTietHDs { get; set; } = new List<ChiTietHD>();
    }
}