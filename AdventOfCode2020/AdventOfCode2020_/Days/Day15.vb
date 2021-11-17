Public Class Day15
	Inherits BaseDay

	Public Sub New()
		MyBase.New(15)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim M = New Memory(LoadInts(StringListTest(0)))
		Dim S = M.PlayTo2020
		Return S = 436
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim M = New Memory(LoadInts(StringListTest(0)))
		Dim S = M.PlayTo30000000
		Return S = 175594
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim M = New Memory(LoadInts(StringList(0)))
		Dim S = M.PlayTo2020
		Return S
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim M = New Memory(LoadInts(StringList(0)))
		Dim S = M.PlayTo30000000
		Return S
	End Function

	Private Class Memory
		Private turn As Integer = 0
		Private last As Integer
		Private spoken As New Dictionary(Of Integer, Integer)

		Public Sub New(ints As List(Of Integer))
			For Each i In ints
				turn += 1
				last = i
				spoken(i) = turn
			Next
			spoken.Remove(ints.Last)
		End Sub

		Public Sub PlayTurn()
			turn += 1
			If spoken.ContainsKey(last) Then
				Dim i = turn - spoken(last) - 1
				spoken(last) = turn - 1
				last = i
			Else
				spoken(last) = turn - 1
				last = 0
			End If
		End Sub

		Public Function PlayTo2020() As Integer
			While turn <> 2020
				PlayTurn()
			End While
			Return last
		End Function

		Public Function PlayTo30000000() As Integer
			While turn <> 30000000
				PlayTurn()
			End While
			Return last
		End Function

	End Class

End Class