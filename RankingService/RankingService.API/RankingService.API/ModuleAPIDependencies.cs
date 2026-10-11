using RankingService.Application.Bases;

namespace RankingService.API
{
    public static class ModuleAPIDependencies
    {
        public static IServiceCollection AddModuleApiDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RankingDatabaseSettings>(
                 configuration.GetSection("RankingDatabase"));

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
