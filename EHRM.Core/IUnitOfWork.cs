using EHRM.DAL.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Role> RoleRepository { get; }
        Task SaveAsync();
    }

}
