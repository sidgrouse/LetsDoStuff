﻿// <auto-generated />
using System;
using LetsDoStuff.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LetsDoStuff.Domain.Migrations
{
    [DbContext(typeof(LdsContext))]
    partial class LdsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("LetsDoStuff.Domain.Models.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("LetsDoStuff.Domain.Models.ActivityTag", b =>
                {
                    b.Property<int>("ActivityId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<int?>("ActivityId1")
                        .HasColumnType("int");

                    b.Property<int?>("TagId1")
                        .HasColumnType("int");

                    b.HasKey("ActivityId", "TagId");

                    b.HasIndex("ActivityId1");

                    b.HasIndex("TagId");

                    b.HasIndex("TagId1");

                    b.ToTable("ActivityTag");
                });

            modelBuilder.Entity("LetsDoStuff.Domain.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("LetsDoStuff.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LetsDoStuff.Domain.Models.Activity", b =>
                {
                    b.HasOne("LetsDoStuff.Domain.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("LetsDoStuff.Domain.Models.ActivityTag", b =>
                {
                    b.HasOne("LetsDoStuff.Domain.Models.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LetsDoStuff.Domain.Models.Activity", null)
                        .WithMany("ActivityTags")
                        .HasForeignKey("ActivityId1");

                    b.HasOne("LetsDoStuff.Domain.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LetsDoStuff.Domain.Models.Tag", null)
                        .WithMany("ActivityTags")
                        .HasForeignKey("TagId1");

                    b.Navigation("Activity");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("LetsDoStuff.Domain.Models.Activity", b =>
                {
                    b.Navigation("ActivityTags");
                });

            modelBuilder.Entity("LetsDoStuff.Domain.Models.Tag", b =>
                {
                    b.Navigation("ActivityTags");
                });
#pragma warning restore 612, 618
        }
    }
}
