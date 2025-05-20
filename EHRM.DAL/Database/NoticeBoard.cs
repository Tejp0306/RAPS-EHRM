using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class NoticeBoard
{
    public int Id { get; set; }

    public string? HeadingName { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? ExpiryDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
