using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Cafe.DAL;       // CafeContext
using Cafe.Entity;    // NguoiDung, VaiTro

namespace Cafe.UI
{
    public partial class frmNguoiDungEdit : Form
    {
        private readonly CafeContext _context;
        private NguoiDung? _user;

        /// <summary>
        /// Property để truyền MaND từ form cha (frmQuanLyNguoiDung)
        /// </summary>
        public int MaND { get; set; } = 0; // 0 = thêm mới, >0 = sửa

        /// <summary>
        /// Constructor nhận DI từ container
        /// </summary>
        public frmNguoiDungEdit(CafeContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private async void FrmNguoiDungEdit_Load(object sender, EventArgs e)
        {
            // Load danh sách vai trò vào ComboBox
            var vaiTros = await _context.VaiTros.ToListAsync();
            cmbVaiTro.DataSource = vaiTros;
            cmbVaiTro.DisplayMember = "TenVaiTro";
            cmbVaiTro.ValueMember = "MaVaiTro";

            if (MaND == 0)
            {
                this.Text = "Thêm người dùng mới";
                chkTrangThai.Checked = true;
                txtMatKhau.Enabled = true; // Bắt buộc nhập mật khẩu khi thêm mới
            }
            else
            {
                this.Text = "Sửa thông tin người dùng";
                txtMatKhau.Enabled = false; // Không cho sửa mật khẩu ở đây

                _user = await _context.NguoiDungs
                    .Include(u => u.VaiTro)
                    .FirstOrDefaultAsync(u => u.MaND == MaND);

                if (_user == null)
                {
                    MessageBox.Show("Không tìm thấy người dùng để sửa!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                txtTenDangNhap.Text = _user.TenDangNhap;
                txtTenDangNhap.Enabled = false; // Không cho sửa tên đăng nhập
                txtHoTen.Text = _user.HoTen ?? "";
                cmbVaiTro.SelectedValue = _user.MaVaiTro ?? 3; // Mặc định Nhân viên
                chkTrangThai.Checked = _user.TrangThai;
            }
        }

        private async void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MaND == 0 && string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cho tài khoản mới!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLuu.Enabled = false;

            try
            {
                if (MaND == 0)
                {
                    // Kiểm tra trùng tên đăng nhập
                    if (await _context.NguoiDungs.AnyAsync(u => u.TenDangNhap == txtTenDangNhap.Text.Trim()))
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var userMoi = new NguoiDung
                    {
                        TenDangNhap = txtTenDangNhap.Text.Trim(),
                        MatKhau = txtMatKhau.Text, // Nên hash sau này (BCrypt)
                        HoTen = txtHoTen.Text.Trim(),
                        MaVaiTro = (int)cmbVaiTro.SelectedValue,
                        TrangThai = chkTrangThai.Checked
                    };

                    _context.NguoiDungs.Add(userMoi);
                }
                else
                {
                    // Sửa
                    if (_user == null)
                    {
                        MessageBox.Show("Không tìm thấy người dùng để sửa!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _user.HoTen = txtHoTen.Text.Trim();
                    _user.MaVaiTro = (int)cmbVaiTro.SelectedValue;
                    _user.TrangThai = chkTrangThai.Checked;

                    _context.NguoiDungs.Update(_user);
                }

                await _context.SaveChangesAsync();

                MessageBox.Show(MaND == 0 ? "Thêm người dùng thành công!" : "Cập nhật thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu người dùng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLuu.Enabled = true;
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}