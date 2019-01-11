using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using MicrosoftZuneLibrary;

namespace LRS.Zune.ClassGeneration {
	class GenerateLRSZuneClasses {

//---------------------------------------------------------------------------------------

		public static void CreateLRSZuneClasses(ZuneLibrary zl, string EnumValName) {
			// XDocument xd = XDocument.Load("QueriesAndSchemae.xml");
			XDocument xd = GenerateQueriesAndSchemae.GetQueriesAndSchemaeAsXdoc(zl);

			StreamWriter wtr = new StreamWriter("LRSZuneClasses.cs");

			wtr.WriteLine("// Copyright (c) 2008 by Larry Smith");
			wtr.WriteLine();
			wtr.WriteLine("// This file was generated on {0} at {1}", 
				DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
			wtr.WriteLine();
			wtr.WriteLine("using System;");
			wtr.WriteLine("using System.Collections;\t\t// For ArrayList");
			wtr.WriteLine("using System.Collections.Generic;");
			wtr.WriteLine("using System.IO;");
			wtr.WriteLine("using System.Linq;");
			wtr.WriteLine("using System.Xml.Linq;");
			wtr.WriteLine();
			wtr.WriteLine("using MicrosoftZuneLibrary;");
			wtr.WriteLine();

			wtr.WriteLine("namespace LRS.Zune.Classes {");

			GenerateZuneTypeFactoryCode(wtr, EnumValName, xd.Root.Elements());

			string ClassName = null;
			string FieldName;
			StringBuilder sb = new StringBuilder();
			string sep = "//---------------------------------------------------------------------------------------";
			foreach (var elem in xd.Root.Elements()) {
				sb.Length = 0;			// Clear
				ClassName = elem.Attribute("qtype").Value;
				wtr.WriteLine("\r\n{0}\r\n{0}\r\n{0}\r\n{0}\r\n{0}\r\n", sep);
				wtr.WriteLine("\tpublic class {0} {{", ClassName);
				wtr.WriteLine("\t\tpublic static EQueryType {0} = EQueryType.{1};\r\n", EnumValName, ClassName);
				sb.AppendFormat("\r\n{0}\r\n\r\n\t\tpublic {1}(ZuneQueryList ZQList, uint n) {{\r\n", sep, ClassName);
				sb.Append("\t\t\tobject\to;\r\n\r\n");
				// Generate Fields
				foreach (var smt in elem.Elements()) {
					FieldName = smt.Value;
					if (FieldName.StartsWith("kiIndex_")) {
						FieldName = FieldName.Substring(8);
					}
					// string type = ZuneTypeFactory.MapTypeToCSharp(smt.Attribute("type").Value);
					string type = MapTypeToCSharp(smt.Attribute("type").Value);
					wtr.WriteLine("\t\tpublic {0}\t{1};", type, FieldName);
					sb.AppendFormat("\t\t\to = ZQList.GetFieldValue(n, typeof({0}), (uint)SchemaMap.{1}); {2} = ({0})(o ?? default({0}));\r\n",
						type.Replace("\t", ""), smt.Value, FieldName);
				}
				sb.AppendFormat("\t\t}}\t\t// {0} ctor", ClassName);
				// Generate ctor
				wtr.WriteLine(sb.ToString());
				// Console.WriteLine("\r\n{0}\r\n", sep);
				// End class
				wtr.WriteLine("\t}}\t\t// {0} class\r\n", ClassName);
			}

			wtr.WriteLine("}");		// Namespace end
			wtr.Close();
		}

//---------------------------------------------------------------------------------------

		static string MapTypeToCSharp(string s) {
			return MapTypeToCSharp(s, true);
		}

//---------------------------------------------------------------------------------------

		static string MapTypeToCSharp(string s, bool bUseTab) {
			switch (s) {
			case "System.Int32":
				// When we're pumping out XML, we don't necessarily want tab
				// characters (they'll get translated to "int&#x9").
				if (bUseTab)
					return "int\t";
				else
					return "int";
			case "System.String":
				return "string";
			case "System.DateTime":
				return "DateTime";
			case "System.UInt64":
				return "ulong";
			case "System.Collections.ArrayList":
				return "ArrayList";
			default:
				return s;
			}
		}

//---------------------------------------------------------------------------------------

		private static void GenerateZuneTypeFactoryCode(StreamWriter wtr, string EnumValName, IEnumerable<XElement> elems) {
			string s = string.Format(@"{0}public static class ZuneTypeFactory {{

		const string EnumValName = ""{1}"";", "\t", EnumValName);
			wtr.WriteLine(s);

			GenerateZuneFactory_Create(wtr, elems);
			GenerateZuneFactory_GetZuneItems(wtr);
			GenerateZuneFactory_ZuneToXml(wtr);
			GenerateZuneFactory_MapCSharp(wtr);
			GenerateZuneFactory_DumpObj(wtr);
			GenerateZuneFactory_DumpZuneItems(wtr);

			wtr.WriteLine("\t}");		// Close off class
		}

		private static void GenerateZuneFactory_DumpZuneItems(StreamWriter wtr) {
			string s = @"
//---------------------------------------------------------------------------------------

		public static void DumpZuneItemsViaClass<QueryType>(
						ZuneLibrary zl,
						string Filename) where QueryType : class {
			IEnumerable<QueryType> Items = ZuneTypeFactory.GetZuneItems<QueryType>(zl);
			StreamWriter wtr = new StreamWriter(Filename);
			foreach (var item in Items) {
				wtr.WriteLine();
				DumpObj(wtr, item);
			}
			wtr.Close();
		}";
			wtr.WriteLine(s);
		}

//---------------------------------------------------------------------------------------

		private static void GenerateZuneFactory_DumpObj(StreamWriter wtr) {
			string s = @"
//---------------------------------------------------------------------------------------

		public static void DumpObj(TextWriter wtr, object o) {
			Type t = o.GetType();
			var flds = from field in t.GetFields()
							 select field;
			string sep = "";
			foreach (var f in flds) {
				object val = t.GetField(f.Name).GetValue(o);
				// This is taking forever. Comment it out, and re-write it a bit
#if true
				if (val is ArrayList) {
					wtr.Write(""{0}{1} = "", sep, f.Name);
					string	vSep = "";
					foreach (var v in (ArrayList)val) {
						wtr.Write(""{0}{1}"", vSep, v);
						vSep = ""~"";
					}
					wtr.WriteLine();
				} else {
					wtr.WriteLine(""{0}{1} = {2}"", sep, f.Name, val);
				}
#else
				wtr.Write(""{0}{1} = "", sep, f.Name);
				if (val is ArrayList) {
					string	vSep = "";
					foreach (var v in (ArrayList)val) {
						wtr.Write(""{0}{1}"", vSep, v);
						vSep = ""~"";
					}
				} else {
					wtr.Write(""{0}"", val);
				}
				wtr.WriteLine();
#endif
				sep = ""\t"";
			}
		}
";
			wtr.WriteLine(s);
		}

//---------------------------------------------------------------------------------------

		private static void GenerateZuneFactory_Create(TextWriter wtr, IEnumerable<XElement> elems) {
			string s = @"
//---------------------------------------------------------------------------------------
		
		public static object Create<T>(ZuneQueryList zql, uint i) where T : class  {";
			StringBuilder sb = new StringBuilder(s);

			sb.Append("\r\n			Type t = typeof(T);\r\n");
			foreach (var elem in elems) {
				string name = elem.Attribute("qtype").Value;
				sb.AppendFormat("\t\t\tif (t == typeof({0})) return new {0}(zql, i);\r\n", name);
			}

			wtr.WriteLine(sb.ToString());
			wtr.WriteLine("\t\t\treturn null;");
			wtr.WriteLine("\t\t}");		// Close off method
		}

//---------------------------------------------------------------------------------------

		private static void GenerateZuneFactory_GetZuneItems(StreamWriter wtr) {
			string s = @"
//---------------------------------------------------------------------------------------

		public static IEnumerable<T> GetZuneItems<T>(ZuneLibrary zl) where T : class {
			Type	t = typeof(T);
			EQueryType type = (EQueryType)t.GetField(EnumValName).GetValue(t);
			using (ZuneQueryList zql = zl.QueryDatabase(type, 0,
				EQuerySortType.eQuerySortOrderAscending, 0, null)) {
				if (zql == null) {
					yield break;
				}
				for (uint i = 0; i < zql.Count; i++) {
					yield return (T)Create<T>(zql, i);
				}
				yield break;
			}
		}";
			wtr.WriteLine(s);
		}

//---------------------------------------------------------------------------------------

		private static void GenerateZuneFactory_MapCSharp(StreamWriter wtr) {
			string s = @"
//---------------------------------------------------------------------------------------

		public static string MapTypeToCSharp(string s) {
			return MapTypeToCSharp(s, true);
		}

//---------------------------------------------------------------------------------------

		public static string MapTypeToCSharp(string s, bool bUseTab) {
			switch (s) {
			case ""System.Int32"":
				// When we're pumping out XML, we don't necessarily want tab
				// characters (they'll get translated to ""int&#x9"").
				if (bUseTab)
					return ""int\t"";
				else
					return ""int"";
			case ""System.String"":
				return ""string"";
			case ""System.DateTime"":
				return ""DateTime"";
			case ""System.UInt64"":
				return ""ulong"";
			case ""System.Collections.ArrayList"":
				return ""ArrayList"";
			default:
				return s;
			}
		}";
			wtr.WriteLine(s);
		}

//---------------------------------------------------------------------------------------

		private static void GenerateZuneFactory_ZuneToXml(StreamWriter wtr) {
			string s = @"
//---------------------------------------------------------------------------------------

		public static XDocument ZuneToXml<QueryType>(
				ZuneLibrary zl, 
				string		TopElementName, 
				string		ElementName) where QueryType : class {
			var ZuneItems = ZuneTypeFactory.GetZuneItems<QueryType>(zl);
			XDocument xd = new XDocument(
				new XElement(TopElementName,
					from item in ZuneItems
					select new XElement(ElementName, 
						from Item in item.GetType().GetFields()
							select new XElement(""Field"", 
								new XAttribute(""Name"", Item.Name),
								new XAttribute(""Type"", MapTypeToCSharp(Item.FieldType.ToString(), false)),
								Item.GetValue(item))
						)
				)
			);
			return xd;
		}";
			wtr.WriteLine(s);
		}
	}
}
