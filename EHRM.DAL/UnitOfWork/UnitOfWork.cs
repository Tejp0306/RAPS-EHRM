using EHRM.DAL.Database;
using EHRM.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EhrmContext _context;

        public UnitOfWork(EhrmContext context)
        {
            _context = context;
            RoleRepository = new RoleRepository(_context);
        }

        public IRoleRepository RoleRepository { get; private set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
