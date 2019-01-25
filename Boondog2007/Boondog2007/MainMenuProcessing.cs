using System;
using System.Collections.Generic;
using System.Text;

namespace Boondog2009 {
	public partial class Boondog2009 {

//---------------------------------------------------------------------------------------

		private void newGameToolStripMenuItem_Click(object sender, EventArgs e) {
			game.Reset();
			board.NewGame();
		}
	}
}
