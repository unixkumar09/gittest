using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class ProductConvertedSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ConvertedSize",
                schema: "SRMS",
                table: "Products",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.Sql(@"UPDATE ""SRMS"".""Products"" SET ""ConvertedSize"" = ""Size"" + 1.5 WHERE ""Type"" = 2;");
            migrationBuilder.Sql(@"UPDATE ""SRMS"".""Products"" SET ""ConvertedSize"" = ""Size"" WHERE ""Type"" <> 2;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConvertedSize",
                schema: "SRMS",
                table: "Products");
        }
    }
}
