using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class OrderedByDeviceForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderedBy",
                schema: "SRMS",
                table: "Orders",
                column: "OrderedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Devices_OrderedBy",
                schema: "SRMS",
                table: "Orders",
                column: "OrderedBy",
                principalSchema: "SRMS",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Devices_OrderedBy",
                schema: "SRMS",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderedBy",
                schema: "SRMS",
                table: "Orders");
        }
    }
}
