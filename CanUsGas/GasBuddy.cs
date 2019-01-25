using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

// http://www.mygasfeed.com/keys/api

namespace CanUsGas {
	class GasBuddy {
		public static Object GetBrands() {
			var wc = new WebClient();
			// TODO: Don't use developer ID (rfej9napna) in next line
			var RawJson = wc.DownloadData("http://devapi.mygasfeed.com//stations/brands/rfej9napna.json");
			var s = ASCIIEncoding.ASCII.GetString(RawJson);


			var ww = JsonConvert.DeserializeObject(s);

			return (Object)ww;

		}
	}
}
