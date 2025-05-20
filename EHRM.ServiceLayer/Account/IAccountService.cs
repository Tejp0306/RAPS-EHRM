using EHRM.DAL.Database;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
using EHRM.ViewModel.Master;


namespace EHRM.ServiceLayer.Employee
{
    public interface IAccountService
    {
        // Method to validate if the email exists in the system
        

        Task<(bool Exists, string Email, string TempPassword, string FirstName)> ValidateEmailAsync(string email);

        Task<bool> CheckPasswordValidAsync(int empId, string currentPassword, string newPassword);

        string GetFaviconPath();
        string GetLogoPath();
        int GetTodayNoticeCount();
        string GetTodayNoticeMessage(); // Optional for user-friendly display


    }
}