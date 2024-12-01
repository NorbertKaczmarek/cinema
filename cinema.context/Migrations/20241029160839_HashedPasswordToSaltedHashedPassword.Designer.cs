﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using cinema.context;

#nullable disable

namespace cinema.context.Migrations
{
    [DbContext(typeof(CinemaDbContext))]
    [Migration("20241029160839_HashedPasswordToSaltedHashedPassword")]
    partial class HashedPasswordToSaltedHashedPassword
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("OrderSeat", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SeatsId")
                        .HasColumnType("char(36)");

                    b.HasKey("OrderId", "SeatsId");

                    b.HasIndex("SeatsId");

                    b.ToTable("OrderSeat");
                });

            modelBuilder.Entity("cinema.context.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("cinema.context.Entities.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Cast")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time(6)");

                    b.Property<string>("PosterUrl")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<double>("Rating")
                        .HasColumnType("double");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("cinema.context.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<Guid?>("ScreeningId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ScreeningId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("cinema.context.Entities.Screening", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("EndDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("MovieId")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("StartDateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("Screenings");
                });

            modelBuilder.Entity("cinema.context.Entities.Seat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("Row")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.HasKey("Id");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("cinema.context.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SaltedHashedPassword")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OrderSeat", b =>
                {
                    b.HasOne("cinema.context.Entities.Order", null)
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cinema.context.Entities.Seat", null)
                        .WithMany()
                        .HasForeignKey("SeatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("cinema.context.Entities.Movie", b =>
                {
                    b.HasOne("cinema.context.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("cinema.context.Entities.Order", b =>
                {
                    b.HasOne("cinema.context.Entities.Screening", "Screening")
                        .WithMany()
                        .HasForeignKey("ScreeningId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Screening");
                });

            modelBuilder.Entity("cinema.context.Entities.Screening", b =>
                {
                    b.HasOne("cinema.context.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Movie");
                });
#pragma warning restore 612, 618
        }
    }
}
