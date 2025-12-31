using Cafe.DAL;
using Cafe.Entity;

namespace Cafe.Forms
{
    public partial class frmBanEdit : Form
    {
        private readonly int _maBan;
        private Ban _ban;

        public frmBanEdit(int maBan = 0)
        {
            InitializeComponent();
            _maBan = maBan;
        }

        private async void frmBanEdit_Load(object sender, EventArgs e)
        {
            if (_maBan == 0)
            {
                this.Text = "Thêm bàn mới";
            }
            else
            {
                this.Text = "Sửa tên bàn";

                using var context = new CafeContext();
                _ban = await context.Bans.FindAsync(_maBan);

                if (_ban != null)
                {
                    txtTenBan.Text = _ban.TenBan;
                }
            }
        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenBan.Text))
            {
                MessageBox.Show("Vui lòng nhập tên bàn!");
                return;
            }

            try
            {
                using var context = new CafeContext();

                if (_maBan == 0)
                {
                    // Thêm mới
                    var banMoi = new Ban
                    {
                        TenBan = txtTenBan.Text.Trim(),
                        TrangThai = "Trong"
                    };
                    context.Bans.Add(banMoi);
                    await context.SaveChangesAsync();
                    MessageBox.Show("Thêm bàn thành công!");
                }
                else
                {
                    // Sửa
                    _ban.TenBan = txtTenBan.Text.Trim();
                    context.Bans.Update(_ban);
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