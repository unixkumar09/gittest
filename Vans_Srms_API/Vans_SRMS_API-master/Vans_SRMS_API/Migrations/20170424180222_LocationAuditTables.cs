using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Vans_SRMS_API.Migrations
{
    public partial class LocationAuditTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationAudits",
                schema: "SRMS",
                columns: table => new
                {
                    LocationAuditId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DeviceId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationAudits", x => x.LocationAuditId);
                    table.ForeignKey(
                        name: "FK_LocationAudits_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "SRMS",
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationAudits_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "SRMS",
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationAudits_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "SRMS",
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationAuditItems",
                schema: "SRMS",
                columns: table => new
                {
                    LocationAuditItemId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LocationAuditId = table.Column<long>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationAuditItems", x => x.LocationAuditItemId);
                    table.ForeignKey(
                        name: "FK_LocationAuditItems_LocationAudits_LocationAuditId",
                        column: x => x.LocationAuditId,
                        principalSchema: "SRMS",
                        principalTable: "LocationAudits",
                        principalColumn: "LocationAuditId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationAuditItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SRMS",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationAudits_DeviceId",
                schema: "SRMS",
                table: "LocationAudits",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationAudits_LocationId",
                schema: "SRMS",
                table: "LocationAudits",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationAudits_StoreId",
                schema: "SRMS",
                table: "LocationAudits",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationAuditItems_LocationAuditId",
                schema: "SRMS",
                table: "LocationAuditItems",
                column: "LocationAuditId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationAuditItems_ProductId",
                schema: "SRMS",
                table: "LocationAuditItems",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationAuditItems",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "LocationAudits",
                schema: "SRMS");
        }
    }
}
