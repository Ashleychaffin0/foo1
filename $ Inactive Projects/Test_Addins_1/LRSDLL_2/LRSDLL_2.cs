using System;
using System.Collections.Generic;
using System.Reflection;

using Bart.Plugins;

namespace LRSDLL_2 {
	public class LRSDLL_2 : MarshalByRefObject, ICcLeads {

		string DbConnectionString;
		int EventID;

//---------------------------------------------------------------------------------------

		int _foo;
		public int foo {
			get { return _foo; }
			set { _foo = value; }
		}

//---------------------------------------------------------------------------------------

		public LRSDLL_2(string DbConnectionString, int EventID) {
			this.DbConnectionString = DbConnectionString;
			this.EventID = EventID;
		}

		#region ICcLeads Members

//---------------------------------------------------------------------------------------

		public void Forward(string DbConnectionString, int EventID) {
			DoForward(DbConnectionString, EventID);
		}

//---------------------------------------------------------------------------------------

		public void Forward() {
			DoForward(DbConnectionString, EventID);
		}

//---------------------------------------------------------------------------------------

		private void DoForward(string DbConnectionString, int EventID) {
			Console.WriteLine("Base Directory = {0}", AppDomain.CurrentDomain.BaseDirectory);
			Console.WriteLine("Importing EventID {0} from LRSDLL_2 to database {1}", EventID, DbConnectionString);
		}

		#endregion
	}
}
