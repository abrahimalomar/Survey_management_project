using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyProject.Migrations
{
    public partial class addPropriesInClassUseQutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "answer",
                table: "userQuestionnaire",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "answer",
                table: "userQuestionnaire");
        }
    }
}
