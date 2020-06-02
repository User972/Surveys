using Comp.Survey.Core.Interfaces;
using Comp.Survey.Core.Interfaces.Services;
using Comp.Survey.Core.Services;
using Comp.Survey.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Comp.Survey.App
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            var conn = $"Data Source={_env.ContentRootPath}/" + Configuration.GetConnectionString("ApplicationDB");

            services.AddDbContext<ApplicationDataContext>(c => c.UseSqlite(conn));

            services.AddScoped(typeof(IEntityBaseRepository<>), typeof(EntityBaseRepository<>));

            services.AddScoped<ISurveyRepository, SurveyRepository>();
            services.AddScoped<ISurveyQuestionRepository, SurveyQuestionRepository>();
            services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();
            services.AddScoped<ICompUserSurveyRepository, CompUserSurveyRepository>();
            services.AddScoped<ICompUserSurveyDetailRepository, CompUserSurveyDetailRepository>();
            services.AddScoped<ICompUserRepository, CompUserRepository>();

            services.AddScoped<ISurveyManagementService, SurveyManagementService>();
            services.AddScoped<ISurveyQuestionManagementService, SurveyQuestionManagementService>();
            services.AddScoped<IQuestionOptionsManagementService, QuestionOptionManagementService>();
            services.AddScoped<ICompUserManagementService, CompUserManagementService>();
            services.AddScoped<ICompUserSurveyManagementService, CompUserSurveyManagementService>();

            var cfg = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddScoped<Serilog.ILogger>(x => new LoggerConfiguration().ReadFrom.Configuration(cfg).CreateLogger());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Survey API", Version = "v1" });
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
