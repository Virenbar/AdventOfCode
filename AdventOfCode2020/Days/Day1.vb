Public Class Day1
	Inherits BaseDay
	Private Const SumTarget = 2020
	Private Numbers As List(Of Integer)

	Public Sub New()
		MyBase.New()
		Numbers = LoadInts()
	End Sub

	Protected Overrides ReadOnly Property DayN As Integer
		Get
			Return 1
		End Get
	End Property

	Protected Overrides Function SolvePart1() As Object
		Return GetSum(2)
	End Function

	Protected Overrides Function SolvePart2() As Object
		Return GetSum(3)
	End Function

	Private Function GetSum(count As Integer) As Integer
		Dim Indexs = New List(Of Integer)()
		Dim Nums = New List(Of Integer)()

		Numbers.Sort()
		For i = 0 To count - 1
			Indexs.Add(i)
		Next

		While (True)
			Nums.Clear()
			For i = 0 To count - 1
				Nums.Add(Numbers(Indexs(i)))
			Next
			Dim sum = Nums.Sum()

			If sum <> SumTarget Then
				For i = 0 To count - 1
					If (i = 0 OrElse Indexs(i - 1) = Indexs(i)) Then
						Indexs(i) += 1

						If (i <> 0) Then
							Indexs(i - 1) = If(i > 1, Indexs(i - 2) + 1, 0)
						End If
					End If
				Next
			Else
				Return Nums.Aggregate(1, Function(agr, x) agr * x)
			End If
		End While
		Return 0
	End Function

End Class