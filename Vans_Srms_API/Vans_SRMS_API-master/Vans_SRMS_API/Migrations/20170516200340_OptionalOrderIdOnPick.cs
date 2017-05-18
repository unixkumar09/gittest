using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class OptionalOrderIdOnPick : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picks_Orders_OrderId",
                schema: "SRMS",
                table: "Picks");

            migrationBuilder.AlterColumn<long>(
                name: "OrderId",
                schema: "SRMS",
                table: "Picks",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Picks_Orders_OrderId",
                schema: "SRMS",
                table: "Picks",
                column: "OrderId",
                principalSchema: "SRMS",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picks_Orders_OrderId",
                schema: "SRMS",
                table: "Picks");

            migrationBuilder.AlterColumn<long>(
                name: "OrderId",
                schema: "SRMS",
                table: "Picks",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Picks_Orders_OrderId",
                schema: "SRMS",
                table: "Picks",
                column: "OrderId",
                principalSchema: "SRMS",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
