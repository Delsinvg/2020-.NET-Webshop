using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using project.shared.Settings;
using project.api.Exceptions;
using project.web.Handlers;
using project.web.Services;
using project.web.Middlewares;

namespace project.web
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
            // Session configuration
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = "Project.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.Configure<FormOptions>(options =>
            {
                // Set the upload files limit to 10MB
                options.MultipartBodyLengthLimit = 10485760;
            });

            // Configure strongly typed file upload settings object
            var fileUploadSettingsSection = Configuration.GetSection("FileUploadSettings");
            services.Configure<FileUploadSettings>(fileUploadSettingsSection);

            services.AddTransient<ValidateHeaderHandler>();

            // Add typed HttpClient with custom HttpMessageHandler
            services.AddHttpClient<ProjectApiService>(c =>
            {
                c.BaseAddress = new Uri("https://localhost:44388/api/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<ValidateHeaderHandler>();

            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new ProjectExceptionConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .AddSessionStateTempDataProvider();

            // Get access to the HttpContext in views and business logic
            services.AddHttpContextAccessor();

            // Mediatheek services
            services.AddProject();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // app.UseCookiePolicy();

            app.UseRouting();

            // app.UseRequestLocalization();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            // app.UseResponseCaching();

            // Start Custom middlewares
            app.UseStateManagement();
            app.UseClaims();
            // End Custom middlewares

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
