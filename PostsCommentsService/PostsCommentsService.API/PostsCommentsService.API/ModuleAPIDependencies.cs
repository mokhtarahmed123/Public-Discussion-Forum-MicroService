using PostsCommentsService.API.Bases;

namespace PostsCommentsService.API
{
    public static class ModuleAPIDependencies
    {
        public static IServiceCollection AddModuleApiDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PostsAndCommentsDatabaseSettings>(
                 configuration.GetSection("PostsAndCommentsDatabase"));

            #region CORS
            services.AddCors(options =>
                     options.AddPolicy("CorsPolicy", policyBuilder =>
                         policyBuilder.SetIsOriginAllowed(_ => true)

                                      .AllowAnyMethod()
                                      .AllowAnyHeader().AllowCredentials())

                 );
            #endregion
            return services;
        }
    }
}
