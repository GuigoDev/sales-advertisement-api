﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SalesAnnouncements.Data;

#nullable disable

namespace SalesAnnouncements.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SalesAnnouncements.Models.Announcement", b =>
                {
                    b.Property<int>("AnnouncementID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnnouncementID"), 1L, 1);

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("AnnouncementID");

                    b.HasIndex("UserID");

                    b.ToTable("Announcement");
                });

            modelBuilder.Entity("SalesAnnouncements.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SalesAnnouncements.Models.Announcement", b =>
                {
                    b.HasOne("SalesAnnouncements.Models.User", null)
                        .WithMany("Announcement")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("SalesAnnouncements.Models.User", b =>
                {
                    b.Navigation("Announcement");
                });
#pragma warning restore 612, 618
        }
    }
}
