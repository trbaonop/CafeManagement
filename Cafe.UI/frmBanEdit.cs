using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Cafe.DAL;       // CafeContext
using Cafe.Entity;    // Ban

namespace Cafe.UI
{
    public partial class frmBanEdit : Form
    {
        private readonly CafeContext _context;
        private Ban? _ban;

        /// <summary>
        /// Property để truyền MaBan từ form cha (frmQuanLyBan)
        /// </summary>
        public int MaBan { get; set; } = 0; // 0 = thêm mới, >0 = sửa

        /// <summary>
        /// Constructor nhận DI từ container
        /// </summary>
        public frmBanEdit(CafeContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private async void FrmBanEdit_Load(object sender, EventArgs e)
        {
            if (MaBan == 0)
            {
                this.Text = "Thêm bàn mới";
            }
            else
            {
                this.Text = "Sửa tên bàn";

                _ban = await _context.Bans.FindAsync(MaBan);

                if (_ban == null)
                {
                    MessageBox.Show("Không tìm thấy bàn để sửa!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                txtTenBan.Text = _ban.TenBan;
            }
        }

        private async void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenBan.Text))
            {
                MessageBox.Show("Vui lòng nhập tên bàn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLuu.Enabled = false;

            try
            {
                if (MaBan == 0)
                {
                    // Thêm mới
                    var banMoi = new Ban
                    {
                        TenBan = txtTenBan.Text.Trim(),
                        TrangThai = "Trong"
                    };

                    _context.Bans.Add(banMoi);
                }
                else
                {
                    // Sửa
                    if (_ban == null)
                    {
                        MessageBox.Show("Không tìm thấy bàn để sửa!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _ban.TenBan = txtTenBan.Text.Trim();
                    _context.Bans.Update(_ban);
                }

                await _context.SaveChangesAsync();

                MessageBox.Show(MaBan == 0 ? "Thêm bàn thành công!" : "Cập nhật thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu bàn: " + ex.Message, "Lỗi",
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