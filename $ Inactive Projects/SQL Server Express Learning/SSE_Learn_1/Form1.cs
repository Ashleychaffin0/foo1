using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace SSE_Learn_1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            var ps = System.Diagnostics.Process.GetProcesses();

            var q1 = from proc in ps
                     // where proc.WorkingSet64 > 100000 && proc.HandleCount > 100
                     where proc.MainWindowTitle.StartsWith("Photo")
                     orderby proc.ProcessName
                     select new { proc.ProcessName, proc.MainWindowTitle, proc.Id };

            foreach (var item in q1) {
                Console.WriteLine("Name = {0}, Title={1}, PID={2}", item.ProcessName, item.MainWindowTitle, item.Id);
            }
        }
    }
}
