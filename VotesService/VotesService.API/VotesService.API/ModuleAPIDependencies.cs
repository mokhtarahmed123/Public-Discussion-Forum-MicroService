using VotesService.Application.Bases;

namespace VotesService.API
{
    public static class ModuleAPIDependencies
    {
        public static IServiceCollection AddModuleApiDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<VotesDatabaseSettings>(
                 configuration.GetSection("VotesDatabase"));

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
