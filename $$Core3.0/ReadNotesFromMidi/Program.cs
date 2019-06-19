using System;

namespace ReadNotesFromMidi {
	class Program {
		const string MidiFilename = @"C:\Program Files (x86)\Microsoft Office\root\CLIPART\PUB60COR\FINCL_01.MID";
		const string MidiFilename2 = @"G:\Downloads\Calvin Harris - My Way  (midi by Carlo Prato) (www.cprato.com).mid";

		static void Main(string[] args) {
			var mid = new LrsMidi(MidiFilename2);
		}
	}
}
