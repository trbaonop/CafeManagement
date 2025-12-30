using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using TEST.Common;
using TEST.DAL;
using TEST.Entity;

namespace TEST.Forms
{
    public partial class frmQuanLyNguoiDung : Form
    {
        public frmQuanLyNguoiDung()
        {
            InitializeComponent();
        }

        private async void frmQuanLyNguoiDung_Load(object sender, EventArgs e)
        {
            // Chỉ Admin mới được vào
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            await LoadNguoiDung();
        }

        private async Task LoadNguoiDung(string keyword = "")
        {
            using var context = new CafeContext();
            var query = context.NguoiDungs
                .Include(u => u.VaiTro)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(u => u.TenDangNhap.ToLower().Contains(keyword) ||
                                         (u.HoTen != null && u.HoTen.ToLower().Contains(keyword)));
            }

            var users = await query.ToListAsync();

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

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadNguoiDung(txtTimKiem.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            await LoadNguoiDung();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var frm = new frmNguoiDungEdit(0); // 0 = thêm mới
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadNguoiDung();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNguoiDung.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần sửa!");
                return;
            }

            int maND = Convert.ToInt32(dgvNguoiDung.SelectedRows[0].Cells["MaND"].Value);
            var frm = new frmNguoiDungEdit(maND);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadNguoiDung();
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNguoiDung.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần khóa!");
                return;
            }

            int maND = Convert.ToInt32(dgvNguoiDung.SelectedRows[0].Cells["MaND"].Value);

            if (MessageBox.Show("Bạn có chắc muốn khóa tài khoản này?\nNgười dùng sẽ không thể đăng nhập.", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using var context = new CafeContext();
                var user = await context.NguoiDungs.FindAsync(maND);
                if (user != null)
                {
                    user.TrangThai = false;
                    await context.SaveChangesAsync();
                }

                await LoadNguoiDung();
            }
        }

        private void dgvNguoiDung_DoubleClick(object sender, EventArgs e)
        {
            btnSua_Click(sender, e);
        }
    }
}