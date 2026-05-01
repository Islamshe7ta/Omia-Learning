using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omia.DAL.Migrations
{
    /// <inheritdoc />
    public partial class disc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Receivers",
                table: "CourseDiscussions",
                newName: "Receiver");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Receiver",
                table: "CourseDiscussions",
                newName: "Receivers");
        }
    }
}
