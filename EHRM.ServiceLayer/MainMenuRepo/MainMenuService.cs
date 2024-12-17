using EHRM.DAL.Database; 
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.MainMenuRepo;
using EHRM.ViewModel.Master;


using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.MainMenu;

namespace EHRM.ServiceLayer.MainMenuRepo
{
    public class MainMenuService : IMainMenuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MainMenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

                var mainmenuRepository = _unitOfWork.GetRepository<MainMenu>();
                await mainmenuRepository.AddAsync(newMainMenu);
                await _unitOfWork.SaveAsync();

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
            var menuRepository = _unitOfWork.GetRepository<MainMenu>();  // Using generic repository
            var menu = await menuRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = menu };
        }

        public async Task<Result> DeleteMainMenuAsync(int id)
        {
            try
            {
                var menuRepository = _unitOfWork.GetRepository<MainMenu>();  // Using generic repository
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
                await _unitOfWork.SaveAsync();

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
                var mainmenuRepository = _unitOfWork.GetRepository<MainMenu>();  // Using generic repository
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
                var mainmenuRepository = _unitOfWork.GetRepository<MainMenu>();  // Using generic repository
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
                await _unitOfWork.SaveAsync();

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





        //Create a new role
        //    public async Task <Result> CreateMainMenuAsync(MainMenuViewModel model, string createdBy)
        //    {
        //        //return Result;
        //    //    try
        //    //    {
        //    //        var newRole = new Role
        //    //        {
        //    //            RoleName = model.RoleName,
        //    //            RoleDescription = model.RoleDescription,
        //    //            IsActive = true,
        //    //            IsDeleted = false,
        //    //            CreatedBy = createdBy,
        //    //            CreateDate = DateTime.Now
        //    //        };

        //    //        var roleRepository = _unitOfWork.GetRepository<Role>();
        //    //        await roleRepository.AddAsync(newRole);
        //    //        await _unitOfWork.SaveAsync();

        //    //        return new Result
        //    //        {
        //    //            Success = true,
        //    //            Message = "Role created successfully."
        //    //        };
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        return new Result
        //    //        {
        //    //            Success = false,
        //    //            Message = $"Error creating role: {ex.Message}"
        //    //        };
        //    //    }
        //    //}


        //}

    }

}