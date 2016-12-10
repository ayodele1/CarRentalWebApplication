using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DomainObjects;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using AutoMapper;
using DomainObjects.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Buffers;

namespace ABCCarRental
{
    public class Startup
    {
        private IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {             
            // Add framework services.
            services.AddMvc(config => 
            {
                if (_env.IsProduction())
                {
                    //config.Filters.Add(new RequireHttpsAttribute());
                }
                //config.OutputFormatters.Clear();
                //config.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings()
                //                            {
                //                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //                            }, ArrayPool<char>.Shared));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSingleton(Configuration);
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<ReservationRepository>();
            services.AddScoped<VehicleRepository>();
            services.AddDistributedMemoryCache();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            Mapper.Initialize(config =>
            {
                config.CreateMap<ReservationViewModel, Reservation>();
                config.CreateMap<ReservationContactViewModel, ApplicationUser>();
                config.CreateMap<RegistrationViewModel, ApplicationUser>();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
