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
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(EhrmContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                _repositories[typeof(T)] = new GenericRepository<T>(_context);
            }
            return (IGenericRepository<T>)_repositories[typeof(T)];
        }

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
