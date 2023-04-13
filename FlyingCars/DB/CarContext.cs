using FlyingCars.EmployeeExample;
using Microsoft.EntityFrameworkCore;

public class CarContext : DbContext
{
    public CarContext(DbContextOptions<CarContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<History> Histories => Set<History>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Employee - Document
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Documents)
            .WithOne()
            .HasForeignKey(d => d.EmployeeId)
            .IsRequired();

        modelBuilder.Entity<Document>()
            .HasOne<Employee>()
            .WithMany(e => e.Documents)
            .IsRequired();

        // Employee - EmployeePositionLink - Position
        modelBuilder.Entity<EmployeePositionLink>()
            .HasKey(ep => new {ep.EmployeeId, ep.PositionId });

        modelBuilder.Entity<EmployeePositionLink>()
            .HasOne<Employee>()
            .WithMany(e => e.Positions)
            .HasForeignKey(ep => ep.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Positions)
            .WithOne()
            .HasForeignKey(ep => ep.EmployeeId)
            .IsRequired();

        modelBuilder.Entity<EmployeePositionLink>()
            .HasOne<Position>()
            .WithMany(p => p.Employees)
            .HasForeignKey(ep => ep.PositionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Position>()
            .HasMany(p => p.Employees)
            .WithOne()
            .HasForeignKey(ep => ep.PositionId)
            .IsRequired();
        
        // Employee - EmployeeDepartmentLink - Department
        modelBuilder.Entity<EmployeeDepartmentLink>()
            .HasKey(ep => new { ep.EmployeeId, ep.DepartmentId });

        modelBuilder.Entity<EmployeeDepartmentLink>()
            .HasOne<Employee>()
            .WithMany(e => e.Departments)
            .HasForeignKey(ep => ep.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Departments)
            .WithOne()
            .HasForeignKey(ep => ep.EmployeeId)
            .IsRequired();

        modelBuilder.Entity<EmployeeDepartmentLink>()
            .HasOne<Department>()
            .WithMany(p => p.Employees)
            .HasForeignKey(ep => ep.DepartmentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Department>()
            .HasMany(p => p.Employees)
            .WithOne()
            .HasForeignKey(ep => ep.DepartmentId)
            .IsRequired();

        modelBuilder.Entity<Document>()
            .Property(d => d.Type)
            .HasConversion<string>();
    }
}
