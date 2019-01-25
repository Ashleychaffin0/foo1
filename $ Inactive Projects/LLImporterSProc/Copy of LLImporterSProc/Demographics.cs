// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Bartizan.Importer {
    public class DemographicAnswer {
        public string ID;
        public string Answer;

//---------------------------------------------------------------------------------------

        public DemographicAnswer(string line) {
            string[] fields = line.Split(new char[] { ',' }, 2);
            // TODO: Check that we do indeed have two fields.
            ID = fields[0].Trim();
            Answer = fields[1].Trim();
        }

//---------------------------------------------------------------------------------------

		public DemographicAnswer(string ID, string Answer) {
			this.ID		= ID;
			this.Answer = Answer;
		}
    }

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

    public class Demographic {
		public string					Question;
        public List<DemographicAnswer>   Answers;

//---------------------------------------------------------------------------------------

		public Demographic(string Question) {
			this.Question = Question;
			Answers = new List<DemographicAnswer>();
		}

//---------------------------------------------------------------------------------------
        
        public static List<Demographic> ParseSetupFile(BartIniFile suf) {
			List<Demographic> Demogs = new List<Demographic>();

			if (! suf.FindSection("DEMOGRAPHICS"))
                return Demogs;

			Demographic		CurDemog = null;
			RegexOptions	reOpt = RegexOptions.Compiled 
								  | RegexOptions.IgnoreCase
								  | RegexOptions.IgnorePatternWhitespace;
			string		reQuestion = @"DHEADER(?<SeqNo>[0-9]+?)\s*=\s*(?<Question>.*)";
			Regex		re = new Regex(reQuestion, reOpt);
			Match		m;
			string		line;
			string		Question;

			while ((line = suf.ReadLine()) != null) {
				if (line.Trim().Length == 0)	// Stop at first empty line
					break;
				m = re.Match(line);
				// If we match, we've got a question line.
				if (m.Success) {		// Check if Question or answer line
					// To be honest, I don't really care about the sequence number. For
					// example, I don't care if all questions have the same sequence
					// number. But if anyone wants to validate this, they're welcome to.
					Question = (string)m.Groups["Question"].Value;
					Question = Question.Trim();
					if (Question.Length > 0) {
						// Note: I suppose we could check to see if the question is
						//		 already in the List<Demographic>. But for now, we don't.
                        CurDemog = new Demographic(Question);
                        Demogs.Add(CurDemog);
					} else {
						// Hmmm. It's not quite clear what to do here. We've got
						// something like "SURQUES23=", with no actual question.
						// OK, we can log an error, but then what? Do we ignore it?
						// Probably not, since the subsequent answers will be 
						// associated with the previous non-empty question (if
						// any). Do we add a question that's empty, and continue
						// adding answers to it? Well, I suppose. (But we also
						// get into the question above about duplicate (and in
						// this case, empty) questions. Or should we throw an
						// exception (actually, that may well be the best solution)?
						// Well, for now, I'm just going to add the empty question,
						// duplicating the code above.
                        CurDemog = new Demographic(Question);
                        Demogs.Add(CurDemog);
					}
				} else {
					// We've got an answer. Make sure it's not an answer before the 
					// first question!
                    if (CurDemog != null) {
                      CurDemog.Answers.Add(new DemographicAnswer(line.Trim()));	
					} else {
						// TODO: Need to pass in a LogFile instance to be able to 
						//		 log error messages like these.
#if ! SQLSERVER
						Console.WriteLine("Demographics Answer ({0}) before first question.", line);
#endif
					}
				}
			}

            return Demogs;
		}
    }
}
