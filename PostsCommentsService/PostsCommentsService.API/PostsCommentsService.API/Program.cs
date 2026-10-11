using PostsCommentsService.Application;
using PostsCommentsService.Infrastructure;

namespace PostsCommentsService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddModuleApiDependencies(builder.Configuration).
                AddInfrastructureDependencies(builder.Configuration).
                AddApplicationDependencies(builder.Configuration);

            var app = builder.Build();




            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}


            app.UseCors("CorsPolicy");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
