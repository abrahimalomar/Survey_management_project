using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyProject.Migrations
{
    public partial class addQutionIdInClassUserQutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuestionsId",
                table: "userQuestionnaire",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QutionId",
                table: "userQuestionnaire",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_userQuestionnaire_QuestionsId",
                table: "userQuestionnaire",
                column: "QuestionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_userQuestionnaire_Questions_QuestionsId",
                table: "userQuestionnaire",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userQuestionnaire_Questions_QuestionsId",
                table: "userQuestionnaire");

            migrationBuilder.DropIndex(
                name: "IX_userQuestionnaire_QuestionsId",
                table: "userQuestionnaire");

            migrationBuilder.DropColumn(
                name: "QuestionsId",
                table: "userQuestionnaire");

            migrationBuilder.DropColumn(
                name: "QutionId",
                table: "userQuestionnaire");
        }
    }
}
