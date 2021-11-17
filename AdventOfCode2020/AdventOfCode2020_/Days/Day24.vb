Imports System.Text.RegularExpressions

Public Class Day24
	Inherits BaseDay

	Public Sub New()
		MyBase.New(24)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim LF = New LobbyFloor(StringListTest)
		Dim S = LF.BlackCount
		Return S = 10
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim LF = New LobbyFloor(StringListTest)
		LF.FlipMore()
		Dim S = LF.BlackCount
		Return S = 2208
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim LF = New LobbyFloor(StringList)
		Dim S = LF.BlackCount
		Return S
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim LF = New LobbyFloor(StringList)
		LF.FlipMore()
		Dim S = LF.BlackCount
		Return S
	End Function

	Public Class LobbyFloor
		Private ReadOnly Dirs As New List(Of String) From {"se", "ne", "e", "sw", "nw", "w"}
		Private ReadOnly Offsets As New List(Of (X As Integer, Y As Integer, Z As Integer)) From {(+1, -1, 0), (+1, 0, -1), (0, +1, -1), (-1, +1, 0), (-1, 0, +1), (0, -1, +1)}
		Private Floor As New HashSet(Of String)
		Const Size = 100

		Public Sub New(strs As List(Of String))
			Dim Tiles = strs.Select(
				Function(str)
					Dim T = New Tile
					For Each dir In Dirs
						Dim C = Regex.Matches(str, dir).Count
						If C > 0 Then
							str = str.Replace(dir, "")
							Select Case dir
								Case "se"
									T.Z += C
									T.Y -= C
								Case "ne"
									T.X += C
									T.Z -= C
								Case "e"
									T.X += C
									T.Y -= C
								Case "sw"
									T.Z += C
									T.X -= C
								Case "nw"
									T.Y += C
									T.Z -= C
								Case "w"
									T.Y += C
									T.X -= C
							End Select
						End If
					Next
					Return T
				End Function)
			For Each Tile In Tiles
				Dim ID = Tile.ToString
				If Floor.Contains(ID) Then
					Floor.Remove(ID)
				Else
					Floor.Add(ID)
				End If
			Next
		End Sub

		Public Sub FlipMore()
			For i = 1 To 100
				FlipOnce()
			Next
		End Sub

		Private Sub FlipOnce()
			Dim F = New HashSet(Of String)
			For x = -Size To Size
				For y = -Size To Size
					For z = -Size To Size
						If x + y + z <> 0 Then Continue For
						Dim tile = New Tile With {.X = x, .Y = y, .Z = z}
						Dim count = CountNeighbors(tile)
						If Floor.Contains(tile.ToString) And (count = 1 Or count = 2) Then
							F.Add(tile.ToString)
						ElseIf Not Floor.Contains(tile.ToString) And count = 2 Then
							F.Add(tile.ToString)
						End If
					Next
				Next
			Next
			Floor = F
		End Sub

		Private Function CountNeighbors(tile As Tile) As Integer
			Dim C = 0
			For Each O In Offsets
				Dim T = tile
				T.X += O.X : T.Y += O.Y : T.Z += O.Z
				If Floor.Contains(T.ToString) Then C += 1
			Next
			Return C
		End Function

		Public ReadOnly Property BlackCount As Integer
			Get
				Return Floor.Count
			End Get
		End Property

		Private Structure Tile
			Public Property X As Integer
			Public Property Y As Integer
			Public Property Z As Integer

			Public Overrides Function ToString() As String
				Return $"{X} {Y} {Z}"
			End Function

		End Structure

	End Class
End Class