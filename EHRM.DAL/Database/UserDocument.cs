using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;
//push to master
public partial class UserDocument
{
    public int DocumentId { get; set; }

    public int EmployeeId { get; set; }

    public string DocumentType { get; set; } = null!;

    public string? Description { get; set; }

    public string FilePath { get; set; } = null!;

    public DateTime? UploadedAt { get; set; }

    public virtual EmployeeDetail Employee { get; set; } = null!;
}
