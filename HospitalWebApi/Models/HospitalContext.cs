using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HospitalWebApi.Models;

public partial class HospitalContext : DbContext
{
    public HospitalContext()
    {
    }

    public HospitalContext(DbContextOptions<HospitalContext> options)
        : base(options)
    {
    }
    public DbSet<Department> Departments { get; set; }
    public virtual DbSet<BillHeader> BillHeaders { get; set; }

    public virtual DbSet<BillReceipt> BillReceipts { get; set; }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Outsource> Outsources { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceReceipt> ServiceReceipts { get; set; }

    public virtual DbSet<Type> Types { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }




    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MOHAN\\SQLEXPRESS;Initial Catalog=HospitalDB;User ID=Sa;Password=123;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillHeader>(entity =>
        {
            entity.HasKey(e => e.BillHeaderId).HasName("PK__BillHead__92E153F25F76DB0B");

            entity.ToTable("BillHeader");

            entity.Property(e => e.BillDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Consultation).WithMany(p => p.BillHeaders)
                .HasForeignKey(d => d.ConsultationId)
                .HasConstraintName("FK_BillHeader_Consultation");

            entity.HasOne(d => d.Patient).WithMany(p => p.BillHeaders)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BillHeader_Patient");
        });

        modelBuilder.Entity<BillReceipt>(entity =>
        {
            entity.HasKey(e => e.BillReceiptId).HasName("PK__BillRece__F594DA2E076C15B6");

            entity.ToTable("BillReceipt");

            entity.Property(e => e.PaidAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMode).HasMaxLength(50);

            entity.HasOne(d => d.BillHeader).WithMany(p => p.BillReceipts)
                .HasForeignKey(d => d.BillHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BillReceipt_BillHeader");
        });

        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasKey(e => e.ConsultationId).HasName("PK__Consulta__5D014A98A13C07D0");

            entity.Property(e => e.ConsultationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Fee).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Notes).HasMaxLength(255);

            entity.HasOne(d => d.Doctor).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consultation_Doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consultation_Patient");

            entity.HasOne(d => d.Service).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consultation_Service");
        });


        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BEDE06E85A1");

            entity.Property(e => e.DepartmentName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);

            entity.HasOne(d => d.Type).WithMany(p => p.Departments)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Departments_Type");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EBF01D00BFC");

            entity.Property(e => e.DoctorName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Qualification).HasMaxLength(100);

            entity.HasOne(d => d.Type)
                .WithMany(p => p.Doctors)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctors_Type");

            // ✅ ADD THIS BLOCK
            entity.HasOne(d => d.Department)
                .WithMany(p => p.Doctors)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctors_Department");
        });


        modelBuilder.Entity<Outsource>(entity =>
        {
            entity.HasKey(e => e.OutsourceId).HasName("PK__Outsourc__AC64BDB3677E9AB8");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContactPerson).HasMaxLength(100);
            entity.Property(e => e.ContractEndDate).HasColumnType("datetime");
            entity.Property(e => e.ContractStartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.OutsourceName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);

            entity.HasOne(d => d.Type).WithMany(p => p.Outsources)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Outsources_Type");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC366C51214C9");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.PatientName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.RegisteredDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00ADC291193");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Fee).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServiceName).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Services)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Department");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Services)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Doctor");

            entity.HasOne(d => d.Outsource).WithMany(p => p.Services)
                .HasForeignKey(d => d.OutsourceId)
                .HasConstraintName("FK_Services_Outsource");

            entity.HasOne(d => d.Type).WithMany(p => p.Services)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Type");
        });

        modelBuilder.Entity<ServiceReceipt>(entity =>
        {
            entity.HasKey(e => e.ServiceReceiptId).HasName("PK__ServiceR__109E6D55B7C35E61");

            entity.ToTable("ServiceReceipt", tb => tb.HasTrigger("trg_UpdateBillHeaderTotal"));

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.BillHeader).WithMany(p => p.ServiceReceipts)
                .HasForeignKey(d => d.BillHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceReceipt_BillHeader");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceReceipts)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceReceipt_Service");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Type__3214EC079786908A");

            entity.ToTable("Type");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.TypeName).HasMaxLength(100);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Type_Parent");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
