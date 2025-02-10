using EHRM.DAL.Database;
using EHRM.ViewModel.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.HR
{
    public interface IHrService
    {
        Task<List<EmployeeDetail>> GetDetailsByEmpIdDOB(int EmpId, string DOB);

        Task<List<GetAllEmployeeViewModel>> GetAllEmployeeRecordDetails(int EmpId);
    }
}
