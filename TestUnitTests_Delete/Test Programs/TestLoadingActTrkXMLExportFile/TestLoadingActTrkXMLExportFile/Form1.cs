using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.Xml;

namespace TestLoadingActTrkXMLExportFile
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(208, 48);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(168, 88);
			this.button1.TabIndex = 0;
			this.button1.Text = "Load XML File";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(568, 334);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e) {
			try {
				XmlDocument	xdoc = new XmlDocument();
				xdoc.Load(@"C:\ExpAll.xml");
				// ProcessSurveys(xdoc);
				ProcessPlacement();
#if false
				// MessageBox.Show("Read it OK");
				// foreach (XmlNode xNode in xdoc.ChildNodes) {
				// foreach (XmlNode xNode in xdoc.SelectNodes("SwipeCardExport/Customers")) {
				foreach (XmlNode xNode in xdoc.SelectNodes("SwipeCardExport/Surveys")) {
					Console.WriteLine("xNode Name='{0}', Value='{1}'", xNode.Name, xNode.Value);
					ShowChildren(xNode, 1);
				}
#endif
			} catch (Exception ex) {
				MessageBox.Show(string.Format("Exception {0} in LoadXML", ex.Message));
			}
		}

		void ShowChildren(XmlNode xNode, int indent) {
			string ind = new string(' ', indent * 4);
			Console.WriteLine("{0}Name='{1}', Value='{2}'", ind, xNode.Name, xNode.Value);
			if (xNode.HasChildNodes) {
				foreach (XmlNode child in xNode) {
					ShowChildren(child, indent + 1);
				}
			}
		}

		void ProcessSurveys(XmlDocument xdoc) {
			XmlNode	Surveys = xdoc.SelectNodes("SwipeCardExport/Surveys")[0];
			foreach (XmlNode xNode in Surveys.ChildNodes) {
				ProcessSingleSurvey(xNode.ChildNodes);
			}
		}

		void ProcessSingleSurvey(XmlNodeList xnl) {
			Console.WriteLine();
			foreach (XmlNode xNode in xnl) {
				Console.WriteLine("Survey field name={0}, value='{1}'", xNode.Name, xNode.ChildNodes[0].Value);
			}
		}

		void ProcessPlacement() {
			XmlTextReader	rdr = new XmlTextReader(@"C:\ExpAll.xml");
			rdr.ReadStartElement("SwipeCardExport/Placements");
			rdr.ReadStartElement();
			if (rdr.NodeType == XmlNodeType.Element && rdr.Name == "Placement") {
				Console.WriteLine("Placement Name='{0}', Value='{1}'", rdr.Name, rdr.Value);
			}
		}
	}
}
