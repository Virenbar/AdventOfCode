Imports System.Numerics

Public Class Day12
	Inherits BaseDay

	Public Sub New()
		MyBase.New(12)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim R = New Route(StringListTest)
		R.Navigate()
		Dim c = R.MD
		Return c = 25
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim R = New Route(StringListTest)
		R.NavigateA()
		Dim c = R.MD
		Return c = 286
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim R = New Route(StringList)
		R.Navigate()
		Dim c = R.MD
		Return c
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim R = New Route(StringList)
		R.NavigateA()
		Dim c = R.MD
		Return c
	End Function

	Private Class Route
		Private V As New Vector2(0, 0)
		Private VW As New Vector2(1, 10)
		Private angle As Integer = 90
		Private ReadOnly instrs As List(Of (D As Direction, A As Integer))

		Public Sub New(list As List(Of String))
			instrs = list.Select(Of (D As Direction, A As Integer))(
				Function(x)
					Return (DirectCast([Enum].Parse(GetType(Direction), x.Substring(0, 1)), Direction), CInt(x.Substring(1)))
				End Function).ToList()
		End Sub

		Public Sub Navigate()
			instrs.ForEach(
				Sub(i)
					Select Case i.D
						Case Direction.N
							V.X += i.A
						Case Direction.E
							V.Y += i.A
						Case Direction.S
							V.X -= i.A
						Case Direction.W
							V.Y -= i.A
						Case Direction.L
							angle -= i.A
						Case Direction.R
							angle += i.A
						Case Direction.F
							Dim r = Math.PI * angle / 180.0
							Dim s = Math.Round(Math.Sin(r), 2), c = Math.Round(Math.Cos(r), 2)
							If s = 0 And c = 1 Then
								V.X += i.A
							ElseIf s = 1 And c = 0 Then
								V.Y += i.A
							ElseIf s = 0 And c = -1 Then
								V.X -= i.A
							ElseIf s = -1 And c = 0 Then
								V.Y -= i.A
							End If
					End Select
				End Sub)
		End Sub

		Public Sub NavigateA()
			instrs.ForEach(
				Sub(i)
					Select Case i.D
						Case Direction.N
							VW.X += i.A
						Case Direction.E
							VW.Y += i.A
						Case Direction.S
							VW.X -= i.A
						Case Direction.W
							VW.Y -= i.A
						Case Direction.L, Direction.R
							Dim R = CSng(Math.PI * If(i.D = Direction.R, i.A, -i.A) / 180.0)
							VW = Vector2.Transform(VW, Matrix3x2.CreateRotation(R))
						Case Direction.F
							V = Vector2.Add(V, Vector2.Multiply(VW, i.A))
					End Select
				End Sub)
		End Sub

		Public ReadOnly Property MD As Integer
			Get
				Return CInt(Math.Abs(V.X) + Math.Abs(V.Y))
			End Get
		End Property

	End Class

	Private Enum Direction
		N
		E
		S
		W
		L
		R
		F
	End Enum

End Class