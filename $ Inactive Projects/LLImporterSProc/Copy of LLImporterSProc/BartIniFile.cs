// Copyright (c) 2003-2006 by Bartizan Data Systems, LLC

using System;
using System.IO;
using System.Text;

namespace Bartizan.Importer {
	/// <summary>
	/// A BartIniFile is a primarily a text setup file, with just a bit of structure. This is
	/// essentially that of an .ini file, except that there can be a bit of stuff at the
	/// beginning of the file that is not in any section. We assume that any line that 
	/// starts with "[" in column 1 begins a section. Sections are usually separated by 
	/// an empty line, but that isn't required. Our Readline routine will return an EOF 
	/// indication (i.e. null) if it finds a real EOF, or if it finds a leading "[".
	/// <p/>
	/// In particular, a BartIniFile knows <ul>nothing</ul> about its contents (other than
	/// the [section] headers). So, for example, while it may recognize a section named,
	/// say, [SURVEY], it has no idea what the contents of the Survey section looks like.
	/// That's the job of the Survey class. 
	/// <p/>
	/// And if we ever add a new section to the setup file, this class doesn't have to
	/// change. 
	/// <p/>
	/// Alternatively, if we have a setup file with sections that a given application
	/// isn't interested in, nobody will ask the SetupFile for that section.
	/// <p/>
	/// Finally, just to try to make things as clear as possible, there's a difference
	/// between a setup <i>file</i>, and setup <i>data</i>. The former is this. The latter
	/// is the class that presumably holds the data extracted from the setup file. It's
	/// that class that has vectors of Survey's and Demographics' etc.
    /// <p/>
    /// Note: Initially this class was called SetupFile, but we realized that as time
    /// went on, we might have other types of setup files (e.g. XML format), so we veered
    /// away from the overly generic FieldName.
	/// </summary>
	public class BartIniFile {			    
		public string	filename;		// Name of the setup file, in case we want it for
										//   status or error messages.
		StreamReader	sr;
		bool			bEOF;			// Real or pseudo EOF hit

		// Pseudo-FieldName for getting at stuff before the first "[".
		public static readonly string	FrontName = "*Front*";

//---------------------------------------------------------------------------------------

        public BartIniFile(string filename) {
			this.filename = filename;
			// The following may fail on an IOException if the file isn't found.
			// Or it may be opened by someone else in exclusive mode.
			// Or whatever.
			// Too bad. Let the caller catch it.
			FileStream	fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			sr = new StreamReader(fs);
            CommonCtorProcessing();			
		}

//---------------------------------------------------------------------------------------

        public BartIniFile(byte[] buf) {
            // TODO: All initial testing will be done with the other ctor(string fname).
            //       Make sure we come back and ensure that this works as well.
            this.filename = "*In memory byte[] buffer*";
            MemoryStream ms = new MemoryStream(buf);
            sr = new StreamReader(ms, true);
            CommonCtorProcessing();
        }

//---------------------------------------------------------------------------------------

		public BartIniFile(MemoryStream ms) {
            // TODO: All initial testing will be done with the other ctor(string fname).
            //       Make sure we come back and ensure that this works as well.
			this.filename = "*In memory MemoryStream*";
            sr = new StreamReader(ms, true);
            CommonCtorProcessing();
		}

//---------------------------------------------------------------------------------------

        void CommonCtorProcessing() {
            bEOF = true;                // Can't read until successful FindSection
        }

//---------------------------------------------------------------------------------------

		public void Close() {
			if (sr != null) {
				sr.Close();
				sr = null;
			}
		}

//---------------------------------------------------------------------------------------

		public bool FindSection(string name) {
			string		s;

			sr.BaseStream.Position = 0;						// Seek to front
			// sr.BaseStream.Seek(0, SeekOrigin.Begin);		// Alternate way to seek
			sr.DiscardBufferedData();		// Keep this stream in synch with
											// the underlying FileStream.
			// Support access to the stuff before the first "[" with a pseudo-FieldName
			if (string.Compare(name, FrontName, true) == 0) {
				bEOF = false;
				return true;
			}
			name = "[" + name.Trim() + "]";
			while ((s = sr.ReadLine()) != null) {
				// Note: Don't use s.Trim(), since we consider a section to start
				//		 when there's a [ in exactly column 1.
				if (s.StartsWith("[")) {
					// Do case-insensitive compare. So don't write: if (s == FieldName)
					if (string.Compare(s, name, true) == 0) {
						// Note: At this point, the file is positioned for reading
						//		 the section.
						bEOF = false;
						return true;
					}
				}
			}
			bEOF = true;
			return false;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// We assume this is called after a successful call to FindSection. It will
		/// return each line in turn, stopping at a new section.
		/// <p/>
		/// We keep a flag that says whether we've hit "EOF" (i.e. real EOF, or just the
		/// start of the next section, and won't allow a further read without "reopening"
		/// the file with another call to FindSection.
		/// </summary>
		/// <returns></returns>
		public string ReadLine() {
			if (bEOF)					// If hit EOF, or new section
				return null;
			try {
				string	s = sr.ReadLine();
				if (s == null) {				// Real EOF
					bEOF = true;
					return null;
				}
				if (s.StartsWith("[")) {
					bEOF = true;
					return null;
				}
				return s;
			} catch (Exception) {
				// I'd have to check what we get on EOF vs anything else. What I'd
				// really like to check for here is just EOF, and anything else we'll
				// let the exception percolate. But for now... TODO:
				bEOF = true;
				return null;
			}
		}
	}
}
