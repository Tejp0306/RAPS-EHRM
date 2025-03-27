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
using EHRM.ViewModel.PostJoining;
using EHRM.ViewModel.Self;
using Microsoft.EntityFrameworkCore;

namespace EHRM.ServiceLayer.PostJoining
{
    public class PostJoiningService : IPostJoiningService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostJoiningService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region Acknowldegement Form

        public async Task<Result> CreateAcknowldegementFormAsync(AcknowledgementFormViewModel model)
        {

            try
            {
                var newAckForm = new AcknowledgementForm
                {
                    EmployeeName = model.EmployeeName,
                    SignatureDate = model.SignatureDate,
                    EmployeeSignature = model.EmployeeSignature,
                };

                var AssetRepository = _unitOfWork.GetRepository<AcknowledgementForm>();
                await AssetRepository.AddAsync(newAckForm);
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
        public async Task<Result> GetAcknowldegementFormByIdAsync(int Id)
        {
            try
            {
                var holidayRepository = _unitOfWork.GetRepository<AcknowledgementForm>();  // Using generic repository
                var holiday = await holidayRepository.GetByIdAsync(Id);  // Fetch Holiday by ID

                if (holiday == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Holidays not found."
                    };
                }

                var acknowledgementFormViewModel = new AcknowledgementFormViewModel
                {
                  
                    EmployeeName = holiday.EmployeeName,
                    SignatureDate = holiday.SignatureDate,
                    EmployeeSignature = holiday.EmployeeSignature
             

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


        public async Task<Result> GetAllAcknowledgeFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<AcknowledgementForm>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }

        public async Task<Result> GetAcknowledgeFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<AcknowledgementForm>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }


        #endregion


        #region Personal Information

        public async Task<Result> CreatePersonalInformationFormAsync(PersonalInfomationViewModel model)
        {
            try
            {
                var newPersonalInfo = new PersonalInfo
                {
                    EmployeeName = model.EmployeeName,
                    PersonalEmail = model.PersonalEmail,
                    PermanentAddress = model.PermanentAddress,
                    CurrentAddress = model.CurrentAddress,
                    HomePhone = model.HomePhone,
                    MobilePhone = model.MobilePhone,
                    EmergencyContact1Name=model.EmergencyContact1Name,
                    EmergencyContact1Relationship = model.EmergencyContact1Relationship,
                    EmergencyContact1Phone = model.EmergencyContact1Phone,
                    EmergencyContact2Name=model.EmergencyContact2Name, 
                    EmergencyContact2Phone = model.EmergencyContact2Phone,
                    EmergencyContact2Relationship = model.EmergencyContact2Relationship,
                    Signature = model.Signature,
                    FormDate = model.FormDate,

                 
                };

                var AssetRepository = _unitOfWork.GetRepository<PersonalInfo>();
                await AssetRepository.AddAsync(newPersonalInfo);
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

        public async Task<Result> GetAllPersonalInformationFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<PersonalInfo>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }

        public async Task<Result> GetPersonalInfoByIdAsync(int Id)
        {
            try
            {
                var personalInfoRepository = _unitOfWork.GetRepository<PersonalInfo>();  // Using generic repository
                var personalInfo = await personalInfoRepository.GetByIdAsync(Id);  // Fetch Holiday by ID

                if (personalInfo == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Holidays not found."
                    };
                }

                var personalInfoFormViewModel = new PersonalInfomationViewModel
                {

                    EmployeeName = personalInfo.EmployeeName,
                    PersonalEmail = personalInfo.PersonalEmail,
                    PermanentAddress = personalInfo.PermanentAddress,
                    CurrentAddress = personalInfo.CurrentAddress,
                    HomePhone = personalInfo.HomePhone,
                    MobilePhone = personalInfo.MobilePhone,
                    EmergencyContact1Name = personalInfo.EmergencyContact1Name,
                    EmergencyContact1Relationship = personalInfo.EmergencyContact1Relationship,
                    EmergencyContact1Phone = personalInfo.EmergencyContact1Phone,
                    EmergencyContact2Name = personalInfo.EmergencyContact2Name,
                    EmergencyContact2Relationship = personalInfo.EmergencyContact2Relationship,
                    EmergencyContact2Phone = personalInfo.EmergencyContact2Phone,
                    Signature = personalInfo.Signature,
                    FormDate = personalInfo.FormDate,

                };

                return new Result
                {
                    Success = true,
                    Data = personalInfoFormViewModel
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

        public async Task<Result> GetPersonalInfoFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<PersonalInfo>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }

        #endregion


        #region Client Propert Declaration

        public async Task<Result> CreateClientPropertDeclarationFormAsync(ClientPropertyDeclarationViewModel model)
        {
            try
            {
                var newClientPropDec = new ClientPropertyDeclaration
                {
                    EmployeeName = model.EmployeeName,
                    ClientName = model.ClientName,
                    ReceivedDate = model.ReceivedDate,
                    ItemsReceived = model.ItemsReceived,
                    EmployeeNameConfirm = model.EmployeeNameConfirm,
                    Signature = model.Signature,
                    ConfirmationDate = model.ConfirmationDate,
                


                };

                var AssetRepository = _unitOfWork.GetRepository<ClientPropertyDeclaration>();
                await AssetRepository.AddAsync(newClientPropDec);
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

        public async Task<Result> GetAllClientPropertDeclarationFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<ClientPropertyDeclaration>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }

        public async Task<Result> GetClientPropertDeclarationByIdAsync(int Id)
        {
            try
            {
                var personalInfoRepository = _unitOfWork.GetRepository<ClientPropertyDeclaration>();  // Using generic repository
                var personalInfo = await personalInfoRepository.GetByIdAsync(Id);  // Fetch Holiday by ID

                if (personalInfo == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Holidays not found."
                    };
                }

                var personalInfoFormViewModel = new ClientPropertyDeclarationViewModel
                {

                    EmployeeName = personalInfo.EmployeeName,
                    ClientName = personalInfo.ClientName,
                    ReceivedDate = personalInfo.ReceivedDate,
                    ItemsReceived = personalInfo.ItemsReceived,
                    EmployeeNameConfirm = personalInfo.EmployeeNameConfirm,
                    Signature = personalInfo.Signature,
                    ConfirmationDate = personalInfo.ConfirmationDate,
                

                };

                return new Result
                {
                    Success = true,
                    Data = personalInfoFormViewModel
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

        public async Task<Result> GetClientPropertDeclarationFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<ClientPropertyDeclaration>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }

        #endregion


        #region NDA FORM

        public async Task<Result> CreateNDAFormAsync(NDAFormViewModel model)
        {

            try
            {
                var newNDA = new NonDisclosureAgreement
                {
                    EmployeeName = model.EmployeeName,
                    Signature = model.Signature,
                    AgreementDate = model.AgreementDate,
            



                };

                var AssetRepository = _unitOfWork.GetRepository<NonDisclosureAgreement>();
                await AssetRepository.AddAsync(newNDA);
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

        public async Task<Result> GetAllNDAFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<NonDisclosureAgreement>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }

        public async Task<Result> GetNDAFormByIdAsync(int Id)
        {
            try
            {
                var personalInfoRepository = _unitOfWork.GetRepository<NonDisclosureAgreement>();  // Using generic repository
                var personalInfo = await personalInfoRepository.GetByIdAsync(Id);  // Fetch Holiday by ID

                if (personalInfo == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Holidays not found."
                    };
                }

                var personalInfoFormViewModel = new NDAFormViewModel
                {

                    EmployeeName = personalInfo.EmployeeName,
                    AgreementDate = personalInfo.AgreementDate,
                    Signature = personalInfo.Signature,


                };

                return new Result
                {
                    Success = true,
                    Data = personalInfoFormViewModel
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

        public async Task<Result> GetNDAFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<NonDisclosureAgreement>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }
        #endregion
    }
}
