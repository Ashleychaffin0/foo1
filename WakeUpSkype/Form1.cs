using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WakeUpSkype {

    public partial class Form1 : Form {

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

//---------------------------------------------------------------------------------------

        public Form1() {
            InitializeComponent();

            var xxx = Process.GetProcesses();
            foreach (var proc in xxx) {
                Console.WriteLine("ProcessName = {0}", proc.ProcessName);
            }

            timer1.Interval = 5 * 50 * 1000;
            timer1.Start();

        }

//---------------------------------------------------------------------------------------

        private void timer1_Tick(object sender, EventArgs e) {
            var Handles = from procs in Process.GetProcesses()
                          where procs.ProcessName == "Skype"
                          select procs.MainWindowHandle;
            foreach (var handle in Handles) {
                // Presumably only 1 handle, but we'll take all we find
                SetForegroundWindow(handle);
                // System.Threading.Thread.Sleep(500);
                SendKeys.Send("%soo");
            }
        }
    }
}
