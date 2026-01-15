using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cafe.BLL;       // MenuService
using Cafe.Entity;    // Menu

namespace Cafe.UI
{
    public partial class frmMonEdit : Form
    {
        private readonly MenuService _menuService;
        private Menu? _monHienTai;

        /// <summary>
        /// Property để truyền MaMon từ form cha (frmQuanLyMenu)
        /// </summary>
        public int MaMon { get; set; } = 0; // 0 = thêm mới, >0 = sửa

        /// <summary>
        /// Constructor nhận DI từ container
        /// </summary>
        public frmMonEdit(MenuService menuService)
        {
            InitializeComponent();
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
        }

        private async void FrmMonEdit_Load(object sender, EventArgs e)
        {
            if (MaMon == 0)
            {
                this.Text = "Thêm món mới";
                chkTrangThai.Checked = true;
            }
            else
            {
                this.Text = "Sửa thông tin món";

                _monHienTai = await _menuService.GetMonByIdAsync(MaMon);

                if (_monHienTai == null)
                {
                    MessageBox.Show("Không tìm thấy món để sửa!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                txtTenMon.Text = _monHienTai.TenMon;
                nudDonGia.Value = _monHienTai.DonGia;
                chkTrangThai.Checked = _monHienTai.TrangThai;
            }
        }

        private async void BtnLuu_Click(object sender, EventArgs e)
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

            btnLuu.Enabled = false;

            try
            {
                if (MaMon == 0)
                {
                    // Thêm mới
                    var monMoi = new Menu
                    {
                        TenMon = txtTenMon.Text.Trim(),
                        DonGia = (int)nudDonGia.Value,  // Ép kiểu
                        TrangThai = chkTrangThai.Checked
                    };

                    await _menuService.AddMonAsync(monMoi);
                    MessageBox.Show("Thêm món thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Sửa
                    if (_monHienTai == null)
                    {
                        MessageBox.Show("Không tìm thấy món để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _monHienTai.TenMon = txtTenMon.Text.Trim();
                    _monHienTai.DonGia = (int)nudDonGia.Value;  // Ép kiểu
                    _monHienTai.TrangThai = chkTrangThai.Checked;

                    await _menuService.UpdateMonAsync(_monHienTai);
                    MessageBox.Show("Cập nhật món thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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