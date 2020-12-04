Public MustInherit Class BaseDay
	Protected ReadOnly Property DayN As Integer
	Protected StringList As List(Of String)
	Protected StringListTest As List(Of String)
	Protected Raw As String
	Protected RawTest As String

	Protected Sub New(d As Integer)
		DayN = d
		LoadFile()
	End Sub

	Public ReadOnly Property Day() As String
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

	Private Sub LoadFile()
		Raw = IO.File.ReadAllText(InputPath)
		StringList = IO.File.ReadAllLines(InputPath).ToList()
		If IO.File.Exists(InputPath(True)) Then
			RawTest = IO.File.ReadAllText(InputPath(True))
			StringListTest = IO.File.ReadAllLines(InputPath(True)).ToList()
		End If
	End Sub

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