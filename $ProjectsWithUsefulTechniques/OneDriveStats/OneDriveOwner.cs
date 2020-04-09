using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveStats {
	class OneDriveOwner {
		string OwnerName;
		public string Path;

//---------------------------------------------------------------------------------------

		public OneDriveOwner(string OwnerName, string Path) {
			this.OwnerName = OwnerName;
			this.Path      = Path;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() => OwnerName;
	}
}
