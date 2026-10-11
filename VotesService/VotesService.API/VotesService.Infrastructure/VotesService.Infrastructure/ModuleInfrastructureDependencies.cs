using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VotesService.Application.ExternalApiService.CommentServiceInterface;
using VotesService.Application.ExternalApiService.PostServiceInterface;
using VotesService.Application.Interface;
using VotesService.Application.RepositoryInterface;
using VotesService.Application.ServiceInterface;
using VotesService.Domain.Helper;
using VotesService.Infrastructure.Clients.CommentClientImplementaion;
using VotesService.Infrastructure.Clients.PostClientImplementaion;
using VotesService.Infrastructure.DataBaseConfiguration;
using VotesService.Infrastructure.InfrastructureBases;
using VotesService.Infrastructure.RepositoryImplementation;

namespace VotesService
{
    public static class ModuleInfrastructureDependencies
    {

        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddSingleton<ForumService>();
            services.AddHostedService<VotesIndexInitializer>();
            services.AddScoped<IVotesService, VotesService.Infrastructure.ServiceImplementation.VotesService>();
            services.AddScoped<IVotesRepository, VotesRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();


            //services.AddHttpClient<IUserClient, UserClient>(client =>
            //{
            //    client.BaseAddress = new Uri(Configuration["Services:Auth:BaseUrl"]!);
            //});

            services.AddHttpClient<ICommentClient, CommentClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["Services:PostAndComment:BaseUrl"]!);
            });

            services.AddHttpClient<IPostClient, PostClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["Services:PostAndComment:BaseUrl"]!);
            });
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
