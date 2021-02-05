using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Id.Overview.Mvc.Data;
using Id.Overview.Mvc.Models;
using Id.Overview.Mvc.Services;

namespace Id.Overview.Mvc
{
    public class Startup //teste de commit
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => { //extension method que adiciona o identity
                //LOCKOUT
                options.Lockout.AllowedForNewUsers = true; //determina se novo user pode ser bloqueado
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //quant. de tempo que o user ficará bloqueado ao acontecer um lockout
                options.Lockout.MaxFailedAccessAttempts = 5; //n° de tentativas antes do lockout(caso lockout = true)


                //PASSWORD
                options.Password.RequireDigit = true; //obriga ter um número entre 0-9 na senha
                options.Password.RequiredLength = 6; 
                options.Password.RequiredUniqueChars = 1;//requer X caracteres diferentes
                options.Password.RequireLowercase = true; 
                options.Password.RequireUppercase = true; 
                options.Password.RequireNonAlphanumeric = true; //requer char especila (!@#$#$)

                //SIGNIN
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                //TOKENS
                //options.Tokens.AuthenticatorTokenProvider 
                //options.Tokens.ChangeEmailTokenProvider
                //options.Tokens.ChangePhoneNumberTokenProvider
                //options.Tokens.EmailConfirmationTokenProvider
                //options.Tokens.PasswordResetTokenProvider

                //USER: criaçõa de users com Identity
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //define os chars para o nome do user
                options.User.RequireUniqueEmail = false;
            }) 
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication(); //add o identity ao pipeline da aplicação

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
