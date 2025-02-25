using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Master;
using EHRM.DAL.Database;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Leave;
using EHRM.ViewModel.Asset;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Review;

namespace EHRM.ServiceLayer.LeaveTypes
{
    public class LeaveTypes : ILeaveTypes
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveTypes(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CreateLeaveTypeAsync(LeaveTypeViewModel model)
        {
            try
            {
                var newLeaveType = new Leavetypee
                {
                    LeaveType = model.LeaveType,
                    LeaveDescription = model.LeaveDescription,
                    IsActive = model.IsActive,
                };

                var AssetRepository = _unitOfWork.GetRepository<Leavetypee>();
                await AssetRepository.AddAsync(newLeaveType);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Asset created successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error creating Asset: {ex.Message}"
                };
            }
        }

        public async Task<Result> UpdateLeaveTypeAsync(int id, LeaveTypeViewModel model)
        {
            try
            {
                var LeaveTypeRepository = _unitOfWork.GetRepository<Leavetypee>();  // Using generic repository
                var existingLeaveType = await LeaveTypeRepository.GetByIdAsync(id);  // Fetch existing role by ID

                if (existingLeaveType == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Employee Type not found."
                    };
                }

                // Update role properties
                existingLeaveType.LeaveType = model.LeaveType;
                existingLeaveType.LeaveDescription = model.LeaveDescription;


                await LeaveTypeRepository.UpdateAsync(existingLeaveType);  // Call update method in the generic repository
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

        public async Task<Result> GetAllLeaveTypeAsync()
        {
            var LeaveTypeRepository = _unitOfWork.GetRepository<Leavetypee>();  // Using generic repository
            var lt = await LeaveTypeRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = lt };
        }

        public async Task<Result> DeleteLeaveTypeAsync(int id)
        {
            try
            {
                var LeaveTypeRepository = _unitOfWork.GetRepository<Leavetypee>();  // Using generic repository
                var LeaveTypeToDelete = await LeaveTypeRepository.GetByIdAsync(id);  // Fetch Asset by ID

                if (LeaveTypeToDelete == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Leave_Type not found."
                    };
                }

                // Perform hard delete
                await LeaveTypeRepository.DeleteAsync(id);  // Call delete method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Leave_Type deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error deleting Leave_Type: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetLeaveTypeByIdAsync(int id)
        {
            try
            {
                var leaveTypeRepository = _unitOfWork.GetRepository<Leavetypee>();  // Using generic repository
                var leave = await leaveTypeRepository.GetByIdAsync(id);  // Fetch role by ID

                if (leave == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Asset not found."
                    };
                }

                var leaveTypeViewModel = new LeaveTypeViewModel
                {
                    Id = leave.Id,
                    LeaveType=leave.LeaveType,
                    LeaveDescription=leave.LeaveDescription,

                };

                return new Result
                {
                    Success = true,
                    Data = leaveTypeViewModel
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error fetching Asset: {ex.Message}"
                };
            }
        }



        #region Leave Apply

        public async Task<Result> GetLeaveTypeAsync()
        {
            var leaveTypeRepository = _unitOfWork.GetRepository<Leavetypee>();
            var leaveType = await leaveTypeRepository.GetAllAsync();
            return new Result { Success = true, Data = leaveType };
        }

        public async Task<Result> LeaveApplyAsync(LeaveApplyViewModel model)
        {
            try
            {
                var newLeaveApply = new LeaveApply
                {
                    LeaveType = model.LeaveType,
                    Description = model.Description,
                    ApplyDate = model.ApplyDate,
                    EmployeeName = model.EmployeeName,
                    LeaveFrom = model.LeaveFrom,
                    LeaveTo = model.LeaveTo,
                    TotalDays = model.TotalDays,
                    EmpId = model.EmpId
                };

                var LeaveRepository = _unitOfWork.GetRepository<LeaveApply>();
                await LeaveRepository.AddAsync(newLeaveApply);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Leave Applied successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error Applying Leave: {ex.Message}"
                };
            }
        }

        public async Task<Result> UpdateLeaveApplyAsync(int id, LeaveApplyViewModel model)
        {
            try
            {
                var LeaveApplyRepository = _unitOfWork.GetRepository<LeaveApply>();  // Using generic repository
                var existingLeaveApply = await LeaveApplyRepository.GetByIdAsync(id);  // Fetch existing role by ID

                if (existingLeaveApply == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Employee Type not found."
                    };
                }

                // Update role properties
                existingLeaveApply.LeaveType = model.LeaveType;
                existingLeaveApply.Description = model.Description;
                existingLeaveApply.EmployeeName = model.EmployeeName;
                existingLeaveApply.ApplyDate = model.ApplyDate;
                existingLeaveApply.LeaveFrom = model.LeaveFrom;
                existingLeaveApply.LeaveTo = model.LeaveTo;
                existingLeaveApply.TotalDays = model.TotalDays;


                await LeaveApplyRepository.UpdateAsync(existingLeaveApply);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Leave is updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating Leave: {ex.Message}"
                };
            }
        }


        public async Task<List<LeaveApplyViewModel>> GetLeaveDataByEmpId(int empId)
        {
            var leaveStatusRepository = _unitOfWork.GetRepository<LeaveStatuss>();
            var LeaveApplyRepository = _unitOfWork.GetRepository<LeaveApply>();

            // Await the async operations to get actual collections
            var ls = await leaveStatusRepository.GetAllAsync();
            var la = await LeaveApplyRepository.GetAllAsync();

            // LINQ query to join LeaveApply and LeaveStatuss using EmpId
            var leaveDetails = (from a in la
                                       join s in ls on a.EmpId equals s.EmpId
                                       where a.EmpId.GetValueOrDefault() == empId 
                                       select new LeaveApplyViewModel
                                       {
                                           Id = a.Id,
                                           EmployeeName = a.EmployeeName,
                                           ApplyDate = a.ApplyDate,
                                           LeaveType = a.LeaveType,
                                           LeaveFrom = a.LeaveFrom,
                                           LeaveTo = a.LeaveTo,
                                           Description = a.Description,
                                           Status = new LeaveStatusViewModel
                                           {
                                               LeaveStatus = s.LeaveStatus,       
                                               ManagerRemark = s.ManagerRemark    
                                           }
                                       }).ToList();

            return leaveDetails;
        }


        #endregion

        #region Leave status

        public async Task<Result> GetLeaveAsync()
        {
            var LeaveRepository = _unitOfWork.GetRepository<LeaveApply>(); 
            var lt = await LeaveRepository.GetAllAsync(); 
            return new Result { Success = true, Data = lt };
        }

        public async Task<Result> LeaveStatusAsync(LeaveStatusViewModel model)
        {
            try
            {
                var newLeaveType = new LeaveStatuss
                {
                    EmpId = model.EmpId,
                    LeaveStatus = model.LeaveStatus,
                    ManagerRemark=model.ManagerRemark,
                };

                var AssetRepository = _unitOfWork.GetRepository<LeaveStatuss>();
                await AssetRepository.AddAsync(newLeaveType);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Asset created successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error creating Asset: {ex.Message}"
                };
            }
        }



        #endregion


    }



}
