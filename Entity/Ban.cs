using System.ComponentModel.DataAnnotations;
namespace TEST.Entity
{
    public class Ban
    {
        [Key]
        public int MaBan { get; set; }
        public string TenBan { get; set; } = string.Empty;
        public string TrangThai { get; set; } = "Trong";

        public ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }
}