Module Program

	Sub Main(args As String())
		Dim Day = New Day25()
		Dim R = Day.Solve()
		Console.WriteLine($"{R.P1} {R.P2}")
		Console.Read()
	End Sub

End Module