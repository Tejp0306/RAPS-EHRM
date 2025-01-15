using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Enumerations
{
    public enum EmployeeType
    {
        Permanent =1,  // Default value is 0
        Temporary= 2,  // Default value is 1
        Contract = 3    // Default value is 2
    }

    public enum EmploymentStatus
    {
        Active=1,
        OnLeave =2,
        Terminated=3

    }
    public class GenericEnums
    {

    }
}
