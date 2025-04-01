using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.DAL.UnitOfWork;
using EHRM.DAL.Database;
using EHRM.ViewModel.PostJoining;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.ExitFormalities;

namespace EHRM.ServiceLayer.ExitFormalities
{
    public class ExitFormalitiesService : IExitFormalitiesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExitFormalitiesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region Exit Interview

        public async Task<Result> CreateExitInterviewFormAsync(ExitInterviewViewModel model)
        {

            try
            {
                var newExitorm = new ExitInterviewForm
                {
                    EmployeeName = model.EmployeeName,
                    InterviewDate = model.InterviewDate,
                    Interviewer = model.Interviewer,
                    Strengths = model.Strengths,
                    AreasOfImprovement = model.AreasOfImprovement,
                    TreatmentAfterResignation = model.TreatmentAfterResignation,
                    Rejoin = model.Rejoin,
                    ReasonForLeaving = model.ReasonForLeaving,
                    ComparisonWithNewJob = model.ComparisonWithNewJob,
                    Recommend = model.Recommend,
                    GreatestChallenge = model.GreatestChallenge,
                    EnjoyedFunctions = model.EnjoyedFunctions,
                    LeastEnjoyedFunctions = model.LeastEnjoyedFunctions,    
                    JobSecurity = model.JobSecurity,
                    JobSecurityDetails = model.JobSecurityDetails,  
                    DepartmentMorale = model.DepartmentMorale,
                    ImproveMorale = model.ImproveMorale,    
                    SupervisorFeedback = model.SupervisorFeedback,
                    WorkingConditions = model.WorkingConditions,
                    BenefitsSatisfactory = model.BenefitsSatisfactory,
                    InformedPolicies = model.InformedPolicies,
                    PoliciesFeedback = model.PoliciesFeedback,
                    ChangeDecision = model.ChangeDecision,
                    AdditionalComments = model.AdditionalComments,
                };

                var AssetRepository = _unitOfWork.GetRepository<ExitInterviewForm>();
                await AssetRepository.AddAsync(newExitorm);
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

        public async Task<Result> GetExitInterviewFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<ExitInterviewForm>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }

        public async Task<Result> GetExitInterviewFormByIdAsync(int Id)
        {
            try
            {
                var holidayRepository = _unitOfWork.GetRepository<ExitInterviewForm>();  // Using generic repository
                var holiday = await holidayRepository.GetByIdAsync(Id);  // Fetch Holiday by ID

                if (holiday == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Holidays not found."
                    };
                }

                var acknowledgementFormViewModel = new ExitInterviewForm
                {

                    EmployeeName = holiday.EmployeeName,
                    InterviewDate = holiday.InterviewDate,
                    Interviewer = holiday.Interviewer,
                    Strengths = holiday.Strengths,
                    AreasOfImprovement = holiday.AreasOfImprovement,
                    TreatmentAfterResignation = holiday.TreatmentAfterResignation,
                    Rejoin = holiday.Rejoin,
                    ReasonForLeaving = holiday.ReasonForLeaving,
                    ComparisonWithNewJob = holiday.ComparisonWithNewJob,
                    Recommend = holiday.Recommend,
                    GreatestChallenge = holiday.GreatestChallenge,
                    EnjoyedFunctions = holiday.EnjoyedFunctions,
                    LeastEnjoyedFunctions = holiday.LeastEnjoyedFunctions,
                    JobSecurity = holiday.JobSecurity,
                    JobSecurityDetails = holiday.JobSecurityDetails,
                    DepartmentMorale = holiday.DepartmentMorale,
                    ImproveMorale = holiday.ImproveMorale,
                    SupervisorFeedback = holiday.SupervisorFeedback,
                    WorkingConditions = holiday.WorkingConditions,
                    BenefitsSatisfactory = holiday.BenefitsSatisfactory,
                    InformedPolicies = holiday.InformedPolicies,
                    PoliciesFeedback = holiday.PoliciesFeedback,
                    ChangeDecision = holiday.ChangeDecision,
                    AdditionalComments = holiday.AdditionalComments,



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



        #region Resignation Form

        public async Task<Result> CreateResignationFormAsync(ResignationFormViewModel model)
        {
            try
            {
                var newExitorm = new ResignationForm
                {
                    EmployeeSignature = model.EmployeeSignature,
                    Position = model.Position,
                    FinalDay = model.FinalDay,
                    EmployeeName = model.EmployeeName,
                    ResignationDate = model.ResignationDate,
                    TotalMonths = model.TotalMonths,

                   
                };

                var AssetRepository = _unitOfWork.GetRepository<ResignationForm>();
                await AssetRepository.AddAsync(newExitorm);
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
        public async Task<Result> GetResignationFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<ResignationForm>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }
        public async Task<Result> GetResignationFormByIdAsync(int Id)
        {
            try
            {
                var holidayRepository = _unitOfWork.GetRepository<ResignationForm>();  // Using generic repository
                var holiday = await holidayRepository.GetByIdAsync(Id);  // Fetch Holiday by ID

                if (holiday == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Holidays not found."
                    };
                }

                var acknowledgementFormViewModel = new ResignationForm
                {

                    EmployeeName = holiday.EmployeeName,
                    Position = holiday.Position,
                    FinalDay = holiday.FinalDay,
                    TotalMonths = holiday.TotalMonths,
                    ResignationDate = holiday.ResignationDate,
                    EmployeeSignature = holiday.EmployeeSignature,




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


        #region Employee Undertaking Form

        public async Task<Result> CreateEmployeeUndertakingFormAsync(EmployeeUndertakingViewModel model)
        {
            try
            {
                var newExitorm = new EmployeeUndertakingForm
                {
                    EmployeeSignature = model.EmployeeSignature,
                    Relation = model.Relation,
                    FatherName = model.FatherName,
                    PermanentAddress = model.PermanentAddress,
                    OfficeAddress = model.OfficeAddress,
                    LastWorkingDate = model.LastWorkingDate,
                    ResignationDate = model.ResignationDate,
                    EmployeeName = model.EmployeeName



                };

                var AssetRepository = _unitOfWork.GetRepository<EmployeeUndertakingForm>();
                await AssetRepository.AddAsync(newExitorm);
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
        public async Task<Result> GetEmpUndertakingFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<EmployeeUndertakingForm>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }
        public async Task<Result> GetEmpUndertakingFormByIdAsync(int Id)
        {
            try
            {
                var holidayRepository = _unitOfWork.GetRepository<EmployeeUndertakingForm>();  // Using generic repository
                var holiday = await holidayRepository.GetByIdAsync(Id);  // Fetch Holiday by ID

                if (holiday == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Holidays not found."
                    };
                }

                var acknowledgementFormViewModel = new EmployeeUndertakingForm
                {

                    EmployeeName = holiday.EmployeeName,
                    Relation = holiday.Relation,
                    FatherName = holiday.FatherName,
                    PermanentAddress = holiday.PermanentAddress,
                    OfficeAddress = holiday.OfficeAddress,
                    LastWorkingDate = holiday.LastWorkingDate,
                    ResignationDate = holiday.ResignationDate,
                    EmployeeSignature = holiday.EmployeeSignature,




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

        #region  Exit CXhecklist

        public async Task<Result> CreateExitChecklistFormAsync(ExitChecklistViewModel model)
        {

            try
            {
                var newExitorm = new EmployeeExitChecklist
                {
                    Name = model.Name,
                    EmpId = model.EmpId,
                    ResignationDate = model.ResignationDate,
                    RelievingDate = model.RelievingDate,
                    StartDate = model.StartDate,
                    ReportingManager = model.ReportingManager,
                    CompletedTasks = model.CompletedTasks,
                    CompletedTasksRemarks = model.CompletedTasksRemarks,
                    KnowledgeTransfer = model.KnowledgeTransfer,    
                    KnowledgeTransferRemarks = model.KnowledgeTransferRemarks,
                    AssetsReturned = model.AssetsReturned,
                    AssetsReturnedRemarks = model.AssetsReturnedRemarks,
                    DocumentsReturned = model.DocumentsReturned,
                    DocumentsReturnedRemarks = model.DocumentsReturnedRemarks,
                    MailForwarding = model.MailForwarding,
                    MailForwardingRemarks = model.MailForwardingRemarks,
                    MailSentToHr = model.MailSentToHR,
                    MailSentToHrRemarks = model.MailSentToHRRemarks,
                    ReportingManagerSignature = model.ReportingManagerSignature,
                    LoginWithdrawn = model.LoginWithdrawn,
                    LoginWithdrawnRemarks = model.LoginWithdrawnRemarks,
                    JobdivaReset = model.JobdivaReset,
                    JobdivaResetRemarks = model.JobdivaResetRemarks,
                    WifiSuspended = model.WifiSuspended,
                    WifiSuspendedRemarks = model.WifiSuspendedRemarks,
                    DoorAccess = model.DoorAccess,
                    DoorAccessRemarks = model.DoorAccessRemarks,
                    RingcentralSuspended = model.RingcentralSuspended,
                    RingcentralSuspendedRemarks = model.RingcentralSuspendedRemarks,
                    VoipPhoneDeactivated = model.VoipPhoneDeactivated,
                    VoipPhoneDeactivatedRemarks = model.VoipPhoneDeactivatedRemarks,
                    ClientPasswordsChanged = model.ClientPasswordsChanged,
                    ClientPasswordsChangedRemarks = model.ClientPasswordsChangedRemarks,
                    HelpdeskSignature = model.HelpdeskSignature,
                    IdCardReturned = model.IdCardReturned,
                    IdCardReturnedRemarks = model.IdCardReturnedRemarks,
                    AccessControlUpdated = model.AccessControlUpdated,
                    AccessControlUpdatedRemarks = model.AccessControlUpdatedRemarks,
                    VisaFeeRecovery = model.VisaFeeRecovery,
                    VisaFeeRecoveryRemarks = model.VisaFeeRecoveryRemarks,
                    TransportDiscontinued = model.TransportDiscontinued,
                    TransportDiscontinuedRemarks = model.TransportDiscontinuedRemarks,
                    AdminSignature = model.AdminSignature,
                    ExitInterview = model.ExitInterview,
                    ExitInterviewRemarks = model.ExitInterviewRemarks,
                    FAndFProcessed = model.FAndFProcessed,
                    FAndFProcessedRemarks = model.FAndFProcessedRemarks,
                    TrainingCost = model.TrainingCost,
                    TrainingCostRemarks = model.TrainingCostRemarks,
                    ResignationUpdated = model.ResignationUpdated,
                    ResignationUpdatedRemarks = model.ResignationUpdatedRemarks,    
                    NameRemoved = model.NameRemoved,
                    NameRemovedRemarks = model.NameRemovedRemarks,
                    InsuranceDiscontinued = model.InsuranceDiscontinued,
                    InsuranceDiscontinuedRemarks = model.InsuranceDiscontinuedRemarks,
                    HrSignature = model.HrSignature,
                    LoansReturned = model.LoansReturned,
                    LoansReturnedRemarks = model.LoansReturnedRemarks,
                    IncentiveDue = model.IncentiveDue,
                    IncentiveDueRemarks = model.IncentiveDueRemarks,
                    DeductionsDue = model.DeductionsDue,
                    DeductionsDueRemarks = model.DeductionsDueRemarks,
                    TravelFeeDue = model.TravelFeeDue,
                    TravelFeeDueRemarks = model.TravelFeeDueRemarks,
                    TrainingCostDue = model.TrainingCostDue,
                    TrainingCostDueRemarks = model.TrainingCostDueRemarks,
                    VisaFeeDue = model.VisaFeeDue,
                    VisaFeeDueRemarks = model.VisaFeeDueRemarks,
                    AdditionalDeductions = model.AdditionalDeductions,
                    AdditionalDeductionsRemarks = model.AdditionalDeductionsRemarks,
                    AccountsManagerSignature = model.AccountsManagerSignature,

                };

                var AssetRepository = _unitOfWork.GetRepository<EmployeeExitChecklist>();
                await AssetRepository.AddAsync(newExitorm);
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

        public async Task<Result> GetExitChecklistFormAsync()
        {
            var AssetRepository = _unitOfWork.GetRepository<EmployeeExitChecklist>();  // Using generic repository
            var Asset = await AssetRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = Asset };
        }

        public async Task<Result> GetExitChecklistFormByIdAsync(int Id)
        {
            try
            {
                var holidayRepository = _unitOfWork.GetRepository<EmployeeExitChecklist>();  // Using generic repository
                var holiday = await holidayRepository.GetByIdAsync(Id);  // Fetch Holiday by ID

                if (holiday == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Holidays not found."
                    };
                }

                var acknowledgementFormViewModel = new EmployeeExitChecklist
                {

                    Name = holiday.Name,
                    EmpId = holiday.EmpId,
                    ResignationDate = holiday.ResignationDate,
                    RelievingDate = holiday.RelievingDate,
                    StartDate = holiday.StartDate,
                    ReportingManager = holiday.ReportingManager,
                    CompletedTasks = holiday.CompletedTasks,
                    CompletedTasksRemarks = holiday.CompletedTasksRemarks,
                    KnowledgeTransfer = holiday.KnowledgeTransfer,
                    KnowledgeTransferRemarks = holiday.KnowledgeTransferRemarks,
                    AssetsReturned = holiday.AssetsReturned,
                    AssetsReturnedRemarks = holiday.AssetsReturnedRemarks,
                    DocumentsReturned = holiday.DocumentsReturned,
                    DocumentsReturnedRemarks = holiday.DocumentsReturnedRemarks,
                    MailForwarding = holiday.MailForwarding,
                    MailForwardingRemarks = holiday.MailForwardingRemarks,
                    MailSentToHr = holiday.MailSentToHr,
                    MailSentToHrRemarks = holiday.MailSentToHrRemarks,
                    ReportingManagerSignature = holiday.ReportingManagerSignature,
                    LoginWithdrawn = holiday.LoginWithdrawn,
                    LoginWithdrawnRemarks = holiday.LoginWithdrawnRemarks,
                    JobdivaReset = holiday.JobdivaReset,
                    JobdivaResetRemarks = holiday.JobdivaResetRemarks,
                    WifiSuspended = holiday.WifiSuspended,
                    WifiSuspendedRemarks = holiday.WifiSuspendedRemarks,
                    DoorAccess = holiday.DoorAccess,
                    DoorAccessRemarks = holiday.DoorAccessRemarks,
                    RingcentralSuspended = holiday.RingcentralSuspended,
                    RingcentralSuspendedRemarks = holiday.RingcentralSuspendedRemarks,
                    VoipPhoneDeactivated = holiday.VoipPhoneDeactivated,
                    VoipPhoneDeactivatedRemarks = holiday.VoipPhoneDeactivatedRemarks,
                    ClientPasswordsChanged = holiday.ClientPasswordsChanged,
                    ClientPasswordsChangedRemarks = holiday.ClientPasswordsChangedRemarks,
                    HelpdeskSignature = holiday.HelpdeskSignature,
                    IdCardReturned = holiday.IdCardReturned,
                    IdCardReturnedRemarks = holiday.IdCardReturnedRemarks,
                    AccessControlUpdated = holiday.AccessControlUpdated,
                    AccessControlUpdatedRemarks = holiday.AccessControlUpdatedRemarks,
                    VisaFeeRecovery = holiday.VisaFeeRecovery,
                    VisaFeeRecoveryRemarks = holiday.VisaFeeRecoveryRemarks,
                    TransportDiscontinued = holiday.TransportDiscontinued,
                    TransportDiscontinuedRemarks = holiday.TransportDiscontinuedRemarks,
                    AdminSignature = holiday.AdminSignature,
                    ExitInterview = holiday.ExitInterview,
                    ExitInterviewRemarks = holiday.ExitInterviewRemarks,
                    FAndFProcessed = holiday.FAndFProcessed,
                    FAndFProcessedRemarks = holiday.FAndFProcessedRemarks,
                    TrainingCost = holiday.TrainingCost,
                    TrainingCostRemarks = holiday.TrainingCostRemarks,
                    ResignationUpdated = holiday.ResignationUpdated,
                    ResignationUpdatedRemarks = holiday.ResignationUpdatedRemarks,
                    NameRemoved = holiday.NameRemoved,
                    NameRemovedRemarks = holiday.NameRemovedRemarks,
                    InsuranceDiscontinued = holiday.InsuranceDiscontinued,
                    InsuranceDiscontinuedRemarks = holiday.InsuranceDiscontinuedRemarks,
                    HrSignature = holiday.HrSignature,
                    LoansReturned = holiday.LoansReturned,
                    LoansReturnedRemarks = holiday.LoansReturnedRemarks,
                    IncentiveDue = holiday.IncentiveDue,
                    IncentiveDueRemarks = holiday.IncentiveDueRemarks,
                    DeductionsDue = holiday.DeductionsDue,
                    DeductionsDueRemarks = holiday.DeductionsDueRemarks,
                    TravelFeeDue = holiday.TravelFeeDue,
                    TravelFeeDueRemarks = holiday.TravelFeeDueRemarks,
                    TrainingCostDue = holiday.TrainingCostDue,
                    TrainingCostDueRemarks = holiday.TrainingCostDueRemarks,
                    VisaFeeDue = holiday.VisaFeeDue,
                    VisaFeeDueRemarks = holiday.VisaFeeDueRemarks,
                    AdditionalDeductions = holiday.AdditionalDeductions,
                    AdditionalDeductionsRemarks = holiday.AdditionalDeductionsRemarks,
                    AccountsManagerSignature = holiday.AccountsManagerSignature,





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
