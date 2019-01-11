using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MicrosoftZuneLibrary;

namespace TestZ_une40_2 {
	class MyTestZ_une {
		ZuneLibrary zl = null;

		public MyTestZ_une(string ZuneDirName) {
			zl = new ZuneLibrary();
			bool dbRebuilt;
			int i1 = zl.Initialize(ZuneDirName, out dbRebuilt);
		}
	}
}
