Public Class Day9
	Inherits BaseDay

	Public Sub New()
		MyBase.New(9)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim X = New XMAS(5, LoadLongs(StringListTest))
		Dim L = X.FindInvalid()
		Return L = 127
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim X = New XMAS(5, LoadLongs(StringListTest))
		Dim L = X.FindInvalid()
		Dim S = X.Break(L)
		Return S = 62
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim X = New XMAS(25, LoadLongs(StringList))
		Dim L = X.FindInvalid()
		Return L
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim X = New XMAS(25, LoadLongs(StringList))
		Dim L = X.FindInvalid()
		Dim S = X.Break(L)
		Return S
	End Function

	Private Class XMAS
		Private ReadOnly pSize As Integer
		Private numbers As List(Of Long)
		Private preamble As FixedQueue(Of Long)

		Public Sub New(p As Integer, list As List(Of Long))
			pSize = p
			numbers = list
		End Sub

		Public Function FindInvalid() As Long
			preamble = New FixedQueue(Of Long)(pSize)
			numbers.Take(pSize).ToList.ForEach(AddressOf preamble.Enqueue)
			For i = pSize To numbers.Count - 1
				Dim N = numbers(i)
				If Not IsValid(N) Then Return N
				preamble.Enqueue(N)
			Next
			Return 0
		End Function

		Public Function Break(number As Long) As Long
			For i = 2 To numbers.Count
				For j = 0 To numbers.Count
					Dim L = numbers.Skip(j).Take(i).ToList()
					If L.Sum = number Then Return L.Min + L.Max
				Next
			Next
			Return 0
		End Function

		Private Function IsValid(number As Long) As Boolean
			'Dim j = preamble.SelectMany(Function(n1) preamble, Function(n1, n2) (n1:=n1, n2:=n2)).Where(Function(x) (x.n1 <> x.n2) And (x.n1 + x.n2 = number))
			Dim d = From n1 In preamble
					From n2 In preamble
					Where n1 <> n2 And n1 + n2 = number
					Select (n1, n2)
			Return d.Count > 0
		End Function

	End Class
End Class