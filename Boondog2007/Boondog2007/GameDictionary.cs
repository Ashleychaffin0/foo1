// Copyright (c) 2007 by Larry Smith

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Boondog2009 {
    class GameDictionary {

        // The format of the Game Dictionary (Boondog.dct) was influenced by performance
        // reasons, back in the 66MHz 486 days. We'll keep this format, in part because
        // we have asperations of this running on a Windows CE device, which might need
        // some of these techniques to achieve adequate performance.
        //  1)  The beginning of the file has the Header. See the DictHeader class below
        //      for its format. There are 26 headers, one for each letter. Each entry
        //      gives the offset into the file where the words starting with that letter
        //      are. The entry also tells how long the entry is. The goal here is to be
        //      able to quickly narrow in on just the words that are relevant, based on
        //      the first letter. 
        //      
        //      For example (in the current (and also original) dictionary), the 4th
        //      entry (for letter "D"), at offset 0x18, has an offset of 0x6C61 and a
        //      length of 0x1EED.
        //      
        //      Since each entry is contains two 4-byte fields, the header consists of
        //      the first 26 * 4 * 2 (208 = 0xD0) bytes.
        //  2)  Except that (back in the old(e) days), that wasn't quick enough. So we
        //      followed the Header with a sub-dictionary (SubDict). This is a 26*26
        //      array of offsets, for letter pairs AA, AB, ... AZ, BA, BB, ... ZZ.
        //      SubDict[x, y] is the 16 bit offset into letter x (as defined in the main
        //      DictHeader), with second letter y. 

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

        public class DictHeader {
            internal int LetterEntryOffset;
            internal int LetterEntryLength;

//---------------------------------------------------------------------------------------

            internal DictHeader(int LetterEntryOffset, int LetterEntryLength) {
                // Note: The original VB (well, QuickBasic) program that built this file
                //       used Origin 1 for offsets. So subtract 1 to get into more
                //       modern values.
                this.LetterEntryOffset = LetterEntryOffset - 1;
                this.LetterEntryLength = LetterEntryLength;
            }
        }

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
        
        private const int AlphabetSize = 26;    // Against the day when we might, just
                                                //   conceivably, support non-English
                                                //   dictionaries, with, say, accented
                                                //   letters. At which point this either
                                                //   has to be passed to the ctor, or
                                                //   better yet, as part of the 
                                                //   dictionary file header.

        DictHeader []   DictHdr;
        ushort[,]       SubDict;
        byte[][]        LetterEntries;          // One for each letter
        StringBuilder   CurWord;
        Encoding        ASC;

//---------------------------------------------------------------------------------------

        public GameDictionary(string DictFilename) {
            FileStream fs = new FileStream(DictFilename, FileMode.Open);    // May throw
            BinaryReader rdr = new BinaryReader(fs);
            byte[] buf;

            // Read in DictHdr
            DictHdr = new DictHeader[AlphabetSize];
            // We know that (at least the current dictionary format; I didn't have the
            //   foresight to put a Version ID at the front of the file. Sigh.) the
            //   Header consistes of 26 pairs of 32-bit integers. So hardcode that.
            buf = new byte[AlphabetSize * 2 * 4];
            rdr.Read(buf, 0, buf.Length);
            for (int i = 0; i < AlphabetSize; i++) {
                int offset = BitConverter.ToInt32(buf, i * 2 * 4);
                int length = BitConverter.ToInt32(buf, i * 2 * 4 + 4);
                DictHdr[i] = new DictHeader(offset, length);
            }

            // OK, now read in the SubDict's
            // The SubDict is an 2-D array, 26*26
            buf = new byte[AlphabetSize * AlphabetSize * 2];    // We know a ushort is 2 bytes
            rdr.Read(buf, 0, buf.Length);
            SubDict = new ushort[AlphabetSize, AlphabetSize];
            for (int letter1 = 0; letter1 < AlphabetSize; ++letter1) {
                for (int letter2 = 0; letter2 < AlphabetSize; ++letter2) {
                    ushort ofs = BitConverter.ToUInt16(buf, letter1 * AlphabetSize * 2 + letter2 * 2);
                    // TODO: Comment next line. See above.
                    SubDict[letter1, letter2] = --ofs;
                }
            }

            // Now, finally, the letter entries, one for each letter. THis will contain
            //  the (compressed) data for each word that starts with the corresponding
            //  letter.
            LetterEntries = new byte[AlphabetSize][];
            for (int i = 0; i < AlphabetSize; ++i) {
                LetterEntries[i] = new byte[DictHdr[i].LetterEntryLength];
                rdr.Read(LetterEntries[i], 0, LetterEntries[i].Length);
            }

            // Clean up
            rdr.Close();
            fs.Close();

            // Used to optimize FindWord
            CurWord = new StringBuilder();

            ASC = Encoding.ASCII;

#if false
            // Some debug code, just to dump things and verify that things look OK.
            byte[] buftest = LetterEntries[2];      // Letter 'C'
            for (int i = 0; i < AlphabetSize; ++i) {
                Console.WriteLine("First bytes for C{0}... ", (char)('A' + i));
                ushort ofs = SubDict[2, i];
                for (int j = 0; j < 16; ++j) {
                    // Note: May fail if there aren't enough bytes for this SubDict entry. Big
                    //       deal. This is only debug code, anyway.
                    Console.Write("{0:X2} ", buftest[ofs + j]);
                }
                Console.WriteLine();
            }
#endif
        }

//---------------------------------------------------------------------------------------

		// Just to practice yield statements
		public IEnumerator<DictHeader> GetEnumerator() {
			foreach (DictHeader hdr in DictHdr) {
				yield return hdr;
			}
		}

//---------------------------------------------------------------------------------------

		public byte[] GetSubDict(char FirstLetter, char SecondLetter, out int ofs) {
			// We assume that all words are in upper case, except possibly the 'u'
			// in "Qu". (We think the board is prettier if it displays Qu rather than
			// QU.) Make a special check for that
			int ix1 = FirstLetter  - 'A';
			int	ix2;
			if (SecondLetter == 'u') {
				ix2 = 'U' - 'A';
			} else {
				ix2 = SecondLetter - 'A';
			}
            byte[] buf = LetterEntries[ix1];		// Entries for word's first letter
			ofs = SubDict[ix1, ix2];
			return buf;
		}

//---------------------------------------------------------------------------------------

		public byte[] GetSubDict(string word, out int ofs) {
            // We assume that word.Length >= 2 (and realistically, >= 3).
			return GetSubDict(word[0], word[1], out ofs);
		}

//---------------------------------------------------------------------------------------

        public bool FindWord(string word, out bool bMorePossibleWords) {
			int		ofs;
			byte[]	buf = GetSubDict(word, out ofs);// Entries for word's first letter
            int WordLen = word.Length;

            CurWord.Length = 0;						// Reset/empty the StringBuilder
            CurWord.Append(word[0]);

			bMorePossibleWords = false;			    // Be pessimistic

			// The format of each entry is as follows:
			//	[0] - The number of chars from the previous word to take (the prefix)
			//	[1] - The number of following characters to take
			//	[2..] - The suffix
			// For example, assume that the previous word was CITED and that we want
			// the next entries to be CITES, CITIZEN, CIVET, we would have
			//	4, 1, "S"		-- 4 from CITED (i.e. CITE), + S to give CITES
			//	3, 4, "IZEN"	-- 3 from CITES (i.e. CIT), + IZEN to give CITIZEN
			//	2, 3, "VET"		-- 2 from CITIZEN (i.e. CI), + VET to give CIVET
			// The last entry is a dummy one, with [2] = 0xff to indicate the end.
			// The entries are, by construction, in ascending alphbetical order.

            while (true) {
                // The last entry has a "string" value of 0xff. Check for that
				if (buf[ofs + 2] == 0xff) {
					return false;
				}
                int PrefLen = buf[ofs + 0];
                int Len = buf[ofs + 1];
                string  CurSuffix = ASC.GetString(buf, ofs + 2, Len);
                CurWord.Length = PrefLen;
                CurWord.Append(CurSuffix);
                string  CurString = CurWord.ToString();
				if (word == CurString) {
					bMorePossibleWords = true;
					return true;
				}
                if (word.CompareTo(CurString) < 0) {
					// To use the example that showed up during testing, if <word> is CIT
					// and CurString is CITABLE, we want to return false for CIT, but set
					// the bMorePossibleWords flag to true to tell our caller that if
					// they were to add on another letter (or more), things might yet
					// work out. This feature is, of course, used when the program is
					// trying to find words in the grid.
					if (string.CompareOrdinal(word, 0, CurString, 0, WordLen) == 0)
						bMorePossibleWords = true;
                    return false;
				}
                ofs += 2 + Len;         // 2 = 1 for Preflen + 1 for Len
            }
        }
    }
}
