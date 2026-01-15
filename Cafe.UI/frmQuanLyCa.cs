using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cafe.BLL;       // CaService
using Cafe.Common;    // CurrentUser
using Cafe.Entity;    // Ca

namespace Cafe.UI
{
    public partial class frmQuanLyCa : Form
    {
        private readonly CaService _caService;

        /// <summary>
        /// Constructor nhận DI từ container
        /// </summary>
        public frmQuanLyCa(CaService caService)
        {
            InitializeComponent();
            _caService = caService ?? throw new ArgumentNullException(nameof(caService));
        }

        private async void FrmQuanLyCa_Load(object sender, EventArgs e)
        {
            if (!(CurrentUser.IsAdmin || CurrentUser.IsQuanLy))
            {
                MessageBox.Show("Bạn không có quyền truy cập quản lý ca!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            await LoadDanhSachCaAsync();
        }

        private async Task LoadDanhSachCaAsync()
        {
            try
            {
                var cas = await _caService.GetAllCaAsync();

                dgvCa.DataSource = cas.Select(c => new
                {
                    c.MaCa,
                    GioBatDau = c.GioBD.ToString("dd/MM/yyyy HH:mm"),
                    GioKetThuc = c.GioKT?.ToString("dd/MM/yyyy HH:mm") ?? "Đang mở",
                    DoanhThu = c.DoanhThu.ToString("#,##0 VNĐ")
                }).ToList();

                dgvCa.Columns["MaCa"].Visible = false;
                dgvCa.Columns["GioBatDau"].HeaderText = "Giờ bắt đầu";
                dgvCa.Columns["GioKetThuc"].HeaderText = "Giờ kết thúc";
                dgvCa.Columns["DoanhThu"].HeaderText = "Doanh thu tạm (DB)";

                dgvCa.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách ca: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnMoCaMoi_Click(object sender, EventArgs e)
        {
            btnMoCaMoi.Enabled = false;

            try
            {
                var caMoi = await _caService.TaoCaMoiAsync();
                MessageBox.Show($"Đã mở ca mới lúc {caMoi.GioBD:HH:mm dd/MM/yyyy}", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDanhSachCaAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở ca mới: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnMoCaMoi.Enabled = true;
            }
        }

        private async void BtnDongCa_Click(object sender, EventArgs e)
        {
            if (dgvCa.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ca cần đóng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maCa = Convert.ToInt32(dgvCa.SelectedRows[0].Cells["MaCa"].Value);

            if (MessageBox.Show("Bạn có chắc muốn đóng ca này?\nDoanh thu sẽ được tính từ các hóa đơn đã thanh toán.", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                btnDongCa.Enabled = false;

                try
                {
                    await _caService.DongCaAsync(maCa);

                    // Tính lại doanh thu thực tế từ hóa đơn
                    decimal doanhThuThuc = await _caService.GetDoanhThuCaAsync(maCa);

                    MessageBox.Show($"Đã đóng ca thành công!\nDoanh thu ca: {doanhThuThuc:#,##0} VNĐ", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await LoadDanhSachCaAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi đóng ca: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnDongCa.Enabled = true;
                }
            }
        }
    }
}