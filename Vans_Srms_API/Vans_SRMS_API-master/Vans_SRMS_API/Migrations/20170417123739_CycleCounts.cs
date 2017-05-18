using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Vans_SRMS_API.Migrations
{
    public partial class CycleCounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CycleCounts",
                schema: "SRMS",
                columns: table => new
                {
                    CycleCountId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DeviceId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CycleCounts", x => x.CycleCountId);
                    table.ForeignKey(
                        name: "FK_CycleCounts_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "SRMS",
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CycleCounts_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "SRMS",
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CycleCounts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "SRMS",
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CycleCountItems",
                schema: "SRMS",
                columns: table => new
                {
                    CycleCountItemId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CycleCountId = table.Column<long>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CycleCountItems", x => x.CycleCountItemId);
                    table.ForeignKey(
                        name: "FK_CycleCountItems_CycleCounts_CycleCountId",
                        column: x => x.CycleCountId,
                        principalSchema: "SRMS",
                        principalTable: "CycleCounts",
                        principalColumn: "CycleCountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CycleCountItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SRMS",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CycleCounts_DeviceId",
                schema: "SRMS",
                table: "CycleCounts",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCounts_LocationId",
                schema: "SRMS",
                table: "CycleCounts",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCounts_StoreId",
                schema: "SRMS",
                table: "CycleCounts",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCountItems_CycleCountId",
                schema: "SRMS",
                table: "CycleCountItems",
                column: "CycleCountId");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCountItems_ProductId",
                schema: "SRMS",
                table: "CycleCountItems",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CycleCountItems",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "CycleCounts",
                schema: "SRMS");
        }
    }
}
