using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace TestXml_1 {
    public partial class Form1 : Form {
        string ConnectStringCe;
        // string ConnectStringSS;

//---------------------------------------------------------------------------------------

        public Form1() {
            InitializeComponent();

            string dbName = @"C:\LRS\TestSqlCeDatabase.sdf";
            ConnectStringCe = "Data Source='" + dbName + "'";

            // ConnectStringSS = "Data Source='MSSQLSERVERLRS\
        }

//---------------------------------------------------------------------------------------

        private void btnLoadData_Click(object sender, EventArgs e) {
            tblTestXml_1DataContext db = new tblTestXml_1DataContext();
            var x = db.GetTable<tblTestXml_1>();
            var dat = new tblTestXml_1() {
                Name = "Test 1",
                Xml  = new XElement("FullName", new XElement("First", "Larry"), new XElement("Last", "Smith"))
            };
            db.tblTestXml_1s.InsertOnSubmit(dat);
            db.SubmitChanges();
        }

//---------------------------------------------------------------------------------------

        private void btnCreateDatabase_Click(object sender, EventArgs e) {
            SqlCeEngine engine = new SqlCeEngine(ConnectStringCe);
            engine.CreateDatabase();
        }
    }
}
