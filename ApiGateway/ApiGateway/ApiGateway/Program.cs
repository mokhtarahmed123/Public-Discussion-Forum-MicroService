using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services
         .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.MapInboundClaims = false;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               RoleClaimType = "roleName",
               NameClaimType = "UserName",
               ValidateIssuer = true,
               ValidIssuer = builder.Configuration["JWT:IssuerIP"],
               ValidateAudience = true,
               ValidAudience = builder.Configuration["JWT:AudienceIP"],
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!)),
               ClockSkew = TimeSpan.Zero
           };


       });

            builder.Services.AddAuthorization(o =>
              o.AddPolicy("admin", p => p.RequireRole("Admin")));

            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));



            var app = builder.Build();



            app.UseAuthentication();
            app.UseAuthorization();

            app.MapReverseProxy();

            app.Run();
        }
    }
}
