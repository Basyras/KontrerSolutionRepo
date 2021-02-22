﻿// <auto-generated />
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.Migrations
{
    [DbContext(typeof(IdGeneratorServiceDbContext))]
    partial class IdGeneratorServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Data.EF.LastUsedIdEntity", b =>
                {
                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LastUsedId")
                        .HasColumnType("int");

                    b.HasKey("GroupName");

                    b.ToTable("LastUsedIds");
                });
#pragma warning restore 612, 618
        }
    }
}