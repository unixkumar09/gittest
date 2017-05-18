using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Vans_SRMS_API.Migrations
{
    public partial class OrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picks_Requests_RequestId",
                schema: "SRMS",
                table: "Picks");

            migrationBuilder.DropIndex(
                name: "IX_Products_IsYouthSize",
                schema: "SRMS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Active",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "FilledAt",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Picked",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SizeConverted",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SizeType",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SuggestedLocationId",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsYouthSize",
                schema: "SRMS",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "VendorStyle",
                schema: "SRMS",
                table: "Requests",
                newName: "VNNumber");

            migrationBuilder.RenameColumn(
                name: "SizeRequested",
                schema: "SRMS",
                table: "Requests",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                schema: "SRMS",
                table: "Requests",
                newName: "RequestedAt");

            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                schema: "SRMS",
                table: "ProductLocations",
                newName: "LastUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                schema: "SRMS",
                table: "Picks",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Picks_RequestId",
                schema: "SRMS",
                table: "Picks",
                newName: "IX_Picks_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "SRMS",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastUpdatedBy",
                schema: "SRMS",
                table: "ProductLocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "SRMS",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                schema: "SRMS",
                table: "CycleCountItems",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "SRMS",
                columns: table => new
                {
                    OrderId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Active = table.Column<bool>(nullable: false),
                    ConvertedSize = table.Column<float>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    LastUpdatedBy = table.Column<int>(nullable: false),
                    Picked = table.Column<bool>(nullable: false),
                    PickedAt = table.Column<DateTime>(nullable: true),
                    PickedBy = table.Column<int>(nullable: false),
                    Size = table.Column<float>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    SuggestedLocationId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    VNNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Devices_LastUpdatedBy",
                        column: x => x.LastUpdatedBy,
                        principalSchema: "SRMS",
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Devices_PickedBy",
                        column: x => x.PickedBy,
                        principalSchema: "SRMS",
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "SRMS",
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductLocations_LastUpdatedBy",
                schema: "SRMS",
                table: "ProductLocations",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LastUpdatedBy",
                schema: "SRMS",
                table: "Orders",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PickedBy",
                schema: "SRMS",
                table: "Orders",
                column: "PickedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId",
                schema: "SRMS",
                table: "Orders",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Picks_Orders_OrderId",
                schema: "SRMS",
                table: "Picks",
                column: "OrderId",
                principalSchema: "SRMS",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLocations_Devices_LastUpdatedBy",
                schema: "SRMS",
                table: "ProductLocations",
                column: "LastUpdatedBy",
                principalSchema: "SRMS",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picks_Orders_OrderId",
                schema: "SRMS",
                table: "Picks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductLocations_Devices_LastUpdatedBy",
                schema: "SRMS",
                table: "ProductLocations");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "SRMS");

            migrationBuilder.DropIndex(
                name: "IX_ProductLocations_LastUpdatedBy",
                schema: "SRMS",
                table: "ProductLocations");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "SRMS",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "SRMS",
                table: "ProductLocations");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "SRMS",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "VNNumber",
                schema: "SRMS",
                table: "Requests",
                newName: "VendorStyle");

            migrationBuilder.RenameColumn(
                name: "Size",
                schema: "SRMS",
                table: "Requests",
                newName: "SizeRequested");

            migrationBuilder.RenameColumn(
                name: "RequestedAt",
                schema: "SRMS",
                table: "Requests",
                newName: "LastUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                schema: "SRMS",
                table: "ProductLocations",
                newName: "LastUpdate");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                schema: "SRMS",
                table: "Picks",
                newName: "RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Picks_OrderId",
                schema: "SRMS",
                table: "Picks",
                newName: "IX_Picks_RequestId");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                schema: "SRMS",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "SRMS",
                table: "Requests",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTime>(
                name: "FilledAt",
                schema: "SRMS",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Picked",
                schema: "SRMS",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "SizeConverted",
                schema: "SRMS",
                table: "Requests",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "SizeType",
                schema: "SRMS",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SuggestedLocationId",
                schema: "SRMS",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsYouthSize",
                schema: "SRMS",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                schema: "SRMS",
                table: "CycleCountItems",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "now()");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsYouthSize",
                schema: "SRMS",
                table: "Products",
                column: "IsYouthSize");

            migrationBuilder.AddForeignKey(
                name: "FK_Picks_Requests_RequestId",
                schema: "SRMS",
                table: "Picks",
                column: "RequestId",
                principalSchema: "SRMS",
                principalTable: "Requests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
