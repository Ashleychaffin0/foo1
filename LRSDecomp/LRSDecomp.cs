using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using nsLRSPE;

// http://stackoverflow.com/questions/2892097/parsing-plain-win32-pe-file-exe-dll-in-net
// dbghelp.dll

namespace LRSDecomp {

//---------------------------------------------------------------------------------------

    public partial class LRSDecomp : Form {
        public LRSDecomp() {
            InitializeComponent();

            InitialTest();
        }

//---------------------------------------------------------------------------------------

        private void InitialTest() {
            string fileName = @"D:\LRS-9450\CPP\AI3\Debug\AI3.exe";
            fileName = @"C:\Windows\Notepad.exe";
            // fileName = @"D:\LRS\Devel\C#-2012\LRSDecomp\Notepad.exe";
            // fileName = @"C:\Program Files (x86)\Fraqtive\bin\fraqtive.exe";
            // fileName = @"LRSDecomp.exe";
            var pe = new LRSPE(fileName);
            // Api.PEFile peFl = Api.PEFile.ReadPEFile(fileName);
        }
    }
}
