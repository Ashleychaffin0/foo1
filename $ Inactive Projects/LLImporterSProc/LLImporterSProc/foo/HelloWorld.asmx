<%@ WebService Language="C#" Class="HelloWorld" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

[WebService(Namespace = "http://63.134.235.22/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class HelloWorld  : System.Web.Services.WebService {

    [WebMethod]
    public string HelloWorld1() {
        return "Hello World!!!";
    }
    
}

