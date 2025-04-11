using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class Client
{
    public int Id { get; set; }

    public string? OrganizationId { get; set; }

    public string OrganizationName { get; set; } = null!;

    public string OrganizationType { get; set; } = null!;

    public int? OrgCapacity { get; set; }

    public string Date { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string ContactEmail { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;

    public string AdminName { get; set; } = null!;

    public string AdminUsername { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string ConfirmPasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
