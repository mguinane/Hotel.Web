using AutoMapper;
using Hotel.Web.Controllers;
using Hotel.Web.Core.Models;
using Hotel.Web.Core.Repositories;
using Hotel.Web.Infrastructure.Repositories;
using Hotel.Web.Infrastructure.Services.Logging;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Hotel.Web
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
            services.Configure<PageSettings>(Configuration.GetSection("pageSettings"));

            // Typically would be AddScoped for a database repository (if using EF via DbContext which also has Scoped lifetime)
            // Using AddSingleton for our in memory repository so that hotels.json file is not deserialised on each request.
            services.AddSingleton<IHotelRepository, HotelRepository>();

            services.AddSingleton<ILoggerAdapter<HotelsController>, LoggerAdapter<HotelsController>>();

            Mapper.Initialize(mapConfig =>
            {
                mapConfig.CreateMap<SearchCriteriaViewModel, SearchCriteria>();
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
