Public Class Day25
	Inherits BaseDay

	Public Sub New()
		MyBase.New(25)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim E = New Encription(CInt(StringListTest(0)), CInt(StringListTest(1)))
		Dim S = E.FindKey
		Return S = 14897079
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Return MyBase.TestPart2()
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim E = New Encription(CInt(StringList(0)), CInt(StringList(1)))
		Dim S = E.FindKey
		Return S
	End Function

	Protected Overrides Function SolvePart2() As Object
		Return ""
	End Function

	Private Class Encription
		Private ReadOnly card As Integer
		Private ReadOnly door As Integer
		Private cardL As Integer
		Private doorL As Integer

		Public Sub New(c As Integer, d As Integer)
			card = c
			door = d
		End Sub

		Public Function FindKey() As Integer
			cardL = FindLoop(card)
			doorL = FindLoop(door)
			Dim key = 1
			For i = 1 To cardL
				key = Transform(key, door)
			Next
			Return key
		End Function

		Private Function FindLoop(pub As Integer) As Integer
			Dim N = 1, i = 0
			While True
				i += 1
				N = Transform(N)
				If pub = N Then Exit While
			End While
			Return i
		End Function

		Private Function Transform(n As Long, Optional sn As Integer = 7) As Integer
			Dim M = n * sn
			Return CInt(M Mod 20201227)
		End Function

	End Class
End Class