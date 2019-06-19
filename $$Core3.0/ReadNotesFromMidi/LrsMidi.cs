using System;
using System.IO;
using System.Text;

// The format of a MIDI file comes from
// https://www.csie.ntu.edu.tw/~r92092/ref/midi/#mff0
// http://www.music.mcgill.ca/~ich/classes/mumt306/StandardMIDIfileformat.html

namespace ReadNotesFromMidi {
	internal class LrsMidi {
		private string midiFilename;

//---------------------------------------------------------------------------------------

		public LrsMidi(string midiFilename) {
			this.midiFilename = midiFilename;
			using var inFile = new BinaryReader(File.OpenRead(midiFilename));
			long len = inFile.BaseStream.Length;
			while (inFile.BaseStream.Position != len) {
				ReadChunk(inFile);
			}
		}

//---------------------------------------------------------------------------------------

		private void ReadChunk(BinaryReader infile) {
			var pos = infile.BaseStream.Position;
			byte[] ChunkName = infile.ReadBytes(4);
			int length = ReadBigEndianInt32(infile);
			byte[] data = infile.ReadBytes(length);
			string name = Encoding.ASCII.GetString(ChunkName);
			Console.WriteLine($"[{pos:X8}]: Chunk name={name}, length={length}");
			switch (name) {
			case "MThd":
				ProcessChunkHead(data);
				break;
			case "MTrk":
				ProcessChunkTrack(data);
				break;
			default:
				break;					// Ignore
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessChunkHead(byte[] data) {
			var format = ToUShort(data.AsSpan().Slice(0, 2));
			// Note: Format 1 means a single track
			//		 Format 2 means multiple tracks played simultaneously
			//		 Format 3 means multiple independent tracks
			var tracks = ToUShort(data.AsSpan().Slice(2, 2));	// # of tracks
			var TicksPerQuarterNote = ToUShort(data.AsSpan().Slice(4, 2));
			// Note: If TicksPerQuarterNote has the high-order (bit 15, 0x8000) on, then
			//		 this 16-bit value is actually two fields. But I'm ignoring that 
			//		 since the documentation talks about "frames per second", which maybe
			//		 involves syncing a midi file with a video file?			
		}

//---------------------------------------------------------------------------------------

		private void ProcessChunkTrack(byte[] data) {
			int Offset = 0;
			while (Offset < data.Length) {
				Offset += VariableLength(data.AsSpan().Slice(Offset), out int deltaTime);
				Console.WriteLine($"Delta Time = {deltaTime}");
				Offset += ProcessEvent(data.AsSpan().Slice(Offset));
			}
		}

//---------------------------------------------------------------------------------------

		private bool IsEvent(Span<byte> span, params byte[] bytes) {
			for (int i = 0; i < bytes.Length; i++) {
				if (span[i] != bytes[i]) { return false; }
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		private int ProcessEvent(Span<byte> span) {
			// TODO: Make table-driven
			int Offset;
			if (IsEvent(span, 0xff, 0x58, 0x04)) {
				Offset = ProcessEventTimeSignature(span.Slice(3));
				return Offset + 3;		// 3 == length of params (0xFF, 0x58, 0x04)
			}
			if (IsEvent(span, 0xFF, 0x2f, 0x00)) {
				Offset = ProcessEventEndOfTrack(span.Slice(3));
				return Offset + 3;
			}

			if (IsEvent(span, 0xff, 0x51, 0x03)) {
				Offset = ProcessEventSetTempo(span.Slice(3, 3));
				return Offset + 3;
			}

			return int.MaxValue;		// TODO:
		}

//---------------------------------------------------------------------------------------

		private int ProcessEventSetTempo(Span<byte> span) {
			int tempo = ToInt24(span);
			Console.WriteLine($"Event Set Tempo: {tempo}");
			return 3;
		}

//---------------------------------------------------------------------------------------

		private int ProcessEventEndOfTrack(Span<byte> span) {
			Console.WriteLine("Event: End of Track");
			return 0;
		}

//---------------------------------------------------------------------------------------

		private int ProcessEventTimeSignature(Span<byte> span) {
			int SigNumerator = span[0];
			int SigDenominator = span[1];
			int NumClocksPerMetrononeTick = span[2];
			int Num32ndNotesInQuarterNote = span[3];
			Console.WriteLine($"Signature: {SigNumerator}/{SigDenominator}");
			return 4;			// TODO: ???
		}

//---------------------------------------------------------------------------------------

		private ushort ToUShort(Span<byte> span) {
			return (ushort)FromBigEndian(span);
		}

//---------------------------------------------------------------------------------------

		private int ToInt32(Span<byte> span) {
			return (int)FromBigEndian(span);
		}

//---------------------------------------------------------------------------------------

		private int ToInt24(Span<byte> span) {
			return FromBigEndian(span);
		}

//---------------------------------------------------------------------------------------

		private int VariableLength(Span<byte> bytes, out int val) {
			val = 0;
			int n = 0;
			while (true) {
				val <<= 7;
				byte b = bytes[n++];
				val |= b & 0x7f;
				bool bHighBitOff = (b & 0x80) == 0;
				if (bHighBitOff) { return n; }
			}
		}

//---------------------------------------------------------------------------------------

		private int FromBigEndian(Span<byte> span) {
			int val = 0;            // TODO: Maybe should be ulong, not int
			for (int i = 0; i < span.Length; i++) {
				val <<= 8;
				val |= span[i];
			}
			return val;
		}

//---------------------------------------------------------------------------------------

		private int ReadBigEndianInt32(BinaryReader infile) {
			var bytes = infile.ReadBytes(4);
			return FromBigEndian(bytes);
		}
	}
}