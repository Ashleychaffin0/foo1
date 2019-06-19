using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nsBoondog2019 {
	public class BoondogForm {
		public static Font SqFont;
		public PlayerProfile ProgramProf;
		public PlayerProfile PlayerProf;

//---------------------------------------------------------------------------------------

		public BoondogForm() {
			ProgramProf = new PlayerProfile();
			PlayerProf  = new PlayerProfile();
		}

//---------------------------------------------------------------------------------------

			// Note: Just a sample method I picked up at some point. Not used.
		public static async Task DownloadAsync(Uri requestUri, string filename) {
			using (var httpClient = new HttpClient()) {
				// TODO: <using> allows multiple clauses. Thus we need 4 of them(!)
				using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri)) {
					using (
						Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(),
						stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 64 * 1024, true)) {
						await contentStream.CopyToAsync(stream);
					}
				}
			}
		}
	}
}

//---------------------------------------------------------------------------------------

namespace LRSUtils {

}
