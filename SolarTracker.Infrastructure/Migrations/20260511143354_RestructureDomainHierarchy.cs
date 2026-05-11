using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RestructureDomainHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinearMotors_InstallationSites_InstallationSiteId",
                table: "LinearMotors");

            migrationBuilder.RenameColumn(
                name: "InstallationSiteId",
                table: "LinearMotors",
                newName: "SolarPanelId");

            migrationBuilder.RenameIndex(
                name: "IX_LinearMotors_InstallationSiteId",
                table: "LinearMotors",
                newName: "IX_LinearMotors_SolarPanelId");

            migrationBuilder.CreateTable(
                name: "CurrentMeasuringUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SolarPanelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    GpioPin = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentMeasuringUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentMeasuringUnits_SolarPanels_SolarPanelId",
                        column: x => x.SolarPanelId,
                        principalTable: "SolarPanels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TiltMeasuringUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InstallationSiteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    GpioPin = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiltMeasuringUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TiltMeasuringUnits_InstallationSites_InstallationSiteId",
                        column: x => x.InstallationSiteId,
                        principalTable: "InstallationSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentMeasuringUnits_SolarPanelId",
                table: "CurrentMeasuringUnits",
                column: "SolarPanelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiltMeasuringUnits_InstallationSiteId",
                table: "TiltMeasuringUnits",
                column: "InstallationSiteId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LinearMotors_SolarPanels_SolarPanelId",
                table: "LinearMotors",
                column: "SolarPanelId",
                principalTable: "SolarPanels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinearMotors_SolarPanels_SolarPanelId",
                table: "LinearMotors");

            migrationBuilder.DropTable(
                name: "CurrentMeasuringUnits");

            migrationBuilder.DropTable(
                name: "TiltMeasuringUnits");

            migrationBuilder.RenameColumn(
                name: "SolarPanelId",
                table: "LinearMotors",
                newName: "InstallationSiteId");

            migrationBuilder.RenameIndex(
                name: "IX_LinearMotors_SolarPanelId",
                table: "LinearMotors",
                newName: "IX_LinearMotors_InstallationSiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinearMotors_InstallationSites_InstallationSiteId",
                table: "LinearMotors",
                column: "InstallationSiteId",
                principalTable: "InstallationSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
