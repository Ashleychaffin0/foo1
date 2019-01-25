using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClispEd {
	public partial class ClispEd : Form {
		public ClispEd() {
			InitializeComponent();

			scintilla.Text = @"(Hello word)
(how are (you today?))
(I'm fine!)
(add 1 2)";

			scintilla.ConfigurationManager.Language = "lisp";
			scintilla.ConfigurationManager.Configure();
		}
	}
}
