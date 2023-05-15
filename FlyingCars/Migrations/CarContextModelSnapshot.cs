﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlyingCars.Migrations
{
    [DbContext(typeof(CarContext))]
    partial class CarContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FlyingCars.Models.Department.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("FlyingCars.Models.Employee.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("FlyingCars.Models.Employee.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MiddleName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("FlyingCars.Models.History.History", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DeletedAt")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("FlyingCars.Models.Linkers.EmployeeDepartmentLink", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("char(36)");

                    b.HasKey("EmployeeId", "DepartmentId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("EmployeeDepartmentLink");
                });

            modelBuilder.Entity("FlyingCars.Models.Linkers.EmployeePositionLink", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("PositionId")
                        .HasColumnType("char(36)");

                    b.HasKey("EmployeeId", "PositionId");

                    b.HasIndex("PositionId");

                    b.ToTable("EmployeePositionLink");
                });

            modelBuilder.Entity("FlyingCars.Models.Position.Position", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("FlyingCars.Models.Employee.Document", b =>
                {
                    b.HasOne("FlyingCars.Models.Employee.Employee", null)
                        .WithMany("Documents")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlyingCars.Models.Linkers.EmployeeDepartmentLink", b =>
                {
                    b.HasOne("FlyingCars.Models.Department.Department", null)
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FlyingCars.Models.Employee.Employee", null)
                        .WithMany("Departments")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlyingCars.Models.Linkers.EmployeePositionLink", b =>
                {
                    b.HasOne("FlyingCars.Models.Employee.Employee", null)
                        .WithMany("Positions")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlyingCars.Models.Position.Position", null)
                        .WithMany("Employees")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("FlyingCars.Models.Department.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("FlyingCars.Models.Employee.Employee", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("Documents");

                    b.Navigation("Positions");
                });

            modelBuilder.Entity("FlyingCars.Models.Position.Position", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
