using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Bartizan {
	class Program {
		static void Main(string[] args) {
			byte[] data = Encoding.ASCII.GetBytes("Hello");
			BartTraceRecord tr = new BartTraceRecord("Hello");
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	public class BartTraceRecord {
		byte []				_Data;
		TraceRecordHeader	hdr;

		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------

		/// <summary>
		/// C# doesn't allow us to describe the data format we want in a natural way. So
		/// we're forced to work around this.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private class TraceRecordHeader {
			uint	_FrontLength;		// Length of this entire record, including
										//	 this field.
			ushort	_EntryClass;		// User defined. Implies the type of the data.
										// For example, graphic, audio, etc. 
			ushort	_EntryType;			// Subtype of EntryClass. For example, if 
										//   EntryClass=Audio, this could indicate
										//	 WAV, MP3, WMA, etc.
			DateTime _Timestamp;
			uint	_Flags;				// User defined. Possible uses might include
										//   0x01=Debug msg, 0x02=Test data, etc
			public uint	DataLen;		// Length of following data

			// The data goes here

			// There's a length field at the end of each record. This can be used as a
			// sort of "reverse pointer", and allows us to read the file in reverse
			// chronological order. However, we must handle this field manually.
			// uint	RearLength;			// For reading the file in reverse

//---------------------------------------------------------------------------------------

			public uint FrontLength {
				get { return _FrontLength; }
				set { _FrontLength = value; }
			}

//---------------------------------------------------------------------------------------

			public DateTime Timestamp {
				get { return _Timestamp; }
				set { _Timestamp = value; }
			}

//---------------------------------------------------------------------------------------

			public uint Flags {
				get { return _Flags; }
				set { _Flags = value; }
			}

//---------------------------------------------------------------------------------------

			public ushort EntryClass {
				get { return _EntryClass; }
				set { _EntryClass = value; }
			}

//---------------------------------------------------------------------------------------

			public ushort EntryType {
				get { return _EntryType; }
				set { _EntryType = value; }
			}

//---------------------------------------------------------------------------------------

			public void WriteHeader(BinaryWriter bw) {
				bw.Write(FrontLength);
				bw.Write(EntryClass);
				bw.Write(EntryType);
				bw.Write(Convert.ToDouble(Timestamp));
				bw.Write(Flags);
				bw.Write(DataLen);
			}
		}

		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------

		public byte[] Data {
			get { return _Data; }
			set {
				_Data = value;
				hdr.DataLen = (uint)_Data.Length;
			}
		}

//---------------------------------------------------------------------------------------

		public BartTraceRecord(byte[] Data) {
			this.Data = Data;
			hdr = new TraceRecordHeader();
			hdr.EntryClass = 0;
			hdr.EntryType = 0;
			hdr.Flags = 0;
			SetLengthAndTime();
		}

//---------------------------------------------------------------------------------------

		public BartTraceRecord(string Data)		// Common special case
			: this(Encoding.ASCII.GetBytes(Data)) {
		}

//---------------------------------------------------------------------------------------

		public BartTraceRecord(byte[] Data, ushort EntryClass, ushort EntryType, uint Flags) {
			this.Data = Data;
			hdr = new TraceRecordHeader();
			hdr.EntryClass = EntryClass;
			hdr.EntryType = EntryType;
			hdr.Flags = Flags;
			SetLengthAndTime();
		}

//---------------------------------------------------------------------------------------

		private void SetLengthAndTime() {
			// Add in size of ReadLength
			hdr.FrontLength = (uint)(Marshal.SizeOf(typeof(TraceRecordHeader)) + Marshal.SizeOf(typeof(uint)));
			hdr.Timestamp = DateTime.Now;
		}

//---------------------------------------------------------------------------------------

		public void Write(BinaryWriter bw) {
			hdr.WriteHeader(bw);
			bw.Write(_Data);
			bw.Write(hdr.FrontLength);
		}

	}
}

