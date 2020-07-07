﻿// <auto-generated />
using System;
using Comp.Survey.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Comp.Survey.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDataContext))]
    [Migration("20200604020154_InitialSetup")]
    partial class InitialSetup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4");

            modelBuilder.Entity("Comp.Survey.Core.Entities.CompUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CompUsers");
                });

            modelBuilder.Entity("Comp.Survey.Core.Entities.CompUserSurvey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CompUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SubmissionTitle")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SurveyId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompUserId");

                    b.HasIndex("SurveyId");

                    b.ToTable("CompUserSurveys");
                });

            modelBuilder.Entity("Comp.Survey.Core.Entities.CompUserSurveyDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CompUserSurvey")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CompUserSurveyId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SelectedOptionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SelectedOptionValue")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SurveyQuestionId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompUserSurveyId");

                    b.HasIndex("SelectedOptionId");

                    b.HasIndex("SurveyQuestionId");

                    b.ToTable("CompUserSurveyDetails");
                });

            modelBuilder.Entity("Comp.Survey.Core.Entities.QuestionOption", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SurveyQuestionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SurveyQuestionId");

                    b.ToTable("QuestionOptions");
                });

            modelBuilder.Entity("Comp.Survey.Core.Entities.Survey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("Comp.Survey.Core.Entities.SurveyQuestion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("QuestionType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SubTitle")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SurveyId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SurveyId");

                    b.ToTable("SurveyQuestions");
                });

            modelBuilder.Entity("Comp.Survey.Core.Entities.TestUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TestUsers");
                });

            modelBuilder.Entity("Comp.Survey.Core.Entities.CompUserSurvey", b =>
                {
                    b.HasOne("Comp.Survey.Core.Entities.CompUser", null)
                        .WithMany("CompUserSurveys")
                        .HasForeignKey("CompUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Comp.Survey.Core.Entities.Survey", null)
                        .WithMany("CompUserSurveys")
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Comp.Survey.Core.Entities.CompUserSurveyDetail", b =>
                {
                    b.HasOne("Comp.Survey.Core.Entities.CompUserSurvey", null)
                        .WithMany("CompUserSurveyDetails")
                        .HasForeignKey("CompUserSurveyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Comp.Survey.Core.Entities.QuestionOption", null)
                        .WithMany("CompUserSurveyDetails")
                        .HasForeignKey("SelectedOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Comp.Survey.Core.Entities.SurveyQuestion", null)
                        .WithMany("CompUserSurveyDetails")
                        .HasForeignKey("SurveyQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Comp.Survey.Core.Entities.QuestionOption", b =>
                {
                    b.HasOne("Comp.Survey.Core.Entities.SurveyQuestion", null)
                        .WithMany("QuestionOptions")
                        .HasForeignKey("SurveyQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Comp.Survey.Core.Entities.SurveyQuestion", b =>
                {
                    b.HasOne("Comp.Survey.Core.Entities.Survey", null)
                        .WithMany("SurveyQuestions")
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}