using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace WSLoad1DData {
    class clsLoad1DData {

        SqlConnection conn;

//---------------------------------------------------------------------------------------

        public int Load1DData(string text) {
            // TODO: Validity check text
            text = "";
            string[] lines = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) {
                // TODO: Do real return value
                System.Windows.Forms.MessageBox.Show("TODO: Passed in empty data");
                return 1;
            }
            OpenDatabase();
            return Load1DAllData(lines);
        }

//---------------------------------------------------------------------------------------

        private int Load1DAllData(string[] lines) {
            string hdr = lines[0];
            // TODO: Process (and validate) header
            for (int i = 1; i < lines.Length; i++) {    // Note we're skipping hdr line
                Load1D(conn, lines[i]);
            }
            return 0;
        }

//---------------------------------------------------------------------------------------

        private void Load1D(SqlConnection conn, string p) {
            throw new NotImplementedException();
        }

//---------------------------------------------------------------------------------------

        private void OpenDatabase() {
            string connectionString;
            // connectionString = @"Data Source=LRS9450-PC\MSSQLSERVERLRS;Initial Catalog=Test_Xml_2;Integrated Security=True;Pooling=False";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"LRS9450-PC\MSSQLSERVERLRS";
            builder.InitialCatalog = "LRS_LL";
            builder.IntegratedSecurity = true;
            connectionString = builder.ConnectionString;
            conn = new SqlConnection(connectionString);
            conn.Open();
        }
    }
}
