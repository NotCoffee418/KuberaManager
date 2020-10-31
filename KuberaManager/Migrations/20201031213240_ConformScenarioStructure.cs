using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace KuberaManager.Migrations
{
    public partial class ConformScenarioStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scenarios");

            migrationBuilder.DropColumn(
                name: "ActiveScenarioId",
                table: "Jobs");

            migrationBuilder.AddColumn<bool>(
                name: "ForceRunUntilComplete",
                table: "Jobs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ScenarioIdentifier",
                table: "Jobs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContinueScenario",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForceRunUntilComplete",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ScenarioIdentifier",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ContinueScenario",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "ActiveScenarioId",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Scenarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_Id",
                table: "Scenarios",
                column: "Id");
        }
    }
}
