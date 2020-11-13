// PW_1: Get database defined and working
// See https://entityframeworkcore.com/providers-sqlite#:~:text=Microsoft.EntityFrameworkCore.Sqlite%20database%20provider%20allows%20Entity%20Framework%20Core%20to,first%20step%20is%20to%20install%20Microsoft.EntityFrameworkCore.Sqlite%20NuGet%20package.
//	1)	Nuget Microsoft.EntityFrameworkCore.Sqlite
//	2)	Nuget Microsoft.EntityFrameworkCore; Add using
//	3)	Nuget Microsoft.EntityFrameworkCore.Tools
//	4)	Add database classes (all fields via Properties)
//	5)	Define PWContext, deriving from DbContext
//	6)	Modify OnModelCreating and OnConfiguring as below
// No... Doesn't need this //	7)	From Package Manager Console, run "Add-Migration Initial"
//	7)	For data annotations (e.g. [Required]), using System.ComponentModel.DataAnnotations

// Retrieve ID after Insert
//		PK automatically updated in class. e.g. Add user "LRS", find UserId in lrs.UserId

// Avoid duplicate entries
//	*	Adding, say, "lrs" to Users more than once will not give an error. But the PK will be updated,
//		which will invalidate various table linkages.
//	*	var hoo = db.Users.Where(u => u.UserName == "xLRS").FirstOrDefault(); will return non-null
//		if entry is there.
//	*	Or, var foo = (from name in db.Users
//			where name.UserName == "LRS"
//			select name).FirstOrDefault();	// Again returning null or not

//	*	Can't seem to flag a field as unique, but creating a unique index on that field
//		would work (but currently untried)

// TODO: Mark fields as Required, Unique, Anything else
//			https://www.learnentityframeworkcore.com/configuration/data-annotation-attributes
// TODO: Figure out ForeignKey's
// TODO: Link tables (just use LINQ?)
// See https://www.tektutorialshub.com/entity-framework/join-query-entity-framework/

// TODO: Indexes? See LibContext.cs below

// See g:\OneDrive\$Dev\$$$ C# Ongoing Projects\LRSLibraryBooks\LibContext.cs

// TODO: Add config file for DBname and text input filename. For now, hardcode

namespace PW_1 {
	class Program {

		static void Main() {
			var app = new TheApp();
			app.PopulateWithTestData(TheApp.ImportFilename);
			// TODO: Add actual things to do later
		}
	}
}
