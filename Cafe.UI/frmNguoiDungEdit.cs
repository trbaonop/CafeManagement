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
    public partial class frmNguoiDungEdit : Form
    {
        private readonly int _maND; // 0 = thêm mới
        private NguoiDung _user;

        public frmNguoiDungEdit(int maND = 0)
        {
            InitializeComponent();
            _maND = maND;
        }

        private async void frmNguoiDungEdit_Load(object sender, EventArgs e)
        {
            // Load danh sách vai trò vào ComboBox
            using var context = new CafeContext();
            var vaiTros = await context.VaiTros.ToListAsync();
            cmbVaiTro.DataSource = vaiTros;
            cmbVaiTro.DisplayMember = "TenVaiTro";
            cmbVaiTro.ValueMember = "MaVaiTro";

            if (_maND == 0)
            {
                this.Text = "Thêm người dùng mới";
                chkTrangThai.Checked = true;
                txtMatKhau.Enabled = true; // Bắt buộc nhập mật khẩu khi thêm mới
            }
            else
            {
                this.Text = "Sửa thông tin người dùng";
                txtMatKhau.Enabled = false; // Không cho sửa mật khẩu ở đây (có thể làm form riêng)

                using var ctx = new CafeContext();
                _user = await ctx.NguoiDungs.Include(u => u.VaiTro).FirstOrDefaultAsync(u => u.MaND == _maND);

                if (_user != null)
                {
                    txtTenDangNhap.Text = _user.TenDangNhap;
                    txtTenDangNhap.Enabled = false; // Không cho sửa tên đăng nhập
                    txtHoTen.Text = _user.HoTen ?? "";
                    cmbVaiTro.SelectedValue = _user.MaVaiTro ?? 3; // Mặc định Nhân viên
                    chkTrangThai.Checked = _user.TrangThai;
                }
            }
        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!");
                return;
            }

            if (_maND == 0 && string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cho tài khoản mới!");
                return;
            }

            try
            {
                using var context = new CafeContext();

                if (_maND == 0)
                {
                    // Kiểm tra trùng tên đăng nhập
                    if (await context.NguoiDungs.AnyAsync(u => u.TenDangNhap == txtTenDangNhap.Text.Trim()))
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại!");
                        return;
                    }

                    var userMoi = new NguoiDung
                    {
                        TenDangNhap = txtTenDangNhap.Text.Trim(),
                        MatKhau = txtMatKhau.Text, // Nên hash sau này
                        HoTen = txtHoTen.Text.Trim(),
                        MaVaiTro = (int)cmbVaiTro.SelectedValue,
                        TrangThai = chkTrangThai.Checked
                    };

                    context.NguoiDungs.Add(userMoi);
                    await context.SaveChangesAsync();

                    MessageBox.Show("Thêm người dùng thành công!");
                }
                else
                {
                    _user.HoTen = txtHoTen.Text.Trim();
                    _user.MaVaiTro = (int)cmbVaiTro.SelectedValue;
                    _user.TrangThai = chkTrangThai.Checked;

                    context.NguoiDungs.Update(_user);
                    await context.SaveChangesAsync();

                    MessageBox.Show("Cập nhật thành công!");
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}