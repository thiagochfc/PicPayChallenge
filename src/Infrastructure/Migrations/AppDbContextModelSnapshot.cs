﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PicPayChallenge.Infrastructure;

#nullable disable

namespace PicPayChallenge.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PicPayChallenge.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid ")
                        .HasColumnName("id");

                    b.Property<decimal>("Balance")
                        .HasColumnType("NUMERIC(13, 2)")
                        .HasColumnName("balance");

                    b.Property<string>("CpfCnpj")
                        .IsRequired()
                        .HasColumnType("VARCHAR(14)")
                        .HasColumnName("cpfCnpj");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(150)")
                        .HasColumnName("email");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(150)")
                        .HasColumnName("fullName");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(150)")
                        .HasColumnName("password");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("VARCHAR(11)")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.ToTable("accounts", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
