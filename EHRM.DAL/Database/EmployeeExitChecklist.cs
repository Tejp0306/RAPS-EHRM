using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class EmployeeExitChecklist
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int EmpId { get; set; }

    public string? ResignationDate { get; set; }

    public string? RelievingDate { get; set; }

    public string? StartDate { get; set; }

    public string? ReportingManager { get; set; }

    public string? CompletedTasks { get; set; }

    public string? CompletedTasksRemarks { get; set; }

    public string? KnowledgeTransfer { get; set; }

    public string? KnowledgeTransferRemarks { get; set; }

    public string? AssetsReturned { get; set; }

    public string? AssetsReturnedRemarks { get; set; }

    public string? DocumentsReturned { get; set; }

    public string? DocumentsReturnedRemarks { get; set; }

    public string? MailForwarding { get; set; }

    public string? MailForwardingRemarks { get; set; }

    public string? MailSentToHr { get; set; }

    public string? MailSentToHrRemarks { get; set; }

    public string? ReportingManagerSignature { get; set; }

    public string? LoginWithdrawn { get; set; }

    public string? LoginWithdrawnRemarks { get; set; }

    public string? JobdivaReset { get; set; }

    public string? JobdivaResetRemarks { get; set; }

    public string? WifiSuspended { get; set; }

    public string? WifiSuspendedRemarks { get; set; }

    public string? DoorAccess { get; set; }

    public string? DoorAccessRemarks { get; set; }

    public string? RingcentralSuspended { get; set; }

    public string? RingcentralSuspendedRemarks { get; set; }

    public string? VoipPhoneDeactivated { get; set; }

    public string? VoipPhoneDeactivatedRemarks { get; set; }

    public string? ClientPasswordsChanged { get; set; }

    public string? ClientPasswordsChangedRemarks { get; set; }

    public string? HelpdeskSignature { get; set; }

    public string? IdCardReturned { get; set; }

    public string? IdCardReturnedRemarks { get; set; }

    public string? AccessControlUpdated { get; set; }

    public string? AccessControlUpdatedRemarks { get; set; }

    public string? VisaFeeRecovery { get; set; }

    public string? VisaFeeRecoveryRemarks { get; set; }

    public string? TransportDiscontinued { get; set; }

    public string? TransportDiscontinuedRemarks { get; set; }

    public string? AdminSignature { get; set; }

    public string? ExitInterview { get; set; }

    public string? ExitInterviewRemarks { get; set; }

    public string? FAndFProcessed { get; set; }

    public string? FAndFProcessedRemarks { get; set; }

    public string? TrainingCost { get; set; }

    public string? TrainingCostRemarks { get; set; }

    public string? ResignationUpdated { get; set; }

    public string? ResignationUpdatedRemarks { get; set; }

    public string? NameRemoved { get; set; }

    public string? NameRemovedRemarks { get; set; }

    public string? InsuranceDiscontinued { get; set; }

    public string? InsuranceDiscontinuedRemarks { get; set; }

    public string? HrSignature { get; set; }

    public string? LoansReturned { get; set; }

    public string? LoansReturnedRemarks { get; set; }

    public string? IncentiveDue { get; set; }

    public string? IncentiveDueRemarks { get; set; }

    public string? DeductionsDue { get; set; }

    public string? DeductionsDueRemarks { get; set; }

    public string? TravelFeeDue { get; set; }

    public string? TravelFeeDueRemarks { get; set; }

    public string? TrainingCostDue { get; set; }

    public string? TrainingCostDueRemarks { get; set; }

    public string? VisaFeeDue { get; set; }

    public string? VisaFeeDueRemarks { get; set; }

    public string? AdditionalDeductions { get; set; }

    public string? AdditionalDeductionsRemarks { get; set; }

    public string? AccountsManagerSignature { get; set; }

    public DateTime? SubmissionDate { get; set; }
}
