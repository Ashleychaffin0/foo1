using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LeadsLightningMockup {
	/// <summary>
	/// Summary description for PseudoWebPage.
	/// </summary>
	public class PseudoWebPage : System.Windows.Forms.UserControl {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Panel	ParentPanel;

		public static ArrayList History = new ArrayList();

		public PseudoWebPage(Panel PseudoPage) {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			ParentPanel = PseudoPage;

			ParentPanel.Controls.Clear();
			// This next line is a little tricky. Because we're normally called 
			// as a base class, inside this ctor the complete object hasn't been
			// constructed yet. In particular, the Form (or more likely, UserControl)
			// hasn't instantiated all its controls, set its background color, etc.
			// Yet when we execute the next line of code, everything displays
			// correctly. How can that happen? My guess is this. Setting the
			// Parent property posts a WM_PAINT, but the message queue won't
			// be queried (normally) until the object has finished being
			// constructed, and we fall back into the normal GetMessage loop.
			this.Parent = PseudoPage;

			// This next line is important, verging on crucial. Currently we
			// display the page merely by instantiating it (new MyPage();).
			// We'd better keep a reference to it around, else it'll be
			// garbage collected out from under us.
			History.Add(this);
		}

		// The Visual Studio forms designer seems to need a default ctor
		public PseudoWebPage() {
			ParentPanel = null;
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
	}
}
