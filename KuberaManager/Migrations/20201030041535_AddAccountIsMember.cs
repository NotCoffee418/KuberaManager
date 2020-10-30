using Microsoft.EntityFrameworkCore.Migrations;

namespace KuberaManager.Migrations
{
    public partial class AddAccountIsMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scenarios_Accounts_AccountId",
                table: "Scenarios");

            migrationBuilder.DropIndex(
                name: "IX_Scenarios_AccountId",
                table: "Scenarios");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Scenarios");

            migrationBuilder.AddColumn<bool>(
                name: "IsMember",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMember",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Scenarios",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_AccountId",
                table: "Scenarios",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scenarios_Accounts_AccountId",
                table: "Scenarios",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
