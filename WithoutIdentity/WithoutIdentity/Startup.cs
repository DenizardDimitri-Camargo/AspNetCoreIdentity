using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WithoutIdentity.Data;
using WithoutIdentity.Models;

namespace WithoutIdentity
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
            var connection = Configuration.GetConnectionString("IdentityDb");
            services.AddDbContext<ApplicationDataContext>(options =>
                options.UseSqlServer(connection)
            );

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>() //por padrão é string, portanto a PK no BD seria string, foi setado para Guid
                .AddEntityFrameworkStores<ApplicationDataContext>() //EF responsável por armazenar dados do Identity
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8; //default = 6
                options.Password.RequiredUniqueChars = 6; //default = 1
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.CookieHttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "Account/Login";
                options.LogoutPath = "Account/Logout";
                options.AccessDeniedPath = "Account/AccessDenied";
                options.SlidingExpiration = true; //renova tempo de expiração
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication(); //identity é adicionado ao pipeline da aplicação

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
