Public Class Day23
	Inherits BaseDay

	Public Sub New()
		MyBase.New(23)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim CC = New CrabCups(RawTest)
		Dim S = CC.SimulateP1()
		Return S = "67384529"
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim CC = New CrabCups(RawTest)
		Dim S = CC.SimulateP2
		Return S = 149245887792
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim CC = New CrabCups(Raw)
		Dim S = CC.SimulateP1()
		Return S
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim CC = New CrabCups(Raw)
		Dim S = CC.SimulateP2
		Return S
	End Function

	Private Class CrabCups
		Private cups As List(Of Cup)
		Private current As Cup
		Private dict As Dictionary(Of Integer, Cup)

		Public Sub New(str As String)
			cups = New List(Of Cup)(str.Select(Function(i) New Cup(Integer.Parse(i))))
		End Sub

		Public Function SimulateP1() As String
			Prepare()
			For i = 1 To 100
				MoveCups()
			Next
			Dim cup = dict(1)
			Return "".PadRight(8).Aggregate("",
				Function(agr, c)
					cup = cup.NextCup
					Return agr + cup.Label.ToString
				End Function)
		End Function

		Public Function SimulateP2() As Long
			AddCups()
			Prepare()
			For i = 1 To 10000000
				'If i Mod 1000 = 0 Then
				'	Console.Write(i.ToString.PadRight(10))
				'	Console.SetCursorPosition(0, 0)
				'End If
				MoveCups()
			Next
			Dim cup = dict(1)
			Return CLng(cup.NextCup.Label) * cup.NextCup.NextCup.Label
		End Function

		Private Sub AddCups()
			For i = 10 To 1000000
				cups.Add(New Cup(i))
			Next
		End Sub

		Private Sub MoveCups()
			Dim taken = current.Take()
			Dim dest = If(current.Label = 1, cups.Count, current.Label - 1)
			While taken.Label = dest Or taken.NextCup.Label = dest Or taken.NextCup.NextCup.Label = dest
				dest -= 1
				If dest = 0 Then dest = cups.Count
			End While
			dict(dest).Place(taken)
			current = current.NextCup
		End Sub

		Private Sub Prepare()
			For i = 0 To cups.Count - 1
				cups(i).NextCup = cups((i + 1) Mod cups.Count)
			Next
			dict = cups.ToDictionary(Function(k) k.Label, Function(v) v)
			current = cups(0)
		End Sub

		Private Class Cup

			Public Sub New(i As Integer)
				Label = i
			End Sub

			Public Property Label As Integer
			Public Property NextCup As Cup

			Public Sub Place(cup As Cup)
				Dim OldNext = NextCup
				NextCup = cup
				cup.NextCup.NextCup.NextCup = OldNext
			End Sub

			Public Function Take() As Cup
				Dim first = NextCup
				Dim third = first.NextCup.NextCup

				NextCup = third.NextCup
				third.NextCup = Nothing
				Return first
			End Function

		End Class
	End Class
End Class