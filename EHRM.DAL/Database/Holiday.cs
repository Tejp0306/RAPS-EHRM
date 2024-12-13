using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class Holiday
{
    public int Id { get; set; }

    public int? TeamId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? HolidayDate { get; set; }

    public virtual Team? Team { get; set; }
}
