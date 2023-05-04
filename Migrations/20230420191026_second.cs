using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCompanion.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Acity",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Aemail",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Aname",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Aphone",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Apin",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pcity",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pemail",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pname",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pphone",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ppin",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acity",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Aemail",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Aname",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Aphone",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Apin",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Pcity",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Pemail",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Pname",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Pphone",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Ppin",
                table: "Jobs");
        }
    }
}
