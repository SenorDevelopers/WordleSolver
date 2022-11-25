﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(UoW))]
    [Migration("20221118133302_PreviousId")]
    partial class PreviousId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Database.Entities.Guess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("GuessNumber")
                        .HasColumnType("int");

                    b.Property<string>("GuessString")
                        .HasColumnType("NVARCHAR(7)");

                    b.Property<string>("Pattern")
                        .HasColumnType("NVARCHAR(7)");

                    b.Property<int?>("PreviousGuessId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Guesses");
                });

            modelBuilder.Entity("Database.Entities.SecondGuess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Pattern")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(7)");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(7)");

                    b.HasKey("Id");

                    b.ToTable("SecondGuesses");
                });

            modelBuilder.Entity("Database.Entities.Word", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Entropy")
                        .HasColumnType("decimal(14,8)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(7)");

                    b.HasKey("Id");

                    b.ToTable("Words");
                });
#pragma warning restore 612, 618
        }
    }
}