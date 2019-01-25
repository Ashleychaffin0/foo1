// TODO: Support single step
// TODO: Show values and type
// TODO: [A##  is A[#] = #
// TODO: V=A[# is V = A[#]
// TODO: Allow longer variable names and integers
// TODO: Floating point???
// TODO: Add Load/Save

// TODO: "The next line should print 7 (as in S=`Z) gives poor error messages

using System;
using System.Collections.Generic;

namespace MyDumbInterpreter {
	class Interpreter {
		// The state of the running program
		int							IP;         // Instruction Pointer
		bool						IsRunning;
		SymbolTable					Symtab;
		string						CurrentLine;

		// For infinite loop detection
		const int MaxOps			= 1_000;	// Arbitrary value
		int NumOps;

		// Lets us access the User Interface
		MyDumbInterpreter			UI;

//---------------------------------------------------------------------------------------

		public Interpreter(MyDumbInterpreter UI) {
			this.UI = UI;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The interpreter itself
		/// </summary>
		/// <param name="Program">The source code</param>
		/// <param name="InitialIP">-1 for new run, else IP for Edit and Continue</param>
		/// <returns>Next line# if stopped at breakpoint, else -1</returns>
		internal int Run(List<string> Program, int InitialIP) {
			IsRunning        = true;

			if (InitialIP == -1) {      // New run, not Edit and Continue
				Symtab = new SymbolTable();
				IP     = 0;
				ScanForLabels(Program);
			} else {
				IP = InitialIP;
			}
			NumOps = 0;					// [Re]start infinite loop detection
			while (IsRunning) {
				if (IP >= Program.Count) {
					Print("End of program reached");
					return -1;
				}

				// Append a blank to the end of the line. Why? It's an invalid character
				// (except for the " operator) and will usually trigger an error message
				// if processed (except for simple assignments like <S=1>). But it also
				// means we can cheat a bit and not have to worry about a subscript out
				// of range trying to process incomplete lines (e.g. <S=1+>.
				CurrentLine = Program[IP] + " ";
				if (NumOps > MaxOps) {
					PrintErr($"Possible infinite loop detected");
					return IP;
				}
				++NumOps;

				char op = CurrentLine[0];
				switch (op) {
					case '*':       // Comment -- Ignore this line
						break;
					case '.':		// Breakpoint
						Print($"Breakpoint reached at line {IP} -- {CurrentLine}");
						return IP + 1;
					case ':':       // Define Label
						// Do nothing. We've already scanned for all labels
						// I suppose we could decrement NumOps, but I'm not gonna bother
						break;
					case '"':       // Print quoted string
						Print(DoBackTick(CurrentLine.Substring(1)));	
						break;
					case '>':       // GOTO Label
									// Syntax is: ><label><relop><varnum1><varnum2>
						DoGoto(CurrentLine.Substring(1));
						break;
					default:
						if (char.IsLetter(op) || char.IsDigit(op)) {
							if (CurrentLine[1] == '=') {
								DoAssignment(CurrentLine);
								break;
							} else {
								Print(Eval(CurrentLine));
								break;
							}
						} else {
							PrintErr($"Unrecognized command: '{op}'");
						}
						break;
				}
				++IP;
			}
			return IP;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Implement the '>' (Goto-If) command.
		/// </summary>
		/// <param name="s"></param>
		/// <syntax>>{label}{relationaloperator}{varnum1}{varnum2}
		/// For example, >LN<5 means Go to label L if N < 5
		/// </syntax>
		private void DoGoto(string s) {
			var SymLabel = GetLabel(s.Substring(0, 1));
			if (SymLabel == null) {
				IsRunning = false;
				return;
			}

			int? Left = GetValue(s[1]);

			(var Cond, int ixNextChar) = GetCC(s);

			int? Right = GetValue(s[ixNextChar]);

			switch (Cond) {
				case ConditionCode.Invalid:	// TODO: Error message already sent?
					break;
				case ConditionCode.LessThanOrEqual:
					if (Left <= Right) IP = (int)SymLabel.Value;
					break;
				case ConditionCode.LessThan:
					if (Left < Right) IP = (int)SymLabel.Value;
					break;
				case ConditionCode.Equal:
					if (Left == Right) IP = (int)SymLabel.Value;
					break;
				case ConditionCode.NotEqual:
					if (Left != Right) IP = (int)SymLabel.Value;
					break;
				case ConditionCode.GreaterThanOrEqual:
					if (Left >= Right) IP = (int)SymLabel.Value;
					break;
				case ConditionCode.GreaterThan:
					if (Left > Right) IP = (int)SymLabel.Value;
					break;
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// We've found a GoTo opcode. Make sure it refers to a valid label
		/// </summary>
		/// <param name="s">The name of the putative label</param>
		/// <returns></returns>
		private Symbol GetLabel(string s) {
			(bool bExists, Symbol Sym) = Symtab.GetSym(s);
			if (!bExists) {
				PrintErr($"Label '{s}' not found");
				return null;
			}
			if (Sym.SymType != Symbol.SymbolType.Label) {
				PrintErr($"Variable '{s}' is not a Label");
				IsRunning = false;
				return null;
			}
			return Sym;
		}

//---------------------------------------------------------------------------------------

		// To be able to branch to a label that we haven't seen yet, do a quick 
		// preliminary scan just for labels and add them to the symbol table
		// Note that for Edit and Continue support, if lines of code are added or deleted
		// then Labels may well move. So we must delete all old Labels first.
		private void ScanForLabels(List<string> Program) {
			Symtab.RemoveAllLabels();
			for (int IP = 0; IP < Program.Count; IP++) {
				String line = Program[IP];
				if (line[0] == ':') {
					bool Duplicate = Symtab.AddLabel(line[1], IP);
					if (Duplicate) {
						Print($"Duplicate label '{line[1]}' at line {IP}");
						IsRunning = false;
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Scan for one of the 6 relations (=, !=, <, <=, >, >=)
		/// </summary>
		/// <param name="s">The text to the right of the '>' operator</param>
		/// <returns>A tuple with the condition code and ixNextChar</returns>
		private (ConditionCode CC, int ixNextChar) GetCC(string s) {
			ConditionCode Cond = ConditionCode.Invalid;
			int ixNextChar = 3;     // Support 1 or 2 character relations. For example,
									// if s is LN>5, then 5 is s.SubString(3, 1). But if
									// the relation is 2 characters (e.g. <=), the this
									// would have to be 4.
			switch (s[2]) {
				case '<':
					if (s[3] == '=') {
						Cond = ConditionCode.LessThanOrEqual;
						++ixNextChar;
					} else {
						Cond = ConditionCode.LessThan;
					}
					break;
				case '=':
					Cond = ConditionCode.Equal;
					break;
				case '>':
					if (s[2] == '=') {
						Cond = ConditionCode.GreaterThanOrEqual;
						++ixNextChar;
					} else {
						Cond = ConditionCode.GreaterThan;
					}
					break;
				case '!':
					if (s[2] == '=') {
						Cond = ConditionCode.NotEqual;
						++ixNextChar;
					} else {
						PrintErr($"Invalid relation: '{s.Substring(2, 2)}'");
					}
					break;
				default:
					PrintErr($"Invalid relation: '{s.Substring(2, 2)}'");
					break;
			}
			return (Cond, ixNextChar);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// We want to be able to display the value of a variable inside a message
		/// defined by the " operator. This supports a backtick (`) character in a
		/// message. The character following it is the name of a field and it inserts
		/// this value into the message. For example: "X and Y are `X and `Y
		/// </summary>
		/// <param name="s">The message</param>
		/// <returns>The message with the value substituted</returns>
		private string DoBackTick(string s) {
			string msg = "";
			while (true) {
				int ix = s.IndexOf('`');
				if (ix == -1) {		 // -1 means "not found"
					msg += s;
					return msg;
				}
				char c = s[ix + 1];
				msg   += s.Substring(0, ix);
				msg   += GetValue(c).ToString();
				s      = s.Substring(ix + 2);
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Implement the assignment statement
		/// </summary>
		/// <param name="line"></param>
		private void DoAssignment(string line) {
			char c = line[0];
			if (! char.IsLetter(c)) {
				PrintErr("Attempt to assign a value to a constant");
				return;
			}
			var NewSym       = Symtab.AddSym(c);
			int? val         = Eval(line.Substring(2));
			NewSym.sym.Value = val;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Evaluate an expression (e.g. X+1)
		/// </summary>
		/// <param name="Expression">The string to be evaluated</param>
		/// <returns>The numeric value of the expression, or null if there's an error</returns>
		private int? Eval(string Expression) {
			// We have several cases:
			// 1) A single variable name or single numeric value
			// 2) NameOrNumber <arithmetic op> NameOrNumber
			// 3) Something else -- error

			// Note: There are sophisticated ways to parse an expression, including ways
			//		 to support priority of operators (e.g. multiplication and division
			//		 before addition and subtraction), parenthesized expressions, 
			//		 function calls (with possible expressions as parameters (including
			//		 possible functions calls!, etc)), subscripting, and maybe other
			//		 things. But in this simple program, we're just going to
			//		 brute-force it.

			char Left     = Expression[0];
			char Operator = Expression[1];
			if (char.IsWhiteSpace(Operator)) {
				// Must be single name or value
				return GetValue(Left);
			} else {            // Expression
				char Right = Expression[2];
				return DoArith(Left, Operator, Right);
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Hey, actually do a calculation! e.g. A+B
		/// </summary>
		/// <param name="Left">Left argument</param>
		/// <param name="Operator">The operation</param>
		/// <param name="Right">Right argument</param>
		/// <returns>The calculation or null if invalid operator</returns>
		private int? DoArith(char Left, char Operator, char Right) {
			int? LeftArg  = GetValue(Left);
			int? RightArg = GetValue(Right);
			switch (Operator) {
				case '+': return LeftArg + RightArg;
				case '-': return LeftArg - RightArg;
				case '*': return LeftArg * RightArg;
				case '/': return LeftArg / RightArg;
			}
			PrintErr($"Invalid operator '{Operator}'");
			return null;
		}

//---------------------------------------------------------------------------------------

		private int? GetValue(char Sym) {
			if (char.IsNumber(Sym)) {
				return Sym - '0';			// Convert (e.g.) '5' to numeric 5
			}
			if (char.IsLetter(Sym)) {
				var SymVal = Symtab.GetValue(Sym.ToString());
				if (!SymVal.bExists) {
					PrintErr($"Symbol '{Sym}' is undefined");
				} else if (SymVal.Sym.SymType == Symbol.SymbolType.Label) {
					PrintErr($"'{Sym}' is a Label and has no value");
				} else {
					return SymVal.Sym.Value;
				}
			} else {
				PrintErr($"Symbol '{Sym}' is not a variable or number");
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Print the value of an expression or an Undefined message
		/// </summary>
		/// <param name="val"></param>
		private void Print(int? val) {
			if (val.HasValue) {
				Print(val.Value.ToString());
			} else {
				PrintErr("Value not defined");
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Display a message on the user interface
		/// </summary>
		/// <param name="msg"></param>
		private void Print(string msg) {
			int LineNumber = UI.LbOutput.Items.Add(msg);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Print an error message with the location of the error
		/// </summary>
		/// <param name="msg"></param>
		private void PrintErr(string msg) {
			Print(msg + $" at line {IP} -- {CurrentLine}");
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public enum ConditionCode {
		Invalid,
		LessThanOrEqual,
		LessThan,
		Equal,
		GreaterThanOrEqual,
		GreaterThan,
		NotEqual
	}
}