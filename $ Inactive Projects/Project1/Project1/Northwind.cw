using System;
using Northwind;

/* NB: Before building this project for the first time, 
   you will need to generate Northwind.dll from the SQL server Northwind database.
   
   Follow these steps:
   In the [Solution Explorer] window, under [References], 
   right-click [Northwind] and select [Properties].
   In the resulting [Comega SQL Generator] dialog box, 
   choose the [Assembly] tab, and click the [Build] button, 
   wait until the build completes, and then click [OK].
   You can now build the project itself.
*/ 

public class Test {
	public static void Main( string[] args ) {
		String connectstring = "Data Source=(local); Initial Catalog=Northwind; Connect Timeout=5; Integrated Security=SSPI";
		Database dbo = new Database(connectstring);

		string mycity = args.Length>0 ? args[0] : "London";
		Console.WriteLine("Contacts in \"{0}\"", mycity);
		Console.WriteLine();
		Console.WriteLine("CustomerID   Contact");
		Console.WriteLine("--------------------------------------------");
			
		// Comega allows variables to be declared without the "type".  It
		// infers the type from the right hand side of the assignment.
		res = select CustomerID, ContactName 
			  from dbo.Customers 
			  where City == mycity 
			  order by ContactName;
		
		// The same type inference applies inside the foreach expression.
		foreach( row in res) {		                
			Console.WriteLine("{0,-12} {1}", row.CustomerID, row.ContactName);
		}
		Console.Write("\nPress any key to continue...");
		Console.In.ReadLine();
		return;
	}
}