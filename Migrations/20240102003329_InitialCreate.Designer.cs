﻿// <auto-generated />
using System;
using DB.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KanbanBoard.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240102003329_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("DB.Tables.Board", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Boards");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9ecc19fb-01c2-449c-959f-b485b6c7477d"),
                            Title = "Platform Launch"
                        },
                        new
                        {
                            Id = new Guid("b1002294-d420-4a25-b27b-9040f3197c60"),
                            Title = "Marketing Plan"
                        },
                        new
                        {
                            Id = new Guid("5f35a7cb-b73b-411e-a27e-0f5ad6632361"),
                            Title = "Roadmap"
                        });
                });

            modelBuilder.Entity("DB.Tables.Status", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("DB.Tables.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("DB.Tables.Status", b =>
                {
                    b.HasOne("DB.Tables.Board", "Board")
                        .WithMany("Statuses")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("DB.Tables.Task", b =>
                {
                    b.HasOne("DB.Tables.Status", "Status")
                        .WithMany("Tasks")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");
                });

            modelBuilder.Entity("DB.Tables.Board", b =>
                {
                    b.Navigation("Statuses");
                });

            modelBuilder.Entity("DB.Tables.Status", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
