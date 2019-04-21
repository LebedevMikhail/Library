using Library.Models;
using Library.Models.Interfaces;
using Library.Models.MSSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:ConnectionToDb"]);

            });

            services.AddDbContext<AppIdentityDbContext>(options =>
               options.UseSqlServer(
                   Configuration["ConnectionStrings:ConnectionToIdentityDb"]));

            services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IGenreRepository, GenreRepository>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();


        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "pagination",
                    template: "/Page/{Page}",
                    defaults: new { Controller = "Book", action = "Index" });

                routes.MapRoute(
                    name: "filter",
                    template: "/{query?}",
                defaults: new { Controller = "Book", action = "Index" });

                routes.MapRoute(
                  name: "orders",
                  template: "/Order/Index",
              defaults: new { Controller = "Order", action = "Index" });

                routes.MapRoute(
                   name: "editUser",
                   template: "/Admin/Edit/{id?}",
               defaults: new { Controller = "Admin", action = "Edit" });

                routes.MapRoute(
                    name: "",
                    template: "{controller=Book}/{action=Index}/{id?}");

                routes.MapRoute(
                   name: "",
                   template: "{controller=Cart}/{action=Index}/{id?}");
                routes.MapRoute(
                  name: "",
                  template: "{controller=Book}/{action=Index}/{query?}");
                routes.MapRoute(
                    name: null, template: "{controller}/{action}/{id?}");

                routes.MapRoute(
                    name: null, template: "{controller}/{action}/{id?}");
            });
        }
    }
}
