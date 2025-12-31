using Microsoft.EntityFrameworkCore;
using Cafe.Entity;

namespace Cafe.DAL
{
    /// <summary>
    /// DbContext chính của ứng dụng - kết nối với database MySQL 'cafe'
    /// </summary>
    public class CafeContext : DbContext
    {
        // Các DbSet tương ứng với bảng trong database
        public DbSet<VaiTro> VaiTros { get; set; } = null!;
        public DbSet<NguoiDung> NguoiDungs { get; set; } = null!;
        public DbSet<Ban> Bans { get; set; } = null!;
        public DbSet<Ca> Cas { get; set; } = null!;
        public DbSet<Menu> Menus { get; set; } = null!;
        public DbSet<HoaDon> HoaDons { get; set; } = null!;
        public DbSet<ChiTietHD> ChiTietHDs { get; set; } = null!;

        /// <summary>
        /// Cấu hình chuỗi kết nối MySQL
        /// Thay đổi password nếu cần
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "server=localhost;port=3306;database=cafe;user=root;password=Bao2005bao;";

                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        /// <summary>
        /// Cấu hình model (mapping với bảng database)
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình ENUM lưu dưới dạng string
            modelBuilder.Entity<Ban>()
                .Property(b => b.TrangThai)
                .HasConversion<string>()
                .HasDefaultValue("Trong");

            modelBuilder.Entity<HoaDon>()
                .Property(h => h.TrangThai)
                .HasConversion<string>()
                .HasDefaultValue("Unpaid");

            // Cấu hình BIT
            modelBuilder.Entity<NguoiDung>()
                .Property(u => u.TrangThai)
                .HasColumnType("bit(1)")
                .HasDefaultValue(true);

            modelBuilder.Entity<Menu>()
                .Property(m => m.TrangThai)
                .HasColumnType("bit(1)")
                .HasDefaultValue(true);

            // Cấu hình khóa ngoại và cascade (tùy chọn)
            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.Ban)
                .WithMany(b => b.HoaDons)
                .HasForeignKey(h => h.MaBan)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.Ca)
                .WithMany(c => c.HoaDons)
                .HasForeignKey(h => h.MaCa)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ChiTietHD>()
                .HasOne(c => c.HoaDon)
                .WithMany(h => h.ChiTietHDs)
                .HasForeignKey(c => c.MaHD)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietHD>()
                .HasOne(c => c.Menu)
                .WithMany(m => m.ChiTietHDs)
                .HasForeignKey(c => c.MaMon)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<NguoiDung>()
                .HasOne(u => u.VaiTro)
                .WithMany(v => v.NguoiDungs)
                .HasForeignKey(u => u.MaVaiTro)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}