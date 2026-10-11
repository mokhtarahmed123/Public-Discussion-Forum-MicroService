using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Concurrent;
using System.Text;
using UserService.Application.dtos;
using UserService.Application.ExternalLogin;
using UserService.Application.Feature.Authentication;
using UserService.Application.Feature.Google;
using UserService.Application.Feature.Roles;
using UserService.Domain.Entities;
using UserService.Domain.Helper;
using UserService.Infrastructure.Abstract.Authentication;
using UserService.Infrastructure.Abstract.Authorization;
using UserService.Infrastructure.Abstract.Role;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Email;
using UserService.Infrastructure.ExternalLoginImple;
using UserService.Infrastructure.InfrastructureBases;
using UserService.Infrastructure.Logging;
namespace UserService.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

            services.Configure<JWTModel>(
                Configuration.GetSection("JWT"));
            #region Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "User Service v1",
                    Version = "v1",
                    Description = "User Service Web API Documentation"
                });
                options.SwaggerDoc("v2", new OpenApiInfo
                {

                    Title = "User Service Project V2",
                    Version = "v2",
                    Description = "User Service API Documentation"
                });

                options.EnableAnnotations(); // This will now be recognized

                // JWT Authentication
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter only the JWT token. Swagger will add 'Bearer' automatically.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            #endregion
            #region Identity

            services.AddIdentity<Users, Role>(
                opt =>
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireUppercase = true;
                    opt.Password.RequiredLength = 8;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.User.RequireUniqueEmail = true;
                    opt.SignIn.RequireConfirmedEmail = true;

                    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    opt.Lockout.MaxFailedAccessAttempts = 5;
                    opt.Lockout.AllowedForNewUsers = true;
                    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                }

                )

                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            #endregion
            #region Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                  .AddJwtBearer(options =>
                  {
                      var secretKey = Configuration["JWT:SecretKey"];
                      if (string.IsNullOrEmpty(secretKey))
                          throw new ArgumentNullException("JWT:SecretKey", "JWT SecretKey is missing in appsettings.json");

                      options.SaveToken = true;
                      options.RequireHttpsMetadata = false;
                      options.MapInboundClaims = false;

                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidIssuer = Configuration["JWT:IssuerIP"],

                          ValidateAudience = true,
                          ValidAudience = Configuration["JWT:AudienceIP"],

                          ValidateIssuerSigningKey = true,
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                          RoleClaimType = "roleName",
                          ValidateLifetime = true,
                          ClockSkew = TimeSpan.Zero
                      };

                  });

            #endregion

            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.Configure<EmailSettings>(Configuration.GetSection("Email"));
            services.Configure<GoogleModelConfiguration>(Configuration.GetSection("Google"));
            services.AddSingleton<
         ConcurrentDictionary<string, RefreshToken>>();
            services.AddScoped<IRoleService, RoleService>();
            return services;
        }
    }
}
