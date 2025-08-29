using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Excel;
using DevExpress.DataAccess.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;

namespace AspNetCoreDashboardBackend {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment) {
            Configuration = configuration;
            FileProvider = hostingEnvironment.ContentRootFileProvider;
        }

        public IConfiguration Configuration { get; }
        public IFileProvider FileProvider { get; }

        public void ConfigureServices(IServiceCollection services) {
            services
                // Configures CORS policies.                
                .AddCors(options => {
                    options.AddPolicy("CorsPolicy", builder => {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.WithHeaders("Content-Type");
                    });
                })
                // Adds the DevExpress middleware.
                .AddDevExpressControls()
                // Adds controllers.
                .AddControllers();
            // Configures the dashboard backend.
            services.AddScoped<DashboardConfigurator>((IServiceProvider serviceProvider) => {
                DashboardConfigurator configurator = new DashboardConfigurator();
                configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(Configuration));

                DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(FileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath);
                configurator.SetDashboardStorage(dashboardFileStorage);

                DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();

                configurator.SetDataSourceStorage(dataSourceStorage);
 

                return configurator;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            // Registers the DevExpress middleware.            
            app.UseDevExpressControls();
            // Registers routing.
            app.UseRouting();
            // Registers CORS policies.
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints => {
                // Maps the dashboard route.
                endpoints.MapDashboardRoute("api/dashboard", "DefaultDashboard");
                // Requires CORS policies.
                endpoints.MapControllers().RequireCors("CorsPolicy");
            });
        }
    }
}