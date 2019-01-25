using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agranams {
    public partial class Agranams : Form {
        public Agranams() {
            InitializeComponent();
        }

//---------------------------------------------------------------------------------------

        private void Form1_Load(object sender, EventArgs e) {
            var words = new Words(@"D:\LRS\Words.txt");
        }
    }
}
