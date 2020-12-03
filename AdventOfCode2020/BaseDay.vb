Public MustInherit Class BaseDay
	Protected ReadOnly Property DayN As Integer
	Protected StringList As List(Of String)
	Protected StringListTest As List(Of String)

	Protected Sub New(d As Integer)
		DayN = d
		StringList = LoadFile()
		StringListTest = LoadFile(True)
	End Sub

	Protected ReadOnly Property Day() As String
		Get
			Return $"Day{DayN}"
		End Get
	End Property

	Public Function Solve() As Result
		Dim T1 = TestPart1()
		Dim T2 = TestPart2()
		If Not (T1 And T2) Then Return New Result(If(T1, "", "Test1 failed"), If(T2, "", "Test2 failed"))

		Dim R1 = SolvePart1()

		Dim R2 = SolvePart2()
		Return New Result(R1, R2)
	End Function

	Protected ReadOnly Property InputPath(Optional test As Boolean = False) As String
		Get
			Return $"{If(test, "InputTest", "Input")}/{Day}.txt"
		End Get
	End Property

	Private Function LoadFile(Optional test As Boolean = False) As List(Of String)
		Return IO.File.ReadAllLines(InputPath(test)).ToList()
	End Function

	Protected Function LoadInts() As List(Of Integer)
		Return StringList.Select(Function(x) Integer.Parse(x)).ToList
	End Function

	Protected MustOverride Function SolvePart1() As Object

	Protected MustOverride Function SolvePart2() As Object

	Protected Overridable Function TestPart1() As Boolean
		Return True
	End Function

	Protected Overridable Function TestPart2() As Boolean
		Return True
	End Function

End Class