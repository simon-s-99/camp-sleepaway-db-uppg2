﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using camp_sleepaway;

#nullable disable

namespace camp_sleepaway.Migrations
{
    [DbContext(typeof(CampContext))]
    partial class CampContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("camp_sleepaway.Camper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CabinId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LeaveDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.HasIndex("CabinId");

                    b.ToTable("Campers");
                });

            modelBuilder.Entity("camp_sleepaway.Counselor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("HiredDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<DateTime?>("TerminationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkTitle")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Counselors");
                });

            modelBuilder.Entity("camp_sleepaway.NextOfKin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CamperId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("RelationType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CamperId");

                    b.ToTable("NextOfKins");
                });

            modelBuilder.Entity("camp_sleepaway.ef_table_classes.Cabin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CabinName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("CounselorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CounselorId")
                        .IsUnique()
                        .HasFilter("[CounselorId] IS NOT NULL");

                    b.ToTable("Cabins");
                });

            modelBuilder.Entity("camp_sleepaway.Camper", b =>
                {
                    b.HasOne("camp_sleepaway.ef_table_classes.Cabin", "Cabin")
                        .WithMany("Campers")
                        .HasForeignKey("CabinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cabin");
                });

            modelBuilder.Entity("camp_sleepaway.NextOfKin", b =>
                {
                    b.HasOne("camp_sleepaway.Camper", "Camper")
                        .WithMany("NextOfKins")
                        .HasForeignKey("CamperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Camper");
                });

            modelBuilder.Entity("camp_sleepaway.ef_table_classes.Cabin", b =>
                {
                    b.HasOne("camp_sleepaway.Counselor", "Counselor")
                        .WithOne("Cabin")
                        .HasForeignKey("camp_sleepaway.ef_table_classes.Cabin", "CounselorId");

                    b.Navigation("Counselor");
                });

            modelBuilder.Entity("camp_sleepaway.Camper", b =>
                {
                    b.Navigation("NextOfKins");
                });

            modelBuilder.Entity("camp_sleepaway.Counselor", b =>
                {
                    b.Navigation("Cabin");
                });

            modelBuilder.Entity("camp_sleepaway.ef_table_classes.Cabin", b =>
                {
                    b.Navigation("Campers");
                });
#pragma warning restore 612, 618
        }
    }
}
