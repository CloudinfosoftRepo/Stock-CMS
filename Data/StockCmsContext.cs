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

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblStock> TblStocks { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=HP\\SQLEXPRESS; database=Stock-CMS;trusted_connection=true; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.ToTable("Tbl_Customer");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(15)
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

        modelBuilder.Entity<TblStock>(entity =>
        {
            entity.ToTable("Tbl_Stock");

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
