using EHRM.DAL.Database;
using EHRM.DAL.Repositories;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Asset;
using EHRM.ServiceLayer.Employee;
using EHRM.ServiceLayer.Self;
using EHRM.ServiceLayer.HR;
using EHRM.ServiceLayer.MainMenuRepo;
using EHRM.ServiceLayer.Master;
using EHRM.ServiceLayer.Review;
using EHRM.ServiceLayer.Utility;
using EHRM.ServiceLayer.LeaveTypes;
using Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static Logger.LoggerService;
using EHRM.ServiceLayer.Dashboard;
using EHRM.ServiceLayer.Calendar;
using EHRM.ServiceLayer.Document;
using EHRM.ServiceLayer.Hierarchy;
using EHRM.ServiceLayer.LeaveDashBoard;
using EHRM.ServiceLayer.PostJoining;
using EHRM.ServiceLayer.ExitFormalities;
using EHRM.ServiceLayer.SuperAdmin;

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
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISelfService, SelfService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IdashboardService, Dashboardservice>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<ILeaveTypes, LeaveTypes>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IHierarchyService, HierarchyService>();
            services.AddScoped<ILeaveDashboardService, LeaveDashboardService>();
            services.AddScoped<IPostJoiningService, PostJoiningService>();
            services.AddScoped<IExitFormalitiesService, ExitFormalitiesService>();
            services.AddScoped<ISuperAdminService, SuperAdminService>();


            //services.AddScoped<ISubMenuService, SubMenuService>();
            services.AddScoped<IHrService, HrService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<IEmailService, EmailService>();
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
                      // Read token from cookies
                      options.Events = new JwtBearerEvents
                      {
                          OnMessageReceived = context =>
                          {
                              context.Token = context.Request.Cookies["JwtToken"];
                              return Task.CompletedTask;
                          }
                      };
                  });

            // Add controllers with views and apply a global authorization filter
            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
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

