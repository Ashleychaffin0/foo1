using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace LRS.Utils {
	// Note: I want T to be a numeric type, mostly Int32, but I can't see how to 
	//		 put in a where T: int clause that the compiler will accept. So no
	//		 generics; this class supports only int's. I suppose I could say T: struct...

	// Note: The reason for this class is that I ran into a case where the nFiles and
	//		 nFilesDone fields got out of sync. I tried making them properties so I could
	//		 put debugging statements in the get; and set; routines. But I also needed to
	//		 update these fields safely inside threads, so I never did ++nFiles, etc.; I 
	//		 always did Interlocked.Increment(ref nFiles). But you can't ref a property!
	//		 Hence this class.

	// Note: If necessary I can add other classes for long, double, unsigned, int?, 
	//		 long?, etc. And if I must, methods to this class for operators >, >=, etc.
	//		 And ditto for assignment/comparison to int? and maybe others. Until then...

	internal class InterlockedInt {
		internal int	val;
		internal string DebugName;

		public InterlockedInt(int val = 0, string DebugName = null) {
			this.val  = val;
			this.DebugName = DebugName;
		}

		public static implicit operator int(InterlockedInt v) => v.val;

		public int Increment() {
			Interlocked.Increment(ref val);
			if (DebugName != null) {
				var (Caller, LineNumber, _) = LrsStackUtils.GetStackUpLevel();
				Console.WriteLine($"Incremented {DebugName} to {val} at {Caller}.{LineNumber}");
			}
			return val;
		}

		public static InterlockedInt operator ++(InterlockedInt v) {
			Interlocked.Increment(ref v.val);
			if (v.DebugName != null) {
				var (Caller, LineNumber, _) = LrsStackUtils.GetStackUpLevel();
				Console.WriteLine($"Incremented {v.DebugName} to {v.val} at {Caller}.{LineNumber}");

			}
			return v;
		}

		public int Decrement() {
			Interlocked.Decrement(ref val);
			if (DebugName != null) {
				var (Caller, LineNumber, _) = LrsStackUtils.GetStackUpLevel();
				Console.WriteLine($"Decremented {DebugName} to {val} at {Caller}.{LineNumber}");
			}
			return val;
		}

		public static InterlockedInt operator --(InterlockedInt v) {
			Interlocked.Decrement(ref v.val);
			return v;
		}

		public static bool operator ==(InterlockedInt v1, InterlockedInt v2) =>
			(!(v1 is null)) && (!(v2 is null)) && (ReferenceEquals(v1, v2) || v1.val == v2.val);

		public static bool operator !=(InterlockedInt v1, InterlockedInt v2) =>
			! (v1 == v2);

		public static bool operator ==(InterlockedInt v, int val) => v.val == val; 

		public static bool operator !=(InterlockedInt v, int val) => v.val != val; 

		public static bool operator ==(int val, InterlockedInt v) => v.val == val; 

		public static bool operator !=(int val, InterlockedInt v) => v.val != val; 

		public int Add(int n) => Interlocked.Add(ref val, n);   // Will also subtract

		public int Exchange(int n) => Interlocked.Exchange(ref val, n);

		public int CompareExchange(ref int location, int newValue, int comparand) =>
			Interlocked.CompareExchange(ref location, newValue, comparand);

		public override string ToString() => val.ToString();

		// The compiler complains when I implement operator== and don't have these

		public override bool Equals(object obj) {
			//       
			// See the full list of guidelines at
			//   http://go.microsoft.com/fwlink/?LinkID=85237  
			// and also the guidance for operator== at
			//   http://go.microsoft.com/fwlink/?LinkId=85238
			//

			if (obj == null || GetType() != obj.GetType()) {
				return false;
			}

			// TODO: write your implementation of Equals() here
			// throw new System.NotImplementedException();
			return base.Equals(obj);
		}

		// override object.GetHashCode
		public override int GetHashCode() {
			// TODO: write your implementation of GetHashCode() here
			// throw new System.NotImplementedException();
			return base.GetHashCode();
		}
	}

	public static class LrsStackUtils {
		static string ReText = @" *at (?<Caller>.*?) in (?<Filename>.*?):line (?<LineNumber>\d+)";
		static Regex re = new Regex(ReText);

		public static (string Caller, int LineNumber, string Filename) GetStackUpLevel(int n = 3) {
			var stk2 = Environment.StackTrace;
			var stk = stk2.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			// Sample:    at LRS.Utils.LrsStackUtils.GetStackUpLevel(Int32 n) in G:\LRS\$Dev\$$$ C# Ongoing Projects\$SNARF\DownloadMsConferences\InterlockedInt.cs:line 105
			var m           = re.Match(stk[n]);
			string Caller   = m.Groups["Caller"].Value;
			string Filename = m.Groups["Filename"].Value;
			string LineNo   = m.Groups["LineNumber"].Value;
			int LineNumber = 0;
			if (LineNo.Length > 0) {
				LineNumber  = Convert.ToInt32(LineNo);
			}
			// int LineNumber  = Convert.ToInt32(m.Groups["LineNumber"].Value);
			return (Caller, LineNumber, Filename);
		}
	}
}
