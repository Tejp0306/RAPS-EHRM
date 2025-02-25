using Microsoft.AspNetCore.Mvc;
using EHRM.DAL.Database;
using EHRM.ViewModel.Hierarchy;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using EHRM.ServiceLayer.Hierarchy;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal; // For JsonPropertyName



public class HierarchyController : Controller
{
    private IHierarchyService _hierarchy;

    public HierarchyController(IHierarchyService hierarchy)
    {
        _hierarchy = hierarchy;
    }

    public IActionResult HierarchyView()
    {
        return View();
    }

    public string ShowHierarchy()
    {
        List<HierarchyViewModel> employees = _hierarchy.GetEmployeesTreeForHierarchy();
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Crucial: Use camelCase
        };

        return JsonSerializer.Serialize(employees, jsonOptions);
      

    }
}
