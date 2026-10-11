using RankingService.Application;
using RankingService.Infrastructure;

namespace RankingService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddControllers();

            builder.Services.AddModuleApiDependencies(builder.Configuration)
                .AddInfrastructureDependencies(builder.Configuration)
                .AddModuleApplicationDependencies(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            //app.UseCors("CorsPolicy");x
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
