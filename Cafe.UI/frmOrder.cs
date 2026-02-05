using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cafe.BLL;
using Cafe.Entity;

namespace Cafe.UI
{
    public partial class frmOrder : Form
    {
        private readonly MenuService _menuService;
        private readonly HoaDonService _hoaDonService;
        private readonly BanService _banService;
        private readonly CaService _caService;

        private int _maBan;
        private string? _tenBan;
        private int _maCaHienTai;
        private List<ChiTietHD> _orderChiTiet = new();

        public int MaBan { get; set; }

        public frmOrder(MenuService menuService, HoaDonService hoaDonService,
                        BanService banService, CaService caService)
        {
            InitializeComponent();
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
            _hoaDonService = hoaDonService ?? throw new ArgumentNullException(nameof(hoaDonService));
            _banService = banService ?? throw new ArgumentNullException(nameof(banService));
            _caService = caService ?? throw new ArgumentNullException(nameof(caService));
        }

        private async void FrmOrder_Load(object sender, EventArgs e)
        {
            _maBan = MaBan;

            var ban = await _banService.GetBanByIdAsync(_maBan);
            _tenBan = ban?.TenBan ?? "Bàn không xác định";
            lblBan.Text = $"ĐANG ORDER: {_tenBan}";

            await LoadMenuAsync();
            SetupGridMenu();           // Thêm cột cho dgvMenu
            SetupGridOrder();          // THÊM CỘT TRƯỚC (quan trọng nhất!)

            // Tải lịch sử order tạm nếu có
            var savedOrder = _hoaDonService.GetPendingOrder(_maBan);
            if (savedOrder != null && savedOrder.Any())
            {
                _orderChiTiet = new List<ChiTietHD>(savedOrder);
                await RefreshOrderGridAsync(); // Bây giờ grid đã có cột → an toàn
                MessageBox.Show("Đã tải lại lịch sử order cũ của bàn này!", "Thông báo");
            }

            var caHienTai = await _caService.GetCaDangMoAsync();
            if (caHienTai == null)
            {
                caHienTai = await _caService.TaoCaMoiAsync();
            }
            _maCaHienTai = caHienTai.MaCa;
        }

        private async Task LoadMenuAsync()
        {
            var menus = await _menuService.GetAllMenusAsync();
            dgvMenu.DataSource = menus.Select(m => new
            {
                m.MaMon,
                m.TenMon,
                DonGia = $"{m.DonGia:#,##0} VNĐ"
            }).ToList();
        }

        private void SetupGridMenu()
        {
            if (dgvMenu.Columns.Count == 0)
            {
                dgvMenu.Columns.Add("MaMon", "Mã món");
                dgvMenu.Columns.Add("TenMon", "Tên món");
                dgvMenu.Columns.Add("DonGia", "Đơn giá");
            }
            dgvMenu.Columns["MaMon"].HeaderText = "Mã món";
            dgvMenu.Columns["TenMon"].HeaderText = "Tên món";
            dgvMenu.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvMenu.ReadOnly = true;
            dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMenu.MultiSelect = false;
        }

        private void SetupGridOrder()
        {
            // Chỉ thêm cột nếu grid chưa có cột nào (tránh lỗi và không xóa cột cũ)
            if (dgvOrder.Columns.Count == 0)
            {
                dgvOrder.Columns.Add("TenMon", "Tên món");
                dgvOrder.Columns.Add("SoLuong", "Số lượng");
                dgvOrder.Columns.Add("DonGia", "Đơn giá");
                dgvOrder.Columns.Add("ThanhTien", "Thành tiền");
            }

            dgvOrder.ReadOnly = true;
        }

