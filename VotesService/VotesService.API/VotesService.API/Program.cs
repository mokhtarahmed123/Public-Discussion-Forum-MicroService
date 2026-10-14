using VotesService.Application;
namespace VotesService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
            });

            builder.Services.AddModuleApiDependencies(builder.Configuration).
                AddApplicationDependencies(builder.Configuration).
                AddInfrastructureDependencies(builder.Configuration);

            var app = builder.Build();


            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}
<<<<<<< HEAD
            //app.UseCors("CorsPolicy");
            app.UseAuthentication();
=======
            app.UseCors("CorsPolicy");
>>>>>>> 6ce00c0b2538e159445141615b97fd4ad1221beb
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
