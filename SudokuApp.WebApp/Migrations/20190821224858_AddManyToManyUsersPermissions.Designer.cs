﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SudokuApp.WebApp.Models.DataModel;

namespace SudokuApp.WebApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190821224858_AddManyToManyUsersPermissions")]
    partial class AddManyToManyUsersPermissions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SudokuApp.Models.Difficulty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DifficultyLevel");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Difficulties");
                });

            modelBuilder.Entity("SudokuApp.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ContinueGame");

                    b.Property<DateTime>("DateCompleted");

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("SudokuMatrixId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("SudokuMatrixId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("SudokuApp.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("PermissionLevel");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("SudokuApp.Models.SudokuCell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<List<int>>("AvailableValues");

                    b.Property<int>("Column");

                    b.Property<int>("DisplayValue");

                    b.Property<int>("Index");

                    b.Property<bool>("Obscured");

                    b.Property<int>("Region");

                    b.Property<int>("Row");

                    b.Property<int?>("SudokuMatrixId");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.HasIndex("SudokuMatrixId");

                    b.ToTable("SudokuCell");
                });

            modelBuilder.Entity("SudokuApp.Models.SudokuMatrix", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DifficultyId");

                    b.HasKey("Id");

                    b.HasIndex("DifficultyId");

                    b.ToTable("SudokuMatrix");
                });

            modelBuilder.Entity("SudokuApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("NickName");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SudokuApp.Models.UserPermission", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("PermissionId");

                    b.HasKey("UserId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("UsersPermissions");
                });

            modelBuilder.Entity("SudokuApp.Models.Game", b =>
                {
                    b.HasOne("SudokuApp.Models.SudokuMatrix", "SudokuMatrix")
                        .WithOne("Game")
                        .HasForeignKey("SudokuApp.Models.Game", "SudokuMatrixId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SudokuApp.Models.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SudokuApp.Models.Permission", b =>
                {
                    b.HasOne("SudokuApp.Models.User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SudokuApp.Models.SudokuCell", b =>
                {
                    b.HasOne("SudokuApp.Models.SudokuMatrix", "SudokuMatrix")
                        .WithMany("SudokuCells")
                        .HasForeignKey("SudokuMatrixId");
                });

            modelBuilder.Entity("SudokuApp.Models.SudokuMatrix", b =>
                {
                    b.HasOne("SudokuApp.Models.Difficulty", "Difficulty")
                        .WithMany("Matrices")
                        .HasForeignKey("DifficultyId");
                });

            modelBuilder.Entity("SudokuApp.Models.UserPermission", b =>
                {
                    b.HasOne("SudokuApp.Models.Permission", "Permission")
                        .WithMany("UsersPermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SudokuApp.Models.User", "User")
                        .WithMany("UsersPermissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
