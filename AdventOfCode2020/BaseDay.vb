Public MustInherit Class BaseDay
	Protected MustOverride ReadOnly Property DayN As Integer
	Protected StringList As List(Of String)

	Protected Sub New()
		StringList = LoadFile()
	End Sub

	Protected ReadOnly Property Day(Optional test As Boolean = False) As String
		Get
			Return $"Day{DayN}"
		End Get
	End Property

	Public Function Solve() As Result
		Dim R1 = SolvePart1()

		Dim R2 = SolvePart2()
		Return New Result(R1, R2)
	End Function

	Protected ReadOnly Property InputPath(Optional test As Boolean = False) As String
		Get
			Return $"{If(test, "InputTest", "Input")}/{Day}.txt"
		End Get
	End Property

	Private Function LoadFile() As List(Of String)
		Return IO.File.ReadAllLines(InputPath).ToList()
	End Function

	Protected Function LoadInts() As List(Of Integer)
		Return StringList.Select(Function(x) Integer.Parse(x)).ToList
	End Function

	Protected MustOverride Function SolvePart1() As Object

	Protected MustOverride Function SolvePart2() As Object

End Class