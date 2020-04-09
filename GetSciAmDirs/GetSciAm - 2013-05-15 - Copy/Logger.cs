using System;
using System.Drawing;
using System.Diagnostics;

namespace GetSciAm {
    internal class Logger {
        internal void Log(FileSystemWatcherRoutines.FsmState state, string msg) {
            string color = FileSystemWatcherRoutines.FsmColors[(int)state].Name;
            string txt = string.Format("<FONT COLOR=\"{0}\">Current state: {1}, {2}</FONT><br/>", color, state, msg);
            WriteLog(txt);
        }

//---------------------------------------------------------------------------------------

        internal void Log(string msg) {
            Log(State, msg);
        }

//---------------------------------------------------------------------------------------

        internal void Log(Color color, string msg) {
            string txt = string.Format("<FONT COLOR=\"{0}\">{1}</FONT><br/>", color, msg);
            WriteLog(txt);
        }

//---------------------------------------------------------------------------------------

        [Conditional("WRITELOG")]
        internal void WriteLog(string txt) {
            bool bGuiThread = GuiThreadId == System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (!bGuiThread) {
                // TODO: Shouldn't be hardcoded 
                Trace.Write("<div>");
            }
            string tstamp = DateTime.Now.ToLongTimeString() + " TID=" + System.Threading.Thread.CurrentThread.ManagedThreadId;
            Trace.WriteLine(tstamp + " " + txt);
            if (!bGuiThread) {
                // TODO: Shouldn't be hardcoded 
                Trace.WriteLine("</div>");
            }
        }
    }
}
