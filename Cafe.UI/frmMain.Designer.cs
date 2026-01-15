namespace Cafe.UI
{
    partial class frmMain : Form
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuQuanLy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQuanLyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQuanLyBan = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQuanLyNguoiDung = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQuanLyCa = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBaoCao = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBaoCaoDoanhThu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHeThong = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDangXuat = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelUser = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonDangXuat = new System.Windows.Forms.ToolStripButton();
            this.flowBan = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuQuanLy,
            this.mnuBaoCao,
            this.mnuHeThong});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1200, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuQuanLy
            // 
            this.mnuQuanLy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuQuanLyMenu,
            this.mnuQuanLyBan,
            this.mnuQuanLyNguoiDung,
            this.mnuQuanLyCa});
            this.mnuQuanLy.Name = "mnuQuanLy";
            this.mnuQuanLy.Size = new System.Drawing.Size(80, 24);
            this.mnuQuanLy.Text = "Quản lý";
            // 
            // mnuQuanLyMenu
            // 
            this.mnuQuanLyMenu.Name = "mnuQuanLyMenu";
            this.mnuQuanLyMenu.Size = new System.Drawing.Size(220, 26);
            this.mnuQuanLyMenu.Text = "Danh mục đồ uống";
            this.mnuQuanLyMenu.Click += new System.EventHandler(this.MnuQuanLyMenu_Click);
            // 
            // mnuQuanLyBan
            // 
            this.mnuQuanLyBan.Name = "mnuQuanLyBan";
            this.mnuQuanLyBan.Size = new System.Drawing.Size(220, 26);
            this.mnuQuanLyBan.Text = "Quản lý bàn";
            this.mnuQuanLyBan.Click += new System.EventHandler(this.MnuQuanLyBan_Click);
            // 
            // mnuQuanLyNguoiDung
            // 
            this.mnuQuanLyNguoiDung.Name = "mnuQuanLyNguoiDung";
            this.mnuQuanLyNguoiDung.Size = new System.Drawing.Size(220, 26);
            this.mnuQuanLyNguoiDung.Text = "Quản lý người dùng";
            this.mnuQuanLyNguoiDung.Click += new System.EventHandler(this.MnuQuanLyNguoiDung_Click);
            // 
            // mnuQuanLyCa
            // 
            this.mnuQuanLyCa.Name = "mnuQuanLyCa";
            this.mnuQuanLyCa.Size = new System.Drawing.Size(220, 26);
            this.mnuQuanLyCa.Text = "Quản lý ca làm việc";
            this.mnuQuanLyCa.Click += new System.EventHandler(this.MnuQuanLyCa_Click);
            // 
            // mnuBaoCao
            // 
            this.mnuBaoCao.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBaoCaoDoanhThu});
            this.mnuBaoCao.Name = "mnuBaoCao";
            this.mnuBaoCao.Size = new System.Drawing.Size(80, 24);
            this.mnuBaoCao.Text = "Báo cáo";
            // 
            // mnuBaoCaoDoanhThu
            // 
            this.mnuBaoCaoDoanhThu.Name = "mnuBaoCaoDoanhThu";
            this.mnuBaoCaoDoanhThu.Size = new System.Drawing.Size(220, 26);
            this.mnuBaoCaoDoanhThu.Text = "Doanh thu theo ca";
            this.mnuBaoCaoDoanhThu.Click += new System.EventHandler(this.MnuBaoCaoDoanhThu_Click);
            // 
            // mnuHeThong
            // 
            this.mnuHeThong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDangXuat});
            this.mnuHeThong.Name = "mnuHeThong";
            this.mnuHeThong.Size = new System.Drawing.Size(80, 24);
            this.mnuHeThong.Text = "Hệ thống";
            // 
            // mnuDangXuat
            // 
            this.mnuDangXuat.Name = "mnuDangXuat";
            this.mnuDangXuat.Size = new System.Drawing.Size(150, 26);
            this.mnuDangXuat.Text = "Đăng xuất";
            this.mnuDangXuat.Click += new System.EventHandler(this.MnuDangXuat_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelUser,
            this.toolStripButtonDangXuat});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1200, 27);
            this.toolStrip1.TabIndex = 1;
            // 
            // toolStripLabelUser
            // 
            this.toolStripLabelUser.Name = "toolStripLabelUser";
            this.toolStripLabelUser.Size = new System.Drawing.Size(150, 24);
            this.toolStripLabelUser.Text = "Người dùng: ...";
            this.toolStripLabelUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripButtonDangXuat
            // 
            this.toolStripButtonDangXuat.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonDangXuat.Name = "toolStripButtonDangXuat";
            this.toolStripButtonDangXuat.Size = new System.Drawing.Size(100, 24);
            this.toolStripButtonDangXuat.Text = "Đăng xuất";
            this.toolStripButtonDangXuat.Click += new System.EventHandler(this.ToolStripButtonDangXuat_Click);
            // 
            // flowBan
            // 
            this.flowBan.AutoScroll = true;
            this.flowBan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowBan.Location = new System.Drawing.Point(0, 55);
            this.flowBan.Name = "flowBan";
            this.flowBan.Size = new System.Drawing.Size(1200, 595);
            this.flowBan.TabIndex = 2;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 650);
            this.Controls.Add(this.flowBan);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Quán Cà Phê - Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuQuanLy;
        private System.Windows.Forms.ToolStripMenuItem mnuQuanLyMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuQuanLyBan;
        private System.Windows.Forms.ToolStripMenuItem mnuQuanLyNguoiDung;
        private System.Windows.Forms.ToolStripMenuItem mnuQuanLyCa;
        private System.Windows.Forms.ToolStripMenuItem mnuBaoCao;
        private System.Windows.Forms.ToolStripMenuItem mnuBaoCaoDoanhThu;
        private System.Windows.Forms.ToolStripMenuItem mnuHeThong;
        private System.Windows.Forms.ToolStripMenuItem mnuDangXuat;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelUser;
        private System.Windows.Forms.ToolStripButton toolStripButtonDangXuat;
        private System.Windows.Forms.FlowLayoutPanel flowBan;
    }
}