using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRS_Lisp2CSharp {

	class LispNode {

//---------------------------------------------------------------------------------------

		public enum NodeType {

		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class LispNode_Obsolete_1 {

//---------------------------------------------------------------------------------------

		public enum NodeType_Obsolete_1 {
			COMMENT,
			LIST,
			UNDEFINED
		}

//---------------------------------------------------------------------------------------

		public NodeType_Obsolete_1 type;
		public string Comment;
		public List<LispNode_Obsolete_1> Items;

//---------------------------------------------------------------------------------------

		public LispNode_Obsolete_1(NodeType_Obsolete_1 type, object o) {
			this.type = type;
			if (type == NodeType_Obsolete_1.COMMENT) {
				Comment = (string)o;
			} else {
				Items = new List<LispNode_Obsolete_1>();
				// Items.Add(o);
			}
		}
	}
}
