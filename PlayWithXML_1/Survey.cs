// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Bartizan.ccLeadsWorking {
	/// <summary>
	/// Represents a survey question and its associated answers.
	/// </summary>
	[Serializable]
	public class Survey {
		public string		Question;
		public List<SurveyAnswer> Answers;

//---------------------------------------------------------------------------------------

		public Survey() {
			this.Question = "N/A";
			Answers = new List<SurveyAnswer>();
		}

//---------------------------------------------------------------------------------------

		public void Add(string Question, string Answer) {
			this.Question = Question;
			this.Answers.Add(new SurveyAnswer(Answer));
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class SurveyAnswer {
		public string		Answer;

//---------------------------------------------------------------------------------------

		public SurveyAnswer() {
			Answer = "N/A";
		}

//---------------------------------------------------------------------------------------

		public SurveyAnswer(string Answer) {
			this.Answer = Answer;
		}
	}
}
