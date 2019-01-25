using System;
using System.Collections.Generic;
using System.Text;

// TODO: Code for 1st, middle, last lines is almost identical. Make common subroutine
// TODO: Allow groupings of lentgh 2, 4, 8 and 16
//		 0001 0203 0405 ...
//		 00010203 04050607 ...
// TODO: Implement header
// TODO: Implement spacer blanks, both in hex and text
// TODO: Polish off the rest of the TODO:s

namespace LRS {
	public class HexDump {
		public Options	Opts		{ get; set; }

		private StringBuilder sbAddress;
		private StringBuilder sbHex;
		private StringBuilder sbText;
		private StringBuilder sbLine;

		// We'll copy over our initial parms to here and use them throughout. This will
		// avoid a lot of "ref" parameters to ensure that things like Length get 
		// updated cleanly.

		byte[]  buf;
		ulong   Address;
		uint    StartIndex;                     // TODO: Rename to Offset?
		uint    Length;
		uint    Align;

		private int AddressLength;

//---------------------------------------------------------------------------------------

		public HexDump() {
			sbAddress = new StringBuilder();
			sbHex     = new StringBuilder();
			sbText    = new StringBuilder();
			sbLine    = new StringBuilder();
		}
//---------------------------------------------------------------------------------------

		public IEnumerable<string> Dump(byte[] buf, uint DumpLength, ulong BaseAddress = 0, uint Align = 16, uint InitialStartIndex = 0) {
			this.buf        = buf;
			Address         = BaseAddress + InitialStartIndex;
			this.Length     = DumpLength;
			this.Align      = Align;
			this.StartIndex = InitialStartIndex;

			AddressLength = GetAddressLength(BaseAddress + (uint)Length);

			if (IsOn(Options.ShowHeader)) {
				// TODO: yield return header line (based on Limit
			}

			ulong AddressRoundedUp  = (Address + Align - 1) / Align * Align;
			uint LengthOfFirstLine  = (uint)(AddressRoundedUp - Address);
			if (LengthOfFirstLine > 0) {
				DoFirstLine(LengthOfFirstLine);
				yield return UnitePieces();
			}

			int NumberOfFullLines = (int)((Length - LengthOfFirstLine) / Align);
			var lines             = DoMiddleLines(NumberOfFullLines);
			foreach (var line in lines) {
				yield return UnitePieces();
			}

			string Last = DoLastLine();
			yield return Last;
		}

//---------------------------------------------------------------------------------------

		private void DoFirstLine(uint Length) {
			CreateLine(Length);

			StartIndex	+= Length;
			Address		+= Length;
			this.Length -= Length;
		}

//---------------------------------------------------------------------------------------

		private IEnumerable<string> DoMiddleLines(int NumberOfFullLines) {
			for (int i = 0; i < NumberOfFullLines; i++) {
				CreateLine(Align);
				yield return UnitePieces();

				StartIndex += Align;
				Address	   += Align;
				Length	   -= Align;
			}
		}

//---------------------------------------------------------------------------------------

		private string DoLastLine() {
			// TODO: Should maybe return IEnumerable<sting>, even though there's only
			//		 one line (just to be consistent with DoMiddleLines)
			CreateLine(Length);
			return UnitePieces();
		}

//---------------------------------------------------------------------------------------

		private void CreateLine(uint Length) {
			NewLine();
			ShowAddress(Address);
			ShowHex(Length);
			ShowText(Length);
		}

//---------------------------------------------------------------------------------------

		private void NewLine() {
			sbAddress.Clear();
			sbHex.Clear();
			sbText.Clear(); sbText.Append("  ");
		}

//---------------------------------------------------------------------------------------

		private string UnitePieces() {
			sbLine.Clear();
			sbLine.Append(sbAddress.ToString());
			sbLine.Append(sbHex.ToString());
			sbLine.Append(sbText.ToString());
			return sbLine.ToString();
		}

//---------------------------------------------------------------------------------------

