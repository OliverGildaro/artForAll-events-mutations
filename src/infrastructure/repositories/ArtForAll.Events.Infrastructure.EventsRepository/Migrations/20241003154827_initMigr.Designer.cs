﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OCP.PortalEvents.Repositories.Context;

#nullable disable

namespace ArtForAll.Events.Infrastructure.EFRepository.Migrations
{
    [DbContext(typeof(EventsContext))]
    [Migration("20241003154827_initMigr")]
    partial class initMigr
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ArtForAll.Events.Core.DomainModel.Entities.Event", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<int?>("Capacity")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("dateTime");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("dateTime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("dateTime");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("dateTime");

                    b.HasKey("Id");

                    b.ToTable("Events", null, t =>
                        {
                            t.HasCheckConstraint("CK_Event_Capacity_Positive", "[Capacity] IS NULL OR [Capacity] >= 0");

                            t.HasCheckConstraint("CK_Event_EndDate_AfterStart", "[EndDate] >= [StartDate]");

                            t.HasCheckConstraint("CK_Event_StartDate_Now", "[StartDate] >= GETDATE()");

                            t.HasCheckConstraint("CK_Price_MonetaryValue_Positive", "[MonetaryValue] IS NULL OR [MonetaryValue] >= 0");
                        });
                });

            modelBuilder.Entity("ArtForAll.Events.Core.DomainModel.Entities.Image", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ContentType")
                        .HasMaxLength(1000)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("EventId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("FileName")
                        .HasMaxLength(1000)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("EventId")
                        .IsUnique();

                    b.ToTable("Images", (string)null);
                });

            modelBuilder.Entity("ArtForAll.Events.Core.DomainModel.Entities.Event", b =>
                {
                    b.OwnsOne("ArtForAll.Events.Core.DomainModel.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<string>("EventId")
                                .HasColumnType("nvarchar(36)");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)")
                                .HasColumnName("City");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)")
                                .HasColumnName("Country");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)")
                                .HasColumnName("Number");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("Street");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)")
                                .HasColumnName("ZipCode");

                            b1.HasKey("EventId");

                            b1.ToTable("Events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.OwnsOne("ArtForAll.Events.Core.DomainModel.ValueObjects.Price", "Price", b1 =>
                        {
                            b1.Property<string>("EventId")
                                .HasColumnType("nvarchar(36)");

                            b1.Property<string>("CurrencyExchange")
                                .HasMaxLength(5)
                                .HasColumnType("nvarchar(5)")
                                .HasColumnName("CurrencyExchange");

                            b1.Property<float?>("MonetaryValue")
                                .HasColumnType("real")
                                .HasColumnName("MonetaryValue");

                            b1.HasKey("EventId");

                            b1.ToTable("Events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Price")
                        .IsRequired();
                });

            modelBuilder.Entity("ArtForAll.Events.Core.DomainModel.Entities.Image", b =>
                {
                    b.HasOne("ArtForAll.Events.Core.DomainModel.Entities.Event", null)
                        .WithOne("Image")
                        .HasForeignKey("ArtForAll.Events.Core.DomainModel.Entities.Image", "EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ArtForAll.Events.Core.DomainModel.Entities.Event", b =>
                {
                    b.Navigation("Image")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}