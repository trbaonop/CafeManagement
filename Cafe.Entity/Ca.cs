using System.ComponentModel.DataAnnotations;
namespace Cafe.Entity
{
    public class Ca
    {
        [Key]
        public int MaCa { get; set; }
        public DateTime GioBD { get; set; }
        public DateTime? GioKT { get; set; }
        public int DoanhThu { get; set; } = 0;

        // Navigation property
        public ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }
}