// Copyright (c) 2007 by Larry Smith

using System;

namespace Boondog2009 {
	class BogWord {
		public string		word;
		public byte []		TrackLetters;

//---------------------------------------------------------------------------------------

		public BogWord(string word, byte[] TrackLetters, int TrackLen) {
			this.word = word;
			this.TrackLetters = new byte[TrackLen];
			Array.Copy(TrackLetters, this.TrackLetters, TrackLen);
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return word;
		}
	}
}
