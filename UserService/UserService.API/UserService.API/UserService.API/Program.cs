namespace UserService.API;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Application;
using UserService.Domain.Entities;
using UserService.Infrastructure;
using UserService.Infrastructure.Context;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddModuleApiDependencies(builder.Configuration)
            .AddInfrastructureDependencies(builder.Configuration)
                       .AddModuleCoreDependencies();

        var app = builder.Build();
<<<<<<< HEAD
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            foreach (var roleName in new[] { "User", "Admin" })
            {
                if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                    roleManager.CreateAsync(new Role { Name = roleName }).GetAwaiter().GetResult();
            }
        }

=======

        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
>>>>>>> 6ce00c0b2538e159445141615b97fd4ad1221beb

        app.UseSwagger();
        app.UseSwaggerUI();

        //app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.Use(async (ctx, next) =>
        {
            try { await next(); }
            catch (FluentValidation.ValidationException ex)
            {
                ctx.Response.StatusCode = 400;
                await ctx.Response.WriteAsJsonAsync(new
                {
                    succeeded = false,
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }
        });

        app.MapControllers();

        app.Run();
    }
}
