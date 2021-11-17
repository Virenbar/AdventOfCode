Public Class Day10
	Inherits BaseDay

	Public Sub New()
		MyBase.New(10)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim C = Count(LoadInts(StringListTest))
		Return C = 220
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim C = CountVariants(LoadInts(StringListTest))
		Return C = 19208
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim C = Count(LoadInts(StringList))
		Return C
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim C = CountVariants(LoadInts(StringList))
		Return C
	End Function

	Private Function Count(list As List(Of Integer)) As Integer
		list.Sort()
		Dim D = New Dictionary(Of Integer, Integer)() From {{1, 0}, {2, 0}, {3, 1}}
		list.Aggregate(0,
			Function(agr, x)
				D(x - agr) += 1
				Return x
			End Function)
		Return D(1) * D(3)
	End Function

	Private Function CountVariants(list As List(Of Integer)) As Long
		list.Sort()
		Dim D As New DefaultDictionary(Of Integer, Long)
		D(0) = 1
		For Each i In list
			D(i) = D(i - 1) + D(i - 2) + D(i - 3)
		Next
		Return D.Values.Last
	End Function

End Class