// Copyright (c) 2007 Bartizan Connects LLC

using System;
using System.Data.SqlClient;

// TODO: It would be nice to generalize this so that the vector of Systems we returned
//		 from GetSystems, could be automatically kept in synch with GetConnectionString.
//		 This could certainly be done, but we'd need delegates or virtual functions 
//		 associated with each System name. Maybe some day. But for now we'll do it
//		 manually.

namespace LL.Database {
	public static class DatabaseConnectionString_Explicit {

//---------------------------------------------------------------------------------------

		public static string[] GetSystemNames() {
			return new string [] {"DBMart", "CrystalTech", "LRS-P4-1"};
		}

//---------------------------------------------------------------------------------------

		public static string GetConnectionString(string System) {
			SqlConnectionStringBuilder bld = new SqlConnectionStringBuilder();
			switch (System) {
			case "DBMart":
				bld.DataSource = "75.126.77.59,1092";
				bld.InitialCatalog = "LeadsLightning";
				bld.UserID = "sa";
				bld.Password = "$yclahtw2007bycnmhd!";
				break;
			case "CrystalTech":
				bld.DataSource = "SQLB5.webcontrolcenter.com";
				bld.InitialCatalog = "LLDevel";
				bld.UserID = "ahmed";
				bld.Password = "i7e9dua$tda@";
				break;
			case "LRS-P4-1":
				bld.DataSource = "(local)";
				bld.InitialCatalog = "LLDevel";
				bld.IntegratedSecurity = true;
				break;
#if false
			case "Ahmed-Local":
				throw new Exception("Ahmed-Local support not yet implemented");
				break;
#endif
			default:
				throw new Exception("Unknown System type - " + System);
			}
			return bld.ConnectionString;
		}
	}
}
