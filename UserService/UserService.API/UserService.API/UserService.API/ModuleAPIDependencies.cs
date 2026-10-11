using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Context;

namespace UserService.API
{
    public static class ModuleAPIDependencies
    {
        public static IServiceCollection AddModuleApiDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("UserServiceDbConnection")));
            #endregion
            //#region CORS
            //services.AddCors(options =>
            //         options.AddPolicy("CorsPolicy", policyBuilder =>
            //             policyBuilder.SetIsOriginAllowed(_ => true)

            //                          .AllowAnyMethod()
            //                          .AllowAnyHeader().AllowCredentials())

            //     );
            //#endregion
            return services;
        }
    }
}
