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

    public virtual DbSet<AssetsDb> AssetsDbs { get; set; }

    public virtual DbSet<DailyEntry> DailyEntries { get; set; }

    public virtual DbSet<Declaration> Declarations { get; set; }

    public virtual DbSet<EmpType> EmpTypes { get; set; }

    public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }

    public virtual DbSet<EmployeesCred> EmployeesCreds { get; set; }

    public virtual DbSet<EmployeesDeclaration> EmployeesDeclarations { get; set; }

    public virtual DbSet<EmployementTypeDetail> EmployementTypeDetails { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<LeaveApply> LeaveApplies { get; set; }

    public virtual DbSet<LeaveBalance> LeaveBalances { get; set; }

    public virtual DbSet<LeavePolicy> LeavePolicies { get; set; }

    public virtual DbSet<LeaveStatuss> LeaveStatusses { get; set; }

    public virtual DbSet<Leavetypee> Leavetypees { get; set; }

    public virtual DbSet<MainMenu> MainMenus { get; set; }

    public virtual DbSet<NoticeBoard> NoticeBoards { get; set; }

    public virtual DbSet<ProbationEvaluationForm> ProbationEvaluationForms { get; set; }

    public virtual DbSet<ProbationEvaluationQuestion> ProbationEvaluationQuestions { get; set; }

    public virtual DbSet<Qualification> Qualifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<SubMenu> SubMenus { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TimeSheet> TimeSheets { get; set; }

    public virtual DbSet<UserDocument> UserDocuments { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-HB9H8DM;Database=EHRM;Trusted_Connection=True;TrustServerCertificate=true");

    //This is only for sending purpose......

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssetsDb>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AssetsDb__3214EC07E0EC7456");

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
            entity.HasKey(e => e.Id).HasName("PK__Declarat__3214EC07FE2AB64F");

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
                .HasConstraintName("FK__Declarati__EmpId__30C33EC3");
        });

        modelBuilder.Entity<EmpType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmpType__3214EC076E1B806F");

            entity.ToTable("EmpType");

            entity.Property(e => e.EmpType1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EmpType");
        });

        modelBuilder.Entity<EmployeeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC274F1CDAE1");

            entity.HasIndex(e => e.EmailAddress, "UQ__Employee__49A14740CEA8BCE2").IsUnique();

            entity.HasIndex(e => e.LoginId, "UQ__Employee__4DDA2839A97C8315").IsUnique();

            entity.HasIndex(e => e.AadharNumber, "UQ__Employee__5003EE65F93986E2").IsUnique();

            entity.HasIndex(e => e.EmpId, "UQ__Employee__AF2DBB98960A2D7E").IsUnique();

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

        modelBuilder.Entity<EmployeesCred>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0723B148BB");

            entity.ToTable("EmployeesCred");

            entity.HasIndex(e => e.EmpId, "UQ__Employee__AF2DBB9883A1949D").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0720FE5208");

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
                .HasConstraintName("FK__Employees__Reaso__0880433F");
        });

        modelBuilder.Entity<EmployementTypeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employem__3214EC07B1DBF9A0");

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
                .HasConstraintName("FK__Employeme__EmpId__2B0A656D");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Holiday__3214EC07BA6B7620");

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

            entity.Property(e => e.TotalLeave).HasComputedColumnSql("(([EarnedLeave]+[SickLeave])+[CasualLeave])", true);

            entity.HasOne(d => d.Emp).WithMany(p => p.LeaveBalances)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK__LeaveBala__EmpId__55BFB948");
        });

        modelBuilder.Entity<LeavePolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__LeavePol__2E1339A4BA892E4D");

            entity.ToTable("LeavePolicy");

            entity.Property(e => e.EarnedLeaveAccrualRate).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<LeaveStatuss>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveSta__3213E83F753CF1E0");

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
            entity.HasKey(e => e.Id).HasName("PK__Leavetyp__3213E83F70641134");

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

        modelBuilder.Entity<NoticeBoard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NoticeBo__3214EC07B87DDB12");

            entity.ToTable("NoticeBoard");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.HeadingName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Image).IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProbationEvaluationForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Probatio__3214EC072C1618BC");

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
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Emp).WithMany(p => p.ProbationEvaluationForms)
                .HasPrincipalKey(p => p.EmpId)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK__Probation__EmpId__0B5CAFEA");
        });

        modelBuilder.Entity<ProbationEvaluationQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Probatio__0DC06F8CCBD076A1");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Qualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Qualific__3214EC07CA4D8403");

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
                .HasConstraintName("FK__Qualifica__EmpId__339FAB6E");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07BE865682");

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
            entity.HasKey(e => e.Id).HasName("PK__Salary__3214EC0782834577");

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
                .HasConstraintName("FK__Salary__EmpId__2DE6D218");
        });

        modelBuilder.Entity<SubMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubMenu__3214EC071A7C586C");

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
                .HasConstraintName("FK__SubMenu__MainMen__65370702");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC076E917D42");

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
            entity.HasKey(e => e.DocumentId).HasName("PK__Document__1ABEEF6F1E8A4CC5");

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
                .HasConstraintName("FK__Documents__Emplo__2057CCD0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
