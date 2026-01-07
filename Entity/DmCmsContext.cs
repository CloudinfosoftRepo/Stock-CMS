using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Stock_CMS.Entity;

public partial class DmCmsContext : DbContext
{
    public DmCmsContext()
    {
    }

    public DmCmsContext(DbContextOptions<DmCmsContext> options)
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

    public virtual DbSet<TblMenu> TblMenus { get; set; }

    public virtual DbSet<TblNominee> TblNominees { get; set; }

    public virtual DbSet<TblPermission> TblPermissions { get; set; }

    public virtual DbSet<TblRelationMapping> TblRelationMappings { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRtaCompany> TblRtaCompanies { get; set; }

    public virtual DbSet<TblStock> TblStocks { get; set; }

    public virtual DbSet<TblTracking> TblTrackings { get; set; }

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
            entity.Property(e => e.BankCity)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.BankEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BankFirstHolder)
                .HasMaxLength(50)
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
            entity.Property(e => e.BankSecondHolder)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankThirdHolder)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Branch)
                .HasMaxLength(50)
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
            entity.Property(e => e.NameAsPerBankAccount)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PinCode)
                .HasMaxLength(10)
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
            entity.Property(e => e.ContactPersonMobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ContactPersonName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DocumentJson).IsUnicode(false);
            entity.Property(e => e.FileNo)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.FinancialYear)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reference)
                .HasMaxLength(50)
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
            entity.Property(e => e.AddressAsPerPassport).IsUnicode(false);
            entity.Property(e => e.AddressAsPerVoterId).IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(25)
                .IsUnicode(false);
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
            entity.Property(e => e.DpName)
                .HasMaxLength(50)
                .IsUnicode(false);
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
            entity.Property(e => e.NameAsPerCertificate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameAsPerDeathCerti)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameAsPerPan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NameAsPerPAN");
            entity.Property(e => e.NameAsPerPassport)
                .HasMaxLength(50)
                .IsUnicode(false);
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
            entity.Property(e => e.PassportNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassportUrl).IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PinCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PlaceOfDeath)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Relationship)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.RelativesName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VoterIdUrl)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.WitnessJson).IsUnicode(false);

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
            entity.Property(e => e.AddressAsPerPassport).IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(25)
                .IsUnicode(false);
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
            entity.Property(e => e.DpName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dpid)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DPId");
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
            entity.Property(e => e.NameAsPerCertificate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameAsPerDeathCerti)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameAsPerPan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NameAsPerPAN");
            entity.Property(e => e.NameAsPerPassport)
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
            entity.Property(e => e.PassportNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassportUrl).IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PinCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PlaceOfDeath)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RelationWithDead)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TBL_Menu_Master");

            entity.ToTable("TBL_Menu");

            entity.Property(e => e.Actions).IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Url)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblNominee>(entity =>
        {
            entity.ToTable("Tbl_Nominee");

            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountType)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.BankCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankPinCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Branch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.Micrcode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MICRCode");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Pan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAN");
            entity.Property(e => e.Pincode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TBL_Permission_Master");

            entity.ToTable("TBL_Permission");

            entity.Property(e => e.Actions).IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.MenuId).HasColumnName("Menu_Id");
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Menu).WithMany(p => p.TblPermissions)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TBL_Permission_TBL_Menu");

            entity.HasOne(d => d.Role).WithMany(p => p.TblPermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TBL_Permission_TBL_Role");

            entity.HasOne(d => d.User).WithMany(p => p.TblPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TBL_Permission_TBL_User");
        });

        modelBuilder.Entity<TblRelationMapping>(entity =>
        {
            entity.ToTable("Tbl_RelationMapping");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RelationWithDead)
                .HasMaxLength(20)
                .IsUnicode(false);
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

        modelBuilder.Entity<TblRtaCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tbl_RTA");

            entity.ToTable("Tbl_RTA_Company");

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

            entity.Property(e => e.Brokerage).HasColumnName("brokerage");
            entity.Property(e => e.ClaimStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Claim_Status");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");
            entity.Property(e => e.FirstHolderId).HasColumnName("First_HolderId");
            entity.Property(e => e.FolioNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Folio_No");
            entity.Property(e => e.NomineeJson).IsUnicode(false);
            entity.Property(e => e.Ptbf)
                .IsUnicode(false)
                .HasColumnName("PTBF");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.SecondHolderId).HasColumnName("Second_HolderId");
            entity.Property(e => e.StockJson).IsUnicode(false);
            entity.Property(e => e.ThirdHolderId).HasColumnName("Third_HolderId");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.TblStocks)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_Tbl_Stock_Tbl_Company");

            entity.HasOne(d => d.Customer).WithMany(p => p.TblStocks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Tbl_Stock_Tbl_Stock");

            entity.HasOne(d => d.FirstHolder).WithMany(p => p.TblStockFirstHolders)
                .HasForeignKey(d => d.FirstHolderId)
                .HasConstraintName("FK_Tbl_Stock_Tbl_Doc");

            entity.HasOne(d => d.SecondHolder).WithMany(p => p.TblStockSecondHolders)
                .HasForeignKey(d => d.SecondHolderId)
                .HasConstraintName("FK_Tbl_Stock_Tbl_Doc1");

            entity.HasOne(d => d.ThirdHolder).WithMany(p => p.TblStockThirdHolders)
                .HasForeignKey(d => d.ThirdHolderId)
                .HasConstraintName("FK_Tbl_Stock_Tbl_Doc2");
        });

        modelBuilder.Entity<TblTracking>(entity =>
        {
            entity.ToTable("Tbl_Tracking");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DateofFollowUp).HasColumnType("datetime");
            entity.Property(e => e.DateofSubmission).HasColumnType("datetime");
            entity.Property(e => e.DpIdClientId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DpName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Iepfstatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IEPFStatus");
            entity.Property(e => e.Process)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.ResponseUrl)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SendTo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SendUrl)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Srndate)
                .HasColumnType("datetime")
                .HasColumnName("SRNDate");
            entity.Property(e => e.Srnno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SRNNo");
            entity.Property(e => e.Status).IsUnicode(false);
            entity.Property(e => e.TrackingId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Stock).WithMany(p => p.TblTrackings)
                .HasForeignKey(d => d.StockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_Tracking_Tbl_Stock");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.ToTable("TBL_User");

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CurrentOtp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CurrentOTP");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryTime).HasColumnType("datetime");
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
