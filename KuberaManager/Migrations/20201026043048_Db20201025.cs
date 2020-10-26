using Microsoft.EntityFrameworkCore.Migrations;

namespace KuberaManager.Migrations
{
    public partial class Db20201025 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Computers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Accounts");
        }
    }
}
