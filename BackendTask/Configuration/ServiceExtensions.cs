using BackendTask.API.Authorization;
using BackendTask.Common;
using BackendTask.Common.Filters;
using BackendTask.Common.Middlewares;
using BackendTask.Data;
using BackendTask.Data.Models.Identity;
using BackendTask.Domain;
using BackendTask.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

namespace BackendTask.API.Configuration
{
    internal static class ServiceExtensions
    {
        /// <summary>
        /// Add Controllers, AutoMapper, Cors
        /// </summary>
        internal static IServiceCollection ConfigureApiControllers(this IServiceCollection services, IConfiguration configuration, string corsPolicyName)
        {
            services.AddControllers(config =>
            {
                config.Filters.Add(new ValidationFilterAttribute());
            })
            .AddDataAnnotationsLocalization(o =>
            {
                o.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(CommonResource));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = SupportedLanguages.ListAll.Select(x => new CultureInfo(x)).ToList();

                options.DefaultRequestCulture = new RequestCulture(SupportedLanguages.Arabic);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.FallBackToParentUICultures = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName,
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            return services;
        }


        /// <summary>
        /// Add Identity specific services, and Api Bearer Authentication
        /// </summary>
        internal static IServiceCollection ConfigureApiIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            //configure password
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            });

            services.AddIdentityCore<UserAccount>()
                .AddRoles<UserRole>()
                .AddEntityFrameworkStores<BackendTaskDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidAudience = configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))

                    };
                });

            services.AddSingleton<IAuthorizationMiddlewareResultHandler, FailedAuthorizationWrapperHandler>();

            // Default authorization policy = User must have the claim UserIsActive which is fetched from db
            // on PermissionsMiddleware
            services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireClaim(Constants.ActiveUserClaimType)
                .Build());

            services.AddScoped<IRolePermissionsService, RolePermissionsService>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            return services;
        }


        /// <summary>
        /// Adds the <see cref="PermissionsMiddleware"/> to the specified <see cref="IApplicationBuilder"/>,
        /// which retrieves permissions from the role of user and add them as claims.
        /// Must be called after UseAuthentication and before UseAuthorization.
        /// </summary>
        internal static IApplicationBuilder UseRolePermissions(this IApplicationBuilder app)
        {
            app.UseMiddleware<PermissionsMiddleware>();
            return app;
        }

        /// <summary>
        /// Add Swagger Documentation
        /// </summary>
        internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "BackendTask", Version = "v1" });

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "Application.xml");
                var sharedXmlPath = Path.Combine(basePath, "Shared.xml");

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {new OpenApiSecurityScheme{
                        Reference = new OpenApiReference
                        {
                            Id="Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                        }, new List<string>()
                    }
                });
            });

            return services;
        }

        /// <summary>
        /// Register Swagger, Swagger UI middlewares
        /// </summary>
        internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseSwagger();

            var uri = new Uri(config["App:ServerRootAddress"]);
            var basePath = uri.LocalPath.TrimEnd('/');

            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
                c.SwaggerEndpoint($"{basePath}/swagger/v1/swagger.json", "BackendTask");
            });

            return app;
        }
    }
}

