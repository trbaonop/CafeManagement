using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cafe.BLL;
using Cafe.Common;

namespace Cafe.Forms
{
    public partial class frmMain : Form
    {
        private readonly BanService _banService = new BanService();

        public frmMain()
        {
            InitializeComponent();
        }

        private async void frmMain_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoggedIn)
            {
                MessageBox.Show("Vui lòng đăng nhập để tiếp tục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                new frmLogin().Show();
                return;
            }

            toolStripLabelUser.Text = $"Người dùng: {CurrentUser.GetInfo()}";

            PhanQuyen();

            await LoadDashboardBan();
        }

        private void PhanQuyen()
        {
            // Chỉ Admin mới được quản lý menu, bàn, người dùng
            mnuQuanLyMenu.Visible = CurrentUser.IsAdmin;
            mnuQuanLyBan.Visible = CurrentUser.IsAdmin;
            mnuQuanLyNguoiDung.Visible = CurrentUser.IsAdmin;

            // Quản lý và Admin mới xem báo cáo
            mnuBaoCaoDoanhThu.Visible = CurrentUser.IsAdmin || CurrentUser.IsQuanLy;
        }

        private async Task LoadDashboardBan()
        {
            flowBan.Controls.Clear();

            var bans = await _banService.GetAllBansAsync();

            foreach (var ban in bans)
            {
                Button btnBan = new Button
                {
                    Width = 130,
                    Height = 130,
                    Text = $"{ban.TenBan}\n\n{(ban.TrangThai == "Trong" ? "TRỐNG" : "ĐANG PHỤC VỤ")}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = ban.TrangThai == "Trong" ? Color.ForestGreen : Color.Firebrick,
                    FlatStyle = FlatStyle.Flat,
                    Tag = ban.MaBan
                };

                btnBan.FlatAppearance.BorderSize = 2;
                btnBan.FlatAppearance.BorderColor = Color.White;

                btnBan.Click += BtnBan_Click;

                flowBan.Controls.Add(btnBan);
            }
        }

        private void BtnBan_Click(object sender, EventArgs e)
        {
            int maBan = (int)((Button)sender).Tag;
            var frmOrder = new frmOrder(maBan);
            frmOrder.ShowDialog();

            // Refresh dashboard sau khi order/thanh toán
            LoadDashboardBan();
        }

        // === SỰ KIỆN CLICK CHO CÁC MENU ===

        private void mnuQuanLyMenu_Click(object sender, EventArgs e)
        {
            new frmQuanLyMenu().ShowDialog();
        }

        private void mnuQuanLyBan_Click(object sender, EventArgs e)  // <--- THÊM DÒNG NÀY
        {
            new frmQuanLyBan().ShowDialog();
            LoadDashboardBan(); // Refresh lại bàn nếu có thay đổi
        }

        private void mnuQuanLyNguoiDung_Click(object sender, EventArgs e)
        {
            new frmQuanLyNguoiDung().ShowDialog();
        }

        private void mnuBaoCaoDoanhThu_Click(object sender, EventArgs e)
        {
            new frmBaoCaoDoanhThu().ShowDialog();
        }

        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            DangXuat();
        }

        private void toolStripButtonDangXuat_Click(object sender, EventArgs e)
        {
            DangXuat();
        }

        private void DangXuat()
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CurrentUser.Logout();
                this.Close();
                new frmLogin().Show();
            }
        }
    }
}