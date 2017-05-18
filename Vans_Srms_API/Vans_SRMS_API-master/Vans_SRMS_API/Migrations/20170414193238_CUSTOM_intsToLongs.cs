using Microsoft.EntityFrameworkCore.Migrations;

namespace Vans_SRMS_API.Migrations
{
    public partial class intsToLongs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = @"
DO
$do$
BEGIN
	IF NOT EXISTS (SELECT 1 FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = '20170414193238_CUSTOM_intsToLongs') THEN
	BEGIN
		    ALTER SEQUENCE ""SRMS"".""Requests_RequestId_seq"" START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;
		    ALTER TABLE ""SRMS"".""Requests"" ALTER COLUMN ""RequestId"" TYPE int8;
		    ALTER TABLE ""SRMS"".""Requests"" ALTER COLUMN ""RequestId"" SET NOT NULL;
		    ALTER TABLE ""SRMS"".""Requests"" ALTER COLUMN ""RequestId"" SET DEFAULT(nextval('""SRMS"".""Requests_RequestId_seq""'));
		    ALTER SEQUENCE ""SRMS"".""Requests_RequestId_seq"" OWNED BY ""SRMS"".""Requests"".""RequestId"";

		    ALTER SEQUENCE ""SRMS"".""Putaways_PutawayId_seq"" START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE; 
		    ALTER TABLE ""SRMS"".""Putaways"" ALTER COLUMN ""PutawayId"" TYPE int8;
		    ALTER TABLE ""SRMS"".""Putaways"" ALTER COLUMN ""PutawayId"" SET NOT NULL;
		    ALTER TABLE ""SRMS"".""Putaways"" ALTER COLUMN ""PutawayId"" SET DEFAULT(nextval('""SRMS"".""Putaways_PutawayId_seq""'));
		    ALTER SEQUENCE ""SRMS"".""Putaways_PutawayId_seq"" OWNED BY ""SRMS"".""Putaways"".""PutawayId"";

		    ALTER SEQUENCE ""SRMS"".""ProductLocations_ProductLocationId_seq"" START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;
		    ALTER TABLE ""SRMS"".""ProductLocations"" ALTER COLUMN ""ProductLocationId"" TYPE int8;
		    ALTER TABLE ""SRMS"".""ProductLocations"" ALTER COLUMN ""ProductLocationId"" SET NOT NULL;
		    ALTER TABLE ""SRMS"".""ProductLocations"" ALTER COLUMN ""ProductLocationId"" SET DEFAULT(nextval('""SRMS"".""ProductLocations_ProductLocationId_seq""'));
		    ALTER SEQUENCE ""SRMS"".""ProductLocations_ProductLocationId_seq"" OWNED BY ""SRMS"".""ProductLocations"".""ProductLocationId"";

		    ALTER TABLE ""SRMS"".""Picks"" ALTER COLUMN ""RequestId"" TYPE int8;
		    ALTER TABLE ""SRMS"".""Picks"" ALTER COLUMN ""RequestId"" SET NOT NULL;
		    ALTER TABLE ""SRMS"".""Picks"" ALTER COLUMN ""RequestId"" DROP DEFAULT;
		    ALTER SEQUENCE ""SRMS"".""Picks_PickId_seq"" START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;
		    ALTER TABLE ""SRMS"".""Picks"" ALTER COLUMN ""PickId"" TYPE int8;
		    ALTER TABLE ""SRMS"".""Picks"" ALTER COLUMN ""PickId"" SET NOT NULL;
		    ALTER TABLE ""SRMS"".""Picks"" ALTER COLUMN ""PickId"" SET DEFAULT(nextval('""SRMS"".""Picks_PickId_seq""'));
		    ALTER SEQUENCE ""SRMS"".""Picks_PickId_seq"" OWNED BY ""SRMS"".""Picks"".""PickId"";

		    INSERT INTO ""__EFMigrationsHistory""(""MigrationId"", ""ProductVersion"") VALUES('20170414193238_CUSTOM_intsToLongs', '1.1.1');
		END;
	END IF;
END
$do$";


            var resp = migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
