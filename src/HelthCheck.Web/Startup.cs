using System;
using System.Threading.Tasks;
using HelthCheck.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelthCheck.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("ApplicationContext")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //InitializeDatabaseAsync(app.ApplicationServices).Wait();
        }

        private async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationContext>();

                if (await db.Database.EnsureCreatedAsync())
                {
                    TargetHost targetHost = new TargetHost()
                    {
                        Id = 1,
                        IP = "127.0.0.1"
                    };

                    Check check1 = new Check()
                    {
                        Id = 1,
                        Cron = @"*/1 * * * *",
                        HelthCheckUrl = "test",
                        Status = CheckStatus.Active,
                        TargetHost = targetHost
                    };

                    Check check2 = new Check()
                    {
                        Id = 2,
                        Cron = @"*/1 * * * *",
                        HelthCheckUrl = "test2",
                        Status = CheckStatus.Active,
                        TargetHost = targetHost
                    };

                    CheckResult checkResult = new CheckResult()
                    {
                        Check = check1,
                        Status = CheckResultStatus.Success
                    };

                    db.TargetHosts.Add(targetHost);
                    db.Checks.Add(check1);
                    db.Checks.Add(check2);
                    db.CheckResults.Add(checkResult);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
