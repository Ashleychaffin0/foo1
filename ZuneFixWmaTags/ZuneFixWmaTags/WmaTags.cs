using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WMFSDKWrapper;

namespace LRS.WMA {
	class WmaTagBase {
		public string TagName { get; set; }
		public string ValueString { get; set; }
		public WMT_ATTR_DATATYPE DataType { get; set; }

//---------------------------------------------------------------------------------------

		public virtual object GetValue() {
			return "*Nonce*";
		}

//---------------------------------------------------------------------------------------

		public WmaTagBase(string TagName) {
			this.TagName = TagName;
		}

//---------------------------------------------------------------------------------------

		public static List<WmaTagBase> GetTags(string Filename) {
			return GetTags(Filename, 0);
		}

//---------------------------------------------------------------------------------------

		public static List<WmaTagBase> GetTags(string Filename, ushort StreamNum) {
			List<WmaTagBase>	tags = new List<WmaTagBase>();

			IWMMetadataEditor	MetadataEditor;
			IWMHeaderInfo3		HeaderInfo3;
			ushort				TagCount;

			WMFSDKFunctions.WMCreateEditor(out MetadataEditor);
			MetadataEditor.Open(Filename);

			HeaderInfo3 = (IWMHeaderInfo3)MetadataEditor;
			HeaderInfo3.GetAttributeCount(StreamNum, out TagCount);

			string				TagName = null;
			ushort				TagNameLength;
			WMT_ATTR_DATATYPE	DataType;
			ushort				LangIndex;
			byte []				Value = null;
			uint				DataLength = 0;
			for (ushort nTag = 0; nTag < TagCount; nTag++) {
				TagName		  = null;
				TagNameLength = 0;		// Means: Get length of TagName
				Value		  = null;
				DataLength	  = 0;
				uint rc = HeaderInfo3.GetAttributeByIndexEx(
					StreamNum, nTag, TagName, ref TagNameLength, out DataType,
					out LangIndex, Value, ref DataLength);
				// We know the length of the TagName, so allocate it and get it
				TagName = new string((char)0, TagNameLength);
				Value	= new byte[DataLength];
				rc = HeaderInfo3.GetAttributeByIndexEx(
					StreamNum, nTag, TagName, ref TagNameLength, out DataType,
					out LangIndex, Value, ref DataLength);
				TagName = TagName.Substring(0, TagName.Length - 1);
				CopyTagToList(tags, TagName, DataType, Value, DataLength);
			}

			MetadataEditor.Close(); 

			return tags;
		}

//---------------------------------------------------------------------------------------

