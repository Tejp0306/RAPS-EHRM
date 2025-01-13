using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class ProbationEvaluationQuestion
{
    public int QuestionId { get; set; }

    public string Question { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }
}
