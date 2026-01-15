using Cafe.BLL;       // BanService
using Cafe.Common;    // CurrentUser
using Cafe.Entity;    // Ban
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Cafe.UI
{
    public partial class frmQuanLyBan : Form
    {
        private readonly BanService _banService;

        /// <summary>
        /// Constructor nhận DI từ container
        /// </summary>
        public frmQuanLyBan(BanService banService)
        {
            InitializeComponent();
            _banService = banService ?? throw new ArgumentNullException(nameof(banService));
        }

        private async void FrmQuanLyBan_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            await LoadBanAsync();
        }

        private async Task LoadBanAsync(string keyword = "")
        {
            var bans = await _banService.GetAllBansAsync();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                bans = bans.Where(b => b.TenBan.ToLower().Contains(keyword)).ToList();
            }

            dgvBan.DataSource = bans.OrderBy(b => b.MaBan).Select(b => new
            {
                b.MaBan,
                b.TenBan,
                TrangThai = b.TrangThai == "Trong" ? "Trống" : "Đang phục vụ"
            }).ToList();

            dgvBan.Columns["MaBan"].Visible = false;
            dgvBan.Columns["TenBan"].HeaderText = "Tên bàn";
            dgvBan.Columns["TrangThai"].HeaderText = "Trạng thái";
        }

        private async void BtnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadBanAsync(txtTimKiem.Text.Trim());
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            await LoadBanAsync();
        }

        private async void BtnThem_Click(object sender, EventArgs e)
        {
            var frm = Program.ServiceProvider.GetRequiredService<frmBanEdit>();
            frm.MaBan = 0; // 0 = thêm mới
            if (frm.ShowDialog() == DialogResult.OK)
            {
                await LoadBanAsync();
            }
        }

        private async void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvBan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bàn cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maBan = Convert.ToInt32(dgvBan.SelectedRows[0].Cells["MaBan"].Value);

            var frm = Program.ServiceProvider.GetRequiredService<frmBanEdit>();
            frm.MaBan = maBan;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                await LoadBanAsync();
            }
        }

        private async void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvBan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bàn cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maBan = Convert.ToInt32(dgvBan.SelectedRows[0].Cells["MaBan"].Value);
            string tenBan = dgvBan.SelectedRows[0].Cells["TenBan"].Value?.ToString() ?? "bàn này";

            if (MessageBox.Show($"Bạn có chắc muốn xóa {tenBan}?\nTất cả hóa đơn liên quan sẽ bị xóa!", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                await _banService.DeleteBanAsync(maBan);
                await LoadBanAsync();
            }
        }

        private void DgvBan_DoubleClick(object sender, EventArgs e)
        {
            BtnSua_Click(sender, e);
        }
    }
}