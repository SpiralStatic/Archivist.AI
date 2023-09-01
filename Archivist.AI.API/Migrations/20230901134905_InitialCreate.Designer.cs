﻿// <auto-generated />
using System;
using Archivist.AI.Core.Repository.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Archivist.AI.API.Migrations
{
    [DbContext(typeof(LibraryContext))]
    [Migration("20230901134905_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("Archivist.AI.Core.Repository.Archive", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Inserted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Archive");
                });

            modelBuilder.Entity("Archivist.AI.Core.Repository.Owner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Inserted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnershipProperties")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Owner");
                });

            modelBuilder.Entity("Archivist.AI.Core.Repository.Record", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ArchiveId")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmbeddingValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Inserted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("WorldDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ArchiveId");

                    b.ToTable("Record");
                });

            modelBuilder.Entity("Archivist.AI.Core.Repository.Archive", b =>
                {
                    b.HasOne("Archivist.AI.Core.Repository.Owner", null)
                        .WithMany("Archives")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Archivist.AI.Core.Repository.Record", b =>
                {
                    b.HasOne("Archivist.AI.Core.Repository.Archive", null)
                        .WithMany("Records")
                        .HasForeignKey("ArchiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Archivist.AI.Core.Repository.Archive", b =>
                {
                    b.Navigation("Records");
                });

            modelBuilder.Entity("Archivist.AI.Core.Repository.Owner", b =>
                {
                    b.Navigation("Archives");
                });
#pragma warning restore 612, 618
        }
    }
}
