using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MVC_App.Models;

public partial class MVCApp : DbContext
{
    public MVCApp()
    {
    }

    public MVCApp(DbContextOptions<MVCApp> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Call> Calls { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<Programm> Programms { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__4A3006F7F49AC148");

            entity.HasOne(d => d.User).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Admin__User_id__3F466844");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bills__CF6E7D4369376886");

            entity.HasOne(d => d.PhoneNumberNavigation).WithMany(p => p.Bills)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bills__PhoneNumb__46E78A0C");

            entity.HasMany(d => d.Calls).WithMany(p => p.Bills)
                .UsingEntity<Dictionary<string, object>>(
                    "BillsCall",
                    r => r.HasOne<Call>().WithMany()
                        .HasForeignKey("CallId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__BillsCall__Call___4CA06362"),
                    l => l.HasOne<Bill>().WithMany()
                        .HasForeignKey("BillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__BillsCall__Bill___4BAC3F29"),
                    j =>
                    {
                        j.HasKey("BillId", "CallId").HasName("PK__BillsCal__6EF0120DAF4D5708");
                        j.ToTable("BillsCalls");
                        j.IndexerProperty<int>("BillId").HasColumnName("Bill_ID");
                        j.IndexerProperty<int>("CallId").HasColumnName("Call_ID");
                    });
        });

        modelBuilder.Entity<Call>(entity =>
        {
            entity.HasKey(e => e.CallId).HasName("PK__Calls__19E6F4EBB48E76CB");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Clients__75A5D7182B365D98");

            entity.HasOne(d => d.User).WithMany(p => p.Clients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clients__User_id__3C69FB99");
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.PhoneNumber).HasName("PK__Phones__85FB4E395D793215");

            entity.HasOne(d => d.ProgrammNameNavigation).WithMany(p => p.Phones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phones__ProgramN__440B1D61");
        });

        modelBuilder.Entity<Programm>(entity =>
        {
            entity.HasKey(e => e.ProgrammName).HasName("PK__Programs__4F925711BFB93566");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.SellerId).HasName("PK__Sellers__016148B1D10B493F");

            entity.HasOne(d => d.User).WithMany(p => p.Sellers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sellers__User_id__398D8EEE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__206A9DF89E4F90D1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
