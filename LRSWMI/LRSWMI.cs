using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LRSWMI {
    public partial class LRSWMI : Form {
        public LRSWMI() {
            InitializeComponent();

            ManagementClass c = new ManagementClass("Win32_Service");

            foreach (ManagementObject o in c.GetInstances()) {
                Console.WriteLine("Service Name = {0} " +
                    "ProcessId = {1} Instance Path = {2}",
                    o["Name"], o["ProcessId"], o.Path);
            }
        }
    }
}
