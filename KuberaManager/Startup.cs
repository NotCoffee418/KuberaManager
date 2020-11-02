using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.PostgreSql;
using KuberaManager.Hangfire;
using KuberaManager.Models.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KuberaManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext to the injection container
            services.AddDbContext<kuberaDbContext>(options =>
                    options.UseNpgsql(
                        this.Configuration.GetConnectionString("DefaultConnection")));

            // Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Cookie auth scheme
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Admin/Login");
                options.LoginPath = new PathString("/Admin/Login");
                options.LogoutPath = new PathString("/Admin/Logout");
            });

            // controller/views
            services.AddControllersWithViews();

            // Hangfire
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, kuberaDbContext context)
        {
            // Show detailed exception page
            // DELETEME & UNCOMMENT BELOW
            app.UseDeveloperExceptionPage();

            /* In production, hide errors
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            */

            // Install/upgrade database:
            context.Database.Migrate();

            // Default
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            // Authentication & authorization
            app.UseAuthentication();
            app.UseAuthorization(); // this MUST be below Authentication or stuff breaks

            // Session
            app.UseSession();

            // Routing
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Hangfire (call after normal authenticate/authorize)
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            // Hangfire scheduled tasks
            RecurringJob.AddOrUpdate(() => ScheduledTasks.RunHourlyJobs(), Cron.Hourly);
            RecurringJob.AddOrUpdate(() => ScheduledTasks.RunMinutelyJobs(), Cron.Minutely);
        }
    }
}
