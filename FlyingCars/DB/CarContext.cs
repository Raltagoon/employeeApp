using FlyingCars.Models;
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

        // Employee - EmployeePositionLink - Position
        modelBuilder.Entity<EmployeePositionLink>()
            .HasKey(ep => new {ep.EmployeeId, ep.PositionId });

        modelBuilder.Entity<EmployeePositionLink>()
            .HasOne<Employee>()
            .WithMany(e => e.Positions)
            .HasForeignKey(ep => ep.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmployeePositionLink>()
            .HasOne<Position>()
            .WithMany(p => p.Employees)
            .HasForeignKey(ep => ep.PositionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        // Employee - EmployeeDepartmentLink - Department
        modelBuilder.Entity<EmployeeDepartmentLink>()
            .HasKey(ep => new { ep.EmployeeId, ep.DepartmentId });

        modelBuilder.Entity<EmployeeDepartmentLink>()
            .HasOne<Employee>()
            .WithMany(e => e.Departments)
            .HasForeignKey(ep => ep.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmployeeDepartmentLink>()
            .HasOne<Department>()
            .WithMany(p => p.Employees)
            .HasForeignKey(ep => ep.DepartmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
