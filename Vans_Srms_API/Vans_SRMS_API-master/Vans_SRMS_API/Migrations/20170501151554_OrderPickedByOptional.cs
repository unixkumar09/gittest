using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class OrderPickedByOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Devices_PickedBy",
                schema: "SRMS",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "PickedBy",
                schema: "SRMS",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Devices_PickedBy",
                schema: "SRMS",
                table: "Orders",
                column: "PickedBy",
                principalSchema: "SRMS",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Devices_PickedBy",
                schema: "SRMS",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "PickedBy",
                schema: "SRMS",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Devices_PickedBy",
                schema: "SRMS",
                table: "Orders",
                column: "PickedBy",
                principalSchema: "SRMS",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
