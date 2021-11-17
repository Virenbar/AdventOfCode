Imports System.Text.RegularExpressions

Public Class Day2
	Inherits BaseDay
	Private PolReg As New Regex("(\d+)-(\d+) (\w): (\w+)")

	Public Sub New()
		MyBase.New(2)
	End Sub

	Protected Overrides Function SolvePart1() As Object
		Dim C = 0
		For Each S In StringList
			If IsCorrect1(S) Then C += 1
		Next
		Return C
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim C = 0
		For Each S In StringList
			If IsCorrect2(S) Then C += 1
		Next
		Return C
	End Function

	Private Function IsCorrect1(str As String) As Boolean
		Dim M = PolReg.Match(str)
		Dim D1 = Integer.Parse(M.Groups.Item(1).Value), D2 = Integer.Parse(M.Groups.Item(2).Value), L = M.Groups.Item(3).Value, Pass = M.Groups.Item(4).Value

		Dim R = New Regex(L)
		Dim C = R.Matches(Pass).Count
		Return D1 <= C And C <= D2
	End Function

	Private Function IsCorrect2(str As String) As Boolean
		Dim M = PolReg.Match(str)
		Dim D1 = Integer.Parse(M.Groups.Item(1).Value), D2 = Integer.Parse(M.Groups.Item(2).Value), L = M.Groups.Item(3).Value, Pass = M.Groups.Item(4).Value

		Return Pass(D1 - 1) = L Xor Pass(D2 - 1) = L
	End Function

End Class