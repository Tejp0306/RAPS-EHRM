using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Hierarchy
{
    public class HierarchyViewModel
    {
        public int key { get; set; } // GoJS requires 'key' for node IDs
        public string name { get; set; }
        public string title { get; set; }

        public string FilePath { get; set; } = null!;

        public int? parent { get; set; } // Nullable int for top-level nodes
    }
}
