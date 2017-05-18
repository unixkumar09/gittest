using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class OrderFullfillPick : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PickedProductId",
                schema: "SRMS",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PickedProductId",
                schema: "SRMS",
                table: "Orders",
                column: "PickedProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_PickedProductId",
                schema: "SRMS",
                table: "Orders",
                column: "PickedProductId",
                principalSchema: "SRMS",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_PickedProductId",
                schema: "SRMS",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PickedProductId",
                schema: "SRMS",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PickedProductId",
                schema: "SRMS",
                table: "Orders");
        }
    }
}
