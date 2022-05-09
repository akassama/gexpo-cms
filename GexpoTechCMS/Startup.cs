using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NgoExpoApp.Models;
using Microsoft.AspNetCore.Http;
using DNTCaptcha.Core;

namespace NgoExpoApp
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
            // Add detection services container and device resolver service.
            services.AddDetection();

            // Add framework services.
            services.AddControllersWithViews();

            services.AddDNTCaptcha(options =>
                options.UseCookieStorageProvider()
                    .ShowThousandsSeparators(false)
            );

            //Read config from apsettings
            services.Configure<SystemConfiguration>(Configuration.GetSection("systemConfiguration"));

            //Read config from appmessages
            services.Configure<AppMessages>(Configuration.GetSection("appMessages"));

            //Registering IHttpContextAccessor and SessionManager in Startup file.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<SessionManager>();

            services.AddMemoryCache();//Adds a non distributed in memory implementation of IMemoryCache to the IServiceCollection.
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // add the accessor to the DI container
            services.AddHttpContextAccessor();

            //Register the database context
            services.AddDbContext<DBConnection>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Error page handlers
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Error/E404";
                    await next();
                }
                else if (context.Response.StatusCode == 400)
                {
                    context.Request.Path = "/Error/E400";
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //AddDetection() Adds the detection services to the services container.
            app.UseDetection();

            app.UseRouting();

            app.UseAuthorization();

            //Use session here
            app.UseSession();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "posts",
                    pattern: "Post/{id?}",
                    defaults: new { controller = "Post", action = "Index" });
                endpoints.MapControllerRoute(name: "pages",
                    pattern: "Page/{id?}",
                    defaults: new { controller = "Page", action = "Index" });
                endpoints.MapControllerRoute(name: "category",
                    pattern: "Category/{id?}",
                    defaults: new { controller = "Category", action = "Index" });
                endpoints.MapControllerRoute(name: "tags",
                    pattern: "Tags/{id?}",
                    defaults: new { controller = "Tags", action = "Index" });
                endpoints.MapControllerRoute(name: "portfolio",
                    pattern: "Portfolio/{id?}",
                    defaults: new { controller = "Portfolio", action = "Index" });
                endpoints.MapControllerRoute(name: "search",
                    pattern: "Search/{id?}",
                    defaults: new { controller = "Search", action = "Index" });
            });
        }
    }
}
