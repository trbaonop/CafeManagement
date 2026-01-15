using Cafe.BLL;
using Cafe.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Cafe.UI
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();

            string connStr = ConfigurationManager.ConnectionStrings["CafeConnection"]?.ConnectionString
                ?? throw new InvalidOperationException("Không tìm thấy 'CafeConnection' trong App.config");

            services.AddDbContext<CafeContext>(options =>
                options.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));

            services.AddScoped<AuthService>();
            services.AddScoped<BanService>();
            services.AddScoped<MenuService>();
            services.AddScoped<HoaDonService>();
            services.AddScoped<CaService>();
            services.AddScoped<NguoiDungService>();

            services.AddTransient<frmLogin>();
            services.AddTransient<frmMain>();
            services.AddTransient<frmOrder>();
            services.AddTransient<frmQuanLyMenu>();
            services.AddTransient<frmQuanLyBan>();
            services.AddTransient<frmQuanLyNguoiDung>();
            services.AddTransient<frmQuanLyCa>();
            services.AddTransient<frmMonEdit>();
            services.AddTransient<frmBanEdit>();
            services.AddTransient<frmBaoCaoDoanhThu>();
            services.AddTransient<frmNguoiDungEdit>();

            ServiceProvider = services.BuildServiceProvider();

            Application.Run(ServiceProvider.GetRequiredService<frmLogin>());
        }
    }
}