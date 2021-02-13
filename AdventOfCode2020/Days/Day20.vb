''' <summary>
''' Solution taken from https://github.com/encse/adventofcode/blob/master/2020/Day20/Solution.cs
''' </summary>
Public Class Day20
	Inherits BaseDay

	Public Sub New()
		MyBase.New(20)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Return MyBase.TestPart1()
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Return MyBase.TestPart2()
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim S = New Solution
		Dim C = S.PartOne(Raw)
		Return C
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim S = New Solution
		Dim C = S.PartTwo(Raw)
		Return C
	End Function

	Friend Class Tile
		Public id As Integer
		Private image As String()
		Public size As Integer
		' int orientation = 0;
		' int flip = 0;

		Private position As Integer = 0
		Public edges As String()

		Public Sub New(ByVal title As Integer, ByVal image As String())
			id = title
			Me.image = image
			size = image.Length

			If image.Length = 11 Then
				Console.WriteLine("x")
			End If

			edges = {edge(0, 0, 0, 1), edge(0, 0, 1, 0), edge(size - 1, 0, 0, 1), edge(size - 1, 0, -1, 0), edge(0, size - 1, 0, -1), edge(0, size - 1, 1, 0), edge(size - 1, size - 1, 0, -1), edge(size - 1, size - 1, -1, 0)}
		End Sub

		Public Sub ChangePosition()
			position += 1
			position = position Mod 8
		End Sub

		Default Public ReadOnly Property Item(ByVal irow As Integer, ByVal icol As Integer) As Char
			Get

				For i = 0 To position Mod 4 - 1
					Dim tmp = irow
					irow = icol
					icol = size - 1 - tmp
					'(irow, icol) = (icol, size - 1 - irow)
				Next

				If position Mod 8 >= 4 Then
					icol = size - 1 - icol
				End If

				Return image(irow)(icol)
			End Get
		End Property

		Private Function edge(ByVal irow As Integer, ByVal icol As Integer, ByVal drow As Integer, ByVal dcol As Integer) As String
			Dim st = ""
			For i = 0 To size - 1
				st += Me(irow, icol)
				irow += drow
				icol += dcol
			Next
			Return st
		End Function

		Public Function row(ByVal irow As Integer) As String
			Return edge(irow, 0, 0, 1)
		End Function

		Public Function top() As String
			Return edge(0, 0, 0, 1)
		End Function

		Public Function bottom() As String
			Return edge(size - 1, 0, 0, 1)
		End Function

		Public Function left() As String
			Return edge(0, 0, 1, 0)
		End Function

		Public Function right() As String
			Return edge(0, size - 1, 1, 0)
		End Function

	End Class

	Friend Class Solution

		Public Function PartOne(ByVal input As String) As Object
			Dim tiles = RestoreTiles(input)
			Return CLng(tiles(0, 0).id) * tiles(11, 11).id * tiles(0, 11).id * tiles(11, 0).id
		End Function

		Private Function Parse(ByVal input As String) As Tile()
			Return input.Split(vbNewLine + vbNewLine).Select(
				Function(block)
					Dim lines = block.Split(vbNewLine)
					Return New Tile(Integer.Parse(lines(0).Trim(":"c).Split(" ")(1)), lines.Skip(1).Where(Function(x) x <> "").ToArray())
				End Function).ToArray()
		End Function

		Private Function RestoreTiles(input As String) As Tile(,)
			Dim tiles = Parse(input).ToList()

			Dim findTile = Function(topPattern As String, leftPattern As String)
							   For Each tile In tiles

								   For i = 0 To 8 - 1
									   Dim topMatch = If(topPattern IsNot Nothing, tile.top() = topPattern, Not tiles.Any(Function(tileB) tileB.id <> tile.id AndAlso tileB.edges.Contains(tile.top())))
									   Dim leftMatch = If(leftPattern IsNot Nothing, tile.left() = leftPattern, Not tiles.Any(Function(tileB) tileB.id <> tile.id AndAlso tileB.edges.Contains(tile.left())))

									   If topMatch AndAlso leftMatch Then
										   Return tile
									   End If

									   tile.ChangePosition()
								   Next
							   Next
							   Throw New Exception()
						   End Function
			Dim mtx = New Tile(11, 11) {}

			For irow = 0 To 12 - 1
				For icol = 0 To 12 - 1
					Dim topPattern = If(irow = 0, Nothing, mtx(irow - 1, icol).bottom())
					Dim leftPattern = If(icol = 0, Nothing, mtx(irow, icol - 1).right())
					Dim tile = findTile(topPattern, leftPattern)
					mtx(irow, icol) = tile
					tiles.Remove(tile)
				Next
			Next

			Return mtx
		End Function

		Public Function PartTwo(ByVal input As String) As Object
			Dim mtx = RestoreTiles(input)
			Dim image = New List(Of String)()

			For irow = 0 To 12 - 1

				For i = 1 To 9 - 1
					Dim st = ""

					For icol = 0 To 12 - 1
						st += mtx(irow, icol).row(i).Substring(1, 8)
					Next

					image.Add(st)
				Next
			Next

			Dim bigTile = New Tile(-1, image.ToArray())
			Dim monster = New String() {"                  # ", "#    ##    ##    ###", " #  #  #  #  #  #   "}

			For i = 0 To 9 - 1
				Dim matches = Function() As Integer
								  Dim res = 0

								  For irow = 0 To bigTile.size - 1
									  For icol = 0 To bigTile.size - 1
										  Dim match = Function() As Boolean
														  Dim ccolM = monster(0).Length
														  Dim crowM = monster.Length

														  If icol + ccolM >= bigTile.size Then
															  Return False
														  End If

														  If irow + crowM >= bigTile.size Then
															  Return False
														  End If

														  For icolM = 0 To ccolM - 1

															  For irowM = 0 To crowM - 1

																  If monster(irowM)(icolM) = "#"c AndAlso bigTile(irow + irowM, icol + icolM) <> "#"c Then
																	  Return False
																  End If
															  Next
														  Next

														  Return True
													  End Function

										  If match() Then
											  res += 1
										  End If
									  Next
								  Next

								  Return res
							  End Function

				Dim cmatch = matches()

				If cmatch > 0 Then
					Dim hashCount = 0

					For irow = 0 To bigTile.size - 1

						For icol = 0 To bigTile.size - 1
							If bigTile(irow, icol) = "#"c Then hashCount += 1
						Next
					Next

					Return hashCount - cmatch * 15
				End If

				bigTile.ChangePosition()
			Next
			Throw New Exception()
		End Function

	End Class

End Class