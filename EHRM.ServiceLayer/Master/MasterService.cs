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
                    Message = $"Error fetching role: {ex.Message}"
                };
            }
        }

            {
                try
                {
                    var EmpType = new EmpType
                    {
                        EmpType1 = model.EmpType1,
                        //RoleDescription = model.RoleDescription,
                        IsActive = true,
                        //IsDeleted = false,
                        //CreatedBy = createdBy,
                        //CreateDate = DateTime.Now
                    };

                    var EmpTypeRepository = _unitOfWork.GetRepository<EmpType>();
                    await EmpTypeRepository.AddAsync(EmpType);
                    await _unitOfWork.SaveAsync();

                    return new Result
                    {
                        Success = true,
                        Message = "Employee Type created successfully."
                    };
                }
                catch (Exception ex)
                {
                    return new Result
                    {
                        Success = false,
                        Message = $"Error creating Employee Type: {ex.Message}"
                    };
                }
            }
        }

        public async Task<Result> UpdateEmployeeTypeAsync(int id, EmployeeTypeViewModel model)
        {
            try
            {
                var EmpTypeRepository = _unitOfWork.GetRepository<EmpType>();  // Using generic repository
                var existingEmpType = await EmpTypeRepository.GetByIdAsync(id);  // Fetch existing role by ID

                if (existingEmpType == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Employee Type not found."
                    };
                }

                // Update role properties
                existingEmpType.EmpType1 = model.EmpType1;
                //existingRole.RoleDescription = model.RoleDescription;
                //existingRole.UpdatedBy = updatedBy;
                //existingRole.UpdateDate = DateTime.Now;

                await EmpTypeRepository.UpdateAsync(existingEmpType);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Employee Type updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating Employee Type: {ex.Message}"
                };
            }
        }

        public async Task<Result> DeleteEmployeeTypeAsync(int id)
        {
            try
            {
                var EmpTypeRepository = _unitOfWork.GetRepository<EmpType>();  // Using generic repository
                var EmpTypeToDelete = await EmpTypeRepository.GetByIdAsync(id);  // Fetch role by ID

                if (EmpTypeToDelete == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Employee Type not found."
                    };
                }

                // Perform hard delete
                await EmpTypeRepository.DeleteAsync(id);  // Call delete method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Employee Type deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error deleting Employee Type: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetEmployeeTypeIdAsync(int id)
        {
            try
            {
                var EmpTypeRepository = _unitOfWork.GetRepository<EmpType>();  // Using generic repository
                var et = await EmpTypeRepository.GetByIdAsync(id);  // Fetch role by ID

                if (et == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Employee Type not found."
                    };
                }

                var EmployeeTypeViewModel = new EmployeeTypeViewModel
                {
                    Id = et.Id,
                    EmpType1 = et.EmpType1,
                    //RoleDescription = role.RoleDescription
                };

                return new Result
                {
                    Success = true,
                    Data = EmployeeTypeViewModel
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error fetching Employee Type: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetAllEmployeeTypeAsync()
        {
            var EmpTypeRepository = _unitOfWork.GetRepository<EmpType>();  // Using generic repository
            var et = await EmpTypeRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = et };
        }
        #endregion
                {
                    Success = true,
                    Message = "Notice created successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error creating Notice: {ex.Message}"
                };
            }
        }

        public async  Task<Result> UpdateAddNoticeBoardAsync(int id, int updatedBy, AddNoticeBoardViewModel model)
        {
            try
            {
                var NoticeBoardRepository = _unitOfWork.GetRepository<NoticeBoard>();  // Using generic repository
                var existingNotiecBoard = await NoticeBoardRepository.GetByIdAsync(id);  // Fetch existing Notice by ID

                if (existingNotiecBoard == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Notice not found."
                    };
                }

                // Update NoticeBoard properties
                existingNotiecBoard.HeadingName = model.HeadingName;
                existingNotiecBoard.Description = model.Description;
                existingNotiecBoard.UpdatedBy = updatedBy;
                existingNotiecBoard.UpdateDate = DateTime.Now;

                await NoticeBoardRepository.UpdateAsync(existingNotiecBoard);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Notice updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating Notice: {ex.Message}"
                };
            }
        }

        public async Task<Result> DeleteAddNoticeBoardAsync(int id)
        {
            try
            {
                var NoticeBoardRepository = _unitOfWork.GetRepository<NoticeBoard >();  // Using generic repository
                var NoticeBoardToDelete = await NoticeBoardRepository.GetByIdAsync(id);  // Fetch Notice by ID

                if (NoticeBoardToDelete == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Notice not found."
                    };
                }

                // Perform hard delete
                await NoticeBoardRepository.DeleteAsync(id);  // Call delete method in the generic repository
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

        public async Task<Result> GetAllAddNoticeBoardAsync()
        {
            {
                var NoticeBoardRepository = _unitOfWork.GetRepository<NoticeBoard>();  // Using generic repository
                var NoticeBoard = await NoticeBoardRepository.GetAllAsync();  // Fetch all Notices
                return new Result { Success = true, Data = NoticeBoard };
            }
        }

        public async  Task<Result> GetAllAddNoticeBoardByIdAsync(int id)
        {

            try
            {
                var NoticeBoardRepository = _unitOfWork.GetRepository<NoticeBoard>();  // Using generic repository
                var nb= await NoticeBoardRepository.GetByIdAsync(id);  // Fetch role by ID

                if (nb == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Notice not found."
                    };
                }

                var AddNoticeBoardViewModel = new AddNoticeBoardViewModel
                {
                    Id = nb.Id,
                    HeadingName = nb.HeadingName,
                    Description = nb.Description,
                    FilePath = nb.Image
                };

                return new Result
                {
                    Success = true,
                    Data = AddNoticeBoardViewModel
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

        public async Task<string> GetFilePathByIdAsync(int id)
        {
            try
            {
                var NoticeBoardRepository = _unitOfWork.GetRepository<NoticeBoard>();  // Using generic repository
                var nb = await NoticeBoardRepository.GetByIdAsync(id);  // Fetch role by ID
                string FilePath = nb.Image.ToString();

                return FilePath;
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion
    }
}
