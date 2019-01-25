using System;
using System.Collections.Generic;
using System.Text;

namespace LRSJAS_Zigzag_1 {
	public class ZigZagData {

		public List<BoxParms>		BoxProperties;

		// TODO: Connectors not yet implemented. We should probably use the Guid for
		//		 each box as an ID.
		// public List<BoxConnector>	Connectors;
		
//---------------------------------------------------------------------------------------

		public ZigZagData() {
			// Empty ctor for serialization
		}

//---------------------------------------------------------------------------------------

		public ZigZagData(List<BoxParms> parms, List<BoxConnector> connectors) {
			this.BoxProperties = parms;
			// this.Connectors = connectors;
		}
	}
}
