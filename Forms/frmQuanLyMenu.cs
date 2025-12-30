using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TEST.BLL;
using TEST.Common;
using TEST.Entity;

namespace TEST.Forms
{
    public partial class frmQuanLyMenu : Form
    {
        private readonly MenuService _menuService = new MenuService();

        public frmQuanLyMenu()
        {
            InitializeComponent();
        }

        private async void frmQuanLyMenu_Load(object sender, EventArgs e)
        {
            // Chỉ Admin mới được vào form này
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            await LoadMenu();
        }

        private async Task LoadMenu(string keyword = "")
        {
            var menus = await _menuService.GetAllMenusAsync(onlyActive: false); // Hiển thị cả món ngưng bán

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                menus = menus.Where(m => m.TenMon.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            dgvMenu.DataSource = menus.Select(m => new
            {
                m.MaMon,
                m.TenMon,
                DonGia = m.DonGia.ToString("#,##0 VNĐ"),
                TrangThai = m.TrangThai ? "Đang bán" : "Ngưng bán"
            }).ToList();

            dgvMenu.Columns["MaMon"].HeaderText = "Mã món";
            dgvMenu.Columns["TenMon"].HeaderText = "Tên món";
            dgvMenu.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvMenu.Columns["TrangThai"].HeaderText = "Trạng thái";
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadMenu(txtTimKiem.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            await LoadMenu();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var frm = new frmMonEdit(0); // 0 = thêm mới
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadMenu();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món cần sửa!");
                return;
            }

            int maMon = Convert.ToInt32(dgvMenu.SelectedRows[0].Cells["MaMon"].Value);
            var frm = new frmMonEdit(maMon);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadMenu();
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món cần ngưng bán!");
                return;
            }

            int maMon = Convert.ToInt32(dgvMenu.SelectedRows[0].Cells["MaMon"].Value);

            if (MessageBox.Show("Bạn có chắc muốn ngưng bán món này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                await _menuService.DeleteMonAsync(maMon);
                await LoadMenu();
            }
        }

        private void dgvMenu_DoubleClick(object sender, EventArgs e)
        {
            btnSua_Click(sender, e);
        }
    }
}