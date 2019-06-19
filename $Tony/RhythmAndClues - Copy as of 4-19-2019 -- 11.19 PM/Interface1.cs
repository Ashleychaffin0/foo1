namespace RhythmAndClues {
	interface ICommand {
		bool CheckSyntax(Interpreter main, string[] tokens);
		bool Command(Interpreter main, string[] tokens);
	}
}
