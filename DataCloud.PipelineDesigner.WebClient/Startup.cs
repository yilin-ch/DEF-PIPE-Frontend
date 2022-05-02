using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataCloud.PipelineDesigner.Repositories;
using Microsoft.Extensions.Options;
using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.Repositories.Services;
using DataCloud.PipelineDesigner.Services;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using DataCloud.PipelineDesigner.WebClient.Controllers.Auth;
using Microsoft.IdentityModel.Tokens;

namespace DataCloud.PipelineDesigner.WebClient
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
            services.AddControllersWithViews().AddNewtonsoftJson();

            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddSingleton<MongoService>();

            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPublicRepoService, PublicRepoService>();


            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;/* Authentication options */ })
                                                .AddJwtBearer(options =>
                                                {
                                                    options.Authority = Environment.GetEnvironmentVariable("KEYCLOAK_AUTHORITY");
                                                    options.TokenValidationParameters =
                                                        new TokenValidationParameters
                                                        {
                                                            ValidateAudience = false,
                                                            NameClaimType = "preferred_username"
                                                        };
                                                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OwnershipPolicy", policy =>
                    policy.Requirements.Add(new OwnershipRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, OwnershipAuthHandler>();

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

            app.UseAuthentication();
            app.UseAuthorization();

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
