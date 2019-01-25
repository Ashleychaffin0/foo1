using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace SmallBasicDoc {
	public partial class SmallBasicDoc : Form {

//---------------------------------------------------------------------------------------

		public SmallBasicDoc() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void SmallBasicDoc_Load(object sender, EventArgs e) {
			FillTree();
		}

//---------------------------------------------------------------------------------------

		private void FillTree() {
			var doc = XDocument.Load(@"C:\Program Files\Microsoft\Small Basic\SmallBasicLibrary.xml");
			// Sample data:
/*
		<member name="T:Microsoft.SmallBasic.Library.Text">
            <summary>
            The Text object provides helpful operations for working with Text.
            </summary>
        </member>
        <member name="M:Microsoft.SmallBasic.Library.Text.Append(Microsoft.SmallBasic.Library.Primitive,Microsoft.SmallBasic.Library.Primitive)">
            <summary>
            Appends two text inputs and returns the result as another text.  This operation is particularly useful when dealing with unknown text in variables which could accidentally be treated as numbers and get added, instead of getting appended.
            </summary>
            <param name="text1">
            First part of the text to be appended.
            </param>
            <param name="text2">
            Second part of the text to be appended.
            </param>
            <returns>
            The appended text containing both the specified parts.
            </returns>
        </member>
*/
			TreeNode CurNode = null;
			var qryMembers = from el in doc.Root.Descendants("member")
							 select el;

			string TypeName = "";
			foreach (var el in qryMembers) {		// el is an XElement
				string val = el.FirstAttribute.Value;	// Assumes it's the name
				// Each "class" (e.g. Text, Array, Timer, Clock, etc) name starts
				// with "T:" (e.g. T:Microsoft.SmallBasic.Library.Text). Presumably the
				// "T" stands for "Type".
				if (val.StartsWith("T:")) {
					TypeName = val;
					int ix = TypeName.LastIndexOf('.') + 1;
					val = TypeName.Substring(ix);
					CurNode = treeDoc.Nodes.Add(val);
				} else {
					// It's a class/type member. These start with "M:". Strip
					// off the prefix to get just the name
					string name = val.Substring(TypeName.Length + 1);
					int ix = name.IndexOf('(');
					if (ix >= 0) {
						// Strip off the pseudo-parameter list
						name = name.Substring(0, ix);
					}
					var KidNode = CurNode.Nodes.Add(name);
					KidNode.Tag = new DocInfo(el);
				}
			}

			treeDoc.Sort();
		}

//---------------------------------------------------------------------------------------

		private void treeDoc_AfterSelect(object sender, TreeViewEventArgs e) {
			ProcessChildNode(e);
		}

//---------------------------------------------------------------------------------------

		private void ProcessChildNode(TreeViewEventArgs e) {
			if (e.Node.Tag == null) {
				return;					// TODO: Root nodes have no data yet
			}
			DocInfo info = (DocInfo)e.Node.Tag;
			richTextBox1.Rtf = info.ToRtf();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class DocInfo {
		public string summary;
		public List<KeyValuePair<string, string>> parms;
		public string returns;
		public string example;
		public string remarks;

//---------------------------------------------------------------------------------------

		public DocInfo(XElement el) {
			foreach (var item in el.Elements()) {
				string name = item.Name.LocalName;
				string val = string.IsNullOrEmpty(item.Value) ? null : item.Value;
				switch (name) {
				case "summary":
					summary = val;
					break;
				case "param":
					if (parms == null) {
						parms = new List<KeyValuePair<string, string>>();
					}
					parms.Add(new KeyValuePair<string, string>(item.FirstAttribute.Value, item.Value));
					break;
				case "returns":
					returns = val;
					break;
				case "example":
					example = val;
					break;
				case "remarks":
					remarks = val;
					break;
				default:
					MessageBox.Show("Unexpected value in DocInfo - " + name, "SmallBasicDoc");
					break;
				}
			}
		}

//---------------------------------------------------------------------------------------

		public string ToRtf() {
			string sum = summary != null ? "\\cf2 SUMMARY\n\\cf1 "   + summary : "";
			string ret = returns != null ? "\n\\cf2 RETURNS\n\\cf1 " + returns : "";
			string ex  = example != null ? "\n\\cf2 EXAMPLE\n\\cf1 " + example : "";
			string rem = remarks != null ? "\n\\cf2 REMARKS\n\\cf1 " + remarks : "";

			string prms = "";
			if (parms != null) {
				prms = "\n\\cf2 PARAMETERS\n\\cf1 ";
				foreach (var item in parms) {
					prms += "\n\\cf3" + item.Key + "\\cf1 " + item.Value;
				}
			}

			string msg = sum + prms + ret + ex + rem;
			if (msg.Length == 0) {
				msg = "\\cf2 NO DOCUMENTATION AVAILABLE\\cf1 "; 
			}

			msg = msg.Replace("\n", "\\line\n");
			return  @"{\rtf1\ansi\deff0\n" +
					@"{\colortbl;\red0\green0\blue0;\red255\green0\blue0;\red0\green0\blue255;}  "
					+ "\\fs30 "
					+ msg;
		}
	}
}
