using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Behavior;
using PostsCommentsService.Application.HandlerMiddleware;
using System.Reflection;

namespace PostsCommentsService.Application
{
    public static class ModuleApplicationDependencies
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(ModuleApplicationDependencies).Assembly));

            services.AddAutoMapper(cfg =>
                cfg.AddMaps(typeof(ModuleApplicationDependencies).Assembly));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IResponseHandler, ResponseHandler>();
            return services;

        }

    }
}
