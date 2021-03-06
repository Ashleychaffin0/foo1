// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;

namespace Bartizan.ActivityTrack30.Common {
	/// <summary>
	/// Summary description for Service.
	/// </summary>
	public class Service {
		string	service;

//---------------------------------------------------------------------------------------

		public Service(string service) {
			this.service = service;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// A static member to read records from a SetupFile (as opposed to, say, an
		/// XML file), and to return an ArrayList of all the Services.
		/// <p/>
		/// Note: The Services must be in the ArrayList in the same order they are
		///		  in the SetupFile, since we reference them by their order. Thus,
		///		  for example, we must use an ordered array (of some sort), rather
		///		  than, say, a Hashtable.
		///	<p/>
		/// Note: If we ever change the input file into something other than
		///       a SetupFile (e.g. something XML based), then just implement
		///       another static member.
		/// </summary>
		/// <param name="suf"></param>
		public static ArrayList ParseSetupFile(SetupFile suf) {
			ArrayList	Services = new ArrayList();
			// Note: For historical reasons, we call this section "Questions",
			// not Services. If necessary, I suppose we could pass the section
			// name as a parameter to this routine.
			if (suf.FindSection("Questions") == false)
				return Services;			// i.e. Empty array
			string	s;
			while ((s = suf.ReadLine()) != null) {
				s = s.Trim();
				if (s.Length != 0) {
					Services.Add(s);
				} // else we've got an empty line, which we'll ignore
			}
			return Services;
		}
	}
}
