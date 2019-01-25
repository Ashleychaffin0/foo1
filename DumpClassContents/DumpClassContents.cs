using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace DumpClassContents {
	public partial class DumpClassContents : Form {
		static public int x = 5;
		static string s = "Hello";

		string[] ss;
		int[] xs = { 5, 43, 645 };

		Dictionary<object, string> SeenBefore;

		Dictionary<string, List<string>> Outputs;

		const string OutFileName = @"G:\lrs\DumpClassContents.html";
		StreamWriter wtr;

//---------------------------------------------------------------------------------------

		public DumpClassContents() {
			InitializeComponent();

			Outputs = new Dictionary<string, List<string>>();
			SeenBefore = new Dictionary<object, string>();

			using (wtr = new StreamWriter(OutFileName)) {
				InitOutputFile();

				ss = "Now is the time...".Split(' ');

				// var t = typeof(Form);
				// var t = typeof(DumpClassContents);
#if WC
				var wc = new WebClient();
				wc.BaseAddress = "http://www.microsoft.com";
				var xx = wc.DownloadData("http://www.ibm.com");
#endif
				var dict = new Dictionary<string, int> {
					["Alice"] = 10,
					["Bob"] = 20
				};

				// DumpFields(dict);
				// DumpFields(this);
				// DumpFields(this.Parent);
				var f = new foo();
				// DumpHierarchy(this);
				// TellMeAboutYourself(this);
				// TellMeAboutYourself(wc);
				string BaseName = "f";
				TellMeAboutYourself(BaseName, f, "");
				wtr.WriteLine(@"</body>
</html>");

				WriteLine("", "$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
				var OutputKeys = Outputs.Keys.OrderBy(k => k);
				foreach (string key in OutputKeys) {
					var Lines = Outputs[key];
					wtr.Write($"Start of fields for key '{key}'<br>");
					WriteLine(BaseName, $"<p/><section>Field {key}</section>");
					foreach (var line in Lines) {
						// WriteLine(BaseName, line);
						wtr.WriteLine(line);
					}
				}

				Process.Start(OutFileName);
			}
		}

//---------------------------------------------------------------------------------------

		private void InitOutputFile() {
			wtr.AutoFlush = true;
			wtr.WriteLine(@"
<html>
<head>
<title>Output from DumpClassContents</title>
<style>
	section { border: 2px solid tomato;  font-weight: bold; font-size: 24; }
	attr { color: red; }
	name { font-weight: bold; }
</style>
<body>
");
		}

//---------------------------------------------------------------------------------------

		private void TellMeAboutYourself(string VarName, object Obj, string ParentName) {
			if (Obj is null) return;
			var t = Obj.GetType();
			if (t.IsPointer) {
				WriteLine(ParentName, $"object {VarName} is a Pointer -- ignored");
				return;
			}
			string Parent2 = ParentName.Length == 0 ? "" : ParentName + ".";
			// WriteLine(ParentName, $"\r\n\r\n<font size=\"6\">Variable {Parent2}{VarName}</font> of Type: " + t.FullName);
			WriteLine(ParentName, $"\r\n\r\n<font size=\"6\">Variable {Parent2}{VarName}</font> of Type: " + t.FullName);

			TellMeAboutYourFields(Obj, t, Parent2 + VarName);

#if false
			BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
			WriteLine("Methods\r\n=======");
			foreach (var item in t.GetMethods(bf)) {
				WriteLine($"\t{ItemName(item.Name)}");	
			}
			WriteLine("Properties\r\n==========");
			foreach (var item in t.GetProperties(bf)) {
				WriteLine($"\t{ItemName(item.Name)}");	
			}

			WriteLine("Enums\r\n=====");
			// foreach (var item in t.GetEnumNames()) {
			WriteLine($"\t{ItemName(item.Name)}");		// TODO:
			// }
			WriteLine("Interfaces\r\n==========");
			foreach (var item in t.GetInterfaces()) {   // TODO: t.GetInterfaceMap(Type xxx) -- MethodInfo[] for methods
				WriteLine($"\t{ItemName(item.Name)}");	
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void TellMeAboutYourFields(object obj, Type t, string ParentName) {
			if (SeenBefore.ContainsKey(obj)) {
				WriteLine(ParentName, $"TODO: We've seen you before at {ParentName}");
				return;
			}
			SeenBefore[obj] = ParentName;   // TODO: Do a bit better than parentname?
			WriteLine(ParentName, $"<section>Field Names for Type {t.FullName}</section>\r\n===========");
			TellMeAboutYourFieldsCommon(obj, t, BindingFlags.Public, ParentName);
			TellMeAboutYourFieldsCommon(obj, t, BindingFlags.NonPublic, ParentName);
		}

//---------------------------------------------------------------------------------------

		private void TellMeAboutYourFieldsCommon(object obj, Type t, BindingFlags flags, string ParentName) {
			if (t.FullName == "System.Reflection.Pointer") {
				WriteLine(ParentName, $"object {ParentName} is a Pointer -- ignored");
				return;
			}
			if (t.FullName == "System.Text.SBCSCodePageEncoding") {
				// TODO: Check list of classes to be skipped
				WriteLine(ParentName, $"System.Text.SBCSCodePageEncoding -- skipped");
				return;
			}
			string stat = "?";
			string @protected;
			string ty;
			var bf = flags | BindingFlags.Static | BindingFlags.Instance;
			string prot = flags.HasFlag(BindingFlags.Public) ? "public" : "NonPublic";  // TODO: Needed?

			var qry = t.GetFields(bf).OrderBy(f => f.Name);
			string FullName;
			foreach (var item in qry) {
				FullName = ParentName + "." + item.Name;
				prot = item.Attributes.ToString();  // Can be part of ShowFieldDef?
				ShowFieldDef(out stat, out @protected, out ty, prot, item);

				if (item.FieldType.Name == "String") {
					WriteLine(ParentName, $"<b>{ItemName(item.Name)}</b> = \"{item.GetValue(obj)}\"");
					continue;
				}

				if (item.FieldType.IsArray) {
					ShowArray(FullName, obj, item);
					continue;
				}

				// val = item.GetValue(obj)?.ToString() ?? "(null)";
				object value = item.GetValue(obj);
				if (value is null) {
					WriteLine(ParentName, $"\t{prot} {stat}{@protected}{ty} <b>{ItemName(item.Name)}</b> = (null)");
					continue;
				}

				if (item.FieldType.Name.StartsWith("Dictionary`") || item.FieldType.Name.StartsWith("List`")) {
					DoDictList(item.Name, stat, @protected, ty, prot, item, value);
					continue;
				}

				var t2 = value.GetType();
				if (t2.IsClass && t2.Name != "String") {    // Sorta a kludge
															// WriteLine($"About to process field {ItemName(item.Name)} of type {t2.FullName}");
					TellMeAboutYourself(item.Name, value, ParentName);
					continue;
				}

				WriteLine(ParentName, $"\t{prot} {stat}{@protected}{ty} {ItemName(item.Name)} = {value}");
			}
			if (stat != "?") WriteLine($"{ParentName}", "");
		}

//---------------------------------------------------------------------------------------

		private void DoDictList(string CollectionName, string stat, string @protected, string ty, string prot, FieldInfo item, object value) {
			// System.Diagnostics.Debugger.Break();
			WriteLine(CollectionName, $"\t{prot} {stat}{@protected}{ty} {ItemName(item.Name)} = {value}");
			foreach (var dictval in value as dynamic) {
				// TODO: WriteLine(CollectionName, dictval);
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowArray(string ArrayName, object obj, FieldInfo item) {
			// Note: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/walkthrough-creating-and-using-dynamic-objects
			var avals = item.GetValue(obj) as dynamic;
			if (avals is null) {
				WriteLine(ArrayName, $"{ItemName(item.Name)}[] = (null)");
				return;
			}
				WriteLine(ArrayName, $"Fieldx {ItemName(item.Name)}");
			if (obj.GetType().FullName.StartsWith("System.Text.") && avals.Length > 32) {
				Write(ArrayName, $"{ItemName(item.Name)}[{avals.Length}] = ");
				int n = 0;
				foreach (var aval2 in avals) {
					if (n++ > 32) break;
					Write(ArrayName, $"{aval2}");
				}
				WriteLine(ArrayName, " ...");
			} else {
				WriteLine(ArrayName, $"{ItemName(item.Name)}[{avals.Length}] =");
				int n = 0;
				foreach (var aval2 in avals) {
					if (n >= 32) break;
					WriteLine(ArrayName, $"[{n++}] = {aval2}");
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowFieldDef(out string stat, out string @protected, out string ty, string prot, FieldInfo item) {
			stat = item.IsStatic ? "static " : "";
			@protected = item.IsFamily ? "protected " : "";
			ty = "<attr>" + item.FieldType.Name + "</attr>";
			var ty2 = item.FieldType as Type;       // TODO: Useful???
			// TODO: WriteLine($"\t{prot} {stat}{@protected}{ty} {ItemName(item.Name)}");
		}

//---------------------------------------------------------------------------------------

		private void DumpArray(string ArrayName, object[] vector) {
			for (int i = 0; i < vector.Length; i++) {
				Write(ArrayName, $"{vector[i]} ");
			}
			WriteLine(ArrayName, "");
		}

//---------------------------------------------------------------------------------------

		private string ItemName(string name) => "<name>" + name + "</name>";

//---------------------------------------------------------------------------------------

		private void Write(string Name, string s) {
			if (s == "<br>") System.Diagnostics.Debugger.Break();
			if (Outputs.Keys.Contains(Name)) {
				Outputs[Name].Add(s);
			} else {
				var lst = new List<string>() { s };
				Outputs[Name] = lst;
			}
			// wtr.Write(s);
		}

//---------------------------------------------------------------------------------------

		private void WriteLine() {
			// TODO: Need <Name> parm
			wtr.WriteLine("<br> -- from WriteLine() from if (stat != \" ? \")");
		}

//---------------------------------------------------------------------------------------

		private void aaaWriteLine(string s) {
			wtr.WriteLine(s);
			wtr.WriteLine("<br>");
		}

//---------------------------------------------------------------------------------------

		private void WriteLine(string Name, string s) {
			Write(Name, s);
			Outputs[Name].Add("<br>");
		}

#region Sort of Obsolete
#if false

		//---------------------------------------------------------------------------------------

		private void Old_TellMeAboutYourFields(object obj, Type t) {
			string stat = "?";
			string ty;
			string val;
			WriteLine("Field Names\r\n===========");
			var qfnsPublic = t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).OrderBy(n => n.Name);
			// WriteLine("\tPublic\r\n\t======");
			foreach (var item in qfnsPublic) {
				stat = item.IsStatic ? "static " : "";
				ty = item.FieldType.Name;
				val = item.GetValue(obj)?.ToString() ?? "(null)";
				WriteLine($"\tpublic {stat}{ty} {ItemName(item.Name)} = {val}");
				if (item.FieldType.IsArray) {
					int k = 1;
					for (int i = 0; i < val.Length; i++) {
						WriteLine($"\t\t[{i}] = {val[i]}");
					}
				}
			}
			if (stat != "?") WriteLine();

			stat = "?";
			var qfnsPrivate = t.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).OrderBy(n => n.Name);
			// WriteLine("\tPrivate\r\n\t=======");
			foreach (var item in qfnsPrivate) {
				stat = item.IsStatic ? "static " : "";
				ty = item.FieldType.Name;
				// val = item.GetValue(obj)?.ToString() ?? "(null)";
				// WriteLine($"\tprivate {stat}{ty} {ItemName(item.Name)} = {val}");
				if (item.FieldType.IsArray) {
					WriteLine($"\tprivate {stat}{ty} {ItemName(item.Name)}");
#if true
					var avals = item.GetValue(obj) as dynamic;
					int n = 0;
					foreach (var aval2 in avals) {
						WriteLine($"[{n++}] = {aval2}");
					}
#else
				dynamic aval = item.GetValue(obj) as dynamic; 
				for (int i = 0; i < aval.Length; i++) {
					WriteLine($"\t\t[{i}] = {aval[i]}");
					// WriteLine($"\t\t[{i}] = {aval.ElementAt(i)}");
				}
#endif
				} else {
					val = item.GetValue(obj)?.ToString() ?? "(null)";
					WriteLine($"\tprivate {stat}{ty} {ItemName(item.Name)} = {val}");
				}
			}
		}

		//---------------------------------------------------------------------------------------

		private void DumpHierarchy(object obj) {
			if (obj is null) return;
			Type t = obj.GetType();
			var ifs = t.GetInterfaces();    // TODO: Do something with this
			var if1 = t.GetInterface("IList`1");
			while (t != null) {
				WriteLine(t.Name);
				t = t.BaseType;
			}
		}

		//---------------------------------------------------------------------------------------

		private void DumpFields(object obj) {
			if (obj is null) return;
			foreach (var item in Enumerable.Repeat("\r\n", 3)) Console.Write(item);
			// WriteLine(Enumerable.Repeat("\r\n", 3).ToArray().ToString());
			Type t = obj.GetType();
			WriteLine($"FullName = {t.FullName}");
			BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
			var flds = t.GetFields(bf);
			foreach (var fld in flds) {
				// TODO: fld.CustomAttributes
				// TODO: ContainsGenericParameters / bool
				// TODO: BaseType
				// TODO: DeclaredMembers
				// TODO: DeclaringType
				// TODO: Module
				var val = fld.GetValue(obj);
				Console.Write($"{fld.Name} -- ");
				// if (fld.DeclaringType.Name.StartsWith("Dictionary`")) System.Diagnostics.Debugger.Break();
				var fldType = fld.GetType();
				// if (fldType.FullName == "System.Reflection.RtFieldInfo") System.Diagnostics.Debugger.Break();
				switch (val) {
					case int IntVal:
						WriteLine($"\tint: {IntVal}");
						break;
					case int[,] IntVectorVal:
						WriteLine($"int[]: {string.Join(", ", IntVectorVal)}");
						break;
					case string StringVal:
						WriteLine($"\tstring: \"{StringVal}\"");
						break;
					case string[] StringVectorVal:
						DumpArray(StringVectorVal);
						break;
					case Array ArrayVal:
						WriteLine($"{fld.FieldType.Name}");
						foreach (var item in ArrayVal) {
							var typ = item.GetType();   // Might be object[]
							WriteLine($@"[{typ.GetField("key").GetValue(item)}] = {typ.GetField("value").GetValue(item)}");
						}

						break;
					case Dictionary<string, int> DictStringIntVal:

						break;
					// case Entry[] EntryVectorVal:

					// break;
					default:
						WriteLine($" -- {fld.FieldType.FullName}: {val}");
						// WriteLine($"\tNonce on type");
						// var ovals = (object[])val;
						break;
				}
				if (fld.Name == "!!keys") {
					// var yyy = val.GetType();
					// var xxx = yyy.GetFields(bf);
					foreach (var item in (val as Array)) {
						var typ = item.GetType();
						WriteLine($@"[{typ.GetField("key").GetValue(item)}] = {typ.GetField("value").GetValue(item)}");
					}
				}
			}
		}
	}
#endif
#endregion
	}

//---------------------------------------------------------------------------------------

	public class foo /* : Form, IComparable<int>, IList<string> */ {
		public Dictionary<string, int> dict;
#if true
		public List<int> listint = new List<int> { 7, 4, 12 };
		public int hoo;
		public string goo;
		public int[] aint = { 5, 6, 7 };
#endif

		public foo() {
			dict = new Dictionary<string, int> {
				["Alice"] = 10,
				["Bob"] = 20
			};
		}
	}
}
