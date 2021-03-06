using Microsoft.CodeAnalysis;
using System;

namespace TestHelper {
	/// <summary>
	/// Location where the diagnostic appears, as determined by path, line number, and column number.
	/// </summary>
	public struct DiagnosticResultLocation {
		public DiagnosticResultLocation(string path, int line, int column) {
			if (line < 0 && column < 0) {
				throw new ArgumentOutOfRangeException("At least one of line and column must be > 0");
			}
			if (line < -1 || column < -1) {
				throw new ArgumentOutOfRangeException("Both line and column must be >= -1");
			}

			this.Path = path;
			this.Line = line;
			this.Column = column;
		}

		public string Path;
		public int Line;
		public int Column;
	}

	/// <summary>
	/// Struct that stores information about a Diagnostic appearing in a source
	/// </summary>
	public struct DiagnosticResult {
		private DiagnosticResultLocation[] locations;

		public DiagnosticResultLocation[] Locations
		{
			get
			{
				if (this.locations == null) {
					this.locations = new DiagnosticResultLocation[] { };
				}
				return this.locations;
			}

			set
			{
				this.locations = value;
			}
		}

		public DiagnosticSeverity Severity { get; set; }

		public string Id { get; set; }

		public string Message { get; set; }

		public string Path
		{
			get
			{
				return this.Locations.Length > 0 ? this.Locations[0].Path : "";
			}
		}

		public int Line
		{
			get
			{
				return this.Locations.Length > 0 ? this.Locations[0].Line : -1;
			}
		}

		public int Column
		{
			get
			{
				return this.Locations.Length > 0 ? this.Locations[0].Column : -1;
			}
		}
	}
}