using AutoMapper;
using LeagueApp.API.Utilites;
using LeagueApp.Domain.IRepositories;
using LeagueApp.Domain.Models;
using LeagueApp.Domain.Shared;
using LeagueApp.Domain.Shared.IManagers;
using LeagueApp.Domain.Shared.IRepositories;
using LeagueApp.Infrastructure.Repositories;
using LeagueApp.Infrastructure.Shared;
using LeagueApp.Infrastructure.Shared.Managers;
using LeagueApp.Infrastructure.Shared.Repositories;
using Loyalty.AppWallet.Api.Mapping;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace LeagueApp
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
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    assembly =>
                    {
                        assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    });
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();

            }).AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();

            services.AddScoped<IPlayerManager, PlayerManager>();
            services.AddScoped<ITeamManager,TeamManager>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<JwtProvider, JwtProvider>();


            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Doc", Version = "v1" });
                c.OperationFilter<CustomHeaderSwaggerAttribute>();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
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

            var key = Encoding.ASCII.GetBytes
                     (Configuration.GetSection("AppSetting:Token").Value);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(optyion =>
                {
                    optyion.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAutoMapper(typeof(Startup));
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TeamProfile());
            });
            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsDevelopment() || Configuration.GetValue<bool>("AppSetting:AllowSwagger"))
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "League App V1");
                    c.RoutePrefix = "swagger";
                });
            }
            app.UseCors(options =>
            {
                options
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials();
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.Use(async (context, next) =>
            {
                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = 405;
                    return;
                }
                await next.Invoke();
            });

            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception) { }
            try
            {
                var AppFilesPath = Configuration.GetValue<string>("AppSetting:AppFilesPath");
                if (!Directory.Exists(AppFilesPath))
                    Directory.CreateDirectory(AppFilesPath);
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(env.ContentRootPath + @"\App-logs"),
                    RequestPath = new PathString("/logs")
                });
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(env.ContentRootPath + $"{AppFilesPath}\\Assets"),
                    RequestPath = new PathString("/appImage"),
                });

            }
            catch (Exception) { };
            loggerFactory.AddSerilog();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello League App!");
                });
            });
        }
    }
}
