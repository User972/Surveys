using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Comp.Survey.Infrastructure.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompUserSurveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SubmissionTitle = table.Column<string>(nullable: true),
                    CompUserId = table.Column<Guid>(nullable: false),
                    SurveyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompUserSurveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompUserSurveys_CompUsers_CompUserId",
                        column: x => x.CompUserId,
                        principalTable: "CompUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompUserSurveys_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    QuestionType = table.Column<int>(nullable: false),
                    SurveyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyQuestions_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    SurveyQuestionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_SurveyQuestions_SurveyQuestionId",
                        column: x => x.SurveyQuestionId,
                        principalTable: "SurveyQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompUserSurveyDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompUserSurvey = table.Column<string>(nullable: true),
                    CompUserSurveyId = table.Column<Guid>(nullable: false),
                    SurveyQuestionId = table.Column<Guid>(nullable: false),
                    SelectedOptionId = table.Column<Guid>(nullable: false),
                    SelectedOptionValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompUserSurveyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompUserSurveyDetails_CompUserSurveys_CompUserSurveyId",
                        column: x => x.CompUserSurveyId,
                        principalTable: "CompUserSurveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompUserSurveyDetails_QuestionOptions_SelectedOptionId",
                        column: x => x.SelectedOptionId,
                        principalTable: "QuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompUserSurveyDetails_SurveyQuestions_SurveyQuestionId",
                        column: x => x.SurveyQuestionId,
                        principalTable: "SurveyQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompUserSurveyDetails_CompUserSurveyId",
                table: "CompUserSurveyDetails",
                column: "CompUserSurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompUserSurveyDetails_SelectedOptionId",
                table: "CompUserSurveyDetails",
                column: "SelectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompUserSurveyDetails_SurveyQuestionId",
                table: "CompUserSurveyDetails",
                column: "SurveyQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompUserSurveys_CompUserId",
                table: "CompUserSurveys",
                column: "CompUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompUserSurveys_SurveyId",
                table: "CompUserSurveys",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_SurveyQuestionId",
                table: "QuestionOptions",
                column: "SurveyQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_SurveyId",
                table: "SurveyQuestions",
                column: "SurveyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompUserSurveyDetails");

            migrationBuilder.DropTable(
                name: "TestUsers");

            migrationBuilder.DropTable(
                name: "CompUserSurveys");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "CompUsers");

            migrationBuilder.DropTable(
                name: "SurveyQuestions");

            migrationBuilder.DropTable(
                name: "Surveys");
        }
    }
}
