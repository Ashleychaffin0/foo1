using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Bartizan.ActivityTrack30.Common {
	/// <summary>
	/// Represents a survey question and its associated answers.
	/// </summary>
	public class Survey	{
		public string		Question;
		public ArrayList	Answers;

//---------------------------------------------------------------------------------------

		public Survey(string Question) {
			this.Question = Question;
			Answers = new ArrayList();
		}

//---------------------------------------------------------------------------------------
		public static ArrayList ParseSetupFile(SetupFile suf) {
			ArrayList Surveys = new ArrayList();

			if (! suf.FindSection("SURVEYQUES"))
				return Surveys;

			Survey		CurSurvey = null;
			RegexOptions	reOpt = RegexOptions.Compiled 
								  | RegexOptions.IgnoreCase
								  | RegexOptions.IgnorePatternWhitespace;
			string		reQuestion = @"SURQUES(?<SeqNo>[0-9]+?)\s*=\s*(?<Question>.*)";
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
						//		 already in the ArrayList. But for now, we don't.
						CurSurvey = new Survey(Question);	
						Surveys.Add(CurSurvey);
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
						CurSurvey = new Survey(Question);	
						Surveys.Add(CurSurvey);
					}
				} else {
					// We've got an answer. Make sure it's not an answer before the 
					// first question!
					if (CurSurvey != null) {
						CurSurvey.Answers.Add(line.Trim());	
					} else {
						// TODO: Need to pass in a LogFile instance to be able to 
						//		 log error messages like these.
						Console.WriteLine("Survey Answer ({0}) before first question.", line);
					}
				}
			}

			return Surveys;
		}
	}
}
