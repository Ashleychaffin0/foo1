using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Serialization.Formatters.Soap;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Play1 {

	[Serializable]
	public class BartCarteCommand {
		public string	Opcode;

		public BartCarteCommand() {
			Opcode = "";
		}

		public BartCarteCommand(string Opcode) {
			this.Opcode = Opcode;
		}
	}

	[Serializable, XmlInclude(typeof(text))]
	public class BartCarteCommand_Print : BartCarteCommand {
		public ArrayList		TextItems;
//		public text []			TextItems;

		public BartCarteCommand_Print()
			: base("print") {
			TextItems = new ArrayList();
//			TextItems = new text[1];
		}

		internal void AddText(text t) {
			TextItems.Add(t);
//			TextItems[0] = t;
		}
	}

	[Serializable]
	public class text {
		public string	txt;
		public Point	xy;

		public text() {
			txt = "";
			xy = new Point(0, 0);
		}

		public text(string txt, Point xy) {
			this.txt = txt;
			this.xy  = xy;
		}
	}

#if false			// Test Foo/Goo Serialization
	[Serializable]
	public class Foo {
		public int		x;
		public string	y;
		public Goo [] 	gooze;

		public Foo() {
			x = -1;
			y = null;
			gooze = null;
		}

		public Foo(int x, string y, Goo [] g) {
			this.x = x;
			this.y = y;
			this.gooze = g;
		}
	}

	public class Goo {
		public int		goo_x;
		public string	goo_y;

		public Goo() {
			goo_x = -2;
			goo_y = "*Undefined*";
		}

		public Goo(int x, string y) {
			goo_x = x;
			goo_y = y;
		}
	}
#endif

	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
#if false			// Foo/Goo Serialization test
			Goo []	gs = new Goo [] {new Goo(27, "hi"), new Goo(19, "there")};
			Foo		foo = new Foo(1, "foo", gs);
			try {
				TextWriter		tw = new StreamWriter(@"C:\foo.xml", false, System.Text.Encoding.ASCII);
				XmlSerializer	xsr = new XmlSerializer(typeof(Foo));
				xsr.Serialize(tw, foo);
				tw.Close();
			} catch (Exception e) {
				MessageBox.Show(string.Format("Serialization error - {0}", e.Message));
			}
#endif

			try {
				BartCarteCommand_Print	cmd = new BartCarteCommand_Print();
				cmd.AddText(new text("Hi", new Point(10, 20)));
				cmd.AddText(new text("world", new Point(50, 72)));
				StreamWriter	tw = new StreamWriter(@"C:\foo.xml", false, System.Text.Encoding.ASCII);
				XmlSerializer	xsr = new XmlSerializer(typeof(BartCarteCommand_Print));
				xsr.Serialize(tw, cmd);
				tw.Close();

				FileStream		fs = new FileStream(@"c:\goo.txt", FileMode.OpenOrCreate);
				SoapFormatter	sf = new SoapFormatter();
				sf.Serialize(fs, cmd);
				fs.Close();
			} catch (Exception e) {
				MessageBox.Show(string.Format("Soap Serialization error - {0}", e.Message));
			}

			XmlDocument		xdoc = new XmlDocument();
			xdoc.Load(@"C:\foo.xml");
			XmlNodeList		xnl;
			xnl = xdoc.GetElementsByTagName("BartCarteCommand_Print");
			// Clearly, we need recursive calls here to truly print out everything.
			// But for now...
			foreach (XmlNode xNode in xnl) {
				Console.WriteLine("Name={0}", xNode.Name);
				foreach (XmlAttribute attr in xNode.Attributes) {
					Console.WriteLine("\tAttribute Name={0}, Value={1}", attr.Name, attr.Value);
				}
				foreach (XmlNode xNode2 in xNode.ChildNodes) {
					Console.WriteLine("\t\tName={0}", xNode2.Name);
				}
			}

			Application.Run(new Form1());
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
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "Form1";
		}
		#endregion
	}
}
