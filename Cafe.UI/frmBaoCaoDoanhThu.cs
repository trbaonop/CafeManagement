using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml; // EPPlus
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Cafe.BLL;
using Cafe.Common;
using Cafe.Entity;

namespace Cafe.UI
{
    public partial class frmBaoCaoDoanhThu : Form
    {
        private readonly CaService _caService;
        private readonly HoaDonService _hoaDonService;

        public frmBaoCaoDoanhThu(CaService caService, HoaDonService hoaDonService)
        {
            InitializeComponent();
            _caService = caService ?? throw new ArgumentNullException(nameof(caService));
            _hoaDonService = hoaDonService ?? throw new ArgumentNullException(nameof(hoaDonService));
        }

        private async void FrmBaoCaoDoanhThu_Load(object sender, EventArgs e)
        {
            if (!(CurrentUser.IsAdmin || CurrentUser.IsQuanLy))
            {
                MessageBox.Show("Bạn không có quyền truy cập báo cáo doanh thu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
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

                dgvCa.Columns["MaCa"].HeaderText = "Mã ca";
                dgvCa.Columns["GioBatDau"].HeaderText = "Giờ bắt đầu";
                dgvCa.Columns["GioKetThuc"].HeaderText = "Giờ kết thúc";
                dgvCa.Columns["DoanhThu"].HeaderText = "Doanh thu tạm (DB)";
                dgvCa.ClearSelection();

                dgvHoaDon.DataSource = null;
                lblTongDoanhThu.Text = "Tổng doanh thu ca: 0 VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách ca: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DgvCa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int maCa = Convert.ToInt32(dgvCa.Rows[e.RowIndex].Cells["MaCa"].Value);

            var hoaDons = await _hoaDonService.GetHoaDonByCaAsync(maCa);
            var hoaDonPaid = hoaDons.Where(h => h.TrangThai == "Paid").ToList();

            decimal tongDoanhThu = hoaDonPaid.Sum(h => h.TongTien);

            dgvHoaDon.DataSource = hoaDonPaid.Select(h => new
            {
                h.MaHD,
                Ban = h.Ban?.TenBan ?? "Không xác định",
                h.NgayLap,
                TongTien = h.TongTien.ToString("#,##0 VNĐ")
            }).ToList();

            lblTongDoanhThu.Text = $"Tổng doanh thu ca: {tongDoanhThu:#,##0} VNĐ";
        }

        private void BtnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dgvCa.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu ca để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Lưu báo cáo doanh thu theo ca",
                FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                // BƯỚC BẮT BUỘC: Set LicenseContext NGAY ĐÂY, TRƯỚC KHI TẠO PACKAGE
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Bây giờ mới tạo package
                using var package = new ExcelPackage();

                var worksheet = package.Workbook.Worksheets.Add("Doanh Thu Theo Ca");

                // Tiêu đề báo cáo
                worksheet.Cells[1, 1].Value = "BÁO CÁO DOANH THU THEO CA";
                worksheet.Cells[1, 1, 1, 5].Merge = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Header cột
                worksheet.Cells[3, 1].Value = "Mã Ca";
                worksheet.Cells[3, 2].Value = "Giờ Bắt Đầu";
                worksheet.Cells[3, 3].Value = "Giờ Kết Thúc";
                worksheet.Cells[3, 4].Value = "Doanh Thu";
                

                using var headerRange = worksheet.Cells[3, 1, 3, 5];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Xuất dữ liệu từ dgvCa
                int row = 4;
                foreach (DataGridViewRow dgvRow in dgvCa.Rows)
                {
                    if (dgvRow.IsNewRow) continue;

                    worksheet.Cells[row, 1].Value = dgvRow.Cells["MaCa"].Value;
                    worksheet.Cells[row, 2].Value = dgvRow.Cells["GioBatDau"].Value;
                    worksheet.Cells[row, 3].Value = dgvRow.Cells["GioKetThuc"].Value;
                    worksheet.Cells[row, 4].Value = dgvRow.Cells["DoanhThu"].Value;


                    // Định dạng tiền tệ
                    worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0 VNĐ";

                    row++;
                }

                // Tự động fit cột
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Lưu file
                var file = new FileInfo(saveFileDialog.FileName);
                package.SaveAs(file);

                MessageBox.Show($"Đã xuất báo cáo thành công!\nFile: {file.FullName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở file Excel (tùy chọn)
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(file.FullName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}