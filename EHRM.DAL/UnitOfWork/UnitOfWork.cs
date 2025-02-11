using EHRM.DAL.Database;
using EHRM.DAL.Proc;
using EHRM.DAL.Repositories;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Proc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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


        // Modified method to execute stored procedure without needing to pass the name
        public async Task<List<SubMenuProcDetails>> ExecuteStoredProcedureAsync(params SqlParameter[] parameters)
        {
            var storedProcedureName = StoredProcedureNames.GetSubMenuAllDetails;
            var results = new List<SubMenuProcDetails>();

            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = $"EXEC {storedProcedureName} {string.Join(", ", parameters.Select(p => p.ParameterName))}";
                    command.CommandType = CommandType.Text;

                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    _context.Database.OpenConnection();

      

                    using (var reader =  command.ExecuteReaderAsync().Result)
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new SubMenuProcDetails
                            {
                                Id = reader.GetInt32(0), // Map column 0 to Id
                                Action = reader.GetString(1),// Map column 1 to Name
                                Controller = reader.GetString(2),
                                MainMenuId = reader.GetInt32(3),
                                RoleId = reader.GetInt32(4),
                                EmployeeId = reader.GetInt32(5),
                                SubMenuName = reader.GetString(6),
                                MainMenuName = reader.GetString(7),
                                RoleName = reader.GetString(8),
                                EmployeeName = reader.GetString(9)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
                throw;
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            return results;
        }



    }



}
