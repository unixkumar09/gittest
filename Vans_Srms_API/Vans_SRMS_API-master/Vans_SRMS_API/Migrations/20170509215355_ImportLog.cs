using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Vans_SRMS_API.Migrations
{
    public partial class ImportLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemMasterImports",
                schema: "SRMS",
                columns: table => new
                {
                    ItemMasterImportLogId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FileLastModifiedDate = table.Column<DateTime>(nullable: false),
                    FinishedAt = table.Column<DateTime>(nullable: false),
                    ProductsFailedToImport = table.Column<int>(nullable: false),
                    ProductsImported = table.Column<int>(nullable: false),
                    RecordsInFile = table.Column<int>(nullable: false),
                    StartedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMasterImports", x => x.ItemMasterImportLogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemMasterImports",
                schema: "SRMS");
        }
    }
}
