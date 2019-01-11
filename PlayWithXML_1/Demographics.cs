using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Bartizan.ccLeadsWorking {
	[Serializable]
	public class DemographicAnswer {
		[XmlIgnore]
		public string ID;
		public string Answer;

//---------------------------------------------------------------------------------------

		public DemographicAnswer() {
			ID = "N/A ID";
			Answer = "N/Answer";
		}

//---------------------------------------------------------------------------------------

		public DemographicAnswer(string line) {
			string[] fields = line.Split(new char[] { ',' }, 2);
			if (fields.Length != 2) {
				string msg = string.Format("Invalid demographic answer ({0}). Must"
								+ " be two fields separated by a comma");
				throw new Exception(msg);
			}
			ID = fields[0].Trim();
			Answer = fields[1].Trim();
		}

//---------------------------------------------------------------------------------------

		public DemographicAnswer(string ID, string Answer) {
			this.ID = ID;
			this.Answer = Answer;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	[Serializable]
	public class Demographic {
		public string Question;
		public List<DemographicAnswer> Answers;
		// A single demographic swipe may may have multiple answers associated with it
		// ("Check all that apply"). So a demographics field in Visitor.txt might be
		// "AD", for the 1st and 4th answers. But if we've got too many answers to this
		// question, the answer string may be longer than a single character. In 
		// principle, "AD" could be then answer to one question, not two. And some
		// answer IDs may be blank padded. For example, if the IDs were "A", "A11" and 
		// "A1", and the attendee indicated them all, the Visitor.txt field would
		// contain "AbbA11A1b", where 'b' represents a blank. So as we process a given
		// swipe we need to know how many characters to process at a time. Hence the
		// following field. Note also that we can do some error checking. If the
		// length of the swipe field isn't a multiple of MaxAnswerWidth (3 in the 
		// previous example), then we know we've got an invalid swipe.

		// Note however that this field is useful only if the data that comes in from
		// Visitor.txt is *not* expanded. If so, this field will be ignored and we'll
		// the field by splitting on '+'. But at this point, we don't know how this
		// Map.Cfg file will be used. Hey, it could even be used with both expanded and
		// non-expanded data in two different imports. So we need to calculate it, even
		// if we wind up not using it.
		[XmlIgnore]
		public int MaxAnswerWidth;

//---------------------------------------------------------------------------------------

		public Demographic() {
			Question = "*N/A*";
			Answers = new List<DemographicAnswer>();
			MaxAnswerWidth = 1;
		}

//---------------------------------------------------------------------------------------

		public Demographic(string Question) {
			this.Question = Question;
			Answers = new List<DemographicAnswer>();
			MaxAnswerWidth = 1;			// Simplest assumption, may change below
		}
	}
}
