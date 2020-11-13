using Microsoft.EntityFrameworkCore.Migrations;

namespace PW_1.Migrations {
	public partial class Initial : Migration {
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.CreateTable(
				name: "tblQuestions",
				columns: table => new {
					QuestionId = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserId = table.Column<int>(nullable: false),
					SiteId = table.Column<int>(nullable: false),
					Q = table.Column<string>(nullable: true),
					A = table.Column<string>(nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_tblQuestions", x => x.QuestionId);
				});

			migrationBuilder.CreateTable(
				name: "tblSites",
				columns: table => new {
					SiteId = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UrlId = table.Column<int>(nullable: false),
					UserId = table.Column<int>(nullable: false),
					LoginID = table.Column<string>(nullable: true),
					Password = table.Column<string>(nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_tblSites", x => x.SiteId);
				});

			migrationBuilder.CreateTable(
				name: "tblUrls",
				columns: table => new {
					UrlId = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					SiteUrl = table.Column<string>(nullable: true),
					SiteName = table.Column<string>(nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_tblUrls", x => x.UrlId);
				});

			migrationBuilder.CreateTable(
				name: "tblUsers",
				columns: table => new {
					UserId = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserName = table.Column<string>(nullable: true),
					SiteId = table.Column<int>(nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_tblUsers", x => x.UserId);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropTable(
				name: "tblQuestions");

			migrationBuilder.DropTable(
				name: "tblSites");

			migrationBuilder.DropTable(
				name: "tblUrls");

			migrationBuilder.DropTable(
				name: "tblUsers");
		}
	}
}
