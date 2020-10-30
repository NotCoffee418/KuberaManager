﻿// <auto-generated />
using System;
using KuberaManager.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace KuberaManager.Migrations
{
    [DbContext(typeof(kuberaDbContext))]
    [Migration("20201030041535_AddAccountIsMember")]
    partial class AddAccountIsMember
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("KuberaManager.Models.Database.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsBanned")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMember")
                        .HasColumnType("boolean");

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

            modelBuilder.Entity("KuberaManager.Models.Database.AccountCompletionData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<int>("Definition")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("AccountCompletionData");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Computer", b =>
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

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("LowCpuMode")
                        .HasColumnType("boolean");

                    b.Property<int>("MaxClients")
                        .HasColumnType("integer");

                    b.Property<bool>("SuperLowCpuMode")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Computers");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Config", b =>
                {
                    b.Property<string>("ConfKey")
                        .HasColumnType("text");

                    b.Property<string>("ConfValue")
                        .HasColumnType("text");

                    b.HasKey("ConfKey");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ActiveScenarioId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean");

                    b.Property<int>("SessionId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<TimeSpan>("TargetDuration")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Levels", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AccountId")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Agility")
                        .HasColumnName("Agility")
                        .HasColumnType("integer");

                    b.Property<int>("Attack")
                        .HasColumnName("Attack")
                        .HasColumnType("integer");

                    b.Property<int>("Construction")
                        .HasColumnName("Construction")
                        .HasColumnType("integer");

                    b.Property<int>("Cooking")
                        .HasColumnName("Cooking")
                        .HasColumnType("integer");

                    b.Property<int>("Crafting")
                        .HasColumnName("Crafting")
                        .HasColumnType("integer");

                    b.Property<int>("Defence")
                        .HasColumnName("Defence")
                        .HasColumnType("integer");

                    b.Property<int>("Farming")
                        .HasColumnName("Farming")
                        .HasColumnType("integer");

                    b.Property<int>("Firemaking")
                        .HasColumnName("Firemaking")
                        .HasColumnType("integer");

                    b.Property<int>("Fishing")
                        .HasColumnName("Fishing")
                        .HasColumnType("integer");

                    b.Property<int>("Fletching")
                        .HasColumnName("Fletching")
                        .HasColumnType("integer");

                    b.Property<int>("Herblore")
                        .HasColumnName("Herblore")
                        .HasColumnType("integer");

                    b.Property<int>("Hitpoints")
                        .HasColumnName("Hitpoints")
                        .HasColumnType("integer");

                    b.Property<int>("Hunter")
                        .HasColumnName("Hunter")
                        .HasColumnType("integer");

                    b.Property<int>("Magic")
                        .HasColumnName("Magic")
                        .HasColumnType("integer");

                    b.Property<int>("Mining")
                        .HasColumnName("Mining")
                        .HasColumnType("integer");

                    b.Property<int>("Prayer")
                        .HasColumnName("Prayer")
                        .HasColumnType("integer");

                    b.Property<int>("Ranged")
                        .HasColumnName("Ranged")
                        .HasColumnType("integer");

                    b.Property<int>("Runecrafting")
                        .HasColumnName("Runecrafting")
                        .HasColumnType("integer");

                    b.Property<int>("Slayer")
                        .HasColumnName("Slayer")
                        .HasColumnType("integer");

                    b.Property<int>("Smithing")
                        .HasColumnName("Smithing")
                        .HasColumnType("integer");

                    b.Property<int>("Strength")
                        .HasColumnName("Strength")
                        .HasColumnType("integer");

                    b.Property<int>("Thieving")
                        .HasColumnName("Thieving")
                        .HasColumnType("integer");

                    b.Property<int>("Woodcutting")
                        .HasColumnName("Woodcutting")
                        .HasColumnType("integer");

                    b.HasKey("AccountId");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Scenario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Scenarios");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Session", b =>
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
#pragma warning restore 612, 618
        }
    }
}
