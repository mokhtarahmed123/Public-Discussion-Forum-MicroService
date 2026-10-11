using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VotesService.Application.Bases;
using VotesService.Application.Behavior;
using VotesService.Application.Interface;
namespace VotesService.Application
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

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(
                        Configuration["RabbitMq:Host"] ?? "localhost",
                        Configuration["RabbitMq:VirtualHost"] ?? "/",
                        h =>
                        {
                            h.Username(Configuration["RabbitMq:Username"] ?? "user");
                            h.Password(Configuration["RabbitMq:Password"] ?? "mypassword");
                        });

                    cfg.ConfigureEndpoints(ctx);
                });
            });



            return services;

        }

    }
}
