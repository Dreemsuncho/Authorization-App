using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using InventoryControl.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace InventoryControl
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
            services.AddAuthentication(Constants.MiddlewareScheme)
                .AddCookie(Constants.MiddlewareScheme,
                    options =>
                    {
                        options.LoginPath = new PathString("/Account/Login/");
                        options.AccessDeniedPath = new PathString("/Account/Forbidden/");
                    });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.AdministratorsOnly, policy => policy.RequireRole("Administrator"));
                config.AddPolicy(Policies.CanEditAlbum, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Administrator");
                    policy.Requirements.Add(new AlbumOwnerRequirement());
                });
            });
            services.AddMvc();

            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IAlbumRepository, AlbumRepository>();

            services.AddScoped<IAuthorizationHandler, AlbumOwnerAuthHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
