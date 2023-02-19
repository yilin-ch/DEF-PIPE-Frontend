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
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

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
            services.AddCors(o => o.AddPolicy("AllowOrigin",
                builder =>
                {

                    builder
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .WithOrigins(new[] { "https://*.euprojects.net" })
                         .SetIsOriginAllowedToAllowWildcardSubdomains()
                         .AllowCredentials();
                }));

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

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "DEF-PIPE",
                    Description = "For the protected end points, you need to genereate an access token (e.g. using postman) and provide it by opening \"Authorize\"",
                   
                    
                });
                options.OperationFilter<SwaggerAuthOperationFilter>();
                
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter you your Keyclaok bearer token in the format bellow (don't forget the \"Bearer\")\n\n Bearer YOUR_ACCESS_TOKEN",
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = "docs";
            });
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

           
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseCors("AllowOrigin");
            app.UseHttpsRedirection();
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
