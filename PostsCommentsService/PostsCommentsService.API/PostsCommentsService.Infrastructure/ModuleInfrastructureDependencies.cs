using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PostsCommentsService.Application.RepositoryInterface;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Data.Interfaces;
using PostsCommentsService.Domain;
using PostsCommentsService.Domain.Helper;
using PostsCommentsService.Infrastructure.DataBaseConfiguration;
using PostsCommentsService.Infrastructure.InfrastructureBases;
using PostsCommentsService.Infrastructure.RepositoryImplementaion;
using PostsCommentsService.Infrastructure.ServiceImplementaion;
using System.Text;
namespace PostsCommentsService.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddSingleton<ForumService>();

            services.AddScoped<ICommentsRepository, CommentsRepository>();
            services.AddScoped<IPostsRepository, PostsRepository>();
            services.AddScoped<IPostsService, PostsService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IImageStorageService, ImageStorageService>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpClient();

            services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidIssuer = Configuration["JWT:IssuerIP"],
                         ValidateAudience = true,
                         ValidAudience = Configuration["JWT:AudienceIP"],
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]!))
                     });


            services.AddHttpContextAccessor();
            return services;
        }


    }
}
