using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NameMP3 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

//---------------------------------------------------------------------------------------

        private void btnGo_Click(object sender, EventArgs e) {
            int val;
            bool bOK = int.TryParse(txtValue.Text, out val);
            if (! bOK) {
                MessageBox.Show("Value must be numeric");
                return;
            }
            string txt = string.Format(txtText.Text, val);
            txtValue.Text = (val + 1).ToString();
            Clipboard.SetText(txt);
        }

//---------------------------------------------------------------------------------------

        private void button2_Click(object sender, EventArgs e) {
            int numDisks = (int)udNumberOfDisks.Value;
            int DiskNo = (int)udDiskNumber.Value;

            string DirName = string.Format(DirNameTemplate.Text, udDiskNumber.Value, numDisks);

            if (!Directory.Exists(DirName)) {
                MessageBox.Show("Please enter a valid directory name", "NameMP3");
                return;
            }

            string CurDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(DirName);

            var Files = Directory.GetFiles(DirName);
            Array.Sort(Files);

            this.UseWaitCursor = true;

            for (int i = 1; i <= Files.Length; i++) {
                string CurName = Path.GetFileName(Files[i - 1]);
                string Ext = Path.GetExtension(Files[i - 1]);
                string NewName = string.Format(txtText.Text, DiskNo, numDisks, i) + Ext;
                // MessageBox.Show("Rename\n" + CurName + "\n\nto\n\n" + NewName);
                File.Move(CurName, NewName);
            }

            Directory.SetCurrentDirectory(CurDir);
            this.UseWaitCursor = false;
        }
    }
}
