﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QueueControlServer.Models;

#nullable disable

namespace QueueControlServer.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20220519103216_InitialAuthentication")]
    partial class InitialAuthentication
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("QueueControlServer.Models.GoodsParameter", b =>
                {
                    b.Property<int>("GoodsParameterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GoodsParameterId"), 1L, 1);

                    b.HasKey("GoodsParameterId");

                    b.ToTable("GoodsParameter");
                });

            modelBuilder.Entity("QueueControlServer.Models.Organization", b =>
                {
                    b.Property<Guid>("OrganizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OrganizationName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrganizationId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("QueueControlServer.Models.Queue", b =>
                {
                    b.Property<Guid>("QueueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("BrandGoodsParameterId")
                        .HasColumnType("int");

                    b.Property<Guid?>("FactoryOrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("PackageGoodsParameterId")
                        .HasColumnType("int");

                    b.Property<string>("QueueName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QueueId");

                    b.HasIndex("BrandGoodsParameterId");

                    b.HasIndex("FactoryOrganizationId");

                    b.HasIndex("PackageGoodsParameterId");

                    b.ToTable("Queue");
                });

            modelBuilder.Entity("QueueControlServer.Models.QueueItems", b =>
                {
                    b.Property<Guid>("QueueItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Barecode")
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<int?>("BrandGoodsParameterId")
                        .HasColumnType("int");

                    b.Property<string>("BrandName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CarNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CarrierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CarrierName")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("———");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("DriverName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("FactoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FactoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OderLine")
                        .HasColumnType("int");

                    b.Property<int?>("PacakegeGoodsParameterId")
                        .HasColumnType("int");

                    b.Property<string>("PackageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QueueId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("QueueName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QueueItemId");

                    b.HasIndex("BrandGoodsParameterId");

                    b.HasIndex("CarrierId");

                    b.HasIndex("FactoryId");

                    b.HasIndex("PacakegeGoodsParameterId");

                    b.HasIndex("QueueId");

                    b.ToTable("QueueItems");
                });

            modelBuilder.Entity("QueueControlServer.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("QueueControlServer.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FactoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid?>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("OrganizationId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("QueueControlServer.Models.Brand", b =>
                {
                    b.HasBaseType("QueueControlServer.Models.GoodsParameter");

                    b.Property<string>("BrandName")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("QueueControlServer.Models.Carrier", b =>
                {
                    b.HasBaseType("QueueControlServer.Models.Organization");

                    b.ToTable("Carriers");
                });

            modelBuilder.Entity("QueueControlServer.Models.Driver", b =>
                {
                    b.HasBaseType("QueueControlServer.Models.User");

                    b.Property<Guid?>("CarrierOrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DriverName")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("CarrierOrganizationId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("QueueControlServer.Models.Factory", b =>
                {
                    b.HasBaseType("QueueControlServer.Models.Organization");

                    b.ToTable("Factories");
                });

            modelBuilder.Entity("QueueControlServer.Models.Package", b =>
                {
                    b.HasBaseType("QueueControlServer.Models.GoodsParameter");

                    b.Property<string>("PackageName")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("QueueControlServer.Models.Supervisor", b =>
                {
                    b.HasBaseType("QueueControlServer.Models.User");

                    b.Property<Guid?>("OrganizationId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SupervisorName")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("OrganizationId1");

                    b.ToTable("Supervisors");
                });

            modelBuilder.Entity("QueueControlServer.Models.Terminal", b =>
                {
                    b.HasBaseType("QueueControlServer.Models.User");

                    b.Property<Guid?>("FactoryOrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TerminalName")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("FactoryOrganizationId");

                    b.ToTable("Terminals");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("QueueControlServer.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("QueueControlServer.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("QueueControlServer.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("QueueControlServer.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QueueControlServer.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("QueueControlServer.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QueueControlServer.Models.Queue", b =>
                {
                    b.HasOne("QueueControlServer.Models.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandGoodsParameterId");

                    b.HasOne("QueueControlServer.Models.Factory", "Factory")
                        .WithMany()
                        .HasForeignKey("FactoryOrganizationId");

                    b.HasOne("QueueControlServer.Models.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageGoodsParameterId");

                    b.Navigation("Brand");

                    b.Navigation("Factory");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("QueueControlServer.Models.QueueItems", b =>
                {
                    b.HasOne("QueueControlServer.Models.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandGoodsParameterId");

                    b.HasOne("QueueControlServer.Models.Carrier", "Carriers")
                        .WithMany("QueueItems")
                        .HasForeignKey("CarrierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QueueControlServer.Models.Factory", "Factory")
                        .WithMany("QueueItems")
                        .HasForeignKey("FactoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QueueControlServer.Models.Package", "Pacakege")
                        .WithMany()
                        .HasForeignKey("PacakegeGoodsParameterId");

                    b.HasOne("QueueControlServer.Models.Queue", "Queue")
                        .WithMany()
                        .HasForeignKey("QueueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Carriers");

                    b.Navigation("Factory");

                    b.Navigation("Pacakege");

                    b.Navigation("Queue");
                });

            modelBuilder.Entity("QueueControlServer.Models.User", b =>
                {
                    b.HasOne("QueueControlServer.Models.Organization", null)
                        .WithMany("Users")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("QueueControlServer.Models.Brand", b =>
                {
                    b.HasOne("QueueControlServer.Models.GoodsParameter", null)
                        .WithOne()
                        .HasForeignKey("QueueControlServer.Models.Brand", "GoodsParameterId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QueueControlServer.Models.Carrier", b =>
                {
                    b.HasOne("QueueControlServer.Models.Organization", null)
                        .WithOne()
                        .HasForeignKey("QueueControlServer.Models.Carrier", "OrganizationId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QueueControlServer.Models.Driver", b =>
                {
                    b.HasOne("QueueControlServer.Models.Carrier", "Carrier")
                        .WithMany("Drivers")
                        .HasForeignKey("CarrierOrganizationId");

                    b.HasOne("QueueControlServer.Models.User", null)
                        .WithOne()
                        .HasForeignKey("QueueControlServer.Models.Driver", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Carrier");
                });

            modelBuilder.Entity("QueueControlServer.Models.Factory", b =>
                {
                    b.HasOne("QueueControlServer.Models.Organization", null)
                        .WithOne()
                        .HasForeignKey("QueueControlServer.Models.Factory", "OrganizationId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QueueControlServer.Models.Package", b =>
                {
                    b.HasOne("QueueControlServer.Models.GoodsParameter", null)
                        .WithOne()
                        .HasForeignKey("QueueControlServer.Models.Package", "GoodsParameterId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QueueControlServer.Models.Supervisor", b =>
                {
                    b.HasOne("QueueControlServer.Models.User", null)
                        .WithOne()
                        .HasForeignKey("QueueControlServer.Models.Supervisor", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("QueueControlServer.Models.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId1");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("QueueControlServer.Models.Terminal", b =>
                {
                    b.HasOne("QueueControlServer.Models.Factory", "Factory")
                        .WithMany("Terminals")
                        .HasForeignKey("FactoryOrganizationId");

                    b.HasOne("QueueControlServer.Models.User", null)
                        .WithOne()
                        .HasForeignKey("QueueControlServer.Models.Terminal", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Factory");
                });

            modelBuilder.Entity("QueueControlServer.Models.Organization", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("QueueControlServer.Models.Carrier", b =>
                {
                    b.Navigation("Drivers");

                    b.Navigation("QueueItems");
                });

            modelBuilder.Entity("QueueControlServer.Models.Factory", b =>
                {
                    b.Navigation("QueueItems");

                    b.Navigation("Terminals");
                });
#pragma warning restore 612, 618
        }
    }
}
