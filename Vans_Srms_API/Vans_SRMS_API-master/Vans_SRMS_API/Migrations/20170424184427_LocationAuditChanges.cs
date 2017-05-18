using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class LocationAuditChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                schema: "SRMS",
                table: "LocationAuditItems");

            migrationBuilder.AddColumn<int>(
                name: "OnRecordQuantity",
                schema: "SRMS",
                table: "LocationAuditItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScannedQuantity",
                schema: "SRMS",
                table: "LocationAuditItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnRecordQuantity",
                schema: "SRMS",
                table: "LocationAuditItems");

            migrationBuilder.DropColumn(
                name: "ScannedQuantity",
                schema: "SRMS",
                table: "LocationAuditItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                schema: "SRMS",
                table: "LocationAuditItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
