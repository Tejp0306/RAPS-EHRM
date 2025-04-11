using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Master;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.PostJoining;
using EHRM.ViewModel.Self;
using EHRM.ViewModel.SuperAdmin;
using Microsoft.EntityFrameworkCore;

namespace EHRM.ServiceLayer.SuperAdmin
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SuperAdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        #region Tenant Registration

        public async Task<Result> CreateTenantRegistrationFormAsync(TenantRegistrationViewModel model)
        {
            try
            {
                if (model.PasswordHash != model.ConfirmPasswordHash)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Password and Confirm Password do not match."
                    };
                }

                // Generate OrganizationID
                string organizationID = GenerateOrganizationID(model.OrgName);

                var newTenForm = new Client
                {
                    Id = model.Id,
                    OrganizationId = organizationID,
                    OrganizationName = model.OrgName,
                    OrgCapacity = model.OrgCapacity,
                    OrganizationType = model.OrgType,
                    Date = model.Date,
                    PostalCode = model.PostalCode,
                    Address = model.Address,
                    City = model.City,
                    Country = model.Country,
                    ContactEmail = model.Email,
                    MobileNumber = model.Mobile,
                    AdminName = model.PrimaryAdminName,
                    AdminUsername = model.AdminUsername,
                    PasswordHash = model.PasswordHash,
                    ConfirmPasswordHash = model.ConfirmPasswordHash,
                };

                var TenantRepository = _unitOfWork.GetRepository<Client>();
                await TenantRepository.AddAsync(newTenForm);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Tenant Form created successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error creating Tenant Form : {ex.Message}"
                };
            }
        }

        //method to generate OrganizationID
        private string GenerateOrganizationID(string orgName)
        {
            // Get the first two characters of each word in the organization name
            var words = orgName.Split(' ');
            string prefix = string.Empty;

            if (words.Length == 1)
            {
                // Take the first two characters of the single word
                prefix = words[0].Substring(0, 2).ToUpper();
            }
            else if (words.Length >= 2)
            {
                // Take the first character from each of the first two words
                prefix = words[0][0].ToString().ToUpper() + words[1][0].ToString().ToUpper();
            }

            // Format the current date as ddMM (e.g., 0704 for April 7th)
            var date = DateTime.Now.ToString("ddMM");

            // Combine the prefix and the date to create the OrganizationID
            return prefix + date;
        }

        public async Task<Result> UpdateTenantFormAsync(int id, TenantRegistrationViewModel model)
        {
            try
            {
                var TeamScreenRepository = _unitOfWork.GetRepository<Client>();  // Using generic repository
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
                existingTeam.OrganizationName = model.OrgName;
                existingTeam.OrgCapacity = model.OrgCapacity;
                existingTeam.OrganizationType = model.OrgType;
                existingTeam.Date = model.Date;
                existingTeam.PostalCode = model.PostalCode;
                existingTeam.Address = model.Address;
                existingTeam.City = model.City;
                existingTeam.Country = model.Country;
                existingTeam.ContactEmail = model.Email;
                existingTeam.MobileNumber = model.Mobile;
                existingTeam.AdminName = model.PrimaryAdminName;
                existingTeam.AdminUsername = model.AdminUsername;
                existingTeam.PasswordHash = model.PasswordHash;
                existingTeam.ConfirmPasswordHash = model.ConfirmPasswordHash;
        

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

        public async Task<Result> GetTenantRegistrationFormAsync(int id)
        {

            try
            {
                var TenantRegistrationRepository = _unitOfWork.GetRepository<Client>();  // Using generic repository
                var tR = await TenantRegistrationRepository.GetByIdAsync(id);  // Fetch role by ID

                if (tR == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Tenant Registration not found."
                    };
                }

                var TenanatRegistrationViewModel = new TenantRegistrationViewModel
                {
                    Id = tR.Id,
                    OrganizationID = tR.OrganizationId,
                    OrgName = tR.OrganizationName,
                    OrgCapacity = tR.OrgCapacity,
                    OrgType = tR.OrganizationType,
                    Date = tR.Date,
                    Address = tR.Address,
                    PostalCode = tR.PostalCode,
                    City = tR.City,
                    Country = tR.Country,
                    Email = tR.ContactEmail,
                    Mobile = tR.MobileNumber,
                    PasswordHash = tR.PasswordHash, 
                    PrimaryAdminName = tR.AdminName,
                    AdminUsername = tR.AdminUsername,
                    ConfirmPasswordHash = tR.ConfirmPasswordHash,
                };

                return new Result
                {
                    Success = true,
                    Data = TenanatRegistrationViewModel
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error fetching Registration Form: {ex.Message}"
                };
            }
        }

        public async Task<Result> GetTenantFormAsync()
        {
            var TenantRepository = _unitOfWork.GetRepository<Client>();  
            var Tenant = await TenantRepository.GetAllAsync();  
            return new Result { Success = true, Data = Tenant };
        }

        public async Task<Result> GetTenantDetailsAsync(int Id)
        {
            try
            {
                var holidayRepository = _unitOfWork.GetRepository<Client>();  // Using generic repository
                var holiday = await holidayRepository.GetByIdAsync(Id);  // Fetch Holiday by ID

                if (holiday == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Holidays not found."
                    };
                }

                var acknowledgementFormViewModel = new TenantRegistrationViewModel
                {

                    OrgName = holiday.OrganizationName,
                    OrgCapacity = holiday.OrgCapacity,
                    OrgType = holiday.OrganizationType,
                    Date = holiday.Date,
                    Address = holiday.Address,
                    PostalCode = holiday.PostalCode,
                    City = holiday.City,
                    Country = holiday.Country,
                    Email = holiday.ContactEmail,
                    Mobile = holiday.MobileNumber,
                    PasswordHash = holiday.PasswordHash,
                    PrimaryAdminName = holiday.AdminName,
                    AdminUsername = holiday.AdminUsername,
                    ConfirmPasswordHash = holiday.ConfirmPasswordHash,


                };

                return new Result
                {
                    Success = true,
                    Data = acknowledgementFormViewModel
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

        #endregion
    }
}
