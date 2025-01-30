using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class ProbationEvaluationForm
{
    public int Id { get; set; }

    public int EmpId { get; set; }

    public string? ApplicationDate { get; set; }

    public int ManagerId { get; set; }

    public int? QuestionId { get; set; }

    public int? Evaluation1stMonth { get; set; }

    public int? Evaluation2ndMonth { get; set; }

    public int? Evaluation3rdMonth { get; set; }

    public string? Recommendation { get; set; }

    public string? RemarksConfirmation { get; set; }

    public string? ManagerSignature { get; set; }

    public string? FinalDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual EmployeeDetail Emp { get; set; } = null!;
}
