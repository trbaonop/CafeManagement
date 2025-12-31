using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cafe.BLL;
using Cafe.Common;

namespace Cafe.Forms
{
    public partial class frmLogin : Form
    {
        private readonly AuthService _authService = new AuthService();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtTenDangNhap.Focus();
        }

        private async void btnDangNhap_Click(object sender, EventArgs e)
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

                // Lưu thông tin người dùng hiện tại
                CurrentUser.Login(user.MaND, user.TenDangNhap, user.HoTen, user.VaiTro?.TenVaiTro);

                MessageBox.Show($"Chào mừng {CurrentUser.HoTen}!\nVai trò: {CurrentUser.TenVaiTro}",
                    "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                new frmMain().ShowDialog();
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