		private static void CopyTagToList(List<WmaTagBase> tags, string TagName, WMT_ATTR_DATATYPE DataType, byte[] Value, uint DataLength) {
			Console.Write("Process tagname {0}, type {1}, DataLength = {2}, Value = ", TagName, DataType, DataLength);
			switch (DataType) {
			case WMT_ATTR_DATATYPE.WMT_TYPE_BINARY:
				tags.Add(new WmaTagBinary(TagName, Value));
				Console.WriteLine("Nonce on type Binary");
				break;
			case WMT_ATTR_DATATYPE.WMT_TYPE_BOOL:
				bool bVal = BitConverter.ToBoolean(Value, 0);
				tags.Add(new WmaTagBool(TagName, bVal));
				Console.WriteLine("{0}", bVal);
				break;
			case WMT_ATTR_DATATYPE.WMT_TYPE_DWORD:
				uint uintVal = BitConverter.ToUInt32( Value, 0 );
				tags.Add(new WmaTagUint(TagName, uintVal));
				Console.WriteLine("{0}", uintVal);
				break;
			case WMT_ATTR_DATATYPE.WMT_TYPE_GUID:
				// TODO: Nonce on GUID
				tags.Add(new WmaTagGUID(TagName, "*Nonce on GUID*"));
				Console.WriteLine("Nonce on type GUID");
				break;
			case WMT_ATTR_DATATYPE.WMT_TYPE_QWORD:
				uint ulVal = BitConverter.ToUInt32(Value, 0);
				tags.Add(new WmaTagUlong(TagName, ulVal));
				Console.WriteLine("{0}", ulVal);
				break;
			case WMT_ATTR_DATATYPE.WMT_TYPE_STRING:
				// TODO: Add Unicode support, as in sample code
				// TODO: I'd hope we could do better than the following
				//		 code, e.g. by using Encoding.*, but for now...
				string sVal;
				if (DataLength == 0) {
					sVal = null;
				} else {
					sVal = Encoding.Unicode.GetString(Value);
					sVal = sVal.Substring(0, sVal.Length - 1);	// Strip trailing \0
				}
				tags.Add(new WmaTagString(TagName, sVal));
				Console.WriteLine("{0}", sVal);
				break;
			case WMT_ATTR_DATATYPE.WMT_TYPE_WORD:
				ushort	usVal = BitConverter.ToUInt16( Value, 0 );
				tags.Add(new WmaTagUint(TagName, usVal));
				Console.WriteLine("{0}", usVal);
				break;
			default:
				break;
			}
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class WmaTagUshort : WmaTagBase {
		public ushort	Value;

//---------------------------------------------------------------------------------------

		public WmaTagUshort(string TagName, ushort Value)
				: base(TagName) {
			this.Value = Value;
			ValueString = Value.ToString();
			DataType = WMT_ATTR_DATATYPE.WMT_TYPE_WORD;
		}

//---------------------------------------------------------------------------------------

		public override object GetValue() {
			return Value;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class WmaTagUint : WmaTagBase {
		public uint	Value;

//---------------------------------------------------------------------------------------

		public WmaTagUint(string TagName, uint Value)
				: base(TagName) {
			this.Value = Value;
			ValueString = Value.ToString();
			DataType = WMT_ATTR_DATATYPE.WMT_TYPE_DWORD;
		}

//---------------------------------------------------------------------------------------

		public override object GetValue() {
			return Value;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class WmaTagString : WmaTagBase {
		public string	Value;

//---------------------------------------------------------------------------------------

		public WmaTagString(string TagName, string Value)
				: base(TagName) {
			this.Value = Value;
			ValueString = Value;
			DataType = WMT_ATTR_DATATYPE.WMT_TYPE_STRING;
		}

//---------------------------------------------------------------------------------------

		public override object GetValue() {
			return Value;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class WmaTagBool : WmaTagBase {
		public bool	Value;

//---------------------------------------------------------------------------------------

		public WmaTagBool(string TagName, bool Value)
				: base(TagName) {
			this.Value = Value;
			ValueString = Value.ToString();
			DataType = WMT_ATTR_DATATYPE.WMT_TYPE_BOOL;
		}

//---------------------------------------------------------------------------------------

		public override object GetValue() {
			return Value;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class WmaTagUlong : WmaTagBase {
		public ulong	Value;

//---------------------------------------------------------------------------------------

		public WmaTagUlong(string TagName, ulong Value)
				: base(TagName) {
			this.Value = Value;
			ValueString = Value.ToString();
			DataType = WMT_ATTR_DATATYPE.WMT_TYPE_QWORD;
		}

//---------------------------------------------------------------------------------------

		public override object GetValue() {
			return Value;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class WmaTagGUID : WmaTagBase {
		public string	Value;		// TODO: Probably not "string"

//---------------------------------------------------------------------------------------

		public WmaTagGUID(string TagName, string Value)
				: base(TagName) {
			this.Value = Value;
			ValueString = Value;
			DataType = WMT_ATTR_DATATYPE.WMT_TYPE_GUID;
		}

//---------------------------------------------------------------------------------------

		public override object GetValue() {
			return Value;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class WmaTagBinary : WmaTagBase {
		public byte []	Value;		// TODO: Probably not "string"

//---------------------------------------------------------------------------------------

		public WmaTagBinary(string TagName, byte[] Value)
				: base(TagName) {
			this.Value = Value;
			ValueString = this.ToString();
			DataType = WMT_ATTR_DATATYPE.WMT_TYPE_BINARY;
		}

//---------------------------------------------------------------------------------------

		public override object GetValue() {
			return Value;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			StringBuilder sb = new StringBuilder(Value.Length * 2 + 2);
			sb.Append("0x");
			for (int i = 0; i < Value.Length; i++) {
				sb.AppendFormat("{0:x}", Value[i]);
			}
			return sb.ToString();
		}
	}
}
