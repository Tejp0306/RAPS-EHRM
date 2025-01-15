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
    public virtual DbSet<Declaration> Declarations { get; set; }

    public virtual DbSet<EmpType> EmpTypes { get; set; }

    public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }

    public virtual DbSet<EmployeesCred> EmployeesCreds { get; set; }

    public virtual DbSet<EmployeesDeclaration> EmployeesDeclarations { get; set; }

    public virtual DbSet<EmployementTypeDetail> EmployementTypeDetails { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<MainMenu> MainMenus { get; set; }

    public virtual DbSet<NoticeBoard> NoticeBoards { get; set; }

    public virtual DbSet<ProbationEvaluationQuestion> ProbationEvaluationQuestions { get; set; }

    public virtual DbSet<Qualification> Qualifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<SubMenu> SubMenus { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-S1TNCS5\\SQLEXPRESS;Database=EHRM;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Declaration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Declarat__3214EC072D152592");

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
                .HasConstraintName("FK__Declarati__EmpId__1BC821DD");
        });

        modelBuilder.Entity<EmpType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmpType__3214EC076A9D326B");

            entity.ToTable("EmpType");

            entity.Property(e => e.EmpType1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EmpType");
        });

        modelBuilder.Entity<EmployeeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27B25F6439");

            entity.HasIndex(e => e.EmailAddress, "UQ__Employee__49A1474005924EBA").IsUnique();

            entity.HasIndex(e => e.LoginId, "UQ__Employee__4DDA28391FF2B365").IsUnique();

            entity.HasIndex(e => e.AadharNumber, "UQ__Employee__5003EE6562D75908").IsUnique();

            entity.HasIndex(e => e.EmpId, "UQ__Employee__AF2DBB985189EDBF").IsUnique();

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
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.Nationality).HasMaxLength(50);
            entity.Property(e => e.OfficePhone).HasMaxLength(15);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PrefixName)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ZipCode).HasMaxLength(20);
        });

        modelBuilder.Entity<EmployeesCred>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07720A199F");

            entity.ToTable("EmployeesCred");

            entity.HasIndex(e => e.EmpId, "UQ__Employee__AF2DBB9890341C7C").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0756E185D5");

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
                .HasConstraintName("FK__Employees__Reaso__74AE54BC");
        });

        modelBuilder.Entity<EmployementTypeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employem__3214EC0772C11FAE");

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
                .HasConstraintName("FK__Employeme__EmpId__160F4887");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Holiday__3214EC0718CF3577");

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

            entity.HasOne(d => d.Team).WithMany(p => p.Holidays)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Holiday__Holiday__68487DD7");
        });

        modelBuilder.Entity<MainMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MainMenu__3214EC07E1265919");

            entity.ToTable("MainMenu");

            entity.Property(e => e.Icon)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NoticeBoard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NoticeBo__3214EC07BC67EAD0");

            entity.ToTable("NoticeBoard");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.HeadingName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Image).IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Qualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Qualific__3214EC077E73E8EF");

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
                .HasConstraintName("FK__Qualifica__EmpId__1EA48E88");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07E8F35265");

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
            entity.HasKey(e => e.Id).HasName("PK__Salary__3214EC07BDA43C9C");

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
                .HasConstraintName("FK__Salary__EmpId__18EBB532");
        });

        modelBuilder.Entity<SubMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubMenu__3214EC0757ED5EF9");

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
                .HasConstraintName("FK__SubMenu__MainMen__7C4F7684");

            entity.HasOne(d => d.Role).WithMany(p => p.SubMenus)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SubMenu__RoleId__7D439ABD");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC0762D63E99");

            entity.ToTable("Team");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
