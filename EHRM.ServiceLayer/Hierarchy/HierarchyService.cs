using EHRM.DAL.UnitOfWork;
using EHRM.ViewModel.Employee;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EHRM.ViewModel.Hierarchy;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EHRM.ViewModel.Document;

namespace EHRM.ServiceLayer.Hierarchy
{
    public class HierarchyService : IHierarchyService
    {
    

        private readonly string _connectionString;

        

        public HierarchyService(IConfiguration configuration)
        {
            _connectionString = ConnectionStringConfiguration._DefaultConnectionString;
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("Database connection string is not configured properly.");
            }
        }

        //Get Employee Dynamic Data for the Admin Partial View

        public List<HierarchyViewModel> GetEmployeesTreeForHierarchy()
        {
            List<HierarchyViewModel> employeesforhierarchy = new List<HierarchyViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetEmployeeHierarchy", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeesforhierarchy.Add(new HierarchyViewModel
                            {
                                key = (int)reader["EmpId"], // GoJS uses 'key' for node IDs
                                name = (string)reader["EmployeeName"],
                                title = (string)reader["Title"],
                                FilePath = (string)reader["ProfileImage"],
                                parent = reader["ManagerId"] == DBNull.Value
                                          ? (int)reader["EmpId"]
                                          : (int?)reader["ManagerId"]
                            });
                        }
                    }
                }
            }
            return employeesforhierarchy;
        }


        //private string Upload(HierarchyViewModel model)
        //{
        //    // Check if a file is provided, if not, simply return null (indicating no file upload)
        //    if (model.FilePath == null || model.FilePath.Length == 0)
        //    {
        //        return null; // No file uploaded, return null or an empty string
        //    }

        //    // Define the directory path to store files
        //    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");


        //    // Create the folder if it doesn't exist
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    // Generate a unique file name to avoid name conflicts (optional, can use the original name)
        //    FileInfo fileInfo = new FileInfo(model.FilePath);
        //    string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;  // Unique file name generation
        //                                                                       // You can also use model.FileName here if you want to allow users to specify the name

        //    // Combine path with the file name to get the full file path
        //    string fileNameWithPath = Path.Combine(path, fileName);

        //    // Save the file to the specified directory
        //    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
        //    {
        //        model.FilePath.CopyTo(stream);
        //    }

        //    // Return the full file path or file name
        //    return Path.Combine("\\Files", fileName);
        //}

    }
}
