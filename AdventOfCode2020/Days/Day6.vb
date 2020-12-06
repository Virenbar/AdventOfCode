Public Class Day6
	Inherits BaseDay

	Public Sub New()
		MyBase.New(6)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim Groups = RawTest.Split(vbNewLine + vbNewLine).ToList()
		Dim Sum = Groups.Sum(Function(x) CountYes(x))
		Return Sum = 11
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim Groups = RawTest.Split(vbNewLine + vbNewLine).ToList()
		Dim Sum = Groups.Sum(Function(x) CountYesE(x))
		Return Sum = 6
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim Groups = Raw.Split(vbNewLine + vbNewLine).ToList()
		Dim Sum = Groups.Sum(Function(x) CountYes(x))
		Return Sum
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim Groups = Raw.Split(vbNewLine + vbNewLine).ToList()
		Dim Sum = Groups.Sum(Function(x) CountYesE(x))
		Return Sum
	End Function

	Private Function CountYes(group As String) As Integer
		Dim Answers = group.Replace(vbNewLine, "")
		Dim H As New HashSet(Of Char)
		Answers.AsEnumerable.ToList.ForEach(Sub(c) H.Add(c))
		Return H.Count
	End Function

	Private Function CountYesE(group As String) As Integer
		Dim Answers = group.Split(vbNewLine).ToList()
		Dim D As New Dictionary(Of Char, Integer)
		Answers.ForEach(
			Sub(x)
				x.AsEnumerable.ToList.ForEach(
				Sub(c)
					If D.ContainsKey(c) Then D(c) += 1 Else D.Add(c, 1)
				End Sub)
			End Sub)
		Return D.Where(Function(x) x.Value = Answers.Count).Count
	End Function

End Class