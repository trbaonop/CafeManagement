namespace Cafe.UI
{
    partial class frmQuanLyCa : Form
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
            this.btnMoCaMoi = new System.Windows.Forms.Button();
            this.btnDongCa = new System.Windows.Forms.Button();
            this.lblThongBao = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCa)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCa
            // 
            this.dgvCa.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCa.Location = new System.Drawing.Point(20, 80);
            this.dgvCa.Name = "dgvCa";
            this.dgvCa.RowTemplate.Height = 30;
            this.dgvCa.Size = new System.Drawing.Size(900, 400);
            this.dgvCa.TabIndex = 0;
            // 
            // btnMoCaMoi
            // 
            this.btnMoCaMoi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnMoCaMoi.Location = new System.Drawing.Point(20, 20);
            this.btnMoCaMoi.Name = "btnMoCaMoi";
            this.btnMoCaMoi.Size = new System.Drawing.Size(200, 50);
            this.btnMoCaMoi.TabIndex = 1;
            this.btnMoCaMoi.Text = "Mở ca mới";
            this.btnMoCaMoi.UseVisualStyleBackColor = true;
            this.btnMoCaMoi.Click += new System.EventHandler(this.BtnMoCaMoi_Click);
            // 
            // btnDongCa
            // 
            this.btnDongCa.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDongCa.Location = new System.Drawing.Point(240, 20);
            this.btnDongCa.Name = "btnDongCa";
            this.btnDongCa.Size = new System.Drawing.Size(200, 50);
            this.btnDongCa.TabIndex = 2;
            this.btnDongCa.Text = "Đóng ca hiện tại";
            this.btnDongCa.UseVisualStyleBackColor = true;
            this.btnDongCa.Click += new System.EventHandler(this.BtnDongCa_Click);
            // 
            // lblThongBao
            // 
            this.lblThongBao.AutoSize = true;
            this.lblThongBao.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblThongBao.ForeColor = System.Drawing.Color.Blue;
            this.lblThongBao.Location = new System.Drawing.Point(460, 35);
            this.lblThongBao.Name = "lblThongBao";
            this.lblThongBao.Size = new System.Drawing.Size(300, 32);
            this.lblThongBao.TabIndex = 3;
            this.lblThongBao.Text = "Chọn ca để đóng";
            // 
            // FrmQuanLyCa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 500);
            this.Controls.Add(this.lblThongBao);
            this.Controls.Add(this.btnDongCa);
            this.Controls.Add(this.btnMoCaMoi);
            this.Controls.Add(this.dgvCa);
            this.Name = "FrmQuanLyCa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Ca Làm Việc";
            this.Load += new System.EventHandler(this.FrmQuanLyCa_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCa)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCa;
        private System.Windows.Forms.Button btnMoCaMoi;
        private System.Windows.Forms.Button btnDongCa;
        private System.Windows.Forms.Label lblThongBao;
    }
}