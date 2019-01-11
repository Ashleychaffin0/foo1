using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pMon {
    public partial class pMon : Form {

        Timer tmr;

        string PreviousMsg = "";

        public pMon() {
            InitializeComponent();

            tmr = new Timer();
            tmr.Interval = 1000;
        }

        private void pMon_Load(object sender, EventArgs e) {
            tmr.Tick += tmr_Tick;
            tmr.Enabled = true;
        }

        void tmr_Tick(object sender, EventArgs e) {
            ShowFF();
        }

        private void ShowFF() {
            var ff = from p in Process.GetProcesses()
                     where p.ProcessName.ToLower().Contains("firefox")
                     select p;
            foreach (Process p in ff) {
                string msg = string.Format("{0:X8} ({1,3})   {2}", p.Id, p.Threads.Count, p.ProcessName);
                if (msg == PreviousMsg) {
                    return;
                }
                PreviousMsg = msg;
                Msg(msg);
            }
        }

        void Msg(string msg) {
            lbMsgs.Items.Insert(0, msg);
        }
    }
}
