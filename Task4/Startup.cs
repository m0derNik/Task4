using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Task4.Filters;
using Task4.Models;
using Task4.Services;

namespace Task4
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
            ConfigureFilters(services);

            ConfigureDatabaseAndIdentity(services);

            ConfigureCustomServices(services);

            ConfigureMvc(services);
        }

        private void ConfigureFilters(IServiceCollection services)
        {
            services.AddScoped<BlockStatusFilter>();
        }

        private void ConfigureDatabaseAndIdentity(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 1; 
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.SignIn.RequireConfirmedEmail = false; 
                options.User.RequireUniqueEmail = true; 
            }).AddEntityFrameworkStores<ApplicationContext>()
              .AddDefaultTokenProviders(); 
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserManagementService, UserManagementService>();
            services.AddTransient<IUserStatusService, UserStatusService>();
        }

        private void ConfigureMvc(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<BlockStatusFilter>();
            })
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureExceptionHandling(app, env);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureExceptionHandling(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); 
            }
        }
    }
}
