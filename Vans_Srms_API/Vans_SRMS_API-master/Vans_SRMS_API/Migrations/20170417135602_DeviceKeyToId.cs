using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class DeviceKeyToId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DeviceKey",
                schema: "SRMS",
                table: "Putaways");

            migrationBuilder.DropColumn(
                name: "DeviceKey",
                schema: "SRMS",
                table: "Picks");

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                schema: "SRMS",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                schema: "SRMS",
                table: "Putaways",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                schema: "SRMS",
                table: "Picks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DeviceId",
                schema: "SRMS",
                table: "Requests",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Putaways_DeviceId",
                schema: "SRMS",
                table: "Putaways",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_DeviceId",
                schema: "SRMS",
                table: "Picks",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Picks_Devices_DeviceId",
                schema: "SRMS",
                table: "Picks",
                column: "DeviceId",
                principalSchema: "SRMS",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Putaways_Devices_DeviceId",
                schema: "SRMS",
                table: "Putaways",
                column: "DeviceId",
                principalSchema: "SRMS",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Devices_DeviceId",
                schema: "SRMS",
                table: "Requests",
                column: "DeviceId",
                principalSchema: "SRMS",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picks_Devices_DeviceId",
                schema: "SRMS",
                table: "Picks");

            migrationBuilder.DropForeignKey(
                name: "FK_Putaways_Devices_DeviceId",
                schema: "SRMS",
                table: "Putaways");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Devices_DeviceId",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_DeviceId",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Putaways_DeviceId",
                schema: "SRMS",
                table: "Putaways");

            migrationBuilder.DropIndex(
                name: "IX_Picks_DeviceId",
                schema: "SRMS",
                table: "Picks");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                schema: "SRMS",
                table: "Putaways");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                schema: "SRMS",
                table: "Picks");

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                schema: "SRMS",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceKey",
                schema: "SRMS",
                table: "Putaways",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceKey",
                schema: "SRMS",
                table: "Picks",
                nullable: false,
                defaultValue: "");
        }
    }
}
