using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TEST.BLL;
using TEST.Common;

namespace TEST.Forms
{
    public partial class frmBaoCaoDoanhThu : Form
    {
        private readonly CaService _caService = new CaService();
        private readonly HoaDonService _hoaDonService = new HoaDonService();

        public frmBaoCaoDoanhThu()
        {
            InitializeComponent();
        }

        private async void frmBaoCaoDoanhThu_Load(object sender, EventArgs e)
        {
            // Phân quyền: chỉ Quản lý và Admin mới vào được
            if (!(CurrentUser.IsAdmin || CurrentUser.IsQuanLy))
            {
                MessageBox.Show("Bạn không có quyền truy cập báo cáo doanh thu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            await LoadDanhSachCa();
        }

        private async Task LoadDanhSachCa()
        {
            var cas = await _caService.GetAllCaAsync();

            dgvCa.DataSource = cas.Select(c => new
            {
                c.MaCa,
                GioBatDau = c.GioBD.ToString("dd/MM/yyyy HH:mm"),
                GioKetThuc = c.GioKT?.ToString("dd/MM/yyyy HH:mm") ?? "Đang mở",
                c.DoanhThu // Doanh thu tạm lưu (nếu có thiết kế cũ)
            }).ToList();

            dgvCa.Columns["MaCa"].HeaderText = "Mã ca";
            dgvCa.Columns["GioBatDau"].HeaderText = "Giờ bắt đầu";
            dgvCa.Columns["GioKetThuc"].HeaderText = "Giờ kết thúc";
            dgvCa.Columns["DoanhThu"].HeaderText = "Doanh thu tạm (DB)";

            dgvCa.ClearSelection();
            dgvHoaDon.DataSource = null;
            lblTongDoanhThu.Text = "Tổng doanh thu ca: 0 VNĐ";
        }

        private async void dgvCa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int maCa = Convert.ToInt32(dgvCa.Rows[e.RowIndex].Cells["MaCa"].Value);

            var hoaDons = await _hoaDonService.GetHoaDonByCaAsync(maCa);

            // Chỉ tính hóa đơn đã thanh toán
            var hoaDonPaid = hoaDons.Where(h => h.TrangThai == "Paid").ToList();

            int tongDoanhThu = hoaDonPaid.Sum(h => h.TongTien);

            dgvHoaDon.DataSource = hoaDonPaid.Select(h => new
            {
                h.MaHD,
                Ban = h.Ban?.TenBan ?? "Không xác định",
                h.NgayLap,
                TongTien = h.TongTien.ToString("#,##0 VNĐ")
            }).ToList();

            lblTongDoanhThu.Text = $"Tổng doanh thu ca: {tongDoanhThu:#,##0} VNĐ";
        }
    }
}