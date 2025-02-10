using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Master;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Asset;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Master;
using Newtonsoft.Json.Linq;

namespace EHRM.ServiceLayer.Asset
{
    public class AssetService : IAssetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CreateAssetAsync(AssetViewModel model, int createdBy)
        {
            try
            {
                var newAsset = new AssetsDb
                {
                    Name = model.Name,
                    Category = model.Category,
                    TeamId = model.TeamId,
                    EmpId = model.EmpId,
                    Value = model.Value,
                    Status = model.Status,
                    Summary = model.Summary,
                    IssueDate = model.IssueDate,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = createdBy,
                };

                var AssetRepository = _unitOfWork.GetRepository<AssetsDb>();
                await AssetRepository.AddAsync(newAsset);
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

        public async Task<Result> DeleteAssetAsync(int id)
        {
            try
            {
                var AssetRepository = _unitOfWork.GetRepository<AssetsDb>();  // Using generic repository
                var AssetToDelete = await AssetRepository.GetByIdAsync(id);  // Fetch Asset by ID

                if (AssetToDelete == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Asset not found."
                    };
                }

                // Perform hard delete
                await AssetRepository.DeleteAsync(id);  // Call delete method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Asset deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error deleting Asset: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetEmployeeByTeamIdAsync(int id)
        {
            try
            {
                var employeeRepository = _unitOfWork.GetRepository<EmployeeDetail>();  // Using generic repository



                var ts = await employeeRepository.GetByTeamIdAsync(id);  // Fetch employees by Role ID

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
                    Message = $"Error fetching Team: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetTeamAsync()
        {
            var teamsRepository = _unitOfWork.GetRepository<Team>();
           IEnumerable<Team> teams = await teamsRepository.GetAllAsync();
            return new Result { Success = true, Data = teams };
        }
        
        public async Task<string> GetTeamByIdAsync(int id)
        {
            var teamsRepository = _unitOfWork.GetRepository<Team>();
           var team = await  teamsRepository.GetByIdAsync(id);
            string teamName = team.Name.ToString();
            return teamName;
        }

        public async Task<Result> UpdateAssetAsync(int id, int updatedBy, AssetViewModel model)
        {
            try
            {
                var AssetRepository = _unitOfWork.GetRepository<AssetsDb>();  // Using generic repository
                var existingAsset = await AssetRepository.GetByIdAsync(id);  // Fetch existing role by ID

                if (existingAsset == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Asset not found."
                    };
                }

                // Update Asset properties
                existingAsset.Name = model.Name;
                existingAsset.Category = model.Category;
                existingAsset.TeamId = model.TeamId;
                existingAsset.EmpId = model.EmpId;
                existingAsset.Value = model.Value;
                existingAsset.Status = model.Status;
                existingAsset.Summary = model.Summary;
                existingAsset.IssueDate = model.IssueDate;
                existingAsset.IsActive = true;
                existingAsset.IsDeleted = false;
                existingAsset.CreatedBy = model.CreatedBy;

                await AssetRepository.UpdateAsync(existingAsset);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Asset updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating Asset: {ex.Message}"
                };
            }
        }
        public async Task<Result> GetAllAssetAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<AssetsDb>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }

        public async Task<Result> GetAssetByIdAsync(int id)
        {
            try
            {
                var assetRepository = _unitOfWork.GetRepository<AssetsDb>();  // Using generic repository
                var asset = await assetRepository.GetByIdAsync(id);  // Fetch role by ID

                if (asset == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Asset not found."
                    };
                }

                var assetViewModel = new AssetViewModel
                {
                    Id = asset.Id,
                    Name = asset.Name,
                    Summary = asset.Summary,
                    Category = asset.Category,
                    Value = (int)asset.Value,
                    Status = asset.Status,
                    TeamId = (int)asset.TeamId,
                    EmpId = (int)asset.EmpId,
                    IssueDate = (DateTime)asset.IssueDate,
                };

                return new Result
                {
                    Success = true,
                    Data = assetViewModel
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
    }
}

