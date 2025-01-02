using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public async Task<Result> SavePersonalInfoAsync(EmployeeViewModel model, String createdById, String filepath)
        {
            try
            {
                var newEmployeeDetails = new EmployeeDetail

                {
                    AadharNumber = model.AadharNumber,
                    CellPhone = model.CellPhone,
                    City = model.City,
                    Country = model.Country,
                    DateOfBirth = model.DateOfBirth,
                    EmailAddress = model.EmailAddress,
                    EmpId = model.EmpId,
                    FirstName = model.FirstName,
                    Gender = model.Gender,
                    HomePhone = model.HomePhone,
                    LastName = model.LastName,
                    MaritalStatus=model.MaritalStatus,
                    MarriageAnniversary=model.MarriageAnniversary,
                    MiddleName=model.MiddleName,
                    Nationality=model.Nationality,
                    OfficePhone=model.OfficePhone,
                    Password=model.Password,
                    PrefixName=model.PrefixName,
                    Image = filepath,
                    RoleId= model.RoleId,
                    Street =model.Street,
                    TeamId =model.TeamId,
                    Title=model.Title,
                    ZipCode=model.ZipCode,
                    LoginId=model.LoginId,
                    



                };

                var employeedetailRepository = _UnitOfWork.GetRepository<EmployeeDetail>();
                await employeedetailRepository.AddAsync(newEmployeeDetails);
                await _UnitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Employee created successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error creating Employee: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetRoleAsync()
        {

            var roleRepository = _UnitOfWork.GetRepository<Role>();
            var role = await roleRepository.GetAllAsync();
            return new Result { Success = true, Data = role };

        }

        public async Task<Result> GetTeamAsync()
        {

            var teamsRepository = _UnitOfWork.GetRepository<Team>();
            var teams = await teamsRepository.GetAllAsync();
            return new Result { Success = true, Data = teams };

        }

        

        public async Task<Result> GetEmployeeDataAsync()
        {
            var employeeRepository = _UnitOfWork.GetRepository<EmployeeDetail>();  // Using generic repository
            var employee = await employeeRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = employee };
        }




    }
}
