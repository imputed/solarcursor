using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRaspberryPiControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "InstallationSites",
                type: "TEXT",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "InstallationSites",
                type: "TEXT",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MoveDownGpioPin",
                table: "LinearMotors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MoveUpGpioPin",
                table: "LinearMotors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "InstallationSites");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "InstallationSites");

            migrationBuilder.DropColumn(
                name: "MoveDownGpioPin",
                table: "LinearMotors");

            migrationBuilder.DropColumn(
                name: "MoveUpGpioPin",
                table: "LinearMotors");
        }
    }
}
