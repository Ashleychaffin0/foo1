using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace TestLinqToSqlWithXml_1 {
    public partial class Form1 : Form {

        //---------------------------------------------------------------------------------------

        public Form1() {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------------------

        private void btnInsertNow_Click(object sender, EventArgs e) {
            var dc1 = new DataClasses1DataContext();
            List<tblTestXml_1> dats = new List<tblTestXml_1>();
            var dat1 = new tblTestXml_1 {
                Name = "LRS2",
                Xml = new XElement("Name", new XElement("First", "Lawrence"), new XElement("LastName", "Smith"))
            };
            var dat2 = new tblTestXml_1 {
                Name = "Now1",
                Xml = new XElement("Now", DateTime.Now)
            };
            dats.Add(dat1);
            dats.Add(dat2);
            // dc1.tblTestXml_1s.InsertOnSubmit(dat1);
            dc1.tblTestXml_1s.InsertAllOnSubmit<tblTestXml_1>(dats);
            dc1.SubmitChanges();
        }

        //---------------------------------------------------------------------------------------

        private void btnList_Click(object sender, EventArgs e) {
            var dc1 = new DataClasses1DataContext();
            var q1 = from x in dc1.tblTestXml_1s
                     select x;

            foreach (var item in q1) {
                Console.WriteLine("{0}", item);
            }

            using (var dc2 = new DataClasses1DataContext()) {
                var q2 = from x in dc1.tblTestXml_1s
                         select x;

                foreach (var item in q2) {
                    Console.WriteLine("{0}", item);
                }
            }
            Console.WriteLine("Hello");
        }
    }
}

