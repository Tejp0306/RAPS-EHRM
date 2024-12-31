using EHRM.DAL.Database;
using EHRM.DAL.Repositories;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.MainMenuRepo;
using EHRM.ServiceLayer.Master;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.Infrastructure.Configurations
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EhrmContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EHRMConnection")));
            // Register Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));  // Register the generic repository
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Custom Services
            services.AddScoped<IMasterService, MasterService>();
            services.AddScoped<IMainMenuService, MainMenuService>();

            // HttpContextAccessor
            services.AddHttpContextAccessor();

            // Add JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = configuration["Jwt:Issuer"],
              ValidAudience = configuration["Jwt:Issuer"],
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
          };
      });
            // Add CORS policys
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("https://localhost:7075/") // Allow your frontend's domain
                           .AllowAnyMethod() // Allow all HTTP methods
                           .AllowAnyHeader(); // Allow all headers
                });
            });

            // Add Authorization services (moved out of the JwtBearer configuration block)
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authorized", policy => policy.RequireAuthenticatedUser());
            });
            // Add Distributed Memory Cache and Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true; // Set cookie options
                options.Cookie.IsEssential = true; // Make the session cookie essential
            });
        
    }

        }

    }
}
