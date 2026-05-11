using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSolarPanelOptimizationAutomation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolarOptimizationScheduleConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IntervalMinutes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarOptimizationScheduleConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolarPanelOptimizationStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SolarPanelId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarPanelOptimizationStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolarPanelOptimizationStates_SolarPanels_SolarPanelId",
                        column: x => x.SolarPanelId,
                        principalTable: "SolarPanels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolarPanelOptimizationStates_SolarPanelId",
                table: "SolarPanelOptimizationStates",
                column: "SolarPanelId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolarOptimizationScheduleConfigurations");

            migrationBuilder.DropTable(
                name: "SolarPanelOptimizationStates");
        }
    }
}
