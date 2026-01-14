using Microsoft.EntityFrameworkCore;
using Cafe.Entity;
using System;

namespace Cafe.DAL
{
  
    public class CafeContext : DbContext
    {
        // Constructor này nhậnDbContextOptions (chứa chuỗi kết nối) từ container
        public CafeContext(DbContextOptions<CafeContext> options) : base(options)
        {
        }

        // Các DbSet tương ứng với bảng trong database
        public DbSet<VaiTro> VaiTros { get; set; } = null!;
        public DbSet<NguoiDung> NguoiDungs { get; set; } = null!;
        public DbSet<Ban> Bans { get; set; } = null!;
        public DbSet<Ca> Cas { get; set; } = null!;
        public DbSet<Menu> Menus { get; set; } = null!;
        public DbSet<HoaDon> HoaDons { get; set; } = null!;
        public DbSet<ChiTietHD> ChiTietHDs { get; set; } = null!;

 
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

            // Cấu hình BIT (boolean)
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

            // Cấu hình Primary Key (nếu chưa có trong Entity)
            modelBuilder.Entity<Ban>().HasKey(b => b.MaBan);
            modelBuilder.Entity<Ca>().HasKey(c => c.MaCa);
            modelBuilder.Entity<Menu>().HasKey(m => m.MaMon);
            modelBuilder.Entity<HoaDon>().HasKey(h => h.MaHD);
            modelBuilder.Entity<ChiTietHD>().HasKey(ct => ct.MaCT);
            modelBuilder.Entity<VaiTro>().HasKey(v => v.MaVaiTro);
            modelBuilder.Entity<NguoiDung>().HasKey(u => u.MaND);

            base.OnModelCreating(modelBuilder);
        }
    }
}