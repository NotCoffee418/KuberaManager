﻿// <auto-generated />
using System;
using KuberaManager.Models;
using KuberaManager.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace KuberaManager.Migrations
{
    [DbContext(typeof(kuberaDbContext))]
    partial class kuberaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("KuberaManager.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PrefStartTimeDay")
                        .HasColumnType("integer");

                    b.Property<int>("PrefStopTimeDay")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("KuberaManager.Models.Computer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("DisableModelRendering")
                        .HasColumnType("boolean");

                    b.Property<bool>("DisableSceneRendering")
                        .HasColumnType("boolean");

                    b.Property<string>("Hostname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LowCpuMode")
                        .HasColumnType("boolean");

                    b.Property<int>("MaxClients")
                        .HasColumnType("integer");

                    b.Property<bool>("SuperLowCpuMode")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Computers");
                });

            modelBuilder.Entity("KuberaManager.Models.Config", b =>
                {
                    b.Property<string>("ConfKey")
                        .HasColumnType("text");

                    b.Property<string>("ConfValue")
                        .HasColumnType("text");

                    b.HasKey("ConfKey");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("KuberaManager.Models.Scenario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AccountId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("Id");

                    b.ToTable("Scenarios");
                });

            modelBuilder.Entity("KuberaManager.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<int>("ActiveComputer")
                        .HasColumnType("integer");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("RspeerSessionTag")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<TimeSpan>("TargetDuration")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("KuberaManager.Models.Scenario", b =>
                {
                    b.HasOne("KuberaManager.Models.Account", null)
                        .WithMany("PreferredActivities")
                        .HasForeignKey("AccountId");
                });
#pragma warning restore 612, 618
        }
    }
}
