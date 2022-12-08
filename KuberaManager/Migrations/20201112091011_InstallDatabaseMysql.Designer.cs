﻿// <auto-generated />
using System;
using KuberaManager.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KuberaManager.Migrations
{
    [DbContext(typeof(kuberaDbContext))]
    [Migration("20201112091011_InstallDatabaseMysql")]
    partial class InstallDatabaseMysql
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("KuberaManager.Models.Database.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ContinueScenario")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsMember")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("PrefStartTimeDay")
                        .HasColumnType("int");

                    b.Property<int>("PrefStopTimeDay")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.AccountCompletionData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("Definition")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AccountCompletionData");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Computer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("DisableModelRendering")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("DisableSceneRendering")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Hostname")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LowCpuMode")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MaxClients")
                        .HasColumnType("int");

                    b.Property<bool>("SuperLowCpuMode")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Computers");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Config", b =>
                {
                    b.Property<string>("ConfKey")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ConfValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ConfKey");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.EventLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("TextId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("EventLogs");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.EventLogText", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("EventLogTexts");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("ForceRunUntilComplete")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ScenarioIdentifier")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeSpan>("TargetDuration")
                        .HasColumnType("time(6)");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Levels", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AccountId");

                    b.Property<int>("Agility")
                        .HasColumnType("int")
                        .HasColumnName("Agility");

                    b.Property<int>("Attack")
                        .HasColumnType("int")
                        .HasColumnName("Attack");

                    b.Property<int>("Construction")
                        .HasColumnType("int")
                        .HasColumnName("Construction");

                    b.Property<int>("Cooking")
                        .HasColumnType("int")
                        .HasColumnName("Cooking");

                    b.Property<int>("Crafting")
                        .HasColumnType("int")
                        .HasColumnName("Crafting");

                    b.Property<int>("Defence")
                        .HasColumnType("int")
                        .HasColumnName("Defence");

                    b.Property<int>("Farming")
                        .HasColumnType("int")
                        .HasColumnName("Farming");

                    b.Property<int>("Firemaking")
                        .HasColumnType("int")
                        .HasColumnName("Firemaking");

                    b.Property<int>("Fishing")
                        .HasColumnType("int")
                        .HasColumnName("Fishing");

                    b.Property<int>("Fletching")
                        .HasColumnType("int")
                        .HasColumnName("Fletching");

                    b.Property<int>("Herblore")
                        .HasColumnType("int")
                        .HasColumnName("Herblore");

                    b.Property<int>("Hitpoints")
                        .HasColumnType("int")
                        .HasColumnName("Hitpoints");

                    b.Property<int>("Hunter")
                        .HasColumnType("int")
                        .HasColumnName("Hunter");

                    b.Property<int>("Magic")
                        .HasColumnType("int")
                        .HasColumnName("Magic");

                    b.Property<int>("Mining")
                        .HasColumnType("int")
                        .HasColumnName("Mining");

                    b.Property<int>("Prayer")
                        .HasColumnType("int")
                        .HasColumnName("Prayer");

                    b.Property<int>("Ranged")
                        .HasColumnType("int")
                        .HasColumnName("Ranged");

                    b.Property<int>("Runecrafting")
                        .HasColumnType("int")
                        .HasColumnName("Runecrafting");

                    b.Property<int>("Slayer")
                        .HasColumnType("int")
                        .HasColumnName("Slayer");

                    b.Property<int>("Smithing")
                        .HasColumnType("int")
                        .HasColumnName("Smithing");

                    b.Property<int>("Strength")
                        .HasColumnType("int")
                        .HasColumnName("Strength");

                    b.Property<int>("Thieving")
                        .HasColumnType("int")
                        .HasColumnName("Thieving");

                    b.Property<int>("Woodcutting")
                        .HasColumnType("int")
                        .HasColumnName("Woodcutting");

                    b.HasKey("AccountId");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("KuberaManager.Models.Database.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("ActiveComputer")
                        .HasColumnType("int");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RspeerSessionTag")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeSpan>("TargetDuration")
                        .HasColumnType("time(6)");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}