using EHRM.DAL.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EhrmContext _context;

        public RoleRepository(EhrmContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync() =>
            await _context.Roles.ToListAsync();

        public async Task<Role> GetRoleByIdAsync(int id) =>
            await _context.Roles.FindAsync(id);

        public async Task AddRoleAsync(Role role) =>
            await _context.Roles.AddAsync(role);

        public async Task UpdateRoleAsync(Role role) =>
            _context.Roles.Update(role);

        public async Task DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }
        }
    }
}
