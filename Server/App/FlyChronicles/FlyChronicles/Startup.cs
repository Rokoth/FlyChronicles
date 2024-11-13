using System;
using FlyChronicles.Db.Repository;
using FlyChronicles.DB.Context;
using FlyChronicles.DB.Interface;
using FlyCronicles.Common.Options;
using FlyCronicles.Contract;
using FlyCronicles.DeployerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FlyChronicles
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CommonOptions>(Configuration);
            services.AddControllersWithViews();
            services.AddLogging();
            services.AddSingleton<IErrorNotifyService, ErrorNotifyService>();
            services.AddDbContextPool<DbPgContext>((opt) =>
            {
                opt.EnableSensitiveDataLogging();
                var connectionString = Configuration.GetConnectionString("MainConnection");
                opt.UseNpgsql(connectionString);
            });

            services.AddCors();
            services
                .AddAuthentication()
                .AddJwtBearer("Token", (options) =>
                {
                    AuthOptions settings = Configuration.GetSection("AuthOptions").Get<AuthOptions>();
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //// укзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        //// строка, представляющая издателя
                        ValidIssuer = settings.Issuer,
                        //// будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        //// установка потребителя токена
                        ValidAudience = settings.Audience,
                        //// будет ли валидироваться время существования
                        ValidateLifetime = true,
                        // установка ключа безопасности
                        IssuerSigningKey = settings.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddAuthorization(options =>
            {
                var defPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Token")
                    .Build();
                options.AddPolicy("Token", defPolicy);
                options.DefaultPolicy = defPolicy;
            });

            services.AddScoped<IRepository<DB.Model.User>, Repository<DB.Model.User>>();
            services.AddScoped<IRepository<DB.Model.UserHistory>, Repository<DB.Model.UserHistory>>();

            //todo: add extension on ready DataServices
            //services.AddDataServices();
            services.AddScoped<IDeployService, DeployService>();

            services.AddRazorPages();

            services.AddSwaggerGen(swagger =>
            {
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                          Array.Empty<string>()
                    }
                });
            });


        }

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
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
