using EHRM.DAL.Database;
using EHRM.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task SaveAsync();
    }

}
