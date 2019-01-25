// Copyright (c) 2005-2006 by Larry Smith

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Net;

namespace Web.Utils {
    class WebIO {

//---------------------------------------------------------------------------------------

        public static string Load(string Url, out bool LoadedOK) {
            LoadedOK = true;
            if (Url.ToLower().StartsWith("http://") || (Url.ToLower().StartsWith("https://"))) {
                // Do nothing
            } else {
                Url = "http://" + Url;
            }
            try {
                WebRequest wreq = WebRequest.Create(Url);
                WebResponse wresp = wreq.GetResponse();
                // Console.WriteLine(wresp.StatusDescription);	// Display the status.
                // Get the stream containing content returned by the server.
                Stream dataStream = wresp.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                wresp.Close();
                return responseFromServer;
#if true
            } catch (Exception ex) {			// Ignore any problems, other than setting LoadedOK
#else
			} catch (Exception ex) {
				string	msg = "Unexpected error in RSSFeed.Load(\"{0}\") -- {1}";
				msg = string.Format(msg, Url, ex.Message);
				MessageBox.Show(msg, "LRSS Internal Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
                LoadedOK = false;
                Trace.WriteLine("RSSFeed.Load(URL) exception on " + Url + " (" + ex.Message + ")");
                return null;
            }
        }
    }
}
