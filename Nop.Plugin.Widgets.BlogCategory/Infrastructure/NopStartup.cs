using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.BlogCategory.Authorization.Policies;
using Nop.Plugin.Widgets.BlogCategory.Authorization.Requirements;
using Nop.Plugin.Widgets.BlogCategory.Factories;
using Nop.Plugin.Widgets.BlogCategory.Filters;
using Nop.Plugin.Widgets.BlogCategory.Services;

namespace Nop.Plugin.Widgets.BlogCategory.Infrastructure
{
    public class NopStartup : INopStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
                services.AddScoped<IBlogCategoryService, BlogCategoryService>();
                services.AddScoped<IBlogCategoryModelFactory, BlogCategoryModelFactory>();
                services.AddScoped<ApiResultFilterAttribute>();

                if (!string.IsNullOrEmpty(BlogCategoryDefaults.SecurityKey))
                {
                    services.AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
                        {
                            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(BlogCategoryDefaults.SecurityKey)),
                                ValidateIssuer = false, // ValidIssuer = "The name of the issuer",
                                ValidateAudience = false, // ValidAudience = "The name of the audience",
                                ValidateLifetime = true, // validate the expiration and not before values in the token
                                ClockSkew = TimeSpan.FromMinutes(BlogCategoryDefaults.AllowedClockSkewInMinutes)
                            };
                        });

                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                    AddAuthorizationPipeline(services);

                }
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
          
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 1;

        private static void AddAuthorizationPipeline(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme,
                    policy =>
                    {
                        policy.Requirements.Add(new ActiveApiPluginRequirement());
                        policy.Requirements.Add(new AuthorizationSchemeRequirement());
                        policy.Requirements.Add(new CustomerRoleRequirement());
                        policy.RequireAuthenticatedUser();
                    });
            });

            services.AddSingleton<IAuthorizationHandler, ActiveApiPluginAuthorizationPolicy>();
            services.AddSingleton<IAuthorizationHandler, ValidSchemeAuthorizationPolicy>();
            services.AddSingleton<IAuthorizationHandler, CustomerRoleAuthorizationPolicy>();

        }

    }
}
