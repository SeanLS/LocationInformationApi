using LocationAPI.Manager;
using LocationAPI.Repository.Phone;
using LocationAPI.Repository.PostCodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace LocationAPI
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
            services.AddCors();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddScoped<ILocationDetailsManager, LocationDetailsManager>();
            services.AddScoped<IPhoneNumberMetadataRepository, PhoneNumberMetadataRepository>();
            services.AddScoped<IPostCodeDetailRepository, PostCodeDetailRepository>();

            // https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.2#problem-details-for-error-status-codes
            services.AddMvc(options =>
                        options.CacheProfiles.Add("Default30ResponseCacheLocationAnyNoStoreFalse", new CacheProfile() { Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false })
                    )
                    #pragma warning disable CS0618 // Type or member is obsolete
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    #pragma warning restore CS0618 // Type or member is obsolete
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressMapClientErrors = true;
                    });

            // https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Territories API",
                    Description = "A ASP.NET Core Web API to gather country specific validation data.",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Sean Lindsay-Smith",
                        Email = string.Empty,
                        Url = new Uri("https://www.linkedin.com/in/sean-lindsay-smith-97b40ab/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under GNU General Public License ver 3.0",
                        Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.en.html"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseStaticFiles();

            //app.Use((context, next) =>
            //{
            //    context.Response.Headers.Remove("Server");
            //    context.Request.Scheme = "https";
            //    return next();
            //});

            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Territories API V1");
                c.RoutePrefix = "LocationInformation/Swagger";
                c.InjectStylesheet("/swagger-ui/custom.css");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/middleware?view=aspnetcore-5.0
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });            
        }
    }
}
