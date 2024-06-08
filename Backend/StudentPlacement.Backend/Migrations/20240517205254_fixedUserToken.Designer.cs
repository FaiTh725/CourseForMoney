﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentPlacement.Backend.Dal;

#nullable disable

namespace StudentPlacement.Backend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240517205254_fixedUserToken")]
    partial class fixedUserToken
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.AllocationRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountPlace")
                        .HasColumnType("int");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("UserId");

                    b.ToTable("AllocationRequests");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("HeadOfDepartment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SpecializationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SpecializationId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AllocationRequestId")
                        .HasColumnType("int");

                    b.Property<string>("Contacts")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AllocationRequestId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Specialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AllocationRequestId")
                        .HasColumnType("int");

                    b.Property<double>("AverageScore")
                        .HasColumnType("float");

                    b.Property<bool>("ExtendedFamily")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<int?>("IdAllocationRequest")
                        .HasColumnType("int");

                    b.Property<bool>("IsMarried")
                        .HasColumnType("bit");

                    b.Property<int>("StatusRequest")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AllocationRequestId");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TimeEndToken")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.AllocationRequest", b =>
                {
                    b.HasOne("StudentPlacement.Backend.Domain.Entities.Department", null)
                        .WithMany("Requests")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("StudentPlacement.Backend.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Group", b =>
                {
                    b.HasOne("StudentPlacement.Backend.Domain.Entities.Specialization", "Specialization")
                        .WithMany()
                        .HasForeignKey("SpecializationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Organization", b =>
                {
                    b.HasOne("StudentPlacement.Backend.Domain.Entities.AllocationRequest", "AllocationRequest")
                        .WithMany()
                        .HasForeignKey("AllocationRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AllocationRequest");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Specialization", b =>
                {
                    b.HasOne("StudentPlacement.Backend.Domain.Entities.Department", null)
                        .WithMany("Specializations")
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Student", b =>
                {
                    b.HasOne("StudentPlacement.Backend.Domain.Entities.AllocationRequest", "AllocationRequest")
                        .WithMany()
                        .HasForeignKey("AllocationRequestId");

                    b.HasOne("StudentPlacement.Backend.Domain.Entities.Group", null)
                        .WithMany("Students")
                        .HasForeignKey("GroupId");

                    b.HasOne("StudentPlacement.Backend.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AllocationRequest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Department", b =>
                {
                    b.Navigation("Requests");

                    b.Navigation("Specializations");
                });

            modelBuilder.Entity("StudentPlacement.Backend.Domain.Entities.Group", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
