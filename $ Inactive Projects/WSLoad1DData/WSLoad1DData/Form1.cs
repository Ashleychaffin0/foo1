using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WSLoad1DData {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

//---------------------------------------------------------------------------------------

        private void btnBrowse_Click(object sender, EventArgs e) {
            MessageBox.Show("Nonce on Browse");
        }

//---------------------------------------------------------------------------------------

        private void btnGo_Click(object sender, EventArgs e) {
            string textFilename = txtFilename.Text.Trim();
            if (textFilename.Length == 0) {
                MessageBox.Show("Text box empty");
                return;
            }
            // It's only a driver for me. Don't bother validating the filename as an
            // actually existing file, readable, etc
            string text = File.ReadAllText(textFilename);
            text = text.Replace("\r", "");
            clsLoad1DData Loader = new clsLoad1DData();
            int rc = Loader.Load1DData(text);
        }
    }
}
