using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplicationCRUD.Models;
namespace WebApplicationCRUD.Data;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; }
    public DbSet<Users> Users { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FatherName).HasMaxLength(50);
            entity.Property(e => e.Standard)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StudentGender).HasMaxLength(8);
            entity.Property(e => e.StudentName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
