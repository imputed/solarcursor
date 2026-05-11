using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveTiltMeasuringUnitToSolarPanel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TiltMeasuringUnits_InstallationSites_InstallationSiteId",
                table: "TiltMeasuringUnits");

            migrationBuilder.RenameColumn(
                name: "InstallationSiteId",
                table: "TiltMeasuringUnits",
                newName: "SolarPanelId");

            migrationBuilder.RenameIndex(
                name: "IX_TiltMeasuringUnits_InstallationSiteId",
                table: "TiltMeasuringUnits",
                newName: "IX_TiltMeasuringUnits_SolarPanelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TiltMeasuringUnits_SolarPanels_SolarPanelId",
                table: "TiltMeasuringUnits",
                column: "SolarPanelId",
                principalTable: "SolarPanels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TiltMeasuringUnits_SolarPanels_SolarPanelId",
                table: "TiltMeasuringUnits");

            migrationBuilder.RenameColumn(
                name: "SolarPanelId",
                table: "TiltMeasuringUnits",
                newName: "InstallationSiteId");

            migrationBuilder.RenameIndex(
                name: "IX_TiltMeasuringUnits_SolarPanelId",
                table: "TiltMeasuringUnits",
                newName: "IX_TiltMeasuringUnits_InstallationSiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_TiltMeasuringUnits_InstallationSites_InstallationSiteId",
                table: "TiltMeasuringUnits",
                column: "InstallationSiteId",
                principalTable: "InstallationSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
