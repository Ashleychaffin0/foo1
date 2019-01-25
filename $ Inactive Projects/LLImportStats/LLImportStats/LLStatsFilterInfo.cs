// Copyright (c) 2007 Bartizan Connects, LLC

using System;
using System.Collections.Generic;
using System.Text;

namespace LLImportStats {
	public class LLStatsFilterInfo  {
		public string		CompanyName;
		public string		EventName;
		public List<string>	RCCompanies;
		public List<string>	EventNames;

		public bool			bIgnoreBartizanTests;

		public DateTime		ImportStartDate, ImportEndDate;

//---------------------------------------------------------------------------------------

		public LLStatsFilterInfo() {
			CompanyName = "";
			EventName	= "";
			RCCompanies = new List<string>();
			EventNames	= new List<string>();

			bIgnoreBartizanTests = true;

			// Next line: 30 days is arbitrary
			ImportStartDate = DateTime.Now - new TimeSpan(30, 0, 0, 0);
			ImportEndDate	= new DateTime(2020, 12, 31);
		}

		// TODO: Maybe put in more ctors with parms, but I'm not yet sure exactly what
		//		 they'll turn out to be. So hang loose for now.

//---------------------------------------------------------------------------------------

		public LLStatsFilterInfo Clone() {
			LLStatsFilterInfo	newInfo = new LLStatsFilterInfo();
			newInfo.CompanyName	= CompanyName;
			newInfo.RCCompanies		= new List<string>(RCCompanies);
			newInfo.EventNames		= new List<string>(EventNames);
			newInfo.ImportStartDate = ImportStartDate;
			newInfo.ImportEndDate	= ImportEndDate;

			return newInfo;
		}
	}
}
