using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Cafe.Common;
using Cafe.DAL;
using Cafe.Entity;

namespace Cafe.Forms
{
    public partial class frmQuanLyBan : Form
    {
        public frmQuanLyBan()
        {
            InitializeComponent();
        }

        private async void frmQuanLyBan_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            await LoadBan();
        }

        private async Task LoadBan(string keyword = "")
        {
            using var context = new CafeContext();
            var query = context.Bans.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(b => b.TenBan.ToLower().Contains(keyword));
            }

            var bans = await query.OrderBy(b => b.MaBan).ToListAsync();

            dgvBan.DataSource = bans.Select(b => new
            {
                b.MaBan,
                b.TenBan,
                TrangThai = b.TrangThai == "Trong" ? "Trống" : "Đang phục vụ"
            }).ToList();

            dgvBan.Columns["MaBan"].Visible = false;
            dgvBan.Columns["TenBan"].HeaderText = "Tên bàn";
            dgvBan.Columns["TrangThai"].HeaderText = "Trạng thái";
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadBan(txtTimKiem.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            await LoadBan();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var frm = new frmBanEdit(0); // 0 = thêm mới
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadBan();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvBan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bàn cần sửa!");
                return;
            }

            int maBan = Convert.ToInt32(dgvBan.SelectedRows[0].Cells["MaBan"].Value);
            var frm = new frmBanEdit(maBan);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadBan();
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvBan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bàn cần xóa!");
                return;
            }

            int maBan = Convert.ToInt32(dgvBan.SelectedRows[0].Cells["MaBan"].Value);
            string tenBan = dgvBan.SelectedRows[0].Cells["TenBan"].Value.ToString();

            if (MessageBox.Show($"Bạn có chắc muốn xóa bàn {tenBan}?\nTất cả hóa đơn liên quan sẽ bị xóa!", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using var context = new CafeContext();
                var ban = await context.Bans.FindAsync(maBan);
                if (ban != null)
                {
                    context.Bans.Remove(ban);
                    await context.SaveChangesAsync();
                }

                await LoadBan();
            }
        }

        private void dgvBan_DoubleClick(object sender, EventArgs e)
        {
            btnSua_Click(sender, e);
        }
    }
}