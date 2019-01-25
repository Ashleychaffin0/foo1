using System;
using System.Collections;

namespace LRS {
	/// <summary>
	/// Summary description for ParseArgs.
	/// </summary>
	public class ParseArgs {

		string		Prefixes;				// e.g. "-/"

		ArrayList	MyArgs;
		Hashtable	NamedArgs;
		ArrayList	Keywords;

//---------------------------------------------------------------------------------------

		public ParseArgs(string Prefixes)	{
			if (Prefixes == null || Prefixes.Length == 0) {
				// TODO: What I really want to do is to throw an exception. But for now,
				// we'll default to "-". Hmmm. Isn't there a default system flag 
				// character defined somewhere?
				Prefixes = "-";
			}
			this.Prefixes = Prefixes;
			Reset();
		}

//---------------------------------------------------------------------------------------

		public void Reset() {
			MyArgs	  = new ArrayList();
			NamedArgs = new Hashtable();
			Keywords  = new ArrayList();
		}

//---------------------------------------------------------------------------------------

		public void Add(params ParseArgsKeyword[] keywords) {
			// TODO: Check to see if the keyword already exists
			foreach (ParseArgsKeyword keyword in keywords)
				Keywords.Add(keyword);
		}

//---------------------------------------------------------------------------------------

		public void Parse(string [] args) {
			string		arg;

			for (int i=0; i<args.Length; ++i) {
				arg = args[i];
				if (arg.Length == 0) {		// User could have "" on command line
					MyArgs.Add(arg);
					continue;
				}
				if (Prefixes.IndexOf(arg[0]) == -1) {
					MyArgs.Add(arg);
				} else {
					ProcessNamedArg(arg, ref i);
				}
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessNamedArg(string arg, ref int i) {

		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class ParseArgsKeyword {
		string		CmdlineFlag;			// e.g. the "f" in /f
		string		HashName;				// e.g. "file". Note that this allows
											//      command line synonyms, such as
											//		/f and /file
		string		DefaultValue;			// May also be null
		Flags		flags;
		string		MultiStateSuffixes;		// e.g. /x+, /x-, /x? -> "+-?"
		// TODO: Maybe an optional delegate on a parm-by-parm basis?

//---------------------------------------------------------------------------------------

		public ParseArgsKeyword(string CmdlineFlag, string HashName, string DefaultValue, 
						 Flags flags, string MultiStateSuffixes) {
			this.CmdlineFlag		= CmdlineFlag;
			this.HashName			= HashName;
			this.DefaultValue		= DefaultValue;
			this.flags				= flags;
			this.MultiStateSuffixes	= MultiStateSuffixes;
		}
	}

//---------------------------------------------------------------------------------------

	[Flags]
	public enum Flags {
		TakesParm		= 0x00000001,		// e.g. /f filename.txt
		MultiState		= 0x00000002,		// e.g. /x+, /x-, /x?
		CanRepeat		= 0x00000004,		// e.g. /f file1 /f file2
		TakeRestOfLine	= 0x00000008		// e.g. /comment Hi there
	}

}
