﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Hero.Server.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hero.Server.DataAccess.Migrations
{
    [DbContext(typeof(HeroDbContext))]
    [Migration("20220917202214_RemoveTheAbiliesFromTheCharacter")]
    partial class RemoveTheAbiliesFromTheCharacter
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Hero")
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Hero.Server.Core.Models.Ability", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsPassive")
                        .HasColumnType("boolean");

                    b.HasKey("Name");

                    b.ToTable("Abilities", "Hero");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.Character", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Dodge")
                        .HasColumnType("double precision");

                    b.Property<int>("HealthPoints")
                        .HasColumnType("integer");

                    b.Property<int>("LightPoints")
                        .HasColumnType("integer");

                    b.Property<double>("MovementSpeed")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<double>("OpticalRange")
                        .HasColumnType("double precision");

                    b.Property<double>("Parry")
                        .HasColumnType("double precision");

                    b.Property<double>("Resistance")
                        .HasColumnType("double precision");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Characters", "Hero");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.Node", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Cost")
                        .HasColumnType("integer");

                    b.Property<int>("Importance")
                        .HasColumnType("integer");

                    b.Property<bool>("IsEasyReachable")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUnlocked")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("NodeTreeId")
                        .HasColumnType("uuid");

                    b.Property<List<Guid>>("Precessors")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<Guid?>("SkillId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<List<Guid>>("Successors")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<double>("XPos")
                        .HasColumnType("double precision");

                    b.Property<double>("YPos")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("NodeTreeId");

                    b.HasIndex("SkillId");

                    b.ToTable("Nodes", "Hero");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.NodeTree", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CharacterId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActiveTree")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.ToTable("NodeTrees", "Hero");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.Skill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AbilityName")
                        .IsRequired()
                        .HasColumnType("character varying(100)");

                    b.Property<double>("DamageBoost")
                        .HasColumnType("double precision");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("DodgeBoost")
                        .HasColumnType("double precision");

                    b.Property<int>("HealthPointsBoost")
                        .HasColumnType("integer");

                    b.Property<string>("IconUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("LightDamageBoost")
                        .HasColumnType("double precision");

                    b.Property<int>("LightPointsBoost")
                        .HasColumnType("integer");

                    b.Property<double>("MeleeDamageBoost")
                        .HasColumnType("double precision");

                    b.Property<double>("MovementSpeedBoost")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("OpticalRangeBoost")
                        .HasColumnType("double precision");

                    b.Property<double>("ParryBoost")
                        .HasColumnType("double precision");

                    b.Property<double>("RangeDamageBoost")
                        .HasColumnType("double precision");

                    b.Property<double>("ResistanceBoost")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("AbilityName");

                    b.ToTable("Skills", "Hero");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users", "Hero");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.Character", b =>
                {
                    b.HasOne("Hero.Server.Core.Models.User", "User")
                        .WithMany("Characters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.Node", b =>
                {
                    b.HasOne("Hero.Server.Core.Models.NodeTree", "NodeTree")
                        .WithMany("AllNodes")
                        .HasForeignKey("NodeTreeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Hero.Server.Core.Models.Skill", "Skill")
                        .WithMany("Nodes")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("NodeTree");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.NodeTree", b =>
                {
                    b.HasOne("Hero.Server.Core.Models.Character", "Character")
                        .WithMany("NodeTrees")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Character");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.Skill", b =>
                {
                    b.HasOne("Hero.Server.Core.Models.Ability", "Ability")
                        .WithMany("Skills")
                        .HasForeignKey("AbilityName")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Ability");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.Ability", b =>
                {
                    b.Navigation("Skills");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.Character", b =>
                {
                    b.Navigation("NodeTrees");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.NodeTree", b =>
                {
                    b.Navigation("AllNodes");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.Skill", b =>
                {
                    b.Navigation("Nodes");
                });

            modelBuilder.Entity("Hero.Server.Core.Models.User", b =>
                {
                    b.Navigation("Characters");
                });
#pragma warning restore 612, 618
        }
    }
}
