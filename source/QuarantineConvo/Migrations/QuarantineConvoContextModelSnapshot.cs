﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuarantineConvo.Data;

namespace QuarantineConvo.Migrations
{
    [DbContext(typeof(QuarantineConvoContext))]
    partial class QuarantineConvoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("QuarantineConvo.Models.Connection", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<string>("user1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("user2")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Connection");
                });

            modelBuilder.Entity("QuarantineConvo.Models.Message", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ConnectionID")
                        .HasColumnType("int");

                    b.Property<string>("Msg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SentBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ConnectionID");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("QuarantineConvo.Models.Message", b =>
                {
                    b.HasOne("QuarantineConvo.Models.Connection", "Connection")
                        .WithMany()
                        .HasForeignKey("ConnectionID");
                });
#pragma warning restore 612, 618
        }
    }
}
