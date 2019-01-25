using System;
using System.Collections.Generic;

using Bart.Plugins;

namespace LRSDLL_1 {
	public class Plugin_Wingate : MarshalByRefObject, ICcLeads {

		string	DbConnectionString;
		int		EventID;

#if false	// Demo code to show how to put a property on a plug-in

//---------------------------------------------------------------------------------------

		int		_foo;
		public int foo { 
			get { return _foo; } 
			set {_foo = value; } 
		}
#endif

//---------------------------------------------------------------------------------------

		public Plugin_Wingate(string DbConnectionString, int EventID) {
			this.DbConnectionString = DbConnectionString;
			this.EventID = EventID;
		}

//---------------------------------------------------------------------------------------

		#region ICcLeads Members

		public void Forward(string DbConnectionString, int EventID) {
			DoForward(DbConnectionString, EventID);
		}

//---------------------------------------------------------------------------------------

		public void Forward() {
			DoForward(DbConnectionString, EventID);
		}

		#endregion

//---------------------------------------------------------------------------------------

		private void DoForward(string DbConnectionString, int EventID) {
			Console.WriteLine("xxx{0} -- Importing EventID {1} from LRSDLL_1 to database {2}", "", EventID, DbConnectionString);
		}
	}
}
