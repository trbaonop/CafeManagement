using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TEST.BLL;
using TEST.Entity;

namespace TEST.Forms
{
    public partial class frmOrder : Form
    {
        private readonly int _maBan;
        private  string _tenBan;
        private readonly MenuService _menuService = new MenuService();
        private readonly HoaDonService _hoaDonService = new HoaDonService();
        private readonly BanService _banService = new BanService();
        private readonly CaService _caService = new CaService();

        private List<ChiTietHD> _orderChiTiet = new List<ChiTietHD>();
        private int _maCaHienTai = 1; // Giả sử có ca hiện tại, bạn có thể lấy từ DB

        public frmOrder(int maBan)
        {
            InitializeComponent();
            _maBan = maBan;
        }

        private async void frmOrder_Load(object sender, EventArgs e)
        {
            // Lấy tên bàn
            var ban = await _banService.GetBanByIdAsync(_maBan);
            _tenBan = ban?.TenBan ?? "Bàn không xác định";
            lblBan.Text = $"ĐANG ORDER: {_tenBan}";

            // Load menu
            await LoadMenu();

            // Cấu hình grid
            SetupGridMenu();
            SetupGridOrder();

            // Load ca hiện tại (giả sử lấy ca mới nhất đang mở)
            // Bạn có thể cải tiến bằng cách lấy ca đang mở
            var caMoi = await _caService.TaoCaMoiAsync();
            _maCaHienTai = caMoi.MaCa;
        }

        private async Task LoadMenu()
        {
            var menus = await _menuService.GetAllMenusAsync();
            dgvMenu.DataSource = menus.Select(m => new
            {
                m.MaMon,
                m.TenMon,
                DonGia = m.DonGia.ToString("#,##0") + " VNĐ"
            }).ToList();
        }

        private void SetupGridMenu()
        {
            dgvMenu.Columns["MaMon"].HeaderText = "Mã món";
            dgvMenu.Columns["TenMon"].HeaderText = "Tên món";
            dgvMenu.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvMenu.ReadOnly = true;
        }

        private void SetupGridOrder()
        {
            dgvOrder.Columns.Clear();
            dgvOrder.Columns.Add("TenMon", "Tên món");
            dgvOrder.Columns.Add("SoLuong", "Số lượng");
            dgvOrder.Columns.Add("DonGia", "Đơn giá");
            dgvOrder.Columns.Add("ThanhTien", "Thành tiền");
            dgvOrder.ReadOnly = true;
        }

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            if (dgvMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món!");
                return;
            }

            int maMon = Convert.ToInt32(dgvMenu.SelectedRows[0].Cells["MaMon"].Value);
            int soLuong = (int)nudSoLuong.Value;

            var chiTiet = _orderChiTiet.FirstOrDefault(c => c.MaMon == maMon);
            if (chiTiet == null)
            {
                chiTiet = new ChiTietHD
                {
                    MaMon = maMon,
                    SoLuong = soLuong,
                    DonGia = 0, // Sẽ lấy từ DB khi thanh toán
                    ThanhTien = 0
                };
                _orderChiTiet.Add(chiTiet);
            }
            else
            {
                chiTiet.SoLuong += soLuong;
            }

            RefreshOrderGrid();
        }

        private async void RefreshOrderGrid()
        {
            dgvOrder.Rows.Clear();
            int tong = 0;

            foreach (var ct in _orderChiTiet)
            {
                var mon = await _menuService.GetMonByIdAsync(ct.MaMon);
                if (mon != null)
                {
                    ct.DonGia = mon.DonGia;
                    ct.ThanhTien = ct.SoLuong * mon.DonGia;
                    tong += ct.ThanhTien;

                    dgvOrder.Rows.Add(mon.TenMon, ct.SoLuong, mon.DonGia.ToString("#,##0"), ct.ThanhTien.ToString("#,##0"));
                }
            }

            lblTongTien.Text = $"Tổng tiền: {tong:#,##0} VNĐ";
        }

        private async void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_orderChiTiet.Count == 0)
            {
                MessageBox.Show("Chưa order món nào!");
                return;
            }

            try
            {
                await _hoaDonService.TaoHoaDonAsync(_maBan, _maCaHienTai, _orderChiTiet);

                MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); // Đóng form, frmMain sẽ refresh bàn
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}