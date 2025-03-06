using EHRM.ViewModel.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Hierarchy
{
    public interface IHierarchyService
    {
        List<HierarchyViewModel> GetEmployeesTreeForHierarchy();
    }
}
