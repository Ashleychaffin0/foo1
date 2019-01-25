using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

// Add references to 
//	Microsoft.SqlServer.ConnectionInfo
//	Microsoft.SqlServer.Smo

// using Microsoft.SqlServer;
// using Microsoft.SqlServer.Management.Smo.Agent;

// Also, see the following projects for stuff a bit too long to be included here:
//	*	DBSchema, for database analysis, also automating Visio
//	*	StartStopSqlServer
//	*	Create NTFS Stream
//	*	VC\GetAttributes

#if false		// Just a place to put WCT struct, for use in another program
typedef struct _WAITCHAIN_NODE_INFO {  
	WCT_OBJECT_TYPE ObjectType;  
	WCT_OBJECT_STATUS ObjectStatus;  
	union {    
		struct {      
			WCHAR ObjectName[WCT_OBJNAME_LENGTH];      
			LARGE_INTEGER Timeout;      
			BOOL Alertable;    
		} LockObject;    
		struct {      
			DWORD ProcessId;      
			DWORD ThreadId;      
			DWORD WaitTime;      
			DWORD ContextSwitches;    
		} ThreadObject;  
	};
} WAITCHAIN_NODE_INFO,  *PWAITCHAIN_NODE_INFO;
#endif


namespace __Little.NET_Techniques {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			var LRS = new LRSTests();
			LRS.Test();
		}
	}

//---------------------------------------------------------------------------------------

	public class LRSTests {

		internal void Test() {
			// TestSMO();

			// TestShowPlanAll();

			// TestSortingWithAnonymousDelegates();

			// Test_MakeRandomDenseList();

			ReflectionAndQueryExpressions();
		}

//---------------------------------------------------------------------------------------

		internal void Test_MakeRandomDenseList() {
			Random	rnd = new Random(); 
			var nums = Enumerable.Range(1, 25).OrderBy(p => rnd.Next());

			// Format it
			string	sep = "";
			foreach (var num in nums) {
				Console.Write("{0}{1}", sep, num);
				sep = ", ";
			}
			Console.WriteLine();
		}

//---------------------------------------------------------------------------------------

		private static void ReflectionAndQueryExpressions() {
			// Note: This doesn't quite work yet...
			var asm = Assembly.GetExecutingAssembly();

			var q1 = (from type in asm.GetTypes()
					  where type.UnderlyingSystemType.Name == "LRSTests"
					  select type).ToList();

			// Small sample of List<>.ForEach
			string sep2 = "";
			q1.ForEach(p => { Console.Write("{0}{1}", sep2, p.UnderlyingSystemType.Name); sep2 = ", "; });
			Console.WriteLine("*");

			Type t = q1[0];
			// MethodInfo [] LRSTestMethods = q1[0].GetMethods(BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo[] LRSTestMethods = q1[0].GetMethods();

			var q2 = from mi in LRSTestMethods
					 select new { name = mi.Name };
		}

//---------------------------------------------------------------------------------------

		internal void TestSMO() {
			ServerConnection srvc = new ServerConnection();
			SqlConnectionStringBuilder bld = new SqlConnectionStringBuilder();
			bld.DataSource = "75.126.77.59,1092";
			bld.UserID = "sa";
			bld.Password = "$yclahtw2007bycnmhd!";
			srvc.ConnectionString = bld.ConnectionString;
			// srvc.Connect();
			Server srv = new Server(srvc);
			DatabaseCollection dbs = srv.Databases;
			for (int i = 0; i < dbs.Count; i++) {
				Console.WriteLine("{0} - {1}", i, dbs[i].Name);
			}
		}

//---------------------------------------------------------------------------------------

		internal void TestShowPlanAll() {
			SqlConnectionStringBuilder builder = SelectDB();
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				conn.Open();
				string SQL = "SET SHOWPLAN_ALL ON";
				SqlCommand cmd = new SqlCommand(SQL, conn);
				int n = cmd.ExecuteNonQuery();

				SQL = "SELECT * from tblSwipes WHERE SwipeID=7489848";
				cmd = new SqlCommand(SQL, conn);
				SqlDataReader rdr = cmd.ExecuteReader();
				object[] vals = new object[rdr.VisibleFieldCount];
				while (rdr.Read()) {
					for (int i = 0; i < rdr.VisibleFieldCount; i++) {
						Console.WriteLine("{0}: {1} - {2}", i, rdr.GetName(i), rdr[i]);
					}
					Console.WriteLine("\n");
					// n = rdr.GetValues(vals);
				}
				rdr.Close();
			}
		}

//---------------------------------------------------------------------------------------

		class fooSort {
			public string name;
			public int age;

			public fooSort(string name, int age) {
				this.name = name;
				this.age = age;
			}
		}

//---------------------------------------------------------------------------------------

		internal void TestSortingWithAnonymousDelegates() {
			fooSort[] foos = new fooSort[3];
			foos[0] = new fooSort("John", 21);
			foos[1] = new fooSort("Fred", 31);
			foos[2] = new fooSort("Shirley", 41);

			Console.WriteLine("Original data");
			for (int i = 0; i < foos.Length; i++) {
				Console.WriteLine("{0}: {1}, age {2}", i, foos[i].name, foos[i].age);
			}

			Array.Sort(foos, delegate(fooSort x, fooSort y) { return x.name.CompareTo(y.name); });
			Console.WriteLine("Sorted by name");
			for (int i = 0; i < foos.Length; i++) {
				Console.WriteLine("{0}: {1}, age {2}", i, foos[i].name, foos[i].age);
			}

			Array.Sort(foos, delegate(fooSort x, fooSort y) { return x.age.CompareTo(y.age); });
			Console.WriteLine("Sorted by age");
			for (int i = 0; i < foos.Length; i++) {
				Console.WriteLine("{0}: {1}, age {2}", i, foos[i].name, foos[i].age);
			}

			// And now, with lambdas

			Array.Sort(foos, (x, y) => y.name.CompareTo(x.name));
			Console.WriteLine("Sorted by name, descending, using a lambda expression");
			for (int i = 0; i < foos.Length; i++) {
				Console.WriteLine("{0}: {1}, age {2}", i, foos[i].name, foos[i].age);
			}

			// With query language
			var q1 = from foo in foos
					 orderby foo.age descending
					 select foo;
			Console.WriteLine("Sorted by age, descending, using query language");
			foreach (var foo in q1) {
				Console.WriteLine("{0}, age {1}", foo.name, foo.age);
			}

			// And extension methods
			var q2 = foos.OrderByDescending(x => x.name);
			Console.WriteLine("Sorted by name, descending, using extenstion methods");
			foreach (var foo in q2) {
				Console.WriteLine("{0}, age {1}", foo.name, foo.age);
			}
		}

//---------------------------------------------------------------------------------------

		private static SqlConnectionStringBuilder SelectDB() {
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
#if NEWSERVER
			builder.DataSource = "75.126.77.59,1092";
			builder.InitialCatalog = "LeadsLightning";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
#else
			builder.DataSource = "SQLB5.webcontrolcenter.com";
			builder.InitialCatalog = "LLDevel";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
#endif
			return builder;
		}
	}
}