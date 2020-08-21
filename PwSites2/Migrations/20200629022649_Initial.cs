using Microsoft.EntityFrameworkCore.Migrations;

namespace PwSites2.Migrations {
	public partial class Initial : Migration {
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.CreateTable(
				name: "TblMapSiteToUidPws",
				columns: table => new {
					MapSiteToUidPwId = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UidPwId = table.Column<int>(nullable: false),
					IDID = table.Column<int>(nullable: false),
					SiteId = table.Column<int>(nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_TblMapSiteToUidPws", x => x.MapSiteToUidPwId);
				});

			migrationBuilder.CreateTable(
				name: "TblSites",
				columns: table => new {
					SiteId = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					SiteName = table.Column<string>(nullable: true),
					Comments = table.Column<string>(nullable: true),
					UidPwId = table.Column<int>(nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_TblSites", x => x.SiteId);
				});

			migrationBuilder.CreateTable(
				name: "TblUidPws",
				columns: table => new {
					UidPwId = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserName = table.Column<string>(nullable: true),
					Password = table.Column<string>(nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_TblUidPws", x => x.UidPwId);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropTable(
				name: "TblMapSiteToUidPws");

			migrationBuilder.DropTable(
				name: "TblSites");

			migrationBuilder.DropTable(
				name: "TblUidPws");
		}
	}
}
