using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omia.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Courses");
        }
    }
}
