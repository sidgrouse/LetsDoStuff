using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LetsDoStuff.Domain;
using LetsDoStuff.WebApi.Services;
using LetsDoStuff.WebApi.Services.Interfaces;
using LetsDoStuff.WebApi.SettingsForAuth;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OData.Swagger.Services;

namespace LetsDoStuff.WebApi
{
    public class Startup
    {
        private const string CorsPolicyName = "_allowByCreds";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LetsDoStuff API");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseCors(CorsPolicyName);
            app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.EnableDependencyInjection();
                    endpoints.Select().Filter().Expand().MaxTop(10);
                    endpoints.MapHub<ParticipationHub>("/ParticipationHub");
                });
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddSignalR();
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LdsContext>(options =>
                options.UseSqlServer(connection));

            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: CorsPolicyName,
                    builder => builder
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.AddControllers().AddNewtonsoftJson();
            services.AddOData();
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddOdataSwaggerSupport();

            services.AddTransient<IActivityService, ActivityManager>();
            services.AddTransient<IParticipationService, ParticipationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IHubNotifier, HubNotifier>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthConstants.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthConstants.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthConstants.SymmetricSecurityKey,
                        ValidateIssuerSigningKey = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = contrxt =>
                        {
                            var accessToken = contrxt.Request.Query["access_token"];

                            var path = contrxt.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(path) && path.StartsWithSegments("/ParticipationHub"))
                            {
                                contrxt.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.ConfigureSwaggerGen(c =>
            {
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    Description = "Login with creds and then post the token here. User: dee@gmail.com / 12test , Admin: admin@gmail.com / 123"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        System.Array.Empty<string>()
                    }
                });
            });
        }
    }
}
