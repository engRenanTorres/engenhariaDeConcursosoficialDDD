﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence.Data;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextEFModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Choice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BaseQuestionId")
                        .HasColumnType("integer");

                    b.Property<char>("Letter")
                        .HasColumnType("character(1)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BaseQuestionId");

                    b.ToTable("Choices");
                });

            modelBuilder.Entity("Domain.Entities.Inharitance.BaseQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<char>("Answer")
                        .HasColumnType("character(1)");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("Created_at");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("character varying(34)");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("Last_updated_at");

                    b.Property<string>("Tip")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Questions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseQuestion");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Entities.BooleanQuestion", b =>
                {
                    b.HasBaseType("Domain.Entities.Inharitance.BaseQuestion");

                    b.HasDiscriminator().HasValue("BooleanQuestion");
                });

            modelBuilder.Entity("Domain.Entities.MultipleChoicesQuestion", b =>
                {
                    b.HasBaseType("Domain.Entities.Inharitance.BaseQuestion");

                    b.Property<int>("ChoiceAId")
                        .HasColumnType("integer");

                    b.Property<int>("ChoiceBId")
                        .HasColumnType("integer");

                    b.Property<int?>("ChoiceCId")
                        .HasColumnType("integer");

                    b.Property<int?>("ChoiceDId")
                        .HasColumnType("integer");

                    b.HasIndex("ChoiceAId");

                    b.HasIndex("ChoiceBId");

                    b.HasIndex("ChoiceCId");

                    b.HasIndex("ChoiceDId");

                    b.HasDiscriminator().HasValue("MultipleChoicesQuestion");
                });

            modelBuilder.Entity("Domain.Entities.Choice", b =>
                {
                    b.HasOne("Domain.Entities.Inharitance.BaseQuestion", null)
                        .WithMany("Choices")
                        .HasForeignKey("BaseQuestionId");
                });

            modelBuilder.Entity("Domain.Entities.MultipleChoicesQuestion", b =>
                {
                    b.HasOne("Domain.Entities.Choice", "ChoiceA")
                        .WithMany()
                        .HasForeignKey("ChoiceAId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Choice", "ChoiceB")
                        .WithMany()
                        .HasForeignKey("ChoiceBId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Choice", "ChoiceC")
                        .WithMany()
                        .HasForeignKey("ChoiceCId");

                    b.HasOne("Domain.Entities.Choice", "ChoiceD")
                        .WithMany()
                        .HasForeignKey("ChoiceDId");

                    b.Navigation("ChoiceA");

                    b.Navigation("ChoiceB");

                    b.Navigation("ChoiceC");

                    b.Navigation("ChoiceD");
                });

            modelBuilder.Entity("Domain.Entities.Inharitance.BaseQuestion", b =>
                {
                    b.Navigation("Choices");
                });
#pragma warning restore 612, 618
        }
    }
}
