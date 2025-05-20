using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EHRM.ServiceLayer.Employee
{
    public class AccountService : IAccountService
    {

        
        private readonly IUnitOfWork _unitOfWork;
        private readonly EhrmContext _context;

        public AccountService(IUnitOfWork unitOfWork, EhrmContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public string GetFaviconPath()
        {
            var loginData = _context.CustomizeLogins.FirstOrDefault();
            if (loginData != null && !string.IsNullOrEmpty(loginData.FaviconPath))
            {
                return loginData.FaviconPath;
            }

            return "~/pic/rapslogo.png"; // default
        }

        public string GetLogoPath()
        {
            var loginData = _context.CustomizeLogins.FirstOrDefault();
            if (loginData != null && !string.IsNullOrEmpty(loginData.LogoPath))
            {
                return loginData.LogoPath;
            }

            return "~/pic/rapslogo.png"; // default
        }

        public int GetTodayNoticeCount()
        {
            var today = DateTime.Today;

            return _context.NoticeBoards
                .Where(n => EF.Functions.DateDiffDay(n.CreateDate, today) == 0)
                .Count();
        }

        public string GetTodayNoticeMessage()
        {
            var count = GetTodayNoticeCount();

            if (count == 0)
                return "📭 No new notices today !!!!";
            else if (count == 1)
                return "📢 You have 1 new notice today !!!!";
            else
                return $"📢 You have {count} new notices today !!!!";
        }


        public async Task<(bool Exists, string Email, string TempPassword, string FirstName)> ValidateEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, null, null, null); // Invalid input
            }

            // Get the repository for EmployeesCred
            var employeeCredRepository = _unitOfWork.GetRepository<EmployeesCred>();

            // Fetch the employee credentials for the given email
            var employeeCredList = await employeeCredRepository.GetEmployeeEmailPasswordByEmailAsync(email);

            // Check if no records are found
            if (employeeCredList == null || !employeeCredList.Any())
            {
                return (false, null, null, null); // Email not found
            }

            // Assuming you want the first match if multiple are found
            var employeeCred = employeeCredList.First();

            // Return the result with the details
            return (true, employeeCred.Email, employeeCred.TempPassword, employeeCred.FirstName);
        }



        public async Task<bool> CheckPasswordValidAsync(int empId, string currentPassword, string newPassword)
        {
            var EmployeeCredRepository = _unitOfWork.GetRepository<EmployeesCred>();
            // Fetch the employee Cred record from the database
            var employee = await EmployeeCredRepository.GetEmployeeCredByIdAsync(empId);

            var updatedemployeecred = employee.FirstOrDefault();
            // Check if the employee exists
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            if (updatedemployeecred.TempPassword == currentPassword)
            {
                // Update the password
                updatedemployeecred.TempPassword = newPassword;

                await EmployeeCredRepository.UpdateAsync(updatedemployeecred);

                await _unitOfWork.SaveAsync();

            }
            else
            {
                return false;
            }

            
            

            return true; // Password updated successfully
        }







    }
}


      