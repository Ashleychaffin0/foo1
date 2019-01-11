using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// See https://www.codeproject.com/Articles/1217887/Getting-started-with-Selenium-using-Visual-Stu

namespace ShopRiteCoupons_V2
{
	public partial class ShopRiteCoupons_V2 : Form
	{
		public ShopRiteCoupons_V2() {
			InitializeComponent();
		}

#if false   // TODO: Delete
		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(String sClassName, String sAppName);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		static extern bool PostMessage(IntPtr /* HandleRef */ hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		// SHDocVw.WebBrowser web2;
#endif
		SHDocVw.InternetExplorer IE;

		string UserID;
		string Password;

		int CurPage;

		List<Coupon> Coupons;

		static string CouponDiscount = @"^Save \$\d+\.\d+ on ";
		Regex re = new Regex(CouponDiscount, RegexOptions.Compiled);

		WebClient wc;

//---------------------------------------------------------------------------------------

		private void ShopRiteCoupons_Load(object sender, EventArgs e) {
			IE = new SHDocVw.InternetExplorer() {
				Visible = true
			};
			IE.DocumentComplete += IE_DocumentComplete;

			PositionWindows();

			wc = new WebClient();

			cmbCardOwner.SelectedIndex = 0;
		}

//---------------------------------------------------------------------------------------

		private void IE_DocumentComplete(object pDisp, ref object URL) {
			if (IE.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) {
				return;
			}

			Console.WriteLine($"Doc Complete. URL={URL}");

			var doc = IE.Document as IHTMLDocument3;
			switch (URL.ToString()) {
				case "https://secure.shoprite.com/User/SignIn/3601?redirect=Authenticate":
					// TODO: Do UserID/Password
					var ids = doc.getElementById("Email");
					ids.innerText = UserID;
					ids = doc.getElementById("Password");
					ids.innerText = Password;
					var Inputs = doc.getElementsByTagName("input");
					for (int i = 0; i < Inputs.length; i++) {
						var inp = Inputs.item(i) as IHTMLElement;
						var attr = (string)inp.getAttribute("value");
						if (attr == "Sign In") {
							inp.click();
							return;
						}
					}
					break;
				case "http://coupons.shoprite.com/main.html":
					ProcessPage();
					break;
				case "https://secure.shoprite.com/Error/3601":
					// IE.Navigate("http://www.shoprite.com");
					IE.Navigate("http://coupons.shoprite.com");
					break;
				default:
					break;
			}
		}

//---------------------------------------------------------------------------------------

		private void cmbCardOwner_SelectedIndexChanged(object sender, EventArgs e) {
			switch (cmbCardOwner.SelectedItem.ToString()) {
				case "LRS":
					UserID = "lrs5@lrs5.net";
					Password = "lrs_5_ShopRite";
					break;
				case "BGA":
					UserID = "bga101@optonline.net";
					Password = "bga_5_ShopRite";
					break;
			}

			IE.Navigate("https://secure.shoprite.com/User/SignIn/3601?redirect=Authenticate");
		}

//---------------------------------------------------------------------------------------

		private void PositionWindows() {
			Rectangle rect = Screen.PrimaryScreen.Bounds;
			this.Top = 0;
			this.Left = 0;
			this.Width = rect.Width;

			IE.Top = this.Height;
			IE.Left = 0;
			IE.Height = rect.Height - this.Height;
			IE.Width = rect.Width;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			CurPage = 0;
			Coupons = new List<Coupon>();
			ProcessPage();
			WriteOutDeals();
			MessageBox.Show("Done");
		}

//---------------------------------------------------------------------------------------

		private void WriteOutDeals() {
			string FileName = @"G:\LRS\ShopRiteCoupons.html";
			using (var sw = new StreamWriter(FileName, false, new System.Text.UTF8Encoding())) {
				sw.WriteLine("<HTML>");
				sw.WriteLine("<HEAD>");
				sw.WriteLine("\t<TITLE>ShopRite Coupons</TITLE>");
				sw.WriteLine(@"\t<meta http-equiv=""Content - Type"" content=""text / html; charset = utf - 8"" />");
				sw.WriteLine("</HEAD>");
				sw.WriteLine("<BODY>");
				sw.WriteLine("<TABLE>");

				Coupons.Sort((first, second) => StripDiscount(first.Title).CompareTo(StripDiscount(second.Title)));
				foreach (var coup in Coupons) {
					sw.WriteLine("\t<TR>");
					// sw.WriteLine($"<img src=\"{coup.ImageLink}\"/> {coup.Title}<br/>");
					sw.WriteLine($"\t\t<TD><img src=\"{coup.ImageLink}\" height=50 width=50></TD>");
					sw.WriteLine($"\t\t<TD valign=\"middle\">{coup.Title}</TD>");
					sw.WriteLine("\t</TR>");
				}

				sw.WriteLine("</BODY>");
				sw.WriteLine("</HTML>");
			}
			Process.Start(FileName);
		}

//---------------------------------------------------------------------------------------

		private string StripDiscount(string s) {
			var m = re.Match(s);
			return s.Substring(m.Length);
		}

//---------------------------------------------------------------------------------------

		private void ProcessPage() {
			bool bMorePages = true;
			var doc = IE.Document as IHTMLDocument2;
			IHTMLWindow2 f = null;
			while (bMorePages) {
				FramesCollection frames = null;
				try {
					frames = doc.frames;
				} catch {
					continue;
				}

				for (int i = 0; i < frames.length; i++) {
					// Go through each frame. (There's probably only one.)
					f = frames.item(i) as IHTMLWindow2;         // The frame
					var fdoc = f.document as IHTMLDocument3;            // Its document
					var fbuts = fdoc.getElementsByTagName("button");    // The buttons
					for (int j = 0; j < fbuts.length; j++) {
						var felem = fbuts.item(j) as IHTMLElement;

						var fParent = felem.parentElement as IHTMLElement2;
						if (fParent == null) {
							continue;
						}
						var f2Kids = fParent.getElementsByTagName("a") as IHTMLElementCollection;
						if (f2Kids.length == 0) {
							continue;
						}

						var f2Images = fParent.getElementsByTagName("img");
						string ImgLink = "";
						for (int l = 0; l < f2Images.length; l++) {
							var ImgElem = f2Images.item(l) as IHTMLImgElement;
							var ImgSrc = ImgElem.src;
							// Kludge city. Dunno why I can't check the "class" attribute
							if (ImgSrc.StartsWith("http://softcoin.com/")) {
								ImgLink = ImgSrc;
								break;
							}
						}

						for (int k = 0; k < f2Kids.length; ++k) {
							var kid = f2Kids.item(k) as IHTMLElement;
							var kidTitle = (string)kid.getAttribute("title");
							var Coup = new Coupon(kidTitle, ImgLink);
							Coupons.Add(Coup);
						}
						felem.click();
					}
				}
				try {
					f.execScript($"onPage({++CurPage})");
				} catch {
					bMorePages = false;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ShopRiteCoupons_FormClosed(object sender, FormClosedEventArgs e) {
			IE.Quit();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class Coupon
	{
		public string Title;
		public string ImageLink;

//---------------------------------------------------------------------------------------

		public Coupon(string Title, string ImageLink) {
			this.Title = Title;
			this.ImageLink = ImageLink;
		}
	}
}

