using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bartizan.Utils.CRC;

namespace ConsoleApplication2 {
	class Program {
		static void Main(string[] args) {

			string Beth = "ADDRESS LINE1=23 FOREST RD¶ADDRESS2=¶BUSINESS=3061.11¶CARD #=008510331¶CITY=WESTON¶COMPANY=CORPLOGOWARE¶COUNTRY=USA¶E_MAIL=BETHKRANE@CORPLOGOWARE.COM¶FAX=(203) 454-1079¶FIRST NAME=BETH¶JOB=1419¶LAST NAME=KRANE¶PHONE=(203) 454-1079¶POSTAL CODE=¶PROVINCE=¶SHOW=NYG¶STATE=CT¶ZIP=068832307¶";
			string Judy = "ADDRESS LINE1=¶ADDRESS2=¶BUSINESS=1622¶CARD #=D04624474¶CITY=¶COMPANY=JAYMAR ENTERPRISES¶COUNTRY=¶E_MAIL=¶FAX=(   )    -¶FIRST NAME=JUDY¶JOB=1404¶LAST NAME=DAVIDSON¶PHONE=(   )    -¶POSTAL CODE=¶PROVINCE=¶SHOW=NYG¶STATE=PA¶ZIP=19335¶";
			// Beth = Beth.Replace('¶', '?');
			// Judy = Judy.Replace('¶', '?');

			BartCRC BethCRC = new BartCRC();
			BartCRC JudyCRC = new BartCRC();
			BartCRC	HelloCRC = new BartCRC();

			BethCRC.AddData(Beth);
			JudyCRC.AddData(Judy);
			HelloCRC.AddData("hello");

			long b = BethCRC.GetCRC();
			long j = JudyCRC.GetCRC();
			long h = HelloCRC.GetCRC();
			Console.WriteLine("Beth CRC = {0} / {0:x8}", b);
			Console.WriteLine("Judy CRC = {0} / {0:x8}", j);
			Console.WriteLine("Hello CRC = {0} / {0:x8}", h);
		}
	}
}

// Copyright (c) 2005-2008 Bartizan Connects LLC

namespace Bartizan.Utils.CRC {

	internal class BartCRC {
		static bool				bTableIsInitialized = false;
		private uint			CrcHighbit;
		private static uint[]	Crc32_Table = new uint[256];
		private uint			crc;
		private uint			crcRev;		// CRC calculated by scanning the data
											//   right-to-left (aka Reversed).
											//	 Note: What I would have liked to have
											//	 done was to name this "crc" spelled
											//	 backwards. Uh, but "crc" is palindromic!
											//	 So we have to settle for "crcRev".

//---------------------------------------------------------------------------------------

		public BartCRC() {
			CrcHighbit = unchecked(1u << 31);
			// Generate lookup table
			Init_CRC32_Table();
			Reset();
		}

//---------------------------------------------------------------------------------------

		public uint GetCRC() {
			return crc ^= 0xFFFFFFFF;
		}

//---------------------------------------------------------------------------------------

		public uint GetCRCReversed() {
			return crcRev ^= 0xFFFFFFFF;
		}

//---------------------------------------------------------------------------------------

		public long GetQuasiCRC64() {
			// We'll fake a (sort of) CRC 64 by combining the forward and reverse CRC
			uint	Crc1  = crc		^= 0xFFFFFFFF;
			ulong	Crc2  = crcRev	^= 0xFFFFFFFF;
			ulong	Crc64 = (Crc2 << 32) | Crc1;	// Glue them together
			return unchecked((long)Crc64);
		}

//---------------------------------------------------------------------------------------

		public void Reset() {
			crc = 0xFFFFFFFF;
			crcRev = 0xFFFFFFFF;
		}

//---------------------------------------------------------------------------------------

		public void AddData(byte[] p) {
			for (int i = 0; i < p.Length; i++) {
				crc = (crc >> 8) ^ Crc32_Table[(crc & 0xff) ^ p[i]];
			}
			for (int i = p.Length - 1; i >= 0; i--) {
				crcRev = (crcRev >> 8) ^ Crc32_Table[(crcRev & 0xff) ^ p[i]];
			}
		}

//---------------------------------------------------------------------------------------

		public void AddData(string str) {
			byte[] rawBytes = System.Text.UnicodeEncoding.Unicode.GetBytes(str);
			// byte[] rawBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
			byte [] rawbytes2 = new byte[rawBytes.Length / 2];
			for (int i = 0; i < rawbytes2.Length; i++) {
				rawbytes2[i] = rawBytes[i * 2];
			}
			// AddData(rawBytes);
			AddData(rawbytes2);
		}

//---------------------------------------------------------------------------------------

		private uint Reflect(uint Crc, int Bitnum) {
			uint i, j = 1, CrcOut = 0;

			for (i = 1u << (Bitnum - 1); i != 0; i >>= 1) {
				if ((Crc & i) != 0) {
					CrcOut |= j;
				}
				j <<= 1;
			}
			return CrcOut;
		}

//---------------------------------------------------------------------------------------

		private void Init_CRC32_Table() {

			if (bTableIsInitialized)
				return;

			uint ulPolynomial = 0x4c11db7;

			uint i, j;
			uint bit, crc;

			for (i = 0; i < 256; i++) {
				crc = i;
				crc = Reflect(crc, 8);
				crc <<= 32 - 8;

				for (j = 0; j < 8; j++) {
					bit = crc & CrcHighbit;
					crc <<= 1;
					if (bit != 0)
						crc ^= ulPolynomial;
				}

				crc = Reflect(crc, 32);
				crc &= 0xFFFFFFFF;
				Crc32_Table[i] = crc;
			}

			bTableIsInitialized = true;
		}
	}
}

