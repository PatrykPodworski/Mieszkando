using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TomtomApiWrapper;
using TomtomApiWrapper.Interafaces;

namespace WebAPI
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
            services.AddMvc();
            services.AddCors();
            services.AddSingleton<ITomtomApi, TomtomApi>();
            services.AddSingleton<IDataRepository<CityRegion>, DatabaseCityRegionRepository>();
            services.AddSingleton<IDataRepository<Offer>, DatabaseOfferRepository>();
            services.AddSingleton<IDatabaseConnectionSettings, DatabaseConnectionSettings>(
                (ctx) => new DatabaseConnectionSettings("mieszkando-db"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseMvc();
        }
    }
}