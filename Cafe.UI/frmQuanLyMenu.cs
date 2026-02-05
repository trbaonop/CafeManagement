using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection; // Để dùng GetRequiredService
using Cafe.BLL;       // MenuService
using Cafe.Common;    // CurrentUser
using Cafe.Entity;    // Menu

namespace Cafe.UI
{
    public partial class frmQuanLyMenu : Form
    {
        private readonly MenuService _menuService;

        /// <summary>
        /// Constructor nhận DI từ container
        /// </summary>
        public frmQuanLyMenu(MenuService menuService)
        {
            InitializeComponent();
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
        }

        private async void FrmQuanLyMenu_Load(object sender, EventArgs e)
        {
            // Chỉ Admin mới được vào form này
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            await LoadMenuAsync();
        }

        private async Task LoadMenuAsync(string keyword = "")
        {
            try
            {
                var menus = await _menuService.GetAllMenusAsync(onlyActive: false); // Hiển thị cả món ngưng bán

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    keyword = keyword.ToLower();
                    menus = menus.Where(m => m.TenMon.ToLower().Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách món: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadMenuAsync(txtTimKiem.Text.Trim());
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            await LoadMenuAsync();
        }

        private async void BtnThem_Click(object sender, EventArgs e)
        {
            var frm = Program.ServiceProvider.GetRequiredService<frmMonEdit>();
            frm.MaMon = 0; // 0 = thêm mới
            if (frm.ShowDialog() == DialogResult.OK)
            {
                await LoadMenuAsync();
            }
        }

        private async void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maMon = Convert.ToInt32(dgvMenu.SelectedRows[0].Cells["MaMon"].Value);

            var frm = Program.ServiceProvider.GetRequiredService<frmMonEdit>();
            frm.MaMon = maMon;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                await LoadMenuAsync();
            }
        }

        private async void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món cần ngưng bán!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maMon = Convert.ToInt32(dgvMenu.SelectedRows[0].Cells["MaMon"].Value);

            if (MessageBox.Show("Bạn có chắc muốn ngưng bán món này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _menuService.DeleteMonAsync(maMon);
                    await LoadMenuAsync();
                    MessageBox.Show("Đã ngưng bán món thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi ngưng bán: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvMenu_DoubleClick(object sender, EventArgs e)
        {
            BtnSua_Click(sender, e);
        }
    }
}