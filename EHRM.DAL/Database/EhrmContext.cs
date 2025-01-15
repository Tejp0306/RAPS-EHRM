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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-HB9H8DM;Database=EHRM;Trusted_Connection=True;TrustServerCertificate=true");

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
            entity.HasKey(e => e.Id).HasName("PK__Holiday__3214EC07D9B268A3");

            entity.ToTable("Holiday");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.HolidayDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Team).WithMany(p => p.Holidays)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Holiday__TeamId__3B75D760");
        });

        modelBuilder.Entity<MainMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MainMenu__3214EC079D17E828");

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
            entity.HasKey(e => e.Id).HasName("PK__SubMenu__3214EC07ECE6B36F");

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
                .HasConstraintName("FK__SubMenu__EmpId__7D439ABD");

            entity.HasOne(d => d.MainMenu).WithMany(p => p.SubMenus)
                .HasForeignKey(d => d.MainMenuId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SubMenu__MainMen__7B5B524B");

            entity.HasOne(d => d.Role).WithMany(p => p.SubMenus)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SubMenu__RoleId__7C4F7684");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
