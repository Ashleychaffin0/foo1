// Copyright (c) 2007 Bartizan Connects, LLC

using System;
using System.Collections.Generic;
using System.Text;

namespace LLImportStats {
	public class LLStatsFilterInfo  {
		public string		CompanyFilter;
		public List<string>	RCCompanies;

		public DateTime		ImportStartDate, ImportEndDate;

//---------------------------------------------------------------------------------------

		public LLStatsFilterInfo() {
			RCCompanies = new List<string>();
			CompanyFilter = "";

			// Next line: 30 days is arbitrary
			ImportStartDate = DateTime.Now - new TimeSpan(30, 0, 0, 0);
			ImportEndDate	= DateTime.Now;
		}

		// TODO: Maybe put in more ctors with parms, but I'm not yet sure exactly what
		//		 they'll turn out to be. So hang loose for now.

//---------------------------------------------------------------------------------------

		public LLStatsFilterInfo Clone() {
			LLStatsFilterInfo	newInfo = new LLStatsFilterInfo();
			newInfo.CompanyFilter	= CompanyFilter;
			newInfo.RCCompanies		= new List<string>(RCCompanies);
			newInfo.ImportStartDate = ImportStartDate;
			newInfo.ImportEndDate	= ImportEndDate;

			return newInfo;
		}
	}
}
