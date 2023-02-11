using BladeMill.BLL.DatatAcess;
using BladeMill.BLL.DatatBaseAcess;
using BladeMill.BLL.Interfaces;
using BladeMill.BLL.Models;
using BladeMill.BLL.Repositories;
using BladeMill.BLL.Services;
using CustomWindow.Middleware;
using MainApp.BLL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace BladeMill.Web
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
            services.AddControllersWithViews();
            var connectionString = Configuration.GetConnectionString("UsersDatabase");
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));//.UseInMemoryDatabase.UseSqlServer.UseLazyLoadingProxies() opcjonalnie do ladowania atomatycznych referancji pomiedzy bazami

            services.AddTransient<UserServiceWithoutDatabase>();//dzieki temu moge wstrzykiwac dane z serwisu do view patrz przyklad ProgramExe

            services.AddTransient<IUserService, UserService>();

            services.AddScoped<AppXmlConfService>();

            services.AddTransient<IBMUserService, BMUserService>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(Repository<>));

            var profileAssembly = typeof(Startup).Assembly;
            services.AddAutoMapper(profileAssembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dataContext)
        {
            var user = new UserServiceWithoutDatabase();
            string userStringSso = Convert.ToString(user.GetUserSso());

            if (userStringSso == "0")
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
                {
                    var context = serviceScope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    context?.Database.Migrate();
                    SeedData.SeedUsers(context);
                }
            }

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
            //app.UseSerilogRequestLogging();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            //app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
