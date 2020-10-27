using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace KuberaManager.Migrations
{
    public partial class AddLevelsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Magic = table.Column<int>(nullable: false),
                    Slayer = table.Column<int>(nullable: false),
                    Strength = table.Column<int>(nullable: false),
                    Defence = table.Column<int>(nullable: false),
                    Fletching = table.Column<int>(nullable: false),
                    Fishing = table.Column<int>(nullable: false),
                    Mining = table.Column<int>(nullable: false),
                    Herblore = table.Column<int>(nullable: false),
                    Hitpoints = table.Column<int>(nullable: false),
                    Smithing = table.Column<int>(nullable: false),
                    Woodcutting = table.Column<int>(nullable: false),
                    Prayer = table.Column<int>(nullable: false),
                    Ranged = table.Column<int>(nullable: false),
                    Attack = table.Column<int>(nullable: false),
                    Crafting = table.Column<int>(nullable: false),
                    Farming = table.Column<int>(nullable: false),
                    Firemaking = table.Column<int>(nullable: false),
                    Runecrafting = table.Column<int>(nullable: false),
                    Construction = table.Column<int>(nullable: false),
                    Cooking = table.Column<int>(nullable: false),
                    Agility = table.Column<int>(nullable: false),
                    Hunter = table.Column<int>(nullable: false),
                    Thieving = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.AccountId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Levels");
        }
    }
}
