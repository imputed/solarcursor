using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSolarTrackingConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolarTrackingConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SolarPanelId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionThresholdDegrees = table.Column<double>(type: "REAL", nullable: false),
                    StepDurationMs = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxAdjustmentSteps = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarTrackingConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolarTrackingConfigurations_SolarPanels_SolarPanelId",
                        column: x => x.SolarPanelId,
                        principalTable: "SolarPanels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolarTrackingConfigurations_SolarPanelId",
                table: "SolarTrackingConfigurations",
                column: "SolarPanelId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolarTrackingConfigurations");
        }
    }
}
