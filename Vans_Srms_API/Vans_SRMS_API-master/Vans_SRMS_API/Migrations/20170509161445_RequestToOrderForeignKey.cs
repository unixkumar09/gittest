using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class RequestToOrderForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RequestId",
                schema: "SRMS",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RequestId",
                schema: "SRMS",
                table: "Orders",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Requests_RequestId",
                schema: "SRMS",
                table: "Orders",
                column: "RequestId",
                principalSchema: "SRMS",
                principalTable: "Requests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Requests_RequestId",
                schema: "SRMS",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RequestId",
                schema: "SRMS",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RequestId",
                schema: "SRMS",
                table: "Orders");
        }
    }
}
