using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cafe.BLL;
using Cafe.Entity;

namespace Cafe.Forms
{
    public partial class frmMonEdit : Form
    {
        private readonly int _maMon; // 0 = thêm mới, >0 = sửa
        private readonly MenuService _menuService = new MenuService();
        private Menu _monHienTai;

        public frmMonEdit(int maMon = 0)
        {
            InitializeComponent();
            _maMon = maMon;
        }

        private async void frmMonEdit_Load(object sender, EventArgs e)
        {
            if (_maMon == 0)
            {
                this.Text = "Thêm món mới";
                chkTrangThai.Checked = true;
            }
            else
            {
                this.Text = "Sửa thông tin món";

                _monHienTai = await _menuService.GetMonByIdAsync(_maMon);
                if (_monHienTai == null)
                {
                    MessageBox.Show("Không tìm thấy món!");
                    this.Close();
                    return;
                }

                txtTenMon.Text = _monHienTai.TenMon;
                nudDonGia.Value = _monHienTai.DonGia;
                chkTrangThai.Checked = _monHienTai.TrangThai;
            }
        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Vui lòng nhập tên món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nudDonGia.Value <= 0)
            {
                MessageBox.Show("Đơn giá phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_maMon == 0)
                {
                    // Thêm mới
                    var monMoi = new Menu
                    {
                        TenMon = txtTenMon.Text.Trim(),
                        DonGia = (int)nudDonGia.Value,
                        TrangThai = chkTrangThai.Checked
                    };

                    await _menuService.AddMonAsync(monMoi);
                    MessageBox.Show("Thêm món thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Sửa
                    _monHienTai.TenMon = txtTenMon.Text.Trim();
                    _monHienTai.DonGia = (int)nudDonGia.Value;
                    _monHienTai.TrangThai = chkTrangThai.Checked;

                    await _menuService.UpdateMonAsync(_monHienTai);
                    MessageBox.Show("Cập nhật món thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}