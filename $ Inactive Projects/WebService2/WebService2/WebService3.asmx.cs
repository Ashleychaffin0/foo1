using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace WebService2 {
	/// <summary>
	/// Summary description for WebService3
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	public class WebService3 : System.Web.Services.WebService {

		[WebMethod]
		public string HelloWorld_3() {
			return "Hello World";
		}
	}
}
