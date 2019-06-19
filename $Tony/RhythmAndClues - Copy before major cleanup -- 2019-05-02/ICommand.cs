namespace RhythmAndClues {
	interface ICommand {
		bool CheckSyntax(string[] tokens);
		bool Execute(string[] tokens);
	}
}
