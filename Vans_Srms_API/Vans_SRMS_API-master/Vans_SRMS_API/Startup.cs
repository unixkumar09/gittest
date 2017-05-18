using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Vans_SRMS_API.Repositories;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Vans_SRMS_API.Filters;
using Vans_SRMS_API.Database;
using Vans_SRMS_API.Websockets;
using NLog.Extensions.Logging;
using NLog.Web;
using System;

namespace Vans_SRMS_API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            env.ConfigureNLog("nlog.config");
            
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
            .AddMvc(options =>
           {
               options.Filters.Add(typeof(CustomExceptionFilterAttribute));
               options.Filters.Add(typeof(ValidateModelAttribute));
           })
           .AddJsonOptions(jsonOptions =>
           {
               jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
           });

            services.AddDistributedMemoryCache();
            services.AddSession();
            
            string connection = Configuration["SRMSConnection"];
            services.AddDbContext<SRMS_DbContext>(options =>
            {
                options.UseNpgsql(connection);
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IShoeRepository, ShoeRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();

            services.AddWebSocketManager();

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<AddRequiredHeaderParameter>();
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "SRMS API",
                    Version = "v1"
                });

                c.DescribeAllEnumsAsStrings();

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "documentation.xml");
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SRMS_DbContext dbContext)
        {
            app.UseDeveloperExceptionPage();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSession();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(1800),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);
            app.MapWebSocketManager("/ws",
                app.ApplicationServices.GetService<OrderMessageHandler>(),
                app.ApplicationServices.GetService<ILogger<WebSocketManagerMiddleware>>());


            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            DbInitializer.Initialize(dbContext);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SRMS API V1");
            });

        }
    }
}
