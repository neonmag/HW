using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HW;

public partial class CAdoHwHwDbMdfContext : DbContext
{
    public CAdoHwHwDbMdfContext()
    {
    }

    public CAdoHwHwDbMdfContext(DbContextOptions<CAdoHwHwDbMdfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\ADO\\HW\\HW\\DB.mdf;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC07848EC8C8");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeleteDt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Managers__3214EC07368BDBF1");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeleteDt).HasColumnType("datetime");
            entity.Property(e => e.IdChief).HasColumnName("Id_chief");
            entity.Property(e => e.IdMainDep).HasColumnName("Id_main_dep");
            entity.Property(e => e.IdSecDep).HasColumnName("Id_sec_dep");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Secname).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);

            entity.HasOne(d => d.IdMainDepNavigation).WithMany(p => p.ManagerIdMainDepNavigations)
                .HasForeignKey(d => d.IdMainDep)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Managers__Id_mai__3A81B327");

            entity.HasOne(d => d.IdSecDepNavigation).WithMany(p => p.ManagerIdSecDepNavigations)
                .HasForeignKey(d => d.IdSecDep)
                .HasConstraintName("FK__Managers__Id_sec__3B75D760");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC0712089AFB");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeleteDt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
