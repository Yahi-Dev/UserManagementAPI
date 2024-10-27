using System;
using System.Collections.Generic;
using Layer_Entities.ModelsDB;
using Microsoft.EntityFrameworkCore;

namespace Layer_DB.Context;

public partial class UserManagementDbContext : DbContext
{
    public UserManagementDbContext()
    {
    }

    public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<User> Users { get; set; }


    public override Task<int> SaveChangesAsync(CancellationToken cancellation = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<User>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.LastLogin = DateTime.Now;
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.IsActive = true;
                    break;

                case EntityState.Modified:
                    entry.Entity.Modified = DateTime.Now;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellation);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.PhoneId).HasName("PK__Phones__F3EE4BD049B01581");

            entity.HasIndex(e => e.Number, "UQ__Phones__78A1A19D74B457DF").IsUnique();

            entity.Property(e => e.PhoneId).HasColumnName("PhoneID");
            entity.Property(e => e.CityCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("City_Code");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Country_Code");
            entity.Property(e => e.Number)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Phones)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phones_UsuariosID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACBDD364E7");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105342E8B1D8E").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastLogin).HasColumnName("Last_login");
            entity.Property(e => e.NameUser)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
