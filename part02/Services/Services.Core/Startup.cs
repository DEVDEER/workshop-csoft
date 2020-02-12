namespace Services.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Logic.DataAccess.TableStorage;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    /// <summary>
    /// App startup used by the web host builder.
    /// </summary>
    public class Startup
    {
        #region member vars

        private readonly string CorsPolicyName = "DefaultCorsPolicy";

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="configuration">Handler to the configuration system.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region methods

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(CorsPolicyName);
            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Telemetry API V1");
                });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The DI collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(
                options =>
                {
                    options.AddPolicy(
                        CorsPolicyName,
                        builder =>
                        {
                            builder.WithOrigins(Configuration["Cors:Origins"]).AllowAnyMethod().AllowAnyHeader();
                        });
                });
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "Telemetry API",
                            Version = "v1",
                            Description = "This is the API for the workshop sample."
                        });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });
            services.AddSingleton<TableStorageAdapterSettings>();
            services.AddTransient<ITableStorageAdapter<TelemeryTableEntity>, TableStorageAdapter<TelemeryTableEntity>>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Handler to the configuration system.
        /// </summary>
        private IConfiguration Configuration { get; }

        #endregion
    }
}