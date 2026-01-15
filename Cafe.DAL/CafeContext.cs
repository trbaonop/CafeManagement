using Microsoft.EntityFrameworkCore;
using Cafe.Entity;
using System;

namespace Cafe.DAL
{
    public class CafeContext : DbContext
    {
        // Constructor nhận DbContextOptions từ container (chuỗi kết nối)
        public CafeContext(DbContextOptions<CafeContext> options) : base(options)
        {
        }

        // DbSet tương ứng với bảng trong database
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

           
            // Các cấu hình cũ của bạn giữ nguyên...

            // === SỬA CHÍNH: Ép tên bảng đúng chữ hoa/thường như trong DB ===
            modelBuilder.Entity<ChiTietHD>().ToTable("ChiTietHD");

            // Để an toàn, cấu hình hết các bảng khác (khớp đúng với DB)
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
            modelBuilder.Entity<VaiTro>().ToTable("VaiTro");
            modelBuilder.Entity<Ban>().ToTable("Ban");
            modelBuilder.Entity<Ca>().ToTable("Ca");
            modelBuilder.Entity<Menu>().ToTable("Menu");
            modelBuilder.Entity<HoaDon>().ToTable("HoaDon");

            // ... các cấu hình ENUM, BIT, FK, HasKey giữ nguyên

            base.OnModelCreating(modelBuilder);
        }

          
        
    }
}