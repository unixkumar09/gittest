using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class OrderedByDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderedAt",
                schema: "SRMS",
                table: "Orders",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<int>(
                name: "OrderedBy",
                schema: "SRMS",
                table: "Orders",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql(@"UPDATE ""SRMS"".""Orders"" SET ""OrderedBy"" = ""LastUpdatedBy"";");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderedAt",
                schema: "SRMS",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderedBy",
                schema: "SRMS",
                table: "Orders");
        }
    }
}
