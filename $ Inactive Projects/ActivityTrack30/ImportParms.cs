// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Bartizan.ActivityTrack30.Common {
	public class ImportParms {

		public string		TerminalName;
		public ArrayList	Services;
		public ArrayList	file;
		public ArrayList	Surveys;

		public const string UnknownTerminalName = "*Unknown*";

//---------------------------------------------------------------------------------------

		public ImportParms() {
			TerminalName = UnknownTerminalName;
			file		 = new ArrayList();

			// Note: We don't need to set defaults for Services or Surveys. The
			//		 Read_xxx_Section routines for these will return (possibly empty)
			//		 ArrayLists for us.
		}

//---------------------------------------------------------------------------------------

		public void LoadFromSetupFile(SetupFile suf) {

			// Yeah, I admit it. We're being inconsistent here. In some routines below
			// we process the data ourselves, and in others, we have the class load 
			// itself. The dividing line is whether we really need a class for, say,
			// the [FILE] section. Which we don't, because the data is a simple vector. 
			// Least of all is [TERMINAL]. OTOH, there's a bit more structure to the 
			// Survey class, so the ReadSurveySection routine below will invoke 
			// a static method of that class to load itself.

			ReadTerminalSection(suf);

			ReadServicesSection(suf);

			// SECTION - LABELS
			// Uh, actually, we don't seem to use these any more. So leave them alone.

			ReadFileSection(suf);

			ReadSurveySection(suf);
		}

//---------------------------------------------------------------------------------------

		void ReadTerminalSection(SetupFile suf) {
			string		s;

			// The TerminalName has been set in the ctor to be UnknownTerminalName. So
			// if any of our checks fail, we can just return.
			if (! suf.FindSection("Terminal"))
				return;
 			if ((s = suf.ReadLine()) == null)
				return;

			TerminalName = s.Trim();

			// To show how regular expressions can really, really help in certain
			// situations (like this one), here's the (#if'd out) code to parse the
			// terminal name using traditional techniques, then the same result
			// achieved using re's. You decide;
#if false
			if (TerminalName.Length == 0)
				TerminalName = UnknownTerminalName;
			else {
				// Now it gets really boring. We expect ID=TerminalName.
				// But error check to make sure that a) we found an equals
				// sign, and that there was indeed something to its right.
				// Note: A regular expression here might be a bit simpler.
				char [] seps = {'='};
				string [] parts = TerminalName.Split(seps, 2);
				if (parts.Length == 1)		// Check if no "=" found
					TerminalName = UnknownTerminalName;
				else {
					TerminalName = parts[1].Trim();
					if (TerminalName.Length == 0) 
						TerminalName = UnknownTerminalName;
				}
			}
#else
			// All we're really looking for is something on the RHS of an '='. We don't
			// really care about what's on the LHS. 
			Regex	re = new Regex(@"^.+?=(?<TermName>.+)$");
			Match	m  = re.Match(TerminalName);
			if (m.Success)
				TerminalName = (string)m.Groups["TermName"].Captures[0].Value;
			else
				TerminalName = UnknownTerminalName;
#endif
		}
				
			// Ignore subsequent lines, if any. Note however that we could keep scanning
			// until we find a non-empty line, and use that as the TerminalName. 

//---------------------------------------------------------------------------------------

		void ReadServicesSection(SetupFile suf) {
			Services = Service.ParseSetupFile(suf);	
		}

//---------------------------------------------------------------------------------------

		void ReadFileSection(SetupFile suf) {
			string		s;

			// The <file> ArrayList has be set to empty in the ctor. So if any of our
			// checks fail, just return.
			if (! suf.FindSection("File"))
				return;				// Already set to empty ArrayList
			if ((s = suf.ReadLine()) == null)
				return;				// Ditto

			// TODO: Not sure what happens if multiple blanks. May need regex
			file = new ArrayList(s.Split(' '));
		}

//---------------------------------------------------------------------------------------

		void ReadSurveySection(SetupFile suf) {
			Surveys = Survey.ParseSetupFile(suf);

#if true		// TODO: Debugging
			Survey	sur;
			for (int i=0; i<Surveys.Count; ++i) {
				sur = ((Survey)Surveys[i]);
				Console.WriteLine("Survey[{0}] Question = {1}", i, sur.Question);
				for (int j=0; j<sur.Answers.Count; ++j) {
					Console.WriteLine("\t\tAnswer[{0}] = {1}", j, sur.Answers[j]);
				}
			}
#endif
		}
	}
}
