using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Master;
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMasterService, MasterService>();
        }
    }
}
