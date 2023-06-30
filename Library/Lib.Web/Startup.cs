using Lib.Domains;
using Lib.Services;
using Lib.Services.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lib.Web
{
    public class Startup
    {
        #region Fields
        public IConfiguration Configuration { get; }
        public string ConnectionString { get { return Configuration.GetConnectionString("DefaultConnection"); } }
        public AppSettings AppSettings { get { return Configuration.GetSection("AppSettings").Get<AppSettings>(); } }
        #endregion

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(ConnectionString));
            services.AddAuthorization();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<AdminSettings>(Configuration.GetSection("AdminSettings"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            ServiceActivator.Configure(app.ApplicationServices);
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
