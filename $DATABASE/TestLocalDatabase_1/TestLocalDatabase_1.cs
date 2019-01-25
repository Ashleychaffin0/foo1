using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestLocalDatabase_1 {
	public partial class TestLocalDatabase_1 : Form {
		public TestLocalDatabase_1() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			// Code from http://msdn.microsoft.com/en-us/library/dw70f090(v=vs.110).aspx
			// See also http://stackoverflow.com/questions/10540438/what-is-the-connection-string-for-localdb-for-version-11

			// string connectionString = @"Server=(localdb)\V11.0;Integrated Security=true;Database=LRSTest;";
			string connectionString = @"Server=(localdb)\V11.0;Integrated Security=true;AttachDbFileName=D:\LRS\LRSTest.mdf;";

			// Provide the query string with a parameter placeholder. 
			string SQL =
				"SELECT ProductID, UnitPrice, ProductName from dbo.products "
					+ "WHERE UnitPrice > @pricePoint "
					+ "ORDER BY UnitPrice DESC;";

			// Specify the parameter value. 
			int paramValue = 5;

			// Create and open the connection in a using block. This 
			// ensures that all resources will be closed and disposed 
			// when the code exits. 
			using (var conn = new SqlConnection(connectionString)) {
				// Create the Command and Parameter objects.
				var cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@pricePoint", paramValue);
				
				// Open the connection in a try/catch block.  
				// Create and execute the DataReader, writing the result 
				// set to the console window. 
				try {
					conn.Open();
					using (var reader = cmd.ExecuteReader()) {
						while (reader.Read()) {
							Console.WriteLine("\t{0}\t{1}\t{2}",
								reader[0], reader[1], reader[2]);
						}
					}
				} catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
				Console.ReadLine();
			}

		}
	}
}
