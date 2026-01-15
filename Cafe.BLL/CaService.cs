using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cafe.DAL;
using Cafe.Entity;

namespace Cafe.BLL
{
    public class CaService
    {
        private readonly CafeContext _context;

        public CaService(CafeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Ca?> GetCaByIdAsync(int maCa)
        {
            return await _context.Cas.FindAsync(maCa);
        }

        public async Task<Ca> TaoCaMoiAsync()
        {
            var caMoi = new Ca
            {
                GioBD = DateTime.Now,
                DoanhThu = 0
            };
            _context.Cas.Add(caMoi);
            await _context.SaveChangesAsync();
            return caMoi;
        }

        public async Task CapNhatDoanhThuCaAsync(int maCa)
        {
            var doanhThu = await _context.HoaDons
                .Where(h => h.MaCa == maCa && h.TrangThai == "Paid")
                .SumAsync(h => h.TongTien);

            var ca = await _context.Cas.FindAsync(maCa);
            if (ca != null)
            {
                ca.DoanhThu = doanhThu; // Bây giờ DoanhThu là decimal → hết lỗi
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Ca?> GetCaDangMoAsync()
        {
            return await _context.Cas
                .Where(c => c.GioKT == null)
                .OrderByDescending(c => c.GioBD)
                .FirstOrDefaultAsync();
        }

        public async Task DongCaAsync(int maCa)
        {
            var ca = await _context.Cas.FindAsync(maCa);
            if (ca != null)
            {
                ca.GioKT = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Ca>> GetAllCaAsync()
        {
            return await _context.Cas
                .OrderByDescending(c => c.GioBD)
                .ToListAsync();
        }

        public async Task<decimal> GetDoanhThuCaAsync(int maCa) // Đổi return type thành decimal
        {
            return await _context.HoaDons
                .Where(h => h.MaCa == maCa && h.TrangThai == "Paid")
                .SumAsync(h => h.TongTien); // Trả về decimal trực tiếp
        }
    }
}