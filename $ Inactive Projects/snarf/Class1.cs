using System;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Specialized;

namespace Snarf {

	internal class MyPolicy : ICertificatePolicy {
		public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate cert, WebRequest request, int problem) {
			return true;
		}
}

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Snarf {

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args) {
			string		remoteUri;

			switch (args.Length) {
#if DEBUG
			case 0:
				// TODO: Remove after testing
				remoteUri = "https://ny.osos.ajb.org/osos_globals.js";
				// remoteUri = "https://ny.osos.ajb.org/domains/job_status.xml";
				// remoteUri = "https://ny.osos.ajb.org/osos.html";
				//				remoteUri = "http://www.bartizan.com";
				break;
#endif
			case 1:
				remoteUri  = args[0];
				break;
			default:
				Console.WriteLine("Usage: Snarf uri");
				return 1;
			}

			if (remoteUri.StartsWith("http://") ||
				remoteUri.StartsWith("https://") ||
				remoteUri.StartsWith("file://")) {
				// Do nothing
			} else {
				remoteUri = "http://" + remoteUri;
			}

			HttpWebRequest	hreq;
			HttpWebResponse	hresp;

			ServicePointManager.CertificatePolicy = new MyPolicy();

			hreq = (HttpWebRequest)WebRequest.Create(remoteUri);

#if false
			NetworkCredential	netcred = new NetworkCredential();
			NetworkCredential	netcred2;
			Uri		uri = new Uri(remoteUri);
			string	type = "https";
			netcred2 = netcred.GetCredential(uri, type);

			X509Certificate	x509;
			x509 = X509Certificate.CreateFromCertFile(@"c:\lrs\snakeoil.cer");
			hreq.ClientCertificates.Add(x509);
			hresp = (HttpWebResponse)hreq.GetResponse();

			X509CertificateCollection	x509_coll;
			x509_coll = hreq.ClientCertificates;
			foreach (X509Certificate x in x509_coll) {
				Console.WriteLine("Certificate - " + x.GetName());
			}

			WebHeaderCollection	hdrs;
			hdrs = hreq.Headers;
			Console.WriteLine("Hdrs count = " + hdrs.Count);
			foreach (NameObjectCollectionBase nm in hdrs) {
				Console.WriteLine("Hdr = " + nm.Keys[0] + " = " + nm.ToString());
			}
#endif

			try {
				// Create a new WebClient instance.
				WebClient myWebClient = new WebClient();
				// Download home page data.
				Console.Error.WriteLine("<!-- Snarf: Downloading " + remoteUri + "-->");                  
				// Download the Web resource and save it into a data buffer.
				byte[] myDataBuffer = myWebClient.DownloadData (remoteUri);

				// Display the downloaded data.
				string download = Encoding.ASCII.GetString(myDataBuffer);
				Console.WriteLine(download);
			} catch (WebException e) {
				string	msg = "Error connecting to " + remoteUri + ", error = " + e.Message;
				Console.WriteLine(msg);
				Console.Error.WriteLine(msg);
				
			}

			return 0;
		}
	}
}
