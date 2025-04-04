using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EHRM.DAL.Database;

public partial class EhrmContext : DbContext
{
    public EhrmContext()
    {
    }

    public EhrmContext(DbContextOptions<EhrmContext> options)
        : base(options)
    {
    }


    public virtual DbSet<AddressDetail> AddressDetails { get; set; }

    public virtual DbSet<AssetsDb> AssetsDbs { get; set; }

    public virtual DbSet<BankDetail> BankDetails { get; set; }

    public virtual DbSet<Bgvform> Bgvforms { get; set; }

    public virtual DbSet<ContactDetail> ContactDetails { get; set; }

    public virtual DbSet<AcknowledgementForm> AcknowledgementForms { get; set; }

    public virtual DbSet<AssetsDb> AssetsDbs { get; set; }

    public virtual DbSet<ClientPropertyDeclaration> ClientPropertyDeclarations { get; set; }


    public virtual DbSet<DailyEntry> DailyEntries { get; set; }

    public virtual DbSet<Declaration> Declarations { get; set; }

    public virtual DbSet<DependentDetail> DependentDetails { get; set; }

    public virtual DbSet<EducationalDetail> EducationalDetails { get; set; }

    public virtual DbSet<EmpType> EmpTypes { get; set; }

    public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }

    public virtual DbSet<EmployeeMaster> EmployeeMasters { get; set; }

    public virtual DbSet<EmployeeExitChecklist> EmployeeExitChecklists { get; set; }

    public virtual DbSet<EmployeePunchDetail> EmployeePunchDetails { get; set; }

    public virtual DbSet<EmployeeUndertakingForm> EmployeeUndertakingForms { get; set; }

    public virtual DbSet<EmployeesCred> EmployeesCreds { get; set; }

    public virtual DbSet<EmployeesDeclaration> EmployeesDeclarations { get; set; }

    public virtual DbSet<EmployementTypeDetail> EmployementTypeDetails { get; set; }


    public virtual DbSet<ExitDetail> ExitDetails { get; set; }

    public virtual DbSet<FamilyDetail> FamilyDetails { get; set; }

    public virtual DbSet<ExitInterviewForm> ExitInterviewForms { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<LeaveApply> LeaveApplies { get; set; }

    public virtual DbSet<LeaveBalance> LeaveBalances { get; set; }

    public virtual DbSet<LeavePolicy> LeavePolicies { get; set; }

    public virtual DbSet<LeaveStatuss> LeaveStatusses { get; set; }

    public virtual DbSet<Leavetypee> Leavetypees { get; set; }

    public virtual DbSet<MainMenu> MainMenus { get; set; }

    public virtual DbSet<MasterEmergencyContact> MasterEmergencyContacts { get; set; }

    public virtual DbSet<MasterPreviousEmployment> MasterPreviousEmployments { get; set; }

    public virtual DbSet<NoticeBoard> NoticeBoards { get; set; }

    public virtual DbSet<PreviousEmployment> PreviousEmployments { get; set; }

    public virtual DbSet<NonDisclosureAgreement> NonDisclosureAgreements { get; set; }

    public virtual DbSet<NoticeBoard> NoticeBoards { get; set; }

    public virtual DbSet<PersonalInfo> PersonalInfos { get; set; }

    public virtual DbSet<ProbationEvaluationForm> ProbationEvaluationForms { get; set; }

    public virtual DbSet<ProbationEvaluationQuestion> ProbationEvaluationQuestions { get; set; }

    public virtual DbSet<Qualification> Qualifications { get; set; }
    
    public virtual DbSet<ReportingDetail> ReportingDetails { get; set; }

    public virtual DbSet<ResignationForm> ResignationForms { get; set; }


    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<SubMenu> SubMenus { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TimeSheet> TimeSheets { get; set; }

    public virtual DbSet<UserDocument> UserDocuments { get; set; }


    public virtual DbSet<WorkExperience> WorkExperiences { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-HB9H8DM;Database=EHRM;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AddressDetail>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__AddressD__091C2AFB4538C871");

            entity.Property(e => e.PermanentAddress).HasMaxLength(200);
            entity.Property(e => e.PostalAddress).HasMaxLength(200);

            entity.HasOne(d => d.Emp).WithMany(p => p.AddressDetails)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__AddressDe__EmpId__54968AE5");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=EHRM;User Id=EHRMAdmin;Password=9YeCsU3QGQp2Mnxebcmj===;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcknowledgementForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Acknowle__3214EC07BDDF1FA6");

            entity.ToTable("AcknowledgementForm");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("empId");
            entity.Property(e => e.EmployeeName).HasMaxLength(255);
            entity.Property(e => e.SignatureDate)
                .HasMaxLength(255)
                .IsUnicode(false);

        });

        modelBuilder.Entity<AssetsDb>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AssetsDb__3214EC07B5283861");

            entity.ToTable("AssetsDb");

            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Summary).IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });


        modelBuilder.Entity<BankDetail>(entity =>
        {
            entity.HasKey(e => e.BankId).HasName("PK__BankDeta__AA08CB13F60179AE");

            entity.Property(e => e.AccountNumber).HasMaxLength(50);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(20)
                .HasColumnName("IFSCCode");

            entity.HasOne(d => d.Emp).WithMany(p => p.BankDetails)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BankDetai__EmpId__5A4F643B");
        });

        modelBuilder.Entity<Bgvform>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BGVForm__3214EC07624457E0");

            entity.ToTable("BGVForm");

            entity.HasIndex(e => e.EmpId, "UQ__BGVForm__AF2DBB98FF78DAE0").IsUnique();

            entity.Property(e => e.AlternateNumber).HasMaxLength(15);
            entity.Property(e => e.CollegeName).HasMaxLength(100);
            entity.Property(e => e.CompleteAddress).HasMaxLength(255);
            entity.Property(e => e.CourseName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.EmpId).IsRequired();
            entity.Property(e => e.FatherFirstName).HasMaxLength(100);
            entity.Property(e => e.FatherLastName).HasMaxLength(100);
            entity.Property(e => e.FatherMiddleName).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MaritalStatus).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.MobileNumber).HasMaxLength(15);
            entity.Property(e => e.Nationality).HasMaxLength(100);
            entity.Property(e => e.NearestLandmark).HasMaxLength(100);
            entity.Property(e => e.PlaceOfBirth).HasMaxLength(100);
            entity.Property(e => e.ProgramType).HasMaxLength(50);
            entity.Property(e => e.ProofType).HasMaxLength(100);
            entity.Property(e => e.ReferenceContact).HasMaxLength(15);
            entity.Property(e => e.ReferenceName).HasMaxLength(100);
            entity.Property(e => e.UniversityBoardName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ContactDetail>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__ContactD__5C66259B19AAADED");

            entity.Property(e => e.EmergencyContactName).HasMaxLength(100);
            entity.Property(e => e.EmergencyContactNumber).HasMaxLength(20);
            entity.Property(e => e.EmergencyRelationship).HasMaxLength(50);
            entity.Property(e => e.OfficialContactNo).HasMaxLength(20);
            entity.Property(e => e.OfficialEmailId).HasMaxLength(100);
            entity.Property(e => e.PersonalContactNo).HasMaxLength(20);
            entity.Property(e => e.PersonalEmailId).HasMaxLength(100);

            entity.HasOne(d => d.Emp).WithMany(p => p.ContactDetails)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ContactDe__EmpId__51BA1E3A");

        modelBuilder.Entity<ClientPropertyDeclaration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClientPr__3214EC07973488EB");

            entity.ToTable("ClientPropertyDeclaration");

            entity.Property(e => e.ClientName).HasMaxLength(255);
            entity.Property(e => e.ConfirmationDate).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("empId");
            entity.Property(e => e.EmployeeName).HasMaxLength(255);
            entity.Property(e => e.EmployeeNameConfirm).HasMaxLength(255);
            entity.Property(e => e.ReceivedDate).HasMaxLength(255);
            entity.Property(e => e.Signature).HasMaxLength(255);

        });

        modelBuilder.Entity<DailyEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DailyEnt__3214EC07BF70E883");

            entity.ToTable("DailyEntry");

            entity.Property(e => e.AssignmentDesc).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HoursWorked).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Remarks).HasColumnType("text");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.TimeSheet).WithMany(p => p.DailyEntries)
                .HasForeignKey(d => d.TimeSheetId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DailyEntry_TimeSheet");
        });

        modelBuilder.Entity<Declaration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Declarat__3214EC07730ED5CB");

            entity.ToTable("Declaration");

            entity.Property(e => e.Date)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.HrContactInfo)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.HrRepresentativeDesignation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HrRepresentativeName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Signature)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Emp).WithMany(p => p.Declarations)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Declarati__EmpId__693CA210");
        });

        modelBuilder.Entity<DependentDetail>(entity =>
        {
            entity.HasKey(e => e.DependentId).HasName("PK__Dependen__9BC67CF1FA2D7089");

            entity.Property(e => e.DependentName).HasMaxLength(100);
            entity.Property(e => e.Relationship).HasMaxLength(50);

            entity.HasOne(d => d.Emp).WithMany(p => p.DependentDetails)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Dependent__EmpId__60083D91");
        });

        modelBuilder.Entity<EducationalDetail>(entity =>
        {
            entity.HasKey(e => e.EducationId).HasName("PK__Educatio__4BBE3805A11CA312");

            entity.Property(e => e.BachelorDegree).HasMaxLength(100);
            entity.Property(e => e.BachelorInstitution).HasMaxLength(200);
            entity.Property(e => e.MasterDegree).HasMaxLength(100);
            entity.Property(e => e.MasterInstitution).HasMaxLength(200);
            entity.Property(e => e.PostDoctorateDegree).HasMaxLength(100);
            entity.Property(e => e.PostDoctorateInstitution).HasMaxLength(200);
            entity.Property(e => e.ProfessionalCoursesDegree).HasMaxLength(100);
            entity.Property(e => e.ProfessionalCoursesInstitution).HasMaxLength(200);
            entity.Property(e => e.XiithInstitution)
                .HasMaxLength(200)
                .HasColumnName("XIIthInstitution");
            entity.Property(e => e.XiithPassingYear).HasColumnName("XIIthPassingYear");
            entity.Property(e => e.XthInstitution).HasMaxLength(200);

            entity.HasOne(d => d.Emp).WithMany(p => p.EducationalDetails)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Education__EmpId__5772F790");
        });

        modelBuilder.Entity<EmpType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmpType__3214EC07688DC6EC");

            entity.ToTable("EmpType");

            entity.Property(e => e.EmpType1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EmpType");
        });

        modelBuilder.Entity<EmployeeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27D2E0C58F");

            entity.HasIndex(e => e.EmailAddress, "UQ__Employee__49A147409E8B4859").IsUnique();

            entity.HasIndex(e => e.LoginId, "UQ__Employee__4DDA2839617BBF18").IsUnique();

            entity.HasIndex(e => e.AadharNumber, "UQ__Employee__5003EE65C4D9A885").IsUnique();

            entity.HasIndex(e => e.EmpId, "UQ__Employee__AF2DBB98B9D6DE67").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AadharNumber).HasMaxLength(20);
            entity.Property(e => e.Active).HasDefaultValue(false);
            entity.Property(e => e.CellPhone).HasMaxLength(15);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedById).HasColumnName("createdById");
            entity.Property(e => e.DateOfBirth)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EmailAddress).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.HomePhone).HasMaxLength(15);
            entity.Property(e => e.IsProfileCompleted).HasDefaultValue(false);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.LoginId)
                .HasMaxLength(50)
                .HasColumnName("LoginID");
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.Nationality).HasMaxLength(50);
            entity.Property(e => e.OfficePhone).HasMaxLength(15);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PrefixName)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Street).HasMaxLength(255);
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ZipCode).HasMaxLength(20);
        });


        modelBuilder.Entity<EmployeeMaster>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK__Employee__AF2DBB99DDBC9234");

            entity.ToTable("EmployeeMaster");

            entity.Property(e => e.EmpId).ValueGeneratedNever();
            entity.Property(e => e.AadharNumber).HasMaxLength(20);
            entity.Property(e => e.BandLevel).HasMaxLength(50);
            entity.Property(e => e.CtcperAnnumOnDoj)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("CTCPerAnnumOnDOJ");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Designation).HasMaxLength(100);
            entity.Property(e => e.FilingPerson).HasMaxLength(100);
            entity.Property(e => e.FilingRecheck).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.FunctionProject).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.MaritalStatus).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.PancardNumber)
                .HasMaxLength(20)
                .HasColumnName("PANCardNumber");
            entity.Property(e => e.ProbationConfirmationStatus).HasMaxLength(50);
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.SrNo).ValueGeneratedOnAdd();
            entity.Property(e => e.TenureInRaps)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("TenureInRAPS");
            entity.Property(e => e.TotalWorkExperience).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Uannumber)
                .HasMaxLength(20)
                .HasColumnName("UANNumber");
            entity.Property(e => e.YearsInRaps)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("YearsInRAPS");

        modelBuilder.Entity<EmployeeExitChecklist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FEC5262D2");

            entity.ToTable("EmployeeExitChecklist");

            entity.HasIndex(e => e.EmpId, "UQ__Employee__263E2DD274FBDA1A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccessControlUpdated)
                .HasMaxLength(10)
                .HasColumnName("access_control_updated");
            entity.Property(e => e.AccessControlUpdatedRemarks)
                .HasMaxLength(500)
                .HasColumnName("access_control_updated_remarks");
            entity.Property(e => e.AccountsManagerSignature)
                .HasMaxLength(255)
                .HasColumnName("accounts_manager_signature");
            entity.Property(e => e.AdditionalDeductions)
                .HasMaxLength(10)
                .HasColumnName("additional_deductions");
            entity.Property(e => e.AdditionalDeductionsRemarks)
                .HasMaxLength(500)
                .HasColumnName("additional_deductions_remarks");
            entity.Property(e => e.AdminSignature)
                .HasMaxLength(255)
                .HasColumnName("admin_signature");
            entity.Property(e => e.AssetsReturned)
                .HasMaxLength(10)
                .HasColumnName("assets_returned");
            entity.Property(e => e.AssetsReturnedRemarks)
                .HasMaxLength(500)
                .HasColumnName("assets_returned_remarks");
            entity.Property(e => e.ClientPasswordsChanged)
                .HasMaxLength(10)
                .HasColumnName("client_passwords_changed");
            entity.Property(e => e.ClientPasswordsChangedRemarks)
                .HasMaxLength(500)
                .HasColumnName("client_passwords_changed_remarks");
            entity.Property(e => e.CompletedTasks)
                .HasMaxLength(10)
                .HasColumnName("completed_tasks");
            entity.Property(e => e.CompletedTasksRemarks)
                .HasMaxLength(500)
                .HasColumnName("completed_tasks_remarks");
            entity.Property(e => e.DeductionsDue)
                .HasMaxLength(10)
                .HasColumnName("deductions_due");
            entity.Property(e => e.DeductionsDueRemarks)
                .HasMaxLength(500)
                .HasColumnName("deductions_due_remarks");
            entity.Property(e => e.DocumentsReturned)
                .HasMaxLength(10)
                .HasColumnName("documents_returned");
            entity.Property(e => e.DocumentsReturnedRemarks)
                .HasMaxLength(500)
                .HasColumnName("documents_returned_remarks");
            entity.Property(e => e.DoorAccess)
                .HasMaxLength(10)
                .HasColumnName("door_access");
            entity.Property(e => e.DoorAccessRemarks)
                .HasMaxLength(500)
                .HasColumnName("door_access_remarks");
            entity.Property(e => e.EmpId).HasColumnName("Emp_id");
            entity.Property(e => e.ExitInterview)
                .HasMaxLength(10)
                .HasColumnName("exit_interview");
            entity.Property(e => e.ExitInterviewRemarks)
                .HasMaxLength(500)
                .HasColumnName("exit_interview_remarks");
            entity.Property(e => e.FAndFProcessed)
                .HasMaxLength(10)
                .HasColumnName("f_and_f_processed");
            entity.Property(e => e.FAndFProcessedRemarks)
                .HasMaxLength(500)
                .HasColumnName("f_and_f_processed_remarks");
            entity.Property(e => e.HelpdeskSignature)
                .HasMaxLength(255)
                .HasColumnName("helpdesk_signature");
            entity.Property(e => e.HrSignature)
                .HasMaxLength(255)
                .HasColumnName("hr_signature");
            entity.Property(e => e.IdCardReturned)
                .HasMaxLength(10)
                .HasColumnName("id_card_returned");
            entity.Property(e => e.IdCardReturnedRemarks)
                .HasMaxLength(500)
                .HasColumnName("id_card_returned_remarks");
            entity.Property(e => e.IncentiveDue)
                .HasMaxLength(10)
                .HasColumnName("incentive_due");
            entity.Property(e => e.IncentiveDueRemarks)
                .HasMaxLength(500)
                .HasColumnName("incentive_due_remarks");
            entity.Property(e => e.InsuranceDiscontinued)
                .HasMaxLength(10)
                .HasColumnName("insurance_discontinued");
            entity.Property(e => e.InsuranceDiscontinuedRemarks)
                .HasMaxLength(500)
                .HasColumnName("insurance_discontinued_remarks");
            entity.Property(e => e.JobdivaReset)
                .HasMaxLength(10)
                .HasColumnName("jobdiva_reset");
            entity.Property(e => e.JobdivaResetRemarks)
                .HasMaxLength(500)
                .HasColumnName("jobdiva_reset_remarks");
            entity.Property(e => e.KnowledgeTransfer)
                .HasMaxLength(10)
                .HasColumnName("knowledge_transfer");
            entity.Property(e => e.KnowledgeTransferRemarks)
                .HasMaxLength(500)
                .HasColumnName("knowledge_transfer_remarks");
            entity.Property(e => e.LoansReturned)
                .HasMaxLength(10)
                .HasColumnName("loans_returned");
            entity.Property(e => e.LoansReturnedRemarks)
                .HasMaxLength(500)
                .HasColumnName("loans_returned_remarks");
            entity.Property(e => e.LoginWithdrawn)
                .HasMaxLength(10)
                .HasColumnName("login_withdrawn");
            entity.Property(e => e.LoginWithdrawnRemarks)
                .HasMaxLength(500)
                .HasColumnName("login_withdrawn_remarks");
            entity.Property(e => e.MailForwarding)
                .HasMaxLength(10)
                .HasColumnName("mail_forwarding");
            entity.Property(e => e.MailForwardingRemarks)
                .HasMaxLength(500)
                .HasColumnName("mail_forwarding_remarks");
            entity.Property(e => e.MailSentToHr)
                .HasMaxLength(10)
                .HasColumnName("mail_sent_to_hr");
            entity.Property(e => e.MailSentToHrRemarks)
                .HasMaxLength(500)
                .HasColumnName("mail_sent_to_hr_remarks");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.NameRemoved)
                .HasMaxLength(10)
                .HasColumnName("name_removed");
            entity.Property(e => e.NameRemovedRemarks)
                .HasMaxLength(500)
                .HasColumnName("name_removed_remarks");
            entity.Property(e => e.RelievingDate)
                .HasMaxLength(50)
                .HasColumnName("Relieving_date");
            entity.Property(e => e.ReportingManager)
                .HasMaxLength(255)
                .HasColumnName("Reporting_manager");
            entity.Property(e => e.ReportingManagerSignature)
                .HasMaxLength(255)
                .HasColumnName("reporting_manager_signature");
            entity.Property(e => e.ResignationDate)
                .HasMaxLength(50)
                .HasColumnName("Resignation_date");
            entity.Property(e => e.ResignationUpdated)
                .HasMaxLength(10)
                .HasColumnName("resignation_updated");
            entity.Property(e => e.ResignationUpdatedRemarks)
                .HasMaxLength(500)
                .HasColumnName("resignation_updated_remarks");
            entity.Property(e => e.RingcentralSuspended)
                .HasMaxLength(10)
                .HasColumnName("ringcentral_suspended");
            entity.Property(e => e.RingcentralSuspendedRemarks)
                .HasMaxLength(500)
                .HasColumnName("ringcentral_suspended_remarks");
            entity.Property(e => e.StartDate)
                .HasMaxLength(50)
                .HasColumnName("Start_date");
            entity.Property(e => e.SubmissionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("submission_date");
            entity.Property(e => e.TrainingCost)
                .HasMaxLength(10)
                .HasColumnName("training_cost");
            entity.Property(e => e.TrainingCostDue)
                .HasMaxLength(10)
                .HasColumnName("training_cost_due");
            entity.Property(e => e.TrainingCostDueRemarks)
                .HasMaxLength(500)
                .HasColumnName("training_cost_due_remarks");
            entity.Property(e => e.TrainingCostRemarks)
                .HasMaxLength(500)
                .HasColumnName("training_cost_remarks");
            entity.Property(e => e.TransportDiscontinued)
                .HasMaxLength(10)
                .HasColumnName("transport_discontinued");
            entity.Property(e => e.TransportDiscontinuedRemarks)
                .HasMaxLength(500)
                .HasColumnName("transport_discontinued_remarks");
            entity.Property(e => e.TravelFeeDue)
                .HasMaxLength(10)
                .HasColumnName("travel_fee_due");
            entity.Property(e => e.TravelFeeDueRemarks)
                .HasMaxLength(500)
                .HasColumnName("travel_fee_due_remarks");
            entity.Property(e => e.VisaFeeDue)
                .HasMaxLength(10)
                .HasColumnName("visa_fee_due");
            entity.Property(e => e.VisaFeeDueRemarks)
                .HasMaxLength(500)
                .HasColumnName("visa_fee_due_remarks");
            entity.Property(e => e.VisaFeeRecovery)
                .HasMaxLength(10)
                .HasColumnName("visa_fee_recovery");
            entity.Property(e => e.VisaFeeRecoveryRemarks)
                .HasMaxLength(500)
                .HasColumnName("visa_fee_recovery_remarks");
            entity.Property(e => e.VoipPhoneDeactivated)
                .HasMaxLength(10)
                .HasColumnName("voip_phone_deactivated");
            entity.Property(e => e.VoipPhoneDeactivatedRemarks)
                .HasMaxLength(500)
                .HasColumnName("voip_phone_deactivated_remarks");
            entity.Property(e => e.WifiSuspended)
                .HasMaxLength(10)
                .HasColumnName("wifi_suspended");
            entity.Property(e => e.WifiSuspendedRemarks)
                .HasMaxLength(500)
                .HasColumnName("wifi_suspended_remarks");

        });

        modelBuilder.Entity<EmployeePunchDetail>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("CalculateTotalHours"));

            entity.Property(e => e.EmployeeName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Month)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalHours).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<EmployeeUndertakingForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27958EEEA1");

            entity.ToTable("EmployeeUndertakingForm");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeName).HasMaxLength(255);
            entity.Property(e => e.EmployeeSignature).HasMaxLength(255);
            entity.Property(e => e.FatherName).HasMaxLength(255);
            entity.Property(e => e.LastWorkingDate).HasMaxLength(500);
            entity.Property(e => e.OfficeAddress).HasMaxLength(500);
            entity.Property(e => e.PermanentAddress).HasMaxLength(500);
            entity.Property(e => e.Relation).HasMaxLength(10);
            entity.Property(e => e.ResignationDate).HasMaxLength(500);
        });

        modelBuilder.Entity<EmployeesCred>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07CF68BC4A");

            entity.ToTable("EmployeesCred");

            entity.HasIndex(e => e.EmpId, "UQ__Employee__AF2DBB983948E035").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.LockoutEndTime).HasColumnType("datetime");
            entity.Property(e => e.LoginId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("LoginID");
            entity.Property(e => e.TempPassword).HasMaxLength(255);

            entity.HasOne(d => d.Emp).WithOne(p => p.EmployeesCred)
                .HasPrincipalKey<EmployeeDetail>(p => p.EmpId)
                .HasForeignKey<EmployeesCred>(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Employees__EmpId__0A9D95DB");
        });

        modelBuilder.Entity<EmployeesDeclaration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC079644136C");

            entity.ToTable("EmployeesDeclaration");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AdharNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BachelorCompleteYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.BachelorDegrees)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BachelorInstitution)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BandLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Ctc)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DateOfJoining).HasColumnType("datetime");
            entity.Property(e => e.Dependent1Dob).HasColumnType("datetime");
            entity.Property(e => e.Dependent1Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Dependent1Relationship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContact1)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContact2)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyName1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyName2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyRelationship1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyRelationship2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ExitDate)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FatherHusbandName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FilingPerson)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FilingRecheck)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FirstOrganisation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FourthOrganisation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IfscCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MasterCompleteYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.MasterInstitution)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OfficialContact)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.OfficialEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PanCardNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PermanentAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PersonalContact)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PersonalEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PostalAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProbationDate).HasColumnType("datetime");
            entity.Property(e => e.ProbationStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Project)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReasonForLeaving)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RelationWithSpouse)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResignationDate)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SecondOrganisation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SpouseFatherDateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.SpouseFatherMotherName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TenureInRaps).HasColumnName("TenureInRAPS");
            entity.Property(e => e.ThirdOrganisation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UanNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.XiithInstitution)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.XiithPassingYear)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.XthInstitution)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.XthPassingYear)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.YearsInRaps).HasColumnName("YearsInRAPS");

            entity.HasOne(d => d.Emp).WithMany(p => p.EmployeesDeclarations)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Employees__EmpId__6B24EA82");
        });

        modelBuilder.Entity<EmployementTypeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employem__3214EC0754ACC10C");

            entity.Property(e => e.AppointmentDate)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.EndDate)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.StartDate)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Emp).WithMany(p => p.EmployementTypeDetails)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Employeme__EmpId__6C190EBB");
        });

        modelBuilder.Entity<ExitInterviewForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExitInte__3213E83F628700CC");

            entity.ToTable("ExitInterviewForm");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdditionalComments).HasColumnName("additional_comments");
            entity.Property(e => e.AreasOfImprovement).HasColumnName("areas_of_improvement");
            entity.Property(e => e.BenefitsSatisfactory)
                .HasMaxLength(10)
                .HasColumnName("benefits_satisfactory");
            entity.Property(e => e.ChangeDecision).HasColumnName("change_decision");
            entity.Property(e => e.ComparisonWithNewJob).HasColumnName("comparison_with_new_job");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DepartmentMorale).HasColumnName("department_morale");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(255)
                .HasColumnName("employee_name");
            entity.Property(e => e.EnjoyedFunctions).HasColumnName("enjoyed_functions");
            entity.Property(e => e.GreatestChallenge).HasColumnName("greatest_challenge");
            entity.Property(e => e.ImproveMorale).HasColumnName("improve_morale");
            entity.Property(e => e.InformedPolicies)
                .HasMaxLength(10)
                .HasColumnName("informed_policies");
            entity.Property(e => e.InterviewDate)
                .HasMaxLength(255)
                .HasColumnName("interview_date");
            entity.Property(e => e.Interviewer)
                .HasMaxLength(255)
                .HasColumnName("interviewer");
            entity.Property(e => e.JobSecurity)
                .HasMaxLength(10)
                .HasColumnName("job_security");
            entity.Property(e => e.JobSecurityDetails).HasColumnName("job_security_details");
            entity.Property(e => e.LeastEnjoyedFunctions).HasColumnName("least_enjoyed_functions");
            entity.Property(e => e.PoliciesFeedback).HasColumnName("policies_feedback");
            entity.Property(e => e.ReasonForLeaving).HasColumnName("reason_for_leaving");
            entity.Property(e => e.Recommend).HasColumnName("recommend");
            entity.Property(e => e.Rejoin)
                .HasMaxLength(10)
                .HasColumnName("rejoin");
            entity.Property(e => e.Strengths).HasColumnName("strengths");
            entity.Property(e => e.SupervisorFeedback).HasColumnName("supervisor_feedback");
            entity.Property(e => e.TreatmentAfterResignation).HasColumnName("treatment_after_resignation");
            entity.Property(e => e.WorkingConditions).HasColumnName("working_conditions");
        });

        modelBuilder.Entity<ExitDetail>(entity =>
        {
            entity.HasKey(e => e.ExitId).HasName("PK__ExitDeta__26D64EB8E262BABA");

            entity.Property(e => e.ReasonForLeaving).HasMaxLength(200);

            entity.HasOne(d => d.Emp).WithMany(p => p.ExitDetails)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ExitDetai__EmpId__689D8392");
        });

        modelBuilder.Entity<FamilyDetail>(entity =>
        {
            entity.HasKey(e => e.FamilyId).HasName("PK__FamilyDe__41D82F6B9F2F431C");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.RelationWithEmployee).HasMaxLength(50);

            entity.HasOne(d => d.Emp).WithMany(p => p.FamilyDetails)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__FamilyDet__EmpId__5D2BD0E6");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Holiday__3214EC07E2363C33");

            entity.ToTable("Holiday");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.HolidayDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LeaveApply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveApp__3214EC27BEEF2DA5");

            entity.ToTable("LeaveApply");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApplyDate)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LeaveFrom)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LeaveType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<LeaveBalance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveBal__3214EC0752E8C4CD");

            entity.ToTable("LeaveBalance");

            entity.Property(e => e.TenureYears).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TotalLeave).HasComputedColumnSql("(([EarnedLeave]+[SickLeave])+[CasualLeave])", true);

            entity.HasOne(d => d.Emp).WithMany(p => p.LeaveBalances)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK__LeaveBala__EmpId__55BFB948");
        });

        modelBuilder.Entity<LeavePolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__LeavePol__2E1339A44E6632E0");

            entity.ToTable("LeavePolicy");

            entity.Property(e => e.EarnedLeaveAccrualRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.YearsOfServiceMax).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.YearsOfServiceMin).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<LeaveStatuss>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveSta__3213E83F333E9A43");

            entity.ToTable("LeaveStatuss");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LeaveStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ManagerRemark)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Leave).WithMany(p => p.LeaveStatusses)
                .HasForeignKey(d => d.LeaveId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_LeaveStatuss_LeaveApply");
        });

        modelBuilder.Entity<Leavetypee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Leavetyp__3213E83F7684E279");

            entity.ToTable("Leavetypee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LeaveDescription).IsUnicode(false);
            entity.Property(e => e.LeaveType)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MainMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MainMenu__3214EC07E1265919");

            entity.ToTable("MainMenu");

            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });


        modelBuilder.Entity<MasterEmergencyContact>(entity =>
        {
            entity.HasKey(e => e.EmergencyId).HasName("PK__MasterEm__7B5544D3B3E55107");

            entity.ToTable("MasterEmergencyContact");

            entity.Property(e => e.EmergencyContactNumber).HasMaxLength(20);
            entity.Property(e => e.EmergencyName).HasMaxLength(255);
            entity.Property(e => e.Relationship).HasMaxLength(100);

            entity.HasOne(d => d.Emp).WithMany(p => p.MasterEmergencyContacts)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK_MasterEmergencyContact_Employee");
        });

        modelBuilder.Entity<MasterPreviousEmployment>(entity =>
        {
            entity.HasKey(e => e.EmploymentId).HasName("PK__MasterPr__FDC872B6DCD5EEF5");

            entity.Property(e => e.Designation).HasMaxLength(100);
            entity.Property(e => e.OrganisationName).HasMaxLength(200);
            entity.Property(e => e.ReasonForLeaving).HasMaxLength(200);
            entity.Property(e => e.YearsOfExperience).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Emp).WithMany(p => p.MasterPreviousEmployments)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MasterPre__EmpId__62E4AA3C");

        modelBuilder.Entity<NonDisclosureAgreement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NonDiscl__3214EC075AD0FC0A");

            entity.ToTable("NonDisclosureAgreement");

            entity.Property(e => e.AgreementDate).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("empId");
            entity.Property(e => e.EmployeeName).HasMaxLength(255);
            entity.Property(e => e.Signature).HasMaxLength(255);

        });

        modelBuilder.Entity<NoticeBoard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NoticeBo__3214EC073BB7E79E");

            entity.ToTable("NoticeBoard");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.HeadingName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Image).IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PreviousEmployment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Previous__3214EC070198DB20");

            entity.HasIndex(e => e.EmpId, "IDX_PreviousEmployments_EmpId");

            entity.Property(e => e.BestTimeToReach).HasMaxLength(50);
            entity.Property(e => e.CompanyLandline).HasMaxLength(20);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CtcperAnnum)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CTCPerAnnum");
            entity.Property(e => e.CurrentDesignation).HasMaxLength(255);
            entity.Property(e => e.Department).HasMaxLength(255);
            entity.Property(e => e.EmployeeCode).HasMaxLength(100);
            entity.Property(e => e.EmploymentTenure).HasMaxLength(100);
            entity.Property(e => e.IsReportingManagerStillInCompany).HasMaxLength(3);
            entity.Property(e => e.NatureOfEmployment).HasMaxLength(255);
            entity.Property(e => e.OfficialTitle).HasMaxLength(255);
            entity.Property(e => e.PayrollCompanyName).HasMaxLength(255);
            entity.Property(e => e.PersonalMobileNo).HasMaxLength(15);
            entity.Property(e => e.ReasonForLeaving).HasMaxLength(255);
            entity.Property(e => e.ReportingManagerDesignation).HasMaxLength(255);
            entity.Property(e => e.ReportingManagerName).HasMaxLength(255);

            entity.HasOne(d => d.Emp).WithMany(p => p.PreviousEmployments)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK_PreviousEmployments_BGVForm");

        modelBuilder.Entity<PersonalInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Personal__3214EC271F90F044");

            entity.ToTable("PersonalInfo");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CurrentAddress).HasMaxLength(200);
            entity.Property(e => e.EmergencyContact1Name).HasMaxLength(255);
            entity.Property(e => e.EmergencyContact1Phone).HasMaxLength(20);
            entity.Property(e => e.EmergencyContact1Relationship).HasMaxLength(100);
            entity.Property(e => e.EmergencyContact2Name).HasMaxLength(255);
            entity.Property(e => e.EmergencyContact2Phone).HasMaxLength(20);
            entity.Property(e => e.EmergencyContact2Relationship).HasMaxLength(100);
            entity.Property(e => e.EmpId).HasColumnName("empId");
            entity.Property(e => e.EmployeeName).HasMaxLength(255);
            entity.Property(e => e.FormDate).HasMaxLength(255);
            entity.Property(e => e.HomePhone).HasMaxLength(20);
            entity.Property(e => e.MobilePhone).HasMaxLength(20);
            entity.Property(e => e.PermanentAddress).HasMaxLength(200);
            entity.Property(e => e.PersonalEmail).HasMaxLength(255);
            entity.Property(e => e.Signature).HasMaxLength(255);

        });

        modelBuilder.Entity<ProbationEvaluationForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Probatio__3214EC072D53B917");

            entity.ToTable("ProbationEvaluationForm");

            entity.Property(e => e.ApplicationDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FinalDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ManagerSignature)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Recommendation).IsUnicode(false);
            entity.Property(e => e.RemarksConfirmation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TotalAverage)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Emp).WithMany(p => p.ProbationEvaluationForms)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK__Probation__EmpId__6EF57B66");
        });

        modelBuilder.Entity<ProbationEvaluationQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Probatio__0DC06F8CFD70327C");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Qualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Qualific__3214EC07263EF4DC");

            entity.ToTable("Qualification");

            entity.Property(e => e.Concentration)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CourseName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Details)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Document)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InstitutionName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassedDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.QualificationEarned)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Emp).WithMany(p => p.Qualifications)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Qualifica__EmpId__6FE99F9F");
        });

        modelBuilder.Entity<ResignationForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Resignat__3214EC0751BB866F");

            entity.ToTable("ResignationForm");

            entity.Property(e => e.EmployeeName).HasMaxLength(255);
            entity.Property(e => e.EmployeeSignature).HasMaxLength(255);
            entity.Property(e => e.FinalDay).HasMaxLength(255);
            entity.Property(e => e.Position).HasMaxLength(255);
            entity.Property(e => e.ResignationDate)
                .HasMaxLength(255)
                .HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TotalMonths).HasMaxLength(255);
        });

        modelBuilder.Entity<ReportingDetail>(entity =>
        {
            entity.HasKey(e => e.ReportingId).HasName("PK__Reportin__D0C742CC97CD2821");

            entity.Property(e => e.DirectReporting).HasMaxLength(100);
            entity.Property(e => e.DottedReporting).HasMaxLength(100);
            entity.Property(e => e.SkipReporting).HasMaxLength(100);

            entity.HasOne(d => d.Emp).WithMany(p => p.ReportingDetails)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reporting__EmpId__65C116E7");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07F4D4EEED");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.RoleDescription).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(100);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Salary__3214EC07348E4CBA");

            entity.ToTable("Salary");

            entity.Property(e => e.Ctc).HasColumnName("CTC");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Endyear)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("ENDYEAR");
            entity.Property(e => e.StartYear)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Emp).WithMany(p => p.Salaries)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Salary__EmpId__70DDC3D8");
        });

        modelBuilder.Entity<SubMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubMenu__3214EC07F5BE460A");

            entity.ToTable("SubMenu");

            entity.Property(e => e.Action)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Controller)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Emp).WithMany(p => p.SubMenus)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SubMenu__EmpId__7E37BEF6");

            entity.HasOne(d => d.MainMenu).WithMany(p => p.SubMenus)
                .HasForeignKey(d => d.MainMenuId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SubMenu__MainMen__72C60C4A");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC071EB46849");

            entity.ToTable("Team");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TimeSheet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TimeShee__3214EC07D5A7E666");

            entity.ToTable("TimeSheet");

            entity.Property(e => e.ClientName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmpName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeSignature)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerSignature)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PresentMonth)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProjectName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SignatureDate).HasColumnType("datetime");
            entity.Property(e => e.SubmissionDate).HasColumnType("datetime");
            entity.Property(e => e.TotalHours).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<UserDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__UserDocu__1ABEEF6FAAE5DB63");

            entity.ToTable("UserDocument");

            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.DocumentType).HasMaxLength(255);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.UserDocuments)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__UserDocum__Emplo__73BA3083");
        });

        modelBuilder.Entity<WorkExperience>(entity =>
        {
            entity.HasKey(e => e.EmploymentId).HasName("PK__WorkExpe__FDC872B69B59A68D");

            entity.ToTable("WorkExperience");

            entity.Property(e => e.Designation).HasMaxLength(100);
            entity.Property(e => e.OrganisationName).HasMaxLength(255);
            entity.Property(e => e.ReasonForLeaving).HasMaxLength(255);
            entity.Property(e => e.YearsOfExperience).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Emp).WithMany(p => p.WorkExperiences)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK_WorkExperience_Employee");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
