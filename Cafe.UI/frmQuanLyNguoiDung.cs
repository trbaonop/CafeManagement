using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection; // Để dùng GetRequiredService
using Cafe.BLL;       
using Cafe.Common;    // CurrentUser
using Cafe.Entity;    // NguoiDung

namespace Cafe.UI
{
    public partial class frmQuanLyNguoiDung : Form
    {
        private readonly NguoiDungService _nguoiDungService;

        /// <summary>
        /// Constructor nhận DI từ container
        /// </summary>
        public frmQuanLyNguoiDung(NguoiDungService nguoiDungService)
        {
            InitializeComponent();
            _nguoiDungService = nguoiDungService ?? throw new ArgumentNullException(nameof(nguoiDungService));
        }

        private async void FrmQuanLyNguoiDung_Load(object sender, EventArgs e)
        {
            // Chỉ Admin mới được vào
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            await LoadNguoiDungAsync();
        }

        private async Task LoadNguoiDungAsync(string keyword = "")
        {
            try
            {
                var users = await _nguoiDungService.GetAllUsersAsync();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    keyword = keyword.ToLower();
                    users = users.Where(u => u.TenDangNhap.ToLower().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                             (u.HoTen != null && u.HoTen.ToLower().Contains(keyword, StringComparison.OrdinalIgnoreCase))).ToList();
                }

                dgvNguoiDung.DataSource = users.Select(u => new
                {
                    u.MaND,
                    u.TenDangNhap,
                    u.HoTen,
                    VaiTro = u.VaiTro?.TenVaiTro ?? "Không có",
                    TrangThai = u.TrangThai ? "Hoạt động" : "Khóa"
                }).ToList();

                dgvNguoiDung.Columns["MaND"].Visible = false;
                dgvNguoiDung.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                dgvNguoiDung.Columns["HoTen"].HeaderText = "Họ tên";
                dgvNguoiDung.Columns["VaiTro"].HeaderText = "Vai trò";
                dgvNguoiDung.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách người dùng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadNguoiDungAsync(txtTimKiem.Text.Trim());
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            await LoadNguoiDungAsync();
        }

        private async void BtnThem_Click(object sender, EventArgs e)
        {
            var frm = Program.ServiceProvider.GetRequiredService<frmNguoiDungEdit>();
            frm.MaND = 0; // 0 = thêm mới
            if (frm.ShowDialog() == DialogResult.OK)
            {
                await LoadNguoiDungAsync();
            }
        }

        private async void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvNguoiDung.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maND = Convert.ToInt32(dgvNguoiDung.SelectedRows[0].Cells["MaND"].Value);

            var frm = Program.ServiceProvider.GetRequiredService<frmNguoiDungEdit>();
            frm.MaND = maND;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                await LoadNguoiDungAsync();
            }
        }

        private async void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNguoiDung.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần khóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maND = Convert.ToInt32(dgvNguoiDung.SelectedRows[0].Cells["MaND"].Value);

            if (MessageBox.Show("Bạn có chắc muốn khóa tài khoản này?\nNgười dùng sẽ không thể đăng nhập.", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _nguoiDungService.KhoaTaiKhoanAsync(maND); // Cần thêm method này trong NguoiDungService
                    MessageBox.Show("Đã khóa tài khoản thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadNguoiDungAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi khóa tài khoản: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvNguoiDung_DoubleClick(object sender, EventArgs e)
        {
            BtnSua_Click(sender, e);
        }
    }
}