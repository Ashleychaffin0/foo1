using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Printing;
// The following has nothing to do with BartCarte, but it's a reminder to look
// into this namespace to see if there's anything relevant to us.
// using System.Drawing.Design;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting;
using System.Windows.Forms;
using System.Text;


namespace nsDemoBartCarte {
	/// <summary>
	/// Summary description for BartCarteDemoPrinter.
	/// </summary>
	public class BartCarteDemoPrinter : MarshalByRefObject {

		// PrintDocument	pd;

		string	SSN, FirstName, LastName;

		public BartCarteDemoPrinter() {
#if false
			string	msg;
			string	HostName = Dns.GetHostName();
			IPHostEntry ipEntry = Dns.GetHostByName (HostName);
			IPAddress [] addr = ipEntry.AddressList;
			msg = string.Format("BartCarteDemoPrinter constructor entered - HostName='{0}', IPAddr='{1}'", HostName, addr[0].ToString());
			MessageBox.Show(msg, "BartCarteDemoPrinter ctor");
#endif

			Assembly	asm = Assembly.GetEntryAssembly();
			MessageBox.Show("Entry Assembly CodeBase = " + asm.CodeBase);
			MessageBox.Show("Entry Assembly FullName = " + asm.FullName);
			MessageBox.Show("Entry Assembly GetName().FullName = " + asm.GetName().FullName);
		}

		public Hashtable PrintSSN(string SSN, string FirstName, string LastName) {
			string	msg;
			string	HostName = Dns.GetHostName();
			IPHostEntry ipEntry = Dns.GetHostByName (HostName);
			IPAddress [] addr = ipEntry.AddressList;
			// msg = string.Format("HostName='{0}', IPAddr='{1}', SSN={2}", HostName, addr[0].ToString(), SSN);

			this.SSN		= SSN;
			this.FirstName	= FirstName;
			this.LastName	= LastName;

#if false
			PrintDocument pd = new PrintDocument();
			pd.DocumentName = "BartCarte";
			PrinterSettings.StringCollection	printers = PrinterSettings.InstalledPrinters;	// For later
			//pd.PrinterSettings.PrinterName = @"\\LRS-P4-1\LRS-940";
			// pd.PrinterSettings.PrinterName = @"\\LRS-P4-1\LRS-DTC510_5";
			pd.PrinterSettings.PrinterName = "DTC510_515 Card Printer";
			pd.PrinterSettings.PrintToFile = true;
			PageSettings ps = pd.PrinterSettings.DefaultPageSettings;
			// pd.PrintController = new PreviewPrintController();
			// PrintPreviewControl	pvc = new PrintPreviewControl();
			// PrintPreviewDialog	pvd = new PrintPreviewDialog();
			// pvd.Document = pd;
			pd.PrintPage += new
				System.Drawing.Printing.PrintPageEventHandler
				(this.printDocument1_PrintPage);
			try {
				pd.Print();
				// pvd.ShowDialog();
			} catch (Exception ex) {
				MessageBox.Show("Exception in pd.Print() - " + ex.Message);
			}
#endif

			msg = "We've just (virutally) created a card for SSN " + SSN;
			// MessageBox.Show(msg);
			Hashtable	ht = new Hashtable();
			ht["CUST_SSN"] = SSN;
			ht["CUSTDTL1_PH"] = "(914) 555-1212";
			ht["CUSTDTL1_ALT"] = "(914) 555-9876";
			return ht;
		}

		private void printDocument1_PrintPage(object sender, 
			System.Drawing.Printing.PrintPageEventArgs e) {
			Font	norm = new Font("Arial", 16);
#if false
			Font	bold = new Font("Helvetica", 16, FontStyle.Bold);
			e.Graphics.DrawString("Bartizan One Stop", new Font("Arial", 32, FontStyle.Italic), Brushes.PowderBlue, 300, 0);

			e.Graphics.DrawString("SSN",		norm, Brushes.Black, 50, 125);
			e.Graphics.DrawString(SSN,			bold, Brushes.Red,	250, 125);
			e.Graphics.DrawString("First Name", norm, Brushes.Black, 50, 175);
			e.Graphics.DrawString(FirstName,	bold, Brushes.Red,	250, 175);
			e.Graphics.DrawString("Last Name",	norm, Brushes.Black, 50, 225);
			e.Graphics.DrawString(LastName,		bold, Brushes.Red,	250, 225);
#else
			string	line1, line2, line3, line;
			line1 = "~1%LARRY^SMITH^914555-1212?";
			line2 = "~2%HOW^NOW^BROWN^COW?";
			line3 = "~3%HOT^DOG^IT^WORKED?";
//			line1 = FmtStripeData('1', "LARRY^SMITH^914555-1212");
//			line2 = FmtStripeData('2', "HOW^NOW^BROWN^COW");
//			line3 = FmtStripeData('3', "HOT^DOG^IT^WORKED");
			// TODO: 
			// e2 = ""; line3 = "";
			line = line2 + line1 + line3;
			line = "~1%LARRY^SMITH^914555-1212? ~2;HOW=NOW=BROWN=COW? ~3;HOT=DOG=IT=WORKED?";
			string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			Bitmap	bm = new Bitmap(Image.FromFile(dir + @"\Jersey Shore - Sept 20, 2003\Jersey Shore - Sept 20, 2003 007.jpg"));
			GraphicsUnit	gu = GraphicsUnit.Pixel;
			RectangleF	rf1 = bm.GetBounds(ref gu);
			Rectangle	r1 = e.PageBounds;
			int			width = bm.Width, height = bm.Height;
			r1.Height = (int)((float)r1.Height / 1.5);
			e.Graphics.DrawImage(bm, r1);
			e.Graphics.DrawString(line1, norm, Brushes.Black, 0, 0);
			e.Graphics.DrawString(line2, norm, Brushes.Black, 0, 0);
			e.Graphics.DrawString(line3, norm, Brushes.Black, 0, 0);
#endif
		}

		string FmtStripeData(char stripe, string data) {
			StringBuilder	sb = new StringBuilder();
			sb.AppendFormat("{0}{1}{2}M", '\x1b', '\x00', '\x02'); 
			sb.Append(stripe);
			sb.Append(data);
			sb.Append('\x00');
			byte []	bytes = new byte[sb.Length];
			bytes = Encoding.ASCII.GetBytes(sb.ToString());
			return sb.ToString();
		}

		~BartCarteDemoPrinter() {
#if false
			MessageBox.Show("BartCarteDemoPrinter dtor called", "BartCarteDemoPrinter destructor");
#endif
		}
	}
}
