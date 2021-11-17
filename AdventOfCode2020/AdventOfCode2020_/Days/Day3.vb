Public Class Day3
	Inherits BaseDay
	Private ReadOnly T As Tobogan
	Private ReadOnly Slope1 As New Slope With {.DX = 3, .Dy = 1}

	Private ReadOnly Slopes As New List(Of Slope) From {
		New Slope With {.DX = 1, .Dy = 1},
		New Slope With {.DX = 3, .Dy = 1},
		New Slope With {.DX = 5, .Dy = 1},
		New Slope With {.DX = 7, .Dy = 1},
		New Slope With {.DX = 1, .Dy = 2}}

	Public Sub New()
		MyBase.New(3)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim T = New Tobogan(StringListTest)
		Dim C = Traverse(T, Slope1)
		Return C = 7
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim CC = Slopes.Aggregate(1,
			Function(agr, x)
				Dim T = New Tobogan(StringListTest)
				Dim C = Traverse(T, x)
				Return agr * C
			End Function)
		Return CC = 336
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim T = New Tobogan(StringList)
		Dim C = Traverse(T, Slope1)
		Return C
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim CC = Slopes.Aggregate(Of Long)(1,
			Function(agr, x)
				Dim T = New Tobogan(StringList)
				Dim C = Traverse(T, x)
				Return agr * C
			End Function)
		Return CC
	End Function

	Private Function Traverse(T As Tobogan, S As Slope) As Integer
		Dim C = 0
		While Not T.IsEnd
			T.Move(S.DX, S.Dy)
			If T.IsTree Then C += 1
		End While
		Return C
	End Function

	Private Class Tobogan
		Private ReadOnly Area As List(Of String)
		Private ReadOnly MaxX As Integer
		Private ReadOnly MaxY As Integer
		Private X As Integer = 0
		Private Y As Integer = 0

		Public Sub New(input As List(Of String))
			Area = input
			MaxX = Area(0).Length - 1
			MaxY = Area.Count - 1
		End Sub

		Public Sub Move(dx As Integer, dy As Integer)
			X += dx
			Y += dy
			If X > MaxX Then X = X - MaxX - 1
		End Sub

		Public ReadOnly Property IsTree() As Boolean
			Get
				Return Area(Y)(X) = "#"c
			End Get
		End Property

		Public ReadOnly Property IsEnd() As Boolean
			Get
				Return Y >= MaxY
			End Get
		End Property

	End Class

	Private Structure Slope
		Public Property DX As Integer
		Public Property Dy As Integer
	End Structure

End Class