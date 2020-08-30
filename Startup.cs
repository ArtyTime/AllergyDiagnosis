using Allergy.Models;
using Allergy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MotleyFlash;
using MotleyFlash.AspNetCore.MessageProviders;

namespace Allergy
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
            services.AddSession();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AllergiesDbContext>(config =>
                config
                    .EnableSensitiveDataLogging()
                    .UseSqlServer(
                        connectionString, options => options.EnableRetryOnFailure()));

            services.AddScoped<IAllergiesService, AllergiesService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(x => x.GetRequiredService<IHttpContextAccessor>().HttpContext.Session);

            services.AddScoped<IMessageProvider, SessionMessageProvider>();
            services.AddScoped<IMessageTypes>(x => new MessageTypes("danger"));
            services.AddScoped<IMessengerOptions, MessengerOptions>();
            services.AddScoped<IMessenger, StackMessenger>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Allergics}/{action=Index}/{id?}");
            });
        }
    }
}
