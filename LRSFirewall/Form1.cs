using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WindowsFirewallTools;

namespace LRSFirewall {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
#if ! COMPILE_MAIN  
			App.LRSMainTest();
#endif
		}
	}

#if ! COMPILE_MAIN  
 public class App {
  //a sample main application that shows the usage of these objects.
	 public static void LRSMainTest() {
		 Console.WriteLine(new string('*', 80));
   try {

    INetFwMgr mgr = (INetFwMgr)new NetFwMgr();

    Console.WriteLine("CurrentProfileType: " + mgr.CurrentProfileType);

    INetFwProfile profile = mgr.LocalPolicy.CurrentProfile;
    Console.WriteLine("FirewallEnabled: " + profile.FirewallEnabled);


    System.Collections.IEnumerator e = null;

    e = profile.AuthorizedApplications._NewEnum;


      

    Console.WriteLine("\r\n-----  Applications  -----  ");
    while (e.MoveNext()) {
     INetFwAuthorizedApplication app = e.Current as INetFwAuthorizedApplication;
     Console.WriteLine("\t{0}\r\n\t\tImageFilename={1}\r\n\t\tEnabled={2}\r\n\t\tIpVersion={3}\r\n\t\tScope={4}\r\n\t\tRemoteAddresses={5}",
           app.Name,
           app.ProcessImageFileName,
           app.Enabled, 
           app.IpVersion,
           app.Scope,
           app.RemoteAddresses
          );
    }


    e = profile.Services._NewEnum;
    Console.WriteLine("\r\n-----  Services  -----  ");
    while (e.MoveNext()) {
     INetFwService service = e.Current as INetFwService;
     Console.WriteLine("\t{0}\r\n\t\tType={1}\r\n\t\tEnabled={2}\r\n\t\tIpVersion={3}"+
           "\r\n\t\tScope={4}\r\n\t\tcustomized={5}\r\n\t\tRemoteAddresses={6}",
           service.Name,
           service.Type,
           service.Enabled, 
           service.IpVersion,
           service.Scope,
           service.Customized,
           service.RemoteAddresses
          );
    }

    e = profile.GloballyOpenPorts._NewEnum;
    Console.WriteLine("\r\n-----  Globally Open Ports  -----  ");
    while (e.MoveNext()) {
     INetFwOpenPort port = e.Current as INetFwOpenPort;
     Console.WriteLine("\t{0}\r\n\t\tIsBuiltIn={1}\r\n\t\tEnabled={2}\r\n\t\tIpVersion={3}"+
           "\r\n\t\tScope={4}\r\n\t\tProtocol={5}\r\n\t\tRemoteAddresses={6}",
           port.Name,
           port.BuiltIn,
           port.Enabled, 
           port.IpVersion,
           port.Scope,
           port.Protocol,
           port.RemoteAddresses
          );
    }


   } catch (Exception ex) {
    Console.WriteLine(ex);
   }
  }
 }

#endif //COMPILE_MAIN

}
