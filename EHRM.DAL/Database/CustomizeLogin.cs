using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class CustomizeLogin
{
    public int Id { get; set; }

    public string? OrganizationName { get; set; }

    public string? Bio { get; set; }

    public string? LogoPath { get; set; }

    public string? FaviconPath { get; set; }
}
