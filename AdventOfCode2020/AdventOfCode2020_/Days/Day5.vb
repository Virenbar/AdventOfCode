Public Class Day5
	Inherits BaseDay

	Public Sub New()
		MyBase.New(5)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		'Return GetID(StringListTest(0)) = 357
		Return StringListTest.Select(Function(x) GetID(x)).SequenceEqual({357, 567, 119, 820})
	End Function

	Protected Overrides Function SolvePart1() As Object
		Return StringList.Max(Function(x) GetID(x))
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim L = StringList.Select(Function(x) GetID(x)).ToList
		L.Sort()
		Dim S = L.Aggregate(Function(agr, x) If(agr + 1 = x, x, agr)) + 1
		Return S
	End Function

	Private Shared Function GetID(str As String) As Integer
		Dim Rmin = 0, Rmax = 127, Cmin = 0, Cmax = 7
		For Each C In str.AsEnumerable
			Select Case C
				Case "F"c
					Rmax -= (Rmax - Rmin) \ 2 + 1
				Case "B"c
					Rmin += (Rmax - Rmin) \ 2 + 1
				Case "L"c
					Cmax -= (Cmax - Cmin) \ 2 + 1
				Case "R"c
					Cmin += (Cmax - Cmin) \ 2 + 1
			End Select
		Next
		Return Rmin * 8 + Cmin
	End Function

End Class