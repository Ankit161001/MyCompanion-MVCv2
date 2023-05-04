using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCompanion.Migrations
{
    /// <inheritdoc />
    public partial class otpmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Aotp",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Potp",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aotp",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Potp",
                table: "Jobs");
        }
    }
}
