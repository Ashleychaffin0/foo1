// Copyright (c) 2005-2006 Bartizan Connects LLC

using System;
using System.Collections.Generic;
using System.Text;

namespace Bartizan.Utils.CRC {

	internal class BartCRC {
		static bool bTableIsInitialized = false;
		private uint CrcHighbit;
		private static uint[] Crc32_Table = new uint[256];
		private uint crc;

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

		public void Reset() {
			crc = 0xFFFFFFFF;
		}

//---------------------------------------------------------------------------------------

		public void AddData(byte[] p) {
			for (int i = 0; i < p.Length; i++) {
				crc = (crc >> 8) ^ Crc32_Table[(crc & 0xff) ^ p[i]];
			}
		}


//---------------------------------------------------------------------------------------

		public void AddData(string str) {
			byte[] rawBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
			AddData(rawBytes);
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
