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

    public virtual DbSet<EmpType> EmpTypes { get; set; }

    public virtual DbSet<EmployeesCred> EmployeesCreds { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    //public virtual DbSet<MainMenu> MainMenus { get; set; }

    //public virtual DbSet<NoticeBoard> NoticeBoards { get; set; }

    //public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    //public virtual DbSet<SubMenu> SubMenus { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //        => optionsBuilder.UseSqlServer("Server=DESKTOP-HB9H8DM;Database=EHRM;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmpType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmpType__3214EC076E1B806F");

            entity.ToTable("EmpType");

            entity.Property(e => e.EmpType1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EmpType");
        });

        modelBuilder.Entity<EmployeesCred>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC077B71D268");

            entity.ToTable("EmployeesCred");

            entity.HasIndex(e => e.EmpId, "UQ__Employee__AF2DBB987208FE69").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.EmpId).HasMaxLength(20);
            entity.Property(e => e.LockoutEndTime).HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasDefaultValue(0);
            entity.Property(e => e.TempPassword).HasMaxLength(255);
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

        //modelBuilder.Entity<MainMenu>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__MainMenu__3214EC079D17E828");

        //    entity.ToTable("MainMenu");

            //entity.Property(e => e.Icon)
            //    .HasMaxLength(30)
            //    .IsUnicode(false);
            //entity.Property(e => e.Name)
            //    .HasMaxLength(255)
            //    .IsUnicode(false);
       // });

        //modelBuilder.Entity<NoticeBoard>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__NoticeBo__3214EC078625739F");

        //    entity.ToTable("NoticeBoard");

        //    entity.Property(e => e.CreateDate).HasColumnType("datetime");
        //    entity.Property(e => e.Description).IsUnicode(false);
        //    entity.Property(e => e.HeadingName)
        //        .HasMaxLength(255)
        //        .IsUnicode(false);
        //    entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        //});

        //modelBuilder.Entity<Post>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__Post__3214EC07FB479F9B");

        //    entity.ToTable("Post");

        //    entity.Property(e => e.Description).IsUnicode(false);
        //    entity.Property(e => e.PostName)
        //        .HasMaxLength(255)
        //        .IsUnicode(false);
        //});

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

        //modelBuilder.Entity<SubMenu>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__SubMenu__3214EC07B9346644");

        //    entity.ToTable("SubMenu");

        //    entity.Property(e => e.Action)
        //        .HasMaxLength(255)
        //        .IsUnicode(false);
        //    entity.Property(e => e.Controller)
        //        .HasMaxLength(255)
        //        .IsUnicode(false);
        //    entity.Property(e => e.Name)
        //        .HasMaxLength(255)
        //        .IsUnicode(false);

        //    entity.HasOne(d => d.MainMenu).WithMany(p => p.SubMenus)
        //        .HasForeignKey(d => d.MainMenuId)
        //        .HasConstraintName("FK__SubMenu__MainMen__4222D4EF");

        //    entity.HasOne(d => d.Role).WithMany(p => p.SubMenus)
        //        .HasForeignKey(d => d.RoleId)
        //        .HasConstraintName("FK__SubMenu__RoleId__4316F928");
        //});

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
