using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWholeFoodsDeliveryWindow {

	public partial class Form1 : Form {
		string Url = "http://www.amazon.com/gp/buy/shipoptionselect/handlers/display.html?hasWorkingJavascript=1";
		string DivClass = "ufss-date-select-toggle-text-day-of-week";

//---------------------------------------------------------------------------------------

		public Form1() {
			InitializeComponent();

			ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
			var wc = new WebClient();
			// ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
			ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
			// wc.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.15) Gecko/20110303 Firefox/3.6.15";
			string html = wc.DownloadString(Url);
			int ix = html.IndexOf(DivClass);
		}

	}
}