		private int GetAddressLength(ulong LastAddress) {
			// Consider the simple case of calling Dump() with
			// a BaseAddress of 0. Without this routine this
			// first line would show an address of "0:  ", but
			// subsequent lines would show, say, "10:  " and so
			// on, throwing things out of alignment. So take the
			// highest address we're going to display and see
			// how many hex digits we'll need.

			// So, given a ulong (LastAddress), find out how many
			// hex digits are required to display it.

			// I'd *like* to be able to use the x86 
			// BitScanForward / BitScanReverse opcode(s). but, not
			// being portable, C# doesn't have an intrinsic
			// function for them. So we'll just have to brute force it

			// Note: Just to make things pretty, I want to have all 
			// addresses as an even number of nibbles.

			int Length = 16;
			while ((LastAddress & 0xF000000000000000ul) == 0) {
				Length -= 2;				
				LastAddress <<= 8;
			}
			return Length;
		}

		//---------------------------------------------------------------------------------------

		public void ShowAddress(ulong Address) {
			if (!IsOn(Options.ShowAddress)) {
				return;
			}

			if (Length > 0) {
				sbAddress.Append($"{Address:X}".PadLeft(AddressLength, '0'))
					 .Append(":  ");
			}

		}

//---------------------------------------------------------------------------------------

		private void ShowHex(uint Length) {
			if (! IsOn(Options.ShowHex)) {
				return;
			}
			if ((Length < Align) && ((Address % Align) != 0)) {
				// TODO: next line doesn't account for extra pad blanks
				int PadWidth = (int)(Align - Length) * 3;
				sbHex.Append(string.Empty.PadRight(PadWidth, ' '));
			}
			uint Offset = StartIndex;
			// TODO: Put in spaces every 8 bytes or so
			for (uint i = Offset; i < Offset + Length; i++) {
				sbHex.Append($"{buf[i]:X2} ");
			};
			if ((Length < Align) && ((Address % Align) == 0)) {
				// TODO: next line doesn't account for extra pad blanks
				int PadWidth = (int)(Align - Length) * 3;
				sbHex.Append(string.Empty.PadRight(PadWidth, ' '));
			}

		}

//---------------------------------------------------------------------------------------

		private void ShowText(uint Length) {
			if (!IsOn(Options.ShowText)) {
				return;
			}
			if ((Length < Align) && ((Address % Align) != 0) ){
				// TODO: next line doesn't account for extra pad blanks
				int PadWidth = (int)(Align - Length);
				sbHex.Append(string.Empty.PadRight(PadWidth, ' '));
			}
			uint Offset = StartIndex;
			if (!IsOn(Options.ShowText)) {
				return;
			}
			// TODO: Put in spaces every 8 bytes or so
			for (uint i = Offset; i < Offset + Length; i++) {
				char c = (char)buf[i];
				if (char.IsControl(c)) {
					sbText.Append('.');
				} else {
					sbText.Append(c);
				}
			}
		}

//---------------------------------------------------------------------------------------

		// For single bits only, or bunches of bits in toto
		private bool IsOn(Options bit) => (Opts & bit) == bit;

//---------------------------------------------------------------------------------------

		public HexDump ShowHeader() {
			Opts |= Options.ShowHeader;
			return this;
		}

//---------------------------------------------------------------------------------------

		public HexDump HideHeader() {
			Opts &= ~Options.ShowHeader;
			return this;
		}

//---------------------------------------------------------------------------------------

		public HexDump ShowAddress() {
			Opts |= Options.ShowAddress;
			return this;
		}

//---------------------------------------------------------------------------------------

		public HexDump HideAddress() {
			Opts &= ~Options.ShowAddress;
			return this;
		}

//---------------------------------------------------------------------------------------

		public HexDump ShowHex() {
			Opts |= Options.ShowHex;
			return this;
		}

//---------------------------------------------------------------------------------------

		public HexDump HideHex() {
			Opts &= ~Options.ShowHex;
			return this;
		}

//---------------------------------------------------------------------------------------

		public HexDump ShowText() {
			Opts |= Options.ShowText;
			return this;
		}

//---------------------------------------------------------------------------------------

		public HexDump HideText() {
			Opts &= ~Options.ShowText;
			return this;
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		[Flags]
		public enum Options : ushort {
			None         = 0x0000,
			ShowHeader   = 0x0001,
			ShowAddress  = 0x0002,			// Show Address
			ShowHex      = 0x0004,			// Show data in hex
			ShowText     = 0x0008,			// *a..$.x.* on right
			// TODO: Remove next option. I don't think we need it any more
			// AddressAlign = 0x0010,		// Address is always a multiple of <Align>
		}
	}

}
