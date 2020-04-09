using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// http://video.ch9.ms/ch9/2c0e/4ecb63f3-8e15-4175-9a0a-f3d270b52c0e/P4187_high.mp4

namespace TestHttpClient {
	public partial class TestHttpClient : Form {
		public TestHttpClient() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			string Address = "http://video.ch9.ms/ch9/2c0e/4ecb63f3-8e15-4175-9a0a-f3d270b52c0e/P4187_high.mp4";

			var wc = new WebClient();
			var strm = wc.OpenRead(Address);
			long len = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);

			// wc.DownloadFile(Address, "foo.mp4");

			// var mhc = new MyHttpClient();
			// var TBuf = mhc.DownloadStringAsync(Address);
			// var buf = TBuf.Result;
			var buf = new byte[4096];
			Task<int> x = strm.ReadAsync(buf, 0, buf.Length);
			var y = x.Result;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class MyHttpClient : HttpClient {
		public MyHttpClient() : base() {
			// TODO: ?
		}

//---------------------------------------------------------------------------------------

		public MyHttpClient(HttpMessageHandler Handler) : base(Handler) {
			// TODO: ?
		}

		//---------------------------------------------------------------------------------------

		public MyHttpClient(HttpMessageHandler Handler, bool bDisposeHandler) : base(Handler, bDisposeHandler) {
			// TODO: ?
		}

		public async Task<byte[]> DownloadStringAsync(string Url) {
		// public byte[] DownloadStringAsync(string Url) {
			var uri = new Uri(Url);
			// var stream = await GetStreamAsync(uri);
			// var stream = await GetAsync(uri);
			// var stream = await GetAsync(uri);
			// HttpResponseMessage response = GetByteArrayAsync
			await GetAsync(uri)
				.ContinueWith(
					requestTask => {
						HttpResponseMessage resp = requestTask.Result;
						System.Diagnostics.Debugger.Break();
					});

			// var hdrs = stream.Headers;
			// bool bCanSeek = stream.CanSeek;
			// var len = stream.Length;
			// var pos = stream.Position;
			var buf = new byte[4096];
			// int nBytesRead = await stream.ReadAsync(buf, 0, buf.Length);
			return buf;
		}
	}
}
