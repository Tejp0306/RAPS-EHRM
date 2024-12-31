using EHRM.DAL.Database; 
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.MainMenuRepo;
using EHRM.ViewModel.Master;


using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.MainMenu;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.SubMenu;
using Microsoft.Data.SqlClient;
using System.Data;
using EHRM.ViewModel.Proc;

namespace EHRM.ServiceLayer.MainMenuRepo
{
    public class MainMenuService : IMainMenuService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public MainMenuService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }


        public async Task<Result> CreateMainMenuAsync(MainMenuViewModel model)
        {
            try
            {
                var newMainMenu = new MainMenu

                {
                    Name = model.Name,
                    Icon = model.Icon,
                    IsActive = true

                };

                var mainmenuRepository = _UnitOfWork.GetRepository<MainMenu>();
                await mainmenuRepository.AddAsync(newMainMenu);
                await _UnitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Main Menu created successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error creating role: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetAllMainMenuAsync()
        {
            var menuRepository = _UnitOfWork.GetRepository<MainMenu>();  // Using generic repository
            var menu = await menuRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = menu };
        }

        public async Task<Result> DeleteMainMenuAsync(int id)
        {
            try
            {
                var menuRepository = _UnitOfWork.GetRepository<MainMenu>();  // Using generic repository
                var roleToDelete = await menuRepository.GetByIdAsync(id);  // Fetch role by ID

                if (roleToDelete == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }

                // Perform hard delete
                await menuRepository.DeleteAsync(id);  // Call delete method in the generic repository
                await _UnitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Menu deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error deleting role: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetmainmenuByIdAsync(int id)
        {
            try
            {
                var mainmenuRepository = _UnitOfWork.GetRepository<MainMenu>();  // Using generic repository
                //var teamRepository = _unitOfWork.GetRepository<Team>();
                //var team = await teamRepository.GetByIdAsync(id);
                var mainmenu = await mainmenuRepository.GetByIdAsync(id);  // Fetch Holiday by ID

                if (mainmenu == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Main menu not found."
                    };
                }

                var mainmenuViewModel = new MainMenuViewModel
                {
                    Id = mainmenu.Id,
                    Name = mainmenu.Name,
                    Icon = mainmenu.Icon,
                    //TeamName = team.Name

                };

                return new Result
                {
                    Success = true,
                    Data = mainmenuViewModel
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error fetching role: {ex.Message}"
                };
            }
        }

        public async Task<Result> UpdateMainMenuAsync(int id, string updatedBy, MainMenuViewModel model)
        {
            try
            {
                var mainmenuRepository = _UnitOfWork.GetRepository<MainMenu>();  // Using generic repository
                var existingmainmenu = await mainmenuRepository.GetByIdAsync(id);  // Fetch existing role by ID

                if (existingmainmenu == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }

                // Update role properties
                existingmainmenu.Id = model.Id;
                existingmainmenu.Name = model.Name;
                existingmainmenu.Icon = model.Icon;

                await mainmenuRepository.UpdateAsync(existingmainmenu);  // Call update method in the generic repository
                await _UnitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Role updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating role: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetMainMenuIdAsync()
        {

            var menuRepository = _UnitOfWork.GetRepository<MainMenu>();
            var menu = await menuRepository.GetAllAsync();
            return new Result { Success = true, Data = menu };

        }

        public async Task<Result> GetRoleAsync()
        {

            var roleRepository = _UnitOfWork.GetRepository<Role>();
            var role = await roleRepository.GetAllAsync();
            return new Result { Success = true, Data = role };

        }


        public async Task<Result> GetEmployeeByRoleIdAsync(int id)
        {
            try
            {
                var employeeRepository = _UnitOfWork.GetRepository<EmployeeDetail>();  // Using generic repository



                var ts = await employeeRepository.GetByRoleIdAsync(id);  // Fetch employees by Role ID

                if (ts == null || !ts.Any())
                {
                    return new Result
                    {
                        Success = false,
                        Message = "No employees found for the specified role."
                    };
                }

                // Map all items to a list of SubMenu objects
                var SubMenus = ts.Select(employee => new EmployeeName
                {
                    EmpId = employee.EmpId,
                    FullName = employee.FirstName + " " + employee.LastName,
                }).ToList();


                return new Result
                {
                    Success = true,
                    Data = SubMenus
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error fetching Tem: {ex.Message}"
                };
            }
        }


        public async Task<Result> CreateSubMenuAsync(SubMenuViewModel model)
        {
            try
            {
                var newSubMenu = new SubMenu

                {
                    Name = model.Name,
                    Action = model.Action,
                    Controller = model.Controller,
                    MainMenuId = model.MainMenuId,
                    RoleId = model.RoleId,
                    EmpId = model.EmpId,
                    IsActive = true



                };

                var submenuRepository = _UnitOfWork.GetRepository<SubMenu>();
                await submenuRepository.AddAsync(newSubMenu);
                await _UnitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Main Menu created successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error creating role: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetSubMenuAsync()
        {
            var menuRepository = _UnitOfWork.GetRepository<SubMenu>();  // Using generic repository
            var submenu = await menuRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = submenu };
        }

        public async Task<List<SubMenuProcDetails>> CallSubMenuStoredProcedureAsync(int Id)
        {
            // Create the SqlParameter with the name '@Id' (this should match the parameter name in the stored procedure)
            var param1Param = new SqlParameter("@Id", SqlDbType.Int) { Value = Id };

            // Call the stored procedure through the UnitOfWork, passing the parameter
            return await _UnitOfWork.ExecuteStoredProcedureAsync(param1Param);
        }

        public async Task<Result> DeleteSubMenuAsync(int id)
        {
            try
            {
                var menuRepository = _UnitOfWork.GetRepository<SubMenu>();  // Using generic repository
                var roleToDelete = await menuRepository.GetByIdAsync(id);  // Fetch role by ID

                if (roleToDelete == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }

                // Perform hard delete
                await menuRepository.DeleteAsync(id);  // Call delete method in the generic repository
                await _UnitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Menu deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error deleting role: {ex.Message}"
                };
            }
        }


        public async Task<Result> GetsubmenuByIdAsync(int id)
        {
            try
            {
                var mainmenuRepository = _UnitOfWork.GetRepository<SubMenu>();  // Using generic repository
                //var teamRepository = _unitOfWork.GetRepository<Team>();
                //var team = await teamRepository.GetByIdAsync(id);
                var submenu = await mainmenuRepository.GetByIdAsync(id);  // Fetch Holiday by ID

                if (submenu == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Main menu not found."
                    };
                }

                var submenuViewModel = new SubMenuViewModel
                {
                    Id = submenu.Id,
                    Name=submenu.Name,
                    Action = submenu.Action,
                    Controller = submenu.Controller,
                    EmpId = submenu.EmpId,
                    MainMenuId =submenu.MainMenuId,
                    RoleId = submenu.RoleId

                };

                return new Result
                {
                    Success = true,
                    Data = submenuViewModel
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error fetching role: {ex.Message}"
                };
            }
        }


        public async Task<Result> UpdateSubMenuAsync(int id, string updatedBy, SubMenuViewModel model)
        {
            try
            {
                var mainmenuRepository = _UnitOfWork.GetRepository<SubMenu>();  // Using generic repository
                var existingsubmenu = await mainmenuRepository.GetByIdAsync(id);  // Fetch existing role by ID

                if (existingsubmenu == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }

                //Update Submenu properties
                existingsubmenu.Id = model.Id;
                existingsubmenu.Name = model.Name;
                existingsubmenu.Action = model.Action;
                existingsubmenu.Controller = model.Controller;
                existingsubmenu.MainMenuId = model.MainMenuId;
                existingsubmenu.RoleId = model.RoleId;
                existingsubmenu.EmpId = model.EmpId;

                await mainmenuRepository.UpdateAsync(existingsubmenu);  // Call update method in the generic repository
                await _UnitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Submenu updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating role: {ex.Message}"
                };
            }
        }


    }
}