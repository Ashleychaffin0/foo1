Module Module1

	Sub Main()
		Dim rnd As New System.Random
		Dim nums = Enumerable.Range(1, 10).OrderBy(Function() rnd.Next)

	End Sub

End Module
