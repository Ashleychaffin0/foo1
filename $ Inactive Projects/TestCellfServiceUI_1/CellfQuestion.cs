using System;
using System.Collections.Generic;
using System.Text;

namespace TestCellfServiceUI_1 {
	class CellfQuestion {
		public string	Name;
		public string	Text;

//---------------------------------------------------------------------------------------

		public CellfQuestion() {
			Name = "";
			Text = "";
		}

//---------------------------------------------------------------------------------------

		public CellfQuestion(string Name, string Text) {
			this.Name = Name;
			this.Text = Text;
		}
	}
}
