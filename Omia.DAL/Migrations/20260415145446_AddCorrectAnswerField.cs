using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omia.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrectAnswerField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "QuizQuestions");
        }
    }
}
