using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Master;
using System;
using System.Collections.Generic;
using System.Linq;
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



        #region Team Screen
        public async Task<Result> CreateTeamAsync(TeamScreenViewModel model, int createdBy)
        {
            try
            {
                var newTeam = new Team
                {
                    Name = model.Name,
                    Description = model.Description,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = createdBy,
                    CreateDate = DateTime.Now
                };

                var TeamScreenRepository = _unitOfWork.GetRepository<Team>();
                await TeamScreenRepository.AddAsync(newTeam);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Team created successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error creating Team: {ex.Message}"
                };
            }
        }

        public async Task<Result> UpdateTeamAsync(int id, int updatedBy, TeamScreenViewModel model)
        {
            try
            {
                var TeamScreenRepository = _unitOfWork.GetRepository<Team>();  // Using generic repository
                var existingTeam = await TeamScreenRepository.GetByIdAsync(id);  // Fetch existing role by ID

                if (existingTeam == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Team not found."
                    };
                }

                // Update role properties
                existingTeam.Name = model.Name;
                existingTeam.Description = model.Description;
                existingTeam.UpdatedBy = updatedBy;
                existingTeam.UpdateDate = DateTime.Now;

                await TeamScreenRepository.UpdateAsync(existingTeam);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Team updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating Team: {ex.Message}"
                };
            }
        }

        public async Task<Result> DeleteTeamAsync(int id)
        {

            try
            {
                var TeamScreenRepository = _unitOfWork.GetRepository<Team>();  // Using generic repository
                var TeamScreenToDelete = await TeamScreenRepository.GetByIdAsync(id);  // Fetch Notice by ID

                if (TeamScreenToDelete == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Team not found."
                    };
                }

                // Perform hard delete
                await TeamScreenRepository.DeleteAsync(id);  // Call delete method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Team deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error deleting Team: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetTeamByIdAsync(int id)
        {
            try
            {
                var TeamScreenRepository = _unitOfWork.GetRepository<Team>();  // Using generic repository
                var ts = await TeamScreenRepository.GetByIdAsync(id);  // Fetch role by ID

                if (ts == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Team not found."
                    };
                }

                var TeamScreenViewModel = new TeamScreenViewModel
                {
                    Id = ts.Id,
                    Name = ts.Name,
                    Description = ts.Description
                };

                return new Result
                {
                    Success = true,
                    Data = TeamScreenViewModel
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

        public async Task<Result> GetAllTeamAsync()
        {
            var TeamScreenRepository = _unitOfWork.GetRepository<Team>();  // Using generic repository
            var team = await TeamScreenRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = team };
        }
    }

        #endregion
    
}
