using Cafe.BLL;       // AuthService
using Cafe.Common;    // CurrentUser
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Cafe.UI
{
    public partial class frmLogin : Form
    {
        private readonly AuthService _authService;

        public frmLogin(AuthService authService)
        {
            InitializeComponent();
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            txtTenDangNhap.Focus();
        }

        private async void BtnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDN = txtTenDangNhap.Text.Trim();
            string mk = txtMatKhau.Text;

            if (string.IsNullOrWhiteSpace(tenDN) || string.IsNullOrWhiteSpace(mk))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnDangNhap.Enabled = false;

            try
            {
                var user = await _authService.LoginAsync(tenDN, mk);

                if (user == null)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CurrentUser.Login(user.MaND, user.TenDangNhap, user.HoTen, user.VaiTro?.TenVaiTro);

                MessageBox.Show($"Chào mừng {CurrentUser.HoTen}!", "Đăng nhập thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();

                var frmMain = Program.ServiceProvider.GetRequiredService<frmMain>();
                frmMain.ShowDialog();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối database:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDangNhap.Enabled = true;
            }
        }
    }
}