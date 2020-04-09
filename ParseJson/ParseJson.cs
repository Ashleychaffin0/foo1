using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

/*
 * Here's my current understanding:
 * A Json file is an Object, which is text pairs surrounding by { ... }
 * A Property is a Name, followed by a :, followed by any of the following...
 *		{ Object }
 *		[ Array ]
 *		" String "
 *		" Number "
 *		" True "
 *		" False "
 *		" Null "
 *	These are comma-separated
 */

namespace ParseJson {
	public partial class ParseJson : Form {
		public ParseJson() {
			InitializeComponent();
			// Note: In Json, "foo: 123", the "foo" is a JsonProperty and the
			//		 "123"	is a JsonElement
			// Note: JsonValueKind.ValueKind is String, Number, Array or Object

			string BookmarksPath = $@"C:\Users\{Environment.UserName}\AppData\Local\Google\Chrome\User Data\Default\Bookmarks";
			// On macOS: /Users/{Environment.UserName}/Library/Application Support/Google/Chrome/Default/Bookmarks
			// Thanks to https://www.technewsera.com/chrome-bookmarks-location/
			string jsonText = File.ReadAllText(BookmarksPath);
			// Clipboard.SetText(jsonText);
			// var t = JsonSerializer.Deserialize(jsonText);
			var jDoc = JsonDocument.Parse(jsonText);
			JsonElement root = jDoc.RootElement;
			ProcessObject(root);

			// Now is the time for all good men to come to the aid of their party. This would be a
			// nice example

			Debug.WriteLine("\r\n********* DONE **********\r\n");
		}

//---------------------------------------------------------------------------------------

		private void ProcessObject(JsonElement node) {
			Debug.WriteLine("\r\n----------------------------\r\n");
			var nodes = node.EnumerateObject();
			// Debug.WriteLine($"Object: name: \"{node.name}\"");
			foreach (JsonProperty obj in nodes) {
				ProcessProperty(obj);
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessProperty(JsonProperty prop) {
			string name = prop.Name;
			JsonElement val = prop.Value;
			switch (prop.Value.ValueKind) {
			case JsonValueKind.Undefined:
				break;

			case JsonValueKind.Object:
				Debug.WriteLine($"\"{name}\" {{");
				ProcessObject(val);
				Debug.WriteLine("}");
				break;

			case JsonValueKind.Array:
				foreach (JsonElement item in val.EnumerateArray()) {
					ProcessElement(item);
				}
				break;

			case JsonValueKind.String:
				Debug.WriteLine($"{name}: \"{val}\"");
				break;

			case JsonValueKind.Number:
				Debug.WriteLine($"{name}: {val}");
				break;

			case JsonValueKind.True:
				Debug.WriteLine($"{name}: true");
				break;

			case JsonValueKind.False:
				Debug.WriteLine($"{name}: false");
				break;

			case JsonValueKind.Null:
				Debug.WriteLine($"{name}: null");
				break;

			default:
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessElement(JsonElement item) {
			switch (item.ValueKind) {
			case JsonValueKind.Undefined:
				break;

			case JsonValueKind.Object:
				ProcessObject(item);
				break;

			case JsonValueKind.Array:
				break;

			case JsonValueKind.String:
				break;

			case JsonValueKind.Number:
				break;

			case JsonValueKind.True:
				break;

			case JsonValueKind.False:
				break;

			case JsonValueKind.Null:
				break;

			default:
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessObjectOld(JsonElement root) {
			const int MaxLen = 400;
			var objs = root.EnumerateObject();
			foreach (JsonProperty obj in objs) {
				JsonElement val = obj.Value;
				string txt = val.ToString();
				if (txt.Length > MaxLen) { txt = txt.Substring(0, MaxLen); }
				// Debug.WriteLine($"{obj.Name}: ({val.ValueKind}){txt}");
				switch (val.ValueKind) {
				case JsonValueKind.Undefined:
					break;

				case JsonValueKind.Object:
					ProcessObjectOld(obj.Value);
					break;

				case JsonValueKind.Array:
					// int ixArray = 0;
					foreach (var item in val.EnumerateArray()) {
						// Check for item.ValueKind == Object
						// Debug.WriteLine($"Array[{ixArray++}]: {item}");
						if (item.ValueKind == JsonValueKind.Object) {
							ProcessObjectOld(item);
						}
					}
					break;

				case JsonValueKind.String:
					if (obj.Name != "name") continue;
					Debug.WriteLine(val);
					break;

				case JsonValueKind.Number:
					break;

				case JsonValueKind.True:
					break;

				case JsonValueKind.False:
					break;

				case JsonValueKind.Null:
					break;

				default:
					break;
				}
			}
		}
	}
}