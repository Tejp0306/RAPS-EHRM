using EHRM.DAL.Database;
using EHRM.DAL.Repositories;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Asset;
using EHRM.ServiceLayer.MainMenuRepo;
using EHRM.ServiceLayer.Master;
//using EHRM.ServiceLayer.SubMenu;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddScoped<IMasterService, MasterService>();
            services.AddScoped<IMainMenuService, MainMenuService>();
            //services.AddScoped<ISubMenuService, SubMenuService>();
            services.AddScoped<IAssetService, AssetService>();
        }
    }
}
