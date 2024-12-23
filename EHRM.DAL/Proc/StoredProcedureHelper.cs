using EHRM.DAL.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.DAL.Proc
{
    public class StoredProcedureHelper
    {
        private readonly EhrmContext _context;

        public StoredProcedureHelper(EhrmContext context)
        {
            _context = context;
        }

        public int CallStoredProcedureAbc(int param1)
        {
            var result = _context.Database
                .ExecuteSqlRaw($"EXECUTE {StoredProcedureNames.GetSubMenuAllDetails} @param1", new SqlParameter("@param1", param1));

            return result; // returns number of affected rows
        }

        // Example for a stored procedure returning a scalar value
        public int GetScalarValueFromStoredProcedure(int param1)
        {
            var result = _context.Database
                .ExecuteSqlRaw($"EXECUTE {StoredProcedureNames.Xyz} @param1", new SqlParameter("@param1", param1));

            return result;
        }

        // Add more stored procedures as needed
    }
}
