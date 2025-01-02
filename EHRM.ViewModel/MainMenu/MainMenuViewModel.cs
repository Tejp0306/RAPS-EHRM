using EHRM.ViewModel.SubMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.MainMenu
{
    public class MainMenuViewModel
    {
        public int Id { get; set; } // Make this nullable
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public List<SubMenuViewModel> SubMenus { get; set; } = new();
    }
}

