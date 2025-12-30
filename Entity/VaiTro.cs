using System.ComponentModel.DataAnnotations;
namespace TEST.Entity
{
    public class VaiTro
    {
        [Key]
        public int MaVaiTro { get; set; }
        public string TenVaiTro { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
    }
}