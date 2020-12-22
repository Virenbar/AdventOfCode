Public Class Day22
	Inherits BaseDay

	Public Sub New()
		MyBase.New(22)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim SC = New SpaceCards(RawTest)
		Dim S = SC.PlayGame
		Return S = 306
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim SC = New SpaceCards(RawTest)
		Dim S = SC.PlayRecursiveGame
		Return S = 291
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim SC = New SpaceCards(Raw)
		Dim S = SC.PlayGame
		Return S
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim SC = New SpaceCards(Raw)
		Dim S = SC.PlayRecursiveGame
		Return S
	End Function

	Private Class SpaceCards
		Private P1 As Queue(Of Integer)
		Private P2 As Queue(Of Integer)

		Public Sub New(str As String)
			Dim P = str.Split(vbNewLine + vbNewLine)
			P1 = New Queue(Of Integer)(P(0).Split(vbNewLine).Skip(1).Select(AddressOf Integer.Parse))
			P2 = New Queue(Of Integer)(P(1).Split(vbNewLine).Skip(1).Select(AddressOf Integer.Parse))
		End Sub

		Private Sub PlayRound()
			Dim C1 = P1.Dequeue, C2 = P2.Dequeue
			If C1 > C2 Then
				P1.Enqueue(C1)
				P1.Enqueue(C2)
			Else
				P2.Enqueue(C2)
				P2.Enqueue(C1)
			End If
		End Sub

		Private Function PlayGame(D1 As Queue(Of Integer), D2 As Queue(Of Integer)) As (P As Integer, D As Queue(Of Integer))
			Dim played As New HashSet(Of String)
			While D1.Count > 0 And D2.Count > 0
				Dim round = String.Join(",", D1) + "|" + String.Join(",", D2)
				If played.Contains(round) Then
					Return (1, D1)
				End If
				played.Add(round)

				Dim C1 = D1.Dequeue, C2 = D2.Dequeue
				Dim Win As Integer
				If (D1.Count >= C1 And D2.Count >= C2) Then
					Win = PlayGame(New Queue(Of Integer)(D1.Take(C1)), New Queue(Of Integer)(D2.Take(C2))).P
				Else
					Win = If(C1 > C2, 1, 2)
				End If
				If Win = 1 Then
					D1.Enqueue(C1)
					D1.Enqueue(C2)
				Else
					D2.Enqueue(C2)
					D2.Enqueue(C1)
				End If
			End While
			Return If(D1.Count > D2.Count, (1, D1), (2, D2))
		End Function

		Public Function PlayGame() As Integer
			While P1.Count > 0 And P2.Count > 0
				PlayRound()
			End While
			Dim Win = If(P1.Count > P2.Count, P1, P2)
			Dim i = 1
			Return Win.Reverse.Aggregate(
				Function(agr, x)
					i += 1
					Return agr + x * i
				End Function)
		End Function

		Public Function PlayRecursiveGame() As Integer
			Dim Win = PlayGame(P1, P2).D
			Dim i = 1
			Return Win.Reverse.Aggregate(
				Function(agr, x)
					i += 1
					Return agr + x * i
				End Function)
		End Function

	End Class
End Class