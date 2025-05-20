using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Master;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Setting;
using EHRM.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EHRM.ServiceLayer.Setting
{
    public class SettingService : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SettingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region Customize Login

        public async Task<Result> CreateCustomLoginAsync(CustomizeSettingViewModel model)
        {
            try
            {
                var newNotice = new CustomizeLogin
                {
                    OrganizationName = model.OrganizationName,
                    Bio = model.Bio,
                    LogoPath = model.ExistingLogoPath,
                    FaviconPath = model.ExistingFaviconPath,

                };

                var NoticeBoardRepository = _unitOfWork.GetRepository<CustomizeLogin>();
                await NoticeBoardRepository.AddAsync(newNotice);
                await _unitOfWork.SaveAsync();

                return new Result
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

        public async Task<Result> GetCustomLoginByIdAsync(int id)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<CustomizeLogin>();
                var entry = await repository.GetByIdAsync(id);

                if (entry == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Custom login entry not found."
                    };
                }

                var viewModel = new CustomizeSettingViewModel
                {
                    Id = entry.Id,
                    OrganizationName = entry.OrganizationName,
                    Bio = entry.Bio,
                    ExistingFaviconPath = entry.FaviconPath,
                    ExistingLogoPath = entry.LogoPath
                };

                return new Result
                {
                    Success = true,
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error fetching entry: {ex.Message}"
                };
            }
        }

       
        public async Task<Result> UpdateCustomLoginAsync(int id, CustomizeSettingViewModel model)
        {
            try
            {
                var NoticeBoardRepository = _unitOfWork.GetRepository<CustomizeLogin>();  // Using generic repository
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
                existingNotiecBoard.OrganizationName = model.OrganizationName;
                existingNotiecBoard.Bio = model.Bio;
                existingNotiecBoard.FaviconPath = model.ExistingFaviconPath;
                existingNotiecBoard.LogoPath = model.ExistingLogoPath;

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

        #endregion
    }

}
