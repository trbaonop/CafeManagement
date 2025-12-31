namespace Cafe.Forms
{
    partial class frmBanEdit
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
            this.lblTenBan = new System.Windows.Forms.Label();
            this.txtTenBan = new System.Windows.Forms.TextBox();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTenBan
            // 
            this.lblTenBan.AutoSize = true;
            this.lblTenBan.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblTenBan.Location = new System.Drawing.Point(50, 60);
            this.lblTenBan.Name = "lblTenBan";
            this.lblTenBan.Size = new System.Drawing.Size(120, 32);
            this.lblTenBan.TabIndex = 0;
            this.lblTenBan.Text = "Tên bàn:";
            // 
            // txtTenBan
            // 
            this.txtTenBan.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtTenBan.Location = new System.Drawing.Point(200, 57);
            this.txtTenBan.Name = "txtTenBan";
            this.txtTenBan.Size = new System.Drawing.Size(350, 39);
            this.txtTenBan.TabIndex = 1;
            // 
            // btnLuu
            // 
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnLuu.Location = new System.Drawing.Point(200, 140);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(160, 60);
            this.btnLuu.TabIndex = 2;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnHuy.Location = new System.Drawing.Point(390, 140);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(160, 60);
            this.btnHuy.TabIndex = 3;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // frmBanEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 250);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.txtTenBan);
            this.Controls.Add(this.lblTenBan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBanEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm / Sửa Bàn";
            this.Load += new System.EventHandler(this.frmBanEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTenBan;
        private System.Windows.Forms.TextBox txtTenBan;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}