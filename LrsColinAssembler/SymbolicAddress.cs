namespace LrsColinAssembler {
	/// <summary>
	/// Parse "foo", "foo[3]", "foo[R5]"
	/// </summary>
	public class SymbolicAddress {
		public int IndexReg = 0;
		public string Address;

//---------------------------------------------------------------------------------------

		public SymbolicAddress(string addr) {
			int ix = addr.IndexOf('[');
			if (ix > 0) {
				Address = addr.Substring(0, ix);
				string temp = addr[(ix + 1)..^1];
				if (temp[0] == 'R') { temp = temp[1..]; }   // Support R0, R1, etc
				IndexReg = int.Parse(temp);
			} else {
				Address = addr;
			}
		}
	}
}