        private async void BtnThemMon_Click(object sender, EventArgs e)
        {
            if (dgvMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maMon = Convert.ToInt32(dgvMenu.SelectedRows[0].Cells["MaMon"].Value);
            int soLuong = (int)nudSoLuong.Value;

            var chiTiet = _orderChiTiet.FirstOrDefault(c => c.MaMon == maMon);
            if (chiTiet == null)
            {
                var mon = await _menuService.GetMonByIdAsync(maMon);
                if (mon == null)
                {
                    MessageBox.Show("Món không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                chiTiet = new ChiTietHD
                {
                    MaMon = maMon,
                    SoLuong = soLuong,
                    DonGia = mon.DonGia,
                    ThanhTien = soLuong * mon.DonGia
                };
                _orderChiTiet.Add(chiTiet);
            }
            else
            {
                chiTiet.SoLuong += soLuong;
                var mon = await _menuService.GetMonByIdAsync(maMon);
                if (mon != null)
                    chiTiet.ThanhTien = chiTiet.SoLuong * mon.DonGia;
            }

            // Đổi trạng thái bàn nếu đây là món đầu tiên
            if (_orderChiTiet.Count == 1)
                await _banService.UpdateTrangThaiAsync(_maBan, "DangPhucVu");

            await RefreshOrderGridAsync();
        }

        private async Task RefreshOrderGridAsync()
        {
            dgvOrder.Rows.Clear(); // Chỉ xóa hàng, giữ cột
            decimal tong = 0;

            foreach (var ct in _orderChiTiet)
            {
                var mon = await _menuService.GetMonByIdAsync(ct.MaMon);
                if (mon != null)
                {
                    ct.DonGia = mon.DonGia;
                    ct.ThanhTien = ct.SoLuong * mon.DonGia;
                    tong += ct.ThanhTien;

                    dgvOrder.Rows.Add(mon.TenMon, ct.SoLuong, $"{mon.DonGia:#,##0}", $"{ct.ThanhTien:#,##0}");
                }
            }

            lblTongTien.Text = $"Tổng tiền: {tong:#,##0} VNĐ";
        }

        private async void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (_orderChiTiet.Count == 0)
            {
                MessageBox.Show("Chưa order món nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnThanhToan.Enabled = false;

            try
            {
                // Kiểm tra và cập nhật thông tin món
                foreach (var ct in _orderChiTiet)
                {
                    var mon = await _menuService.GetMonByIdAsync(ct.MaMon);
                    if (mon == null)
                    {
                        MessageBox.Show($"Món ID {ct.MaMon} không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ct.DonGia = mon.DonGia;
                    ct.ThanhTien = ct.SoLuong * mon.DonGia;
                }

                // Tạo và thanh toán hóa đơn
                var hoaDon = await _hoaDonService.TaoHoaDonAsync(_maBan, _maCaHienTai, _orderChiTiet);
                await _hoaDonService.ThanhToanAsync(hoaDon.MaHD);

                // Xóa lịch sử tạm sau thanh toán
                _hoaDonService.ClearPendingOrder(_maBan);

                MessageBox.Show("Thanh toán thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                if (ex.InnerException != null)
                    errorMsg += "\n\nChi tiết lỗi: " + ex.InnerException.Message;

                MessageBox.Show(errorMsg, "Lỗi thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnThanhToan.Enabled = true;
            }
        }

        public async Task LoadPendingOrder(List<ChiTietHD> pendingOrder)
        {
            if (pendingOrder == null || !pendingOrder.Any())
                return;

            _orderChiTiet.Clear();
            _orderChiTiet.AddRange(pendingOrder);
            await RefreshOrderGridAsync();
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            if (_orderChiTiet.Any())
            {
                // Lưu lịch sử tạm vào HoaDonService
                _hoaDonService.SavePendingOrder(_maBan, _orderChiTiet);

                // Lưu file lịch sử (tùy chọn)
                var historyLines = new List<string>
                {
                    $"Bàn: {_tenBan ?? "Không xác định"} - Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}"
                };
                foreach (var ct in _orderChiTiet)
                {
                    var mon = _menuService.GetMonByIdAsync(ct.MaMon).Result;
                    historyLines.Add($"Món: {mon?.TenMon ?? "ID " + ct.MaMon} - SL: {ct.SoLuong} - Đơn giá: {ct.DonGia:#,##0} - Thành tiền: {ct.ThanhTien:#,##0}");
                }
                string fileName = $"LichSuOrder_Ban{_maBan}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                System.IO.File.WriteAllLines(fileName, historyLines);

                MessageBox.Show($"Đã lưu lịch sử order tạm cho bàn này (file: {fileName})", "Thông báo");
            }

            Close();
        }
    }
}