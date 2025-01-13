using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Review
{
    public class EvaluationQuestion
    {
        public int Id { get; set; }

        public string Question { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public bool? IsActive { get; set; }
    }
}
