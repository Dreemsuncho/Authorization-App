using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AuthorizationLab.AuthorizationHandlers;

namespace AuthorizationLab
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
                {
                    config.LoginPath = new PathString("/Account/Login");
                    config.AccessDeniedPath = new PathString("/Account/Forbidden");
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
                config.AddPolicy("EmployeeId", policy => policy.RequireClaim("EmployeeId", "123", "321"));
                config.AddPolicy("Over21Only", policy => policy.Requirements.Add(new MinimumAgeRequirement(21)));
                config.AddPolicy("BuildingEntry", policy => policy.Requirements.Add(new OfficeEntryRequirement()));
            });

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddScoped<IAuthorizationHandler, MinimumAgeHandler>();
            services.AddScoped<IAuthorizationHandler, HasBadgeHandler>();
            services.AddScoped<IAuthorizationHandler, HasTemporaryPassHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
