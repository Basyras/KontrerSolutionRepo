﻿// <auto-generated />
using System;
using Kontrer.OwnerServer.OrderService.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kontrer.OwnerServer.OrderService.Infrastructure.Migrations
{
    [DbContext(typeof(OrderServiceDbContext))]
    [Migration("20210701224219_AddingPKToAccoOrder")]
    partial class AddingPKToAccoOrder
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrders.AccommodationOrderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("CustomerNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OwnerPrivateNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrders.AccommodationOrderEntity", b =>
                {
                    b.OwnsOne("Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements.AccommodationRequirement", "Requirment", b1 =>
                        {
                            b1.Property<int>("AccommodationOrderEntityId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.HasKey("AccommodationOrderEntityId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("AccommodationOrderEntityId");

                            b1.OwnsMany("Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements.RoomRequirement", "Rooms", b2 =>
                                {
                                    b2.Property<int>("AccommodationRequirementAccommodationOrderEntityId")
                                        .HasColumnType("int");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int")
                                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                    b2.HasKey("AccommodationRequirementAccommodationOrderEntityId", "Id");

                                    b2.ToTable("RoomRequirement");

                                    b2.WithOwner()
                                        .HasForeignKey("AccommodationRequirementAccommodationOrderEntityId");
                                });

                            b1.Navigation("Rooms");
                        });

                    b.Navigation("Requirment");
                });
#pragma warning restore 612, 618
        }
    }
}
