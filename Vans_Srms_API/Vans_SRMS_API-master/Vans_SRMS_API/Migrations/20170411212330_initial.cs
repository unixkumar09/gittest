using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Vans_SRMS_API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SRMS");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", "'uuid-ossp', '', ''");

            migrationBuilder.CreateTable(
                name: "Colors",
                schema: "SRMS",
                columns: table => new
                {
                    ColorId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Hex = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ColorId);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "SRMS",
                columns: table => new
                {
                    DeviceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DeviceNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "SRMS",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Color = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    GTIN = table.Column<long>(nullable: false),
                    InStock = table.Column<bool>(nullable: false, defaultValue: false),
                    IsYouthSize = table.Column<bool>(nullable: false),
                    ItemNumber = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    Retail = table.Column<decimal>(nullable: false),
                    SKU = table.Column<string>(nullable: true),
                    Size = table.Column<float>(nullable: false),
                    Style = table.Column<string>(nullable: false),
                    VendorStyle = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                schema: "SRMS",
                columns: table => new
                {
                    SizeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EURSize = table.Column<float>(nullable: false),
                    Kids = table.Column<bool>(nullable: false),
                    MEXSize = table.Column<float>(nullable: false),
                    UKSize = table.Column<float>(nullable: false),
                    USSize = table.Column<float>(nullable: false),
                    WomenSize = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.SizeId);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                schema: "SRMS",
                columns: table => new
                {
                    StoreId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    DistrictManager = table.Column<string>(nullable: true),
                    DistrictNumber = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    RegionalDirector = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    StoreNumber = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Zip = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "SRMS",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Barcode = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FlaggedForAudit = table.Column<bool>(nullable: false, defaultValue: false),
                    FlaggedForCycleCount = table.Column<bool>(nullable: false, defaultValue: false),
                    Name = table.Column<string>(nullable: false),
                    StoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "SRMS",
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                schema: "SRMS",
                columns: table => new
                {
                    RequestId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    FilledAt = table.Column<DateTime>(nullable: true),
                    InStockQuantity = table.Column<int>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    Picked = table.Column<bool>(nullable: false),
                    SizeConverted = table.Column<float>(nullable: false),
                    SizeRequested = table.Column<float>(nullable: false),
                    SizeType = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    VendorStyle = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_Requests_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "SRMS",
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreDevices",
                schema: "SRMS",
                columns: table => new
                {
                    StoreDeviceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Active = table.Column<bool>(nullable: false),
                    ColorId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    DeviceId = table.Column<int>(nullable: false),
                    DeviceKey = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    LastUpdate = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    StoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreDevices", x => x.StoreDeviceId);
                    table.ForeignKey(
                        name: "FK_StoreDevices_Colors_ColorId",
                        column: x => x.ColorId,
                        principalSchema: "SRMS",
                        principalTable: "Colors",
                        principalColumn: "ColorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreDevices_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "SRMS",
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreDevices_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "SRMS",
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductLocations",
                schema: "SRMS",
                columns: table => new
                {
                    ProductLocationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LastUpdate = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    LocationId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLocations", x => x.ProductLocationId);
                    table.ForeignKey(
                        name: "FK_ProductLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "SRMS",
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductLocations_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SRMS",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Putaways",
                schema: "SRMS",
                columns: table => new
                {
                    PutawayId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DeviceKey = table.Column<string>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Putaways", x => x.PutawayId);
                    table.ForeignKey(
                        name: "FK_Putaways_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "SRMS",
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Putaways_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SRMS",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Picks",
                schema: "SRMS",
                columns: table => new
                {
                    PickId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DeviceKey = table.Column<string>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    RequestId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picks", x => x.PickId);
                    table.ForeignKey(
                        name: "FK_Picks_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "SRMS",
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Picks_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SRMS",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Picks_Requests_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "SRMS",
                        principalTable: "Requests",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_StoreId",
                schema: "SRMS",
                table: "Locations",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_LocationId",
                schema: "SRMS",
                table: "Picks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_ProductId",
                schema: "SRMS",
                table: "Picks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_RequestId",
                schema: "SRMS",
                table: "Picks",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_GTIN",
                schema: "SRMS",
                table: "Products",
                column: "GTIN");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsYouthSize",
                schema: "SRMS",
                table: "Products",
                column: "IsYouthSize");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorStyle",
                schema: "SRMS",
                table: "Products",
                column: "VendorStyle");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLocations_LocationId",
                schema: "SRMS",
                table: "ProductLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLocations_ProductId",
                schema: "SRMS",
                table: "ProductLocations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Putaways_LocationId",
                schema: "SRMS",
                table: "Putaways",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Putaways_ProductId",
                schema: "SRMS",
                table: "Putaways",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_StoreId",
                schema: "SRMS",
                table: "Requests",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreDevices_ColorId",
                schema: "SRMS",
                table: "StoreDevices",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreDevices_DeviceId",
                schema: "SRMS",
                table: "StoreDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreDevices_StoreId",
                schema: "SRMS",
                table: "StoreDevices",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Picks",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "ProductLocations",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "Putaways",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "Sizes",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "StoreDevices",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "Requests",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "Colors",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "SRMS");

            migrationBuilder.DropTable(
                name: "Stores",
                schema: "SRMS");
        }
    }
}
