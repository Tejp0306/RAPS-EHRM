using EHRM.DAL.Database;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Self
{
    public interface ISelfService
    {
        Task<List<EmployeeDetail>> GetDetailsByEmpIdDOB(int EmpId, string DOB);

        //Task<Result> GetDataIdAsync(int EmpId);
        Task<List<GetAllEmployeeViewModel>> GetAllSelfEmployeeRecordDetails(int EmpId);

        Task<List<GetAllEmployeeViewModel>> GetAllEmployeeDataDetails(int EmpId);

    }
}
