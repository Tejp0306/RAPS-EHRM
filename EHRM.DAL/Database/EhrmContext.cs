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

    public virtual DbSet<Holiday> Holidays { get; set; }

    //public virtual DbSet<MainMenu> MainMenus { get; set; }

    //public virtual DbSet<NoticeBoard> NoticeBoards { get; set; }

    //public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    //public virtual DbSet<SubMenu> SubMenus { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-S1TNCS5\\SQLEXPRESS;Database=EHRM;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmpType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmpType__3214EC076A9D326B");

            entity.ToTable("EmpType");

            entity.Property(e => e.EmpType1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EmpType");
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
            //entity.Property(e => e.TeamName)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);
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

        //    entity.Property(e => e.Name)
        //        .HasMaxLength(255)
        //        .IsUnicode(false);
        //});

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
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07E8F35265");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.RoleDescription).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(100);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
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
