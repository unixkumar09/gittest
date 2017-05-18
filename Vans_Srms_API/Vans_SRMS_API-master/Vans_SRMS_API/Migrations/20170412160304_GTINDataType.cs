using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class GTINDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GTIN",
                schema: "SRMS",
                table: "Products",
                nullable: false,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "GTIN",
                schema: "SRMS",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
