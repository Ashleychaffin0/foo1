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

// See http://stackoverflow.com/questions/8476103/accessing-database-connection-string-using-app-config-in-c-sharp-winform
// See https://www.connectionstrings.com/sqlconnection/

namespace TestSqlServer {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			// Connect to a database
			string connectionString = "Data Source=192.168.123.45;Initial Catalog=MyDatabase;Integrated Security=SSPI;";
			connectionString = "Data Source=(local);Initial Catalog=FingerprintsDb;Integrated Security=True;";
			connectionString = "Server=(localdb)\v11.0;Initial Catalog=FingerprintsDb;Integrated Security=True;";
			using (SqlConnection connection = new SqlConnection(connectionString)) {
				using (SqlCommand command = new SqlCommand("SELECT Region FROM dbo.tlkpRegion WHERE RegionID=30", connection)) {
					connection.Open();
					string result = (string)command.ExecuteScalar();
					MessageBox.Show("Region = " + result);
				}
			}
		}
	}
}
