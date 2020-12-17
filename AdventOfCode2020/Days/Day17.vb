Public Class Day17
	Inherits BaseDay

	Public Sub New()
		MyBase.New(17)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim CC = New ConwayCubes(StringListTest)
		CC.Simulate()
		Dim S = CC.Active
		Return S = 112
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim CC = New ConwayHypercubes(StringListTest)
		CC.Simulate()
		Dim S = CC.Active
		Return S = 848
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim CC = New ConwayCubes(StringList)
		CC.Simulate()
		Dim S = CC.Active
		Return S
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim CC = New ConwayHypercubes(StringList)
		CC.Simulate()
		Dim S = CC.Active
		Return S
	End Function

	Private Class ConwayCubes
		Const size As Integer = 30
		Private cube As New HashSet(Of (x As Integer, y As Integer, z As Integer))

		Public Sub New(strs As List(Of String))
			Dim dx = strs.Count \ 2, dy = strs(0).Length \ 2
			For i = 0 To strs.Count - 1
				For j = 0 To strs(i).Length - 1
					If strs(i)(j) = "#"c Then
						cube.Add((i - dx, j - dy, 0))
					End If
				Next
			Next
		End Sub

		Private Function CountNiegbours(cell As (x As Integer, y As Integer, z As Integer)) As Integer
			Dim count = 0
			For dx = -1 To 1
				For dy = -1 To 1
					For dz = -1 To 1
						If dx = 0 And dy = 0 And dz = 0 Then Continue For
						If cube.Contains((cell.x + dx, cell.y + dy, cell.z + dz)) Then
							count += 1
						End If
					Next
				Next
			Next
			Return count
		End Function

		Public Sub Simulate()
			For i = 1 To 6
				SimulateCycle()
			Next
		End Sub

		Private Sub SimulateCycle()
			Dim C = New HashSet(Of (x As Integer, y As Integer, z As Integer))
			For x = -size To size
				For y = -size To size
					For z = -size To size
						Dim cell = (x, y, z)
						Dim count = CountNiegbours(cell)
						If cube.Contains(cell) And (count = 2 Or count = 3) Then
							C.Add(cell)
						ElseIf Not cube.Contains(cell) And count = 3 Then
							C.Add(cell)
						End If
					Next
				Next
			Next
			cube = C
		End Sub

		Public ReadOnly Property Active As Integer
			Get
				Return cube.Count
			End Get
		End Property

	End Class

	Private Class ConwayHypercubes
		Const size As Integer = 20
		Private hypercube As New HashSet(Of (x As Integer, y As Integer, z As Integer, w As Integer))

		Public Sub New(strs As List(Of String))
			Dim dx = strs.Count \ 2, dy = strs(0).Length \ 2
			For i = 0 To strs.Count - 1
				For j = 0 To strs(i).Length - 1
					If strs(i)(j) = "#"c Then
						hypercube.Add((i - dx, j - dy, 0, 0))
					End If
				Next
			Next
		End Sub

		Private Function CountNiegbours(cell As (x As Integer, y As Integer, z As Integer, w As Integer)) As Integer
			Dim count = 0
			For dx = -1 To 1
				For dy = -1 To 1
					For dz = -1 To 1
						For dw = -1 To 1
							If dx = 0 And dy = 0 And dz = 0 And dw = 0 Then Continue For
							If hypercube.Contains((cell.x + dx, cell.y + dy, cell.z + dz, cell.w + dw)) Then
								count += 1
							End If
						Next
					Next
				Next
			Next
			Return count
		End Function

		Public Sub Simulate()
			For i = 1 To 6
				SimulateCycle()
			Next
		End Sub

		Private Sub SimulateCycle()
			Dim C = New HashSet(Of (x As Integer, y As Integer, z As Integer, w As Integer))
			For x = -size To size
				For y = -size To size
					For z = -size To size
						For w = -size To size
							Dim cell = (x, y, z, w)
							Dim count = CountNiegbours(cell)
							If hypercube.Contains(cell) And (count = 2 Or count = 3) Then
								C.Add(cell)
							ElseIf Not hypercube.Contains(cell) And count = 3 Then
								C.Add(cell)
							End If
						Next
					Next
				Next
			Next
			hypercube = C
		End Sub

		Public ReadOnly Property Active As Integer
			Get
				Return hypercube.Count
			End Get
		End Property

	End Class
End Class