// Copyright (c) 2006 Bartizan Connects LLC

using System;
using System.Text;

namespace Bartizan.Utils {
	//	$$ Duplicate
	public class Base_N {
		protected string Alphabet;
		protected uint AlphaLen;
		protected bool IsCaseInsensitive;

//---------------------------------------------------------------------------------------

		public Base_N()
			: this("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ") {	// Base 36
		}

//---------------------------------------------------------------------------------------

		public Base_N(string Alphabet, bool IsCaseSensitive) {
			MyCtor(Alphabet, IsCaseSensitive);
		}

//---------------------------------------------------------------------------------------

		public Base_N(string Alphabet)
			: this(Alphabet, false) {
		}

//---------------------------------------------------------------------------------------

		// NOTE!!! Using this ctor with n=64 does *not* give the exact same results as
		// Convert.ToBase64String. See the Convert documentation for details.
		public Base_N(int n, bool IsCaseSensitive) {
			if ((n < 2) || (n == 63) || (n > 64)) {
				throw new ArgumentException("Base_N must be in the range 2..62, or 64");
			}
			if (n < 64) {
				string Alpha = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
				MyCtor(Alpha.Substring(0, n), IsCaseInsensitive);
			} else {
				// Note a different ordering than what, say, base 16 uses.
				string Alpha64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
				MyCtor(Alpha64.Substring(0, n), IsCaseInsensitive);
			}
		}

//---------------------------------------------------------------------------------------

		// NOTE!!! Using this ctor with n=64 does *not* give the exact same results as
		// Convert.ToBase64String. See the Convert documentation for details.
		public Base_N(int n)
			: this(n, false) {
		}

//---------------------------------------------------------------------------------------

		protected void MyCtor(string Alphabet, bool IsCaseSensitive) {
			this.Alphabet = Alphabet;
			AlphaLen = (uint)Alphabet.Length;
			this.IsCaseInsensitive = IsCaseSensitive;
		}

//---------------------------------------------------------------------------------------

		public string ToString(uint n, int nLength) {	// Don't support -ve nums
			// Note: nLength is the minimum length (left-padded with zeros). However if
			//		 the value <n> is too large, the result can be longer than this.
			// Note: For no padding, pass in nLength = 0
			StringBuilder sb = new StringBuilder();
			while (n != 0) {
				uint i = n % AlphaLen;
				sb.Insert(0, Alphabet[(int)i]);
				n /= AlphaLen;
			}
			string s = sb.ToString();
			s = s.PadLeft(nLength, '0');
			return s;
		}

//---------------------------------------------------------------------------------------

		public string ToString(uint n) {
			return ToString(n, 8);
		}

//---------------------------------------------------------------------------------------

		public string ToString(int n) {
			return ToString((uint)n);
		}

//---------------------------------------------------------------------------------------

		public string ToString(int n, int nLength) {
			return ToString((uint)n, nLength);
		}

//---------------------------------------------------------------------------------------

		public int ToInt(string s) {
			int val = 0;
			string Save_s = s;
			string UseAlphabet = Alphabet;
			if (!IsCaseInsensitive) {
				s = s.ToUpper();
				UseAlphabet = Alphabet.ToUpper();
			}
			unchecked {
				int ScaleFactor = 1;
				for (int n = s.Length - 1; n >= 0; n--) {
					int i = UseAlphabet.IndexOf(s[n]);
					if (i == -1) {
						string msg;
						msg = string.Format("Base_N:ToInt - Invalid character ({0}"
							+ ") found in string '{1}'.", s[n], Save_s);
						if (IsCaseInsensitive) {
							msg += " Remember that this instance is case sensitive.";
						}
						throw new ArgumentException(msg);
					}
					val += i * ScaleFactor;
					ScaleFactor *= (int)AlphaLen;
				}
			}
			return val;
		}
	}
}