using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Stock_CMS.Entity;

public partial class StockCmsContext : DbContext
{
    public StockCmsContext()
    {
    }

    public StockCmsContext(DbContextOptions<StockCmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBank> TblBanks { get; set; }

    public virtual DbSet<TblCompany> TblCompanies { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblDoc> TblDocs { get; set; }

    public virtual DbSet<TblForm> TblForms { get; set; }

    public virtual DbSet<TblGenratedForm> TblGenratedForms { get; set; }

    public virtual DbSet<TblHolderDoc> TblHolderDocs { get; set; }

    public virtual DbSet<TblLegalHeir> TblLegalHeirs { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRta> TblRta { get; set; }

    public virtual DbSet<TblStock> TblStocks { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
=> optionsBuilder.UseSqlServer("Name=ConnectionStrings:DBConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBank>(entity =>
        {
            entity.ToTable("Tbl_Bank");

            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpeningDate).HasColumnType("datetime");
            entity.Property(e => e.AccountType)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.BankEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BankManagerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankMangerEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Branch)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.Micrcode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MICRCode");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PostalAddress).IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblCompany>(entity =>
        {
            entity.ToTable("Tbl_Company");

            entity.Property(e => e.CompanyAddress)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CompanyPinCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Rtaid).HasColumnName("RTAId");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Rta).WithMany(p => p.TblCompanies)
                .HasForeignKey(d => d.Rtaid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_Company_Tbl_RTA");
        });

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.ToTable("Tbl_Customer");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblDoc>(entity =>
        {
            entity.ToTable("Tbl_Doc");

            entity.Property(e => e.Aadhar)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AadharUrl)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AddressAsPerAadhar).IsUnicode(false);
            entity.Property(e => e.AddressAsPerVoterId).IsUnicode(false);
            entity.Property(e => e.ClientId)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DateOfDeath).HasColumnType("datetime");
            entity.Property(e => e.DeathCertiUrl)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Dpid)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DPId");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameAsPerAadhar)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameAsPerDeathCerti)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameAsPerPan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NameAsPerPAN");
            entity.Property(e => e.NameAsPerVoterId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAN");
            entity.Property(e => e.Panurl)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PANUrl");
            entity.Property(e => e.PlaceOfDeath)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Relationship)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.RelativesName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.VoterIdUrl)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.TblDocs)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Tbl_Doc_Tbl_Customer");
        });

        modelBuilder.Entity<TblForm>(entity =>
        {
            entity.ToTable("Tbl_Form");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FormName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("URL");
        });

        modelBuilder.Entity<TblGenratedForm>(entity =>
        {
            entity.ToTable("Tbl_Genrated_Forms");

            entity.Property(e => e.ClientName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FormName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JsonString).IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Url)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Stock).WithMany(p => p.TblGenratedForms)
                .HasForeignKey(d => d.StockId)
                .HasConstraintName("FK_Tbl_Genrated_Forms_Tbl_Stock");
        });

        modelBuilder.Entity<TblHolderDoc>(entity =>
        {
            entity.ToTable("Tbl_HolderDocs");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DocUrl)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.DocumentName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Holder).WithMany(p => p.TblHolderDocs)
                .HasForeignKey(d => d.HolderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_HolderDocs_Tbl_Doc");
        });

        modelBuilder.Entity<TblLegalHeir>(entity =>
        {
            entity.ToTable("Tbl_LegalHeir");

            entity.Property(e => e.Aadhar)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AadharUrl)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AddressAsPerAadhar).IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsNoc).HasColumnName("IsNOC");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameAsPerAadhar)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameAsPerPan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NameAsPerPAN");
            entity.Property(e => e.Pan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAN");
            entity.Property(e => e.Panurl)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PANUrl");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.ToTable("TBL_Role");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblRta>(entity =>
        {
            entity.ToTable("Tbl_RTA");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.RtaAddress)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RtaName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblStock>(entity =>
        {
            entity.ToTable("Tbl_Stock");

            entity.Property(e => e.ActualQty)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Brokerage).HasColumnName("brokerage");
            entity.Property(e => e.ClamStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Clam_Status");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");
            entity.Property(e => e.FirstHolder)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("First_Holder");
            entity.Property(e => e.FolioNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Folio_No");
            entity.Property(e => e.Ptbf)
                .IsUnicode(false)
                .HasColumnName("PTBF");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.SecondHolder)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Second_Holder");
            entity.Property(e => e.StockJson).IsUnicode(false);
            entity.Property(e => e.ThirdHolder)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Third_Holder");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.TblStocks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Tbl_Stock_Tbl_Stock");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.ToTable("TBL_User");

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TBL_User_TBL_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
