namespace Cafe.Forms
{
    partial class frmBaoCaoDoanhThu
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvCa = new System.Windows.Forms.DataGridView();
            this.dgvHoaDon = new System.Windows.Forms.DataGridView();
            this.lblTongDoanhThu = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCa
            // 
            this.dgvCa.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCa.Location = new System.Drawing.Point(20, 80);
            this.dgvCa.Name = "dgvCa";
            this.dgvCa.RowTemplate.Height = 30;
            this.dgvCa.Size = new System.Drawing.Size(560, 400);
            this.dgvCa.TabIndex = 0;
            this.dgvCa.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCa.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCa_CellClick);
            // 
            // dgvHoaDon
            // 
            this.dgvHoaDon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHoaDon.Location = new System.Drawing.Point(600, 80);
            this.dgvHoaDon.Name = "dgvHoaDon";
            this.dgvHoaDon.RowTemplate.Height = 30;
            this.dgvHoaDon.Size = new System.Drawing.Size(560, 400);
            this.dgvHoaDon.TabIndex = 1;
            // 
            // lblTongDoanhThu
            // 
            this.lblTongDoanhThu.AutoSize = true;
            this.lblTongDoanhThu.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTongDoanhThu.ForeColor = System.Drawing.Color.Green;
            this.lblTongDoanhThu.Location = new System.Drawing.Point(600, 500);
            this.lblTongDoanhThu.Name = "lblTongDoanhThu";
            this.lblTongDoanhThu.Size = new System.Drawing.Size(300, 41);
            this.lblTongDoanhThu.TabIndex = 2;
            this.lblTongDoanhThu.Text = "Tổng doanh thu ca: 0 VNĐ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(20, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 32);
            this.label1.TabIndex = 3;
            this.label1.Text = "Danh sách ca làm việc";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(600, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 32);
            this.label2.TabIndex = 4;
            this.label2.Text = "Chi tiết hóa đơn trong ca";
            // 
            // frmBaoCaoDoanhThu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 570);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTongDoanhThu);
            this.Controls.Add(this.dgvHoaDon);
            this.Controls.Add(this.dgvCa);
            this.Name = "frmBaoCaoDoanhThu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Báo Cáo Doanh Thu Theo Ca";
            this.Load += new System.EventHandler(this.frmBaoCaoDoanhThu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCa;
        private System.Windows.Forms.DataGridView dgvHoaDon;
        private System.Windows.Forms.Label lblTongDoanhThu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}