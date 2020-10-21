using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KuberaManager.Migrations
{
    public partial class ConfigAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "RspeerSession",
                table: "Sessions");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Sessions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateTime",
                table: "Sessions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RspeerSessionTag",
                table: "Sessions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ConfKey = table.Column<string>(nullable: false),
                    ConfValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ConfKey);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_Id",
                table: "Scenarios",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropIndex(
                name: "IX_Scenarios_Id",
                table: "Scenarios");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "LastUpdateTime",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "RspeerSessionTag",
                table: "Sessions");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Sessions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RspeerSession",
                table: "Sessions",
                type: "text",
                nullable: true);
        }
    }
}
