using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Master;


using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Master
{
    public class MasterService : IMasterService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MasterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Create a new role
        public async Task<Result> CreateRoleAsync(RoleViewModel model, string createdBy)
        {
            try
            {
                var newRole = new Role
                {
                    RoleName = model.RoleName,
                    RoleDescription = model.RoleDescription,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = createdBy,
                    CreateDate = DateTime.Now
                };

                var roleRepository = _unitOfWork.GetRepository<Role>();
                await roleRepository.AddAsync(newRole);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Role created successfully."
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

       



        // Soft delete a role
        public async Task<Result> DeleteRoleAsync(int id)
        {
            try
            {
                var roleRepository = _unitOfWork.GetRepository<Role>();  // Using generic repository
                var roleToDelete = await roleRepository.GetByIdAsync(id);  // Fetch role by ID

                if (roleToDelete == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }

                // Perform hard delete
                await roleRepository.DeleteAsync(id);  // Call delete method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Role deleted successfully."
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

        public async Task<Result> GetAllRolesAsync()
        {
            var roleRepository = _unitOfWork.GetRepository<Role>();  // Using generic repository
            var roles = await roleRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = roles };
        }

        // Get a role by ID
        public async Task<Result> GetRoleByIdAsync(int id)
        {
            try
            {
                var roleRepository = _unitOfWork.GetRepository<Role>();  // Using generic repository
                var role = await roleRepository.GetByIdAsync(id);  // Fetch role by ID

                if (role == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }

                var roleViewModel = new RoleViewModel
                {
                    Id = role.Id,
                    RoleName = role.RoleName,
                    RoleDescription = role.RoleDescription
                };

                return new Result
                {
                    Success = true,
                    Data = roleViewModel
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


      




        // Update an existing role
        public async Task<Result> UpdateRoleAsync(int id, string updatedBy, RoleViewModel model)
        {
            try
            {
                var roleRepository = _unitOfWork.GetRepository<Role>();  // Using generic repository
                var existingRole = await roleRepository.GetByIdAsync(id);  // Fetch existing role by ID

                if (existingRole == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }

                // Update role properties
                existingRole.RoleName = model.RoleName;
                existingRole.RoleDescription = model.RoleDescription;
                existingRole.UpdatedBy = updatedBy;
                existingRole.UpdateDate = DateTime.Now;

                await roleRepository.UpdateAsync(existingRole);  // Call update method in the generic repository
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


        #region
        // Create Holiday
        public async Task<Result> CreateHolidayAsync(HolidayViewModel model, string createdBy)
        {
            try {

                var newHoliday = new Holiday
                {
                    TeamId=model.TeamId,
                    Name = model.Name,
                    Description = model.Description,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = createdBy,
                    CreateDate = DateTime.Now,
                    HolidayDate = model.HolidayDate,
                   
                };

                var holidayRepository = _unitOfWork.GetRepository<Holiday>();
                //var teams = _unitOfWork.GetRepository<Team>();
                await holidayRepository.AddAsync(newHoliday);
                await _unitOfWork.SaveAsync();





                return new Result
                {
                    Success = true,
                    Message = "Holiday created successfully."
                };

                

            }
            catch (Exception ex) {
                return new Result
                {
                    Success = false,
                    Message = $"Error creating Holiday: {ex.Message}"
                };

            }
        }

        //Get All Holiday

        public async Task<Result> GetAllHolidayAsync()
        {
            var holidayRepository = _unitOfWork.GetRepository<Holiday>();  // Using generic repository
            var holiday = await holidayRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = holiday };
        }


        //Get Holiday By Id

        public async Task<Result> GetHolidayByIdAsync(int id)
        {
            try
            {
                var holidayRepository = _unitOfWork.GetRepository<Holiday>();  // Using generic repository
                //var teamRepository = _unitOfWork.GetRepository<Team>();
                //var team = await teamRepository.GetByIdAsync(id);
                var holiday = await holidayRepository.GetByIdAsync(id);  // Fetch Holiday by ID

                if (holiday == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Holidays not found."
                    };
                }

                var holidayViewModel = new HolidayViewModel
                {
                    Id = holiday.Id,
                    Name = holiday.Name,
                    Description = holiday.Description,
                    HolidayDate = holiday.HolidayDate,
                    TeamId = holiday.TeamId,
                    //TeamName = team.Name

                };

                return new Result
                {
                    Success = true,
                    Data = holidayViewModel
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

        // Delete Holiday Data

        public async Task<Result> DeleteHolidayAsync(int id)
        {
            try
            {
                var HolidayRepository = _unitOfWork.GetRepository<Holiday>();  // Using generic repository
                var HolidayDelete = await HolidayRepository.GetByIdAsync(id);  // Fetch Notice by ID

                if (HolidayDelete == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Notice not found."
                    };
                }

                // Perform hard delete
                await HolidayRepository.DeleteAsync(id);  // Call delete method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Notice deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error deleting Notice: {ex.Message}"
                };
            }
        }
        

        // Update an existing Holiday

        public async Task<Result> UpdateHolidayAsync(int id, string updatedBy, HolidayViewModel model)
        {
            try
            {
                var holidayRepository = _unitOfWork.GetRepository<Holiday>();  // Using generic repository
                var existingholiday = await holidayRepository.GetByIdAsync(id);  // Fetch existing role by ID

                if (existingholiday == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }

                // Update role properties
                existingholiday.Name = model.Name;
                existingholiday.Description = model.Description;
                existingholiday.UpdatedBy = updatedBy;
                existingholiday.UpdateDate = DateTime.Now;

                await holidayRepository.UpdateAsync(existingholiday);  // Call update method in the generic repository
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


        // Get Teams from Database

        public async Task<Result> GetTeamAsync()
        {

            var teamsRepository = _unitOfWork.GetRepository<Team>();
            var teams = await teamsRepository.GetAllAsync();
            return new Result { Success = true, Data = teams };

        }

        #endregion
    }

}
