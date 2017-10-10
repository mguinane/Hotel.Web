using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Hotel.Web.Data;
using AutoMapper;
using Hotel.Web.ViewModels;
using Hotel.Web.Models;
using Newtonsoft.Json.Serialization;

namespace Hotel.Web
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
                
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Typically would be AddScoped for a database repository (if using EF via DbContext which also has Scoped lifetime)
            // Using AddSingleton for our in memory repository so that hotels.json file is not deserialised on each request.
            services.AddSingleton<IHotelRepository, HotelRepository>();

            Mapper.Initialize(mapConfig =>
            {
                mapConfig.CreateMap<Establishment, EstablishmentViewModel>();
                mapConfig.CreateMap<AvailabilitySearch, AvailabilitySearchViewModel>();
            });

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(config =>
                {
                    // Allow camel casing in JSON property names
                    config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Hotels}/{action=Index}/{id?}");
            });
        }
    }
}
