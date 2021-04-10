
using GloboTicket.Services.IdentityServerAsmbly.swagger;
using GloboTicket.Services.IdentityServerAsmbly.Usersinfo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace GloboTicket.Services.IdentityServerAsmbly.Jwt
{
    public static class Extensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration, bool IsDependency)
        {

            var jwtsection = configuration.GetSection("JwtOptions");
            var jwtOptions = jwtsection.Get<JwtOptions>();
            var key = Encoding.UTF8.GetBytes(jwtOptions.Secret);
            if (IsDependency)
            {
                JwtOptions options = new JwtOptions();
                jwtsection.Bind(options);
            }
            services.Configure<JwtOptions>(jwtsection);
            services.AddSingleton<IJwtBuilder, JwtBuilder>();


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                x.RequireHttpsMetadata = false;

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true
                };
            });
        }
        public static void AddCors(this IServiceCollection services, string AllowOriginName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AllowOriginName,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                                        .AllowAnyHeader()
                                                        .AllowAnyMethod();
                                  });
            });
        }
        public static void addUsers(this IServiceCollection services, IHttpContextAccessor _HttpContext)
        {

            services.AddSingleton(new GloboUsers(_HttpContext));
        }
      

        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var swagggersection = configuration.GetSection("swaggerInfo");
            SwaggerInfo swagggerOptions = swagggersection.Get<SwaggerInfo>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = swagggerOptions.TitleConf,
                    Description = swagggerOptions.Description,
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Globo",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under Globo",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}

                    }
                });

            });
        }
        public static void AddSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swagggersection = configuration.GetSection("swaggerInfo");
            SwaggerInfo swagggerOptions = swagggersection.Get<SwaggerInfo>();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", swagggerOptions.TitleApp);
                c.RoutePrefix = swagggerOptions.Route;
            });
        }

    }
}
