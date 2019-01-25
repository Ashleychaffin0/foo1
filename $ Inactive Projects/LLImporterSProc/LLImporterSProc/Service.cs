// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;
using System.Collections.Generic;

namespace Bartizan.Importer {
	/// <summary>
	/// Summary description for Service.
	/// </summary>
	public class Service {
		public string	service;

//---------------------------------------------------------------------------------------

		public Service(string service) {
			this.service = service;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// A static member to read records from a SetupFile (as opposed to, say, an
		/// XML file), and to return a List<Service> of all the Services.
		/// <p/>
		/// Note: The Services must be in the List<Service> in the same order they are
		///		  in the SetupFile, since we reference them by their order. Thus,
		///		  for example, we must use an ordered array (of some sort), rather
		///		  than, say, a Hashtable.
		///	<p/>
		/// Note: If we ever change the input file into something other than
		///       a SetupFile (e.g. something XML based), then just implement
		///       another static member.
		/// </summary>
		/// <param name="suf"></param>
        public static List<Service> ParseSetupFile(BartIniFile suf) {
			List<Service> Services = new List<Service>();
			// Note: For historical reasons, we call this section "Questions",
			// not Services. If necessary, I suppose we could pass the section
			// name as a parameter to this routine.
			if (suf.FindSection("Questions") == false)
				return Services;			// i.e. Empty array
			string	s;
			while ((s = suf.ReadLine()) != null) {
				s = s.Trim();
				if (s.Length != 0) {
					Services.Add(new Service(s));
				} // else we've got an empty line, which we'll ignore
			}
			return Services;
		}
	}
}
