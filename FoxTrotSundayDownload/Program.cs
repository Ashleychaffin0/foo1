using System;
using System.Collections.Generic;
using System.Net;

namespace FoxTrotSundayDownload {
	class Program {
		static void Main(string[] args) {
			var fox = new FoxTrot();
			fox.Download("https://foxtrot.com/2011/07/10/in-character/");
		}
	}
}
