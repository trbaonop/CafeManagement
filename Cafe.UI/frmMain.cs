using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection; // Để dùng GetRequiredService
using Cafe.BLL;       // BanService
using Cafe.Common;    // CurrentUser

namespace Cafe.UI
{
    public partial class frmMain : Form
    {
        private readonly BanService _banService;

        public frmMain(BanService banService)
        {
            InitializeComponent();
            _banService = banService ?? throw new ArgumentNullException(nameof(banService));
        }

        private async void FrmMain_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập
            if (!CurrentUser.IsLoggedIn)
            {
                MessageBox.Show("Vui lòng đăng nhập để tiếp tục!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                var frmLogin = Program.ServiceProvider.GetRequiredService<frmLogin>();
                frmLogin.Show();
                return;
            }

            // Hiển thị thông tin người dùng
            toolStripLabelUser.Text = $"Người dùng: {CurrentUser.GetInfo()}";

            // Phân quyền menu
            PhanQuyen();

            // Load dashboard bàn
            await LoadDashboardBanAsync();
        }

        private void PhanQuyen()
        {
            mnuQuanLyMenu.Visible = CurrentUser.IsAdmin;
            mnuQuanLyBan.Visible = CurrentUser.IsAdmin;
            mnuQuanLyNguoiDung.Visible = CurrentUser.IsAdmin;
            mnuQuanLyCa.Visible = CurrentUser.IsAdmin || CurrentUser.IsQuanLy;
            mnuBaoCaoDoanhThu.Visible = CurrentUser.IsAdmin || CurrentUser.IsQuanLy;
        }

        private async Task LoadDashboardBanAsync()
        {
            flowBan.Controls.Clear();

            var bans = await _banService.GetAllBansAsync();

            foreach (var ban in bans)
            {
                var btnBan = new Button
                {
                    Width = 130,
                    Height = 130,
                    Text = $"{ban.TenBan}\n\n{(ban.TrangThai == "Trong" ? "TRỐNG" : "ĐANG PHỤC VỤ")}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = ban.TrangThai == "Trong" ? Color.ForestGreen : Color.Firebrick,
                    FlatStyle = FlatStyle.Flat,
                    Tag = ban.MaBan,
                    Margin = new Padding(10)
                };

                btnBan.FlatAppearance.BorderSize = 3;
                btnBan.FlatAppearance.BorderColor = Color.White;
                btnBan.Click += BtnBan_Click;

                flowBan.Controls.Add(btnBan);
            }
        }

        private void BtnBan_Click(object sender, EventArgs e)
        {
            int maBan = (int)((Button)sender).Tag;

            var frmOrder = Program.ServiceProvider.GetRequiredService<frmOrder>();
            frmOrder.MaBan = maBan;

            frmOrder.ShowDialog();

            // Sau khi đóng form, refresh dashboard bàn
            LoadDashboardBanAsync(); // Không await vì event handler sync, nhưng vẫn chạy async
        }

        // ================== SỰ KIỆN MENU ==================
        private void MnuQuanLyMenu_Click(object sender, EventArgs e)
        {
            var frm = Program.ServiceProvider.GetRequiredService<frmQuanLyMenu>();
            frm.ShowDialog();
        }

        private void MnuQuanLyBan_Click(object sender, EventArgs e)
        {
            var frm = Program.ServiceProvider.GetRequiredService<frmQuanLyBan>();
            frm.ShowDialog();
            LoadDashboardBanAsync(); // Không await vì event handler sync, nhưng vẫn chạy async
        }

        private void MnuQuanLyNguoiDung_Click(object sender, EventArgs e)
        {
            var frm = Program.ServiceProvider.GetRequiredService<frmQuanLyNguoiDung>();
            frm.ShowDialog();
        }

        private void MnuQuanLyCa_Click(object sender, EventArgs e)
        {
            var frm = Program.ServiceProvider.GetRequiredService<frmQuanLyCa>();
            frm.ShowDialog();
        }

        private void MnuBaoCaoDoanhThu_Click(object sender, EventArgs e)
        {
            var frm = Program.ServiceProvider.GetRequiredService<frmBaoCaoDoanhThu>();
            frm.ShowDialog();
        }

        private void MnuDangXuat_Click(object sender, EventArgs e)
        {
            DangXuat();
        }

        private void ToolStripButtonDangXuat_Click(object sender, EventArgs e)
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
                var frmLogin = Program.ServiceProvider.GetRequiredService<frmLogin>();
                frmLogin.Show();
            }
        }
    }
}