Public Class Day11
	Inherits BaseDay

	Public Sub New()
		MyBase.New(11)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim W = New WaitingArea(StringListTest)
		W.SimulateP1()
		Dim C = W.Occupied
		Return C = 37
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim W = New WaitingArea(StringListTest)
		W.SimulateP2()
		Dim C = W.Occupied
		Return C = 26
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim W = New WaitingArea(StringList)
		W.SimulateP1()
		Dim C = W.Occupied
		Return C
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim W = New WaitingArea(StringList)
		W.SimulateP2()
		Dim C = W.Occupied
		Return C
	End Function

	Private Class WaitingArea
		Private ReadOnly W As Integer
		Private ReadOnly H As Integer
		Private _seats As List(Of SeatState)
		Private ReadOnly Offsets() As (x As Integer, y As Integer) = {(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)}

		Public Sub New(strs As List(Of String))
			W = strs(0).Length
			H = strs.Count
			_seats = strs.SelectMany(Function(x) x,
				Function(str, c) As SeatState
					Select Case c
						Case "."c
							Return SeatState.Floor
						Case Else
							Return SeatState.Empty
					End Select
				End Function).ToList()
		End Sub

		Public ReadOnly Property Occupied As Integer
			Get
				Return _seats.Where(Function(x) x = SeatState.Occupied).Count
			End Get
		End Property

		Public Sub SimulateP1()
			While True
				Dim s = New List(Of SeatState)(_seats)
				For x = 0 To W - 1
					For y = 0 To H - 1
						If s(x + y * W) = SeatState.Floor Then Continue For
						Dim O = CountOccupiedP1(x, y)
						If O = 0 Then
							s(x + y * W) = SeatState.Occupied
						ElseIf O >= 4 Then
							s(x + y * W) = SeatState.Empty
						End If
					Next
				Next
				If _seats.SequenceEqual(s) Then Exit Sub
				_seats = s
			End While
		End Sub

		Public Sub SimulateP2()
			While True
				Dim s = New List(Of SeatState)(_seats)
				For x = 0 To W - 1
					For y = 0 To H - 1
						If s(x + y * W) = SeatState.Floor Then Continue For
						Dim O = CountOccupiedP2(x, y)
						If O = 0 Then
							s(x + y * W) = SeatState.Occupied
						ElseIf O >= 5 Then
							s(x + y * W) = SeatState.Empty
						End If
					Next
				Next
				If _seats.SequenceEqual(s) Then Exit Sub
				_seats = s
			End While
		End Sub

		Private Function CountOccupiedP1(x As Integer, y As Integer) As Integer
			Return Offsets.Where(
				Function(off)
					Return Seats(x + off.x, y + off.y) = SeatState.Occupied
				End Function).Count
		End Function

		Private Function CountOccupiedP2(x As Integer, y As Integer) As Integer
			Return Offsets.Where(
				Function(off)
					Dim ix = x, iy = y
					Dim t As SeatState = SeatState.Floor
					While t = SeatState.Floor
						ix += off.x
						iy += off.y
						t = Seats(ix, iy)
					End While
					Return t = SeatState.Occupied
				End Function).Count
		End Function

		Private Function IsOccupied(ix As Integer, iy As Integer, x As Integer, y As Integer) As Boolean
			Dim t As SeatState = SeatState.Floor
			While t = SeatState.Floor
				x += ix
				y += iy
				t = Seats(x, y)
			End While
			Return t = SeatState.Occupied
		End Function

		Public ReadOnly Property Seats(x As Integer, y As Integer) As SeatState
			Get
				If 0 > x Or x > W - 1 Or 0 > y Or y > H - 1 Then Return SeatState.Border
				Return _seats(x + y * W)
			End Get
		End Property

	End Class

	Private Enum SeatState
		Empty = 0
		Occupied = 1
		Floor = 2
		Border = 100
	End Enum

End Class