Imports System.Text.RegularExpressions

Public Class Day18
	Inherits BaseDay

	Public Sub New()
		MyBase.New(18)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim C = New Calculator(StringListTest)
		Dim S = C.GetResults()
		Return S.SequenceEqual({26, 437, 12240, 13632})
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim C = New Calculator(StringListTest)
		Dim S = C.GetResults(True)
		Return S.SequenceEqual({46, 1445, 669060, 23340})
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim C = New Calculator(StringList)
		Dim S = C.GetResults()
		Return S.Sum
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim C = New Calculator(StringList)
		Dim S = C.GetResults(True)
		Return S.Sum
	End Function

	Private Class Calculator
		Private Shared RD As New Regex("\d+")
		Private Shared RO As New Regex("[*+]")
		Private Expressions As List(Of String)

		Public Sub New(strs As List(Of String))
			Expressions = strs.Select(Function(x) x.Replace(" ", "")).ToList
		End Sub

		Private Function Evaluate(str As String, adv As Boolean) As Long
			Dim i = 0
			While i < str.Length - 1
				Dim c = str(i)
				If c = "("c Then
					Dim P = 0
					For j = i + 1 To str.Length - 1
						Dim c2 = str(j)
						If c2 = "("c Then
							P += 1
						ElseIf c2 = ")"c Then
							If P = 0 Then
								Dim subexp = str.Substring(i + 1, j - (i + 1))
								Dim V = Evaluate(subexp, adv)
								str = str.Replace($"({subexp})", V.ToString)
								Exit For
							End If
							P -= 1
						End If
					Next
				End If
				i += 1
			End While
			Dim Vals = RD.Matches(str).Select(Function(n) CLng(n.Value)).ToList
			Dim Ops = RO.Matches(str).Select(Function(n) n.Value).ToList
			If adv Then
				i = 0
				While i < Ops.Count
					If Ops(i) = "+" Then
						Vals = Vals.Take(i).Concat({Vals(i) + Vals(i + 1)}).Concat(Vals.Skip(i + 2)).ToList
						Ops = Ops.Take(i).Concat(Ops.Skip(i + 1)).ToList
					Else
						i += 1
					End If
				End While
				Return Vals.Multiply()
			Else
				Dim Result = Vals.First
				Vals = Vals.Skip(1).ToList()
				While Vals.Count > 0
					If Ops.First = "*" Then
						Result *= Vals.First
					Else
						Result += Vals.First
					End If
					Vals = Vals.Skip(1).ToList()
					Ops = Ops.Skip(1).ToList()
				End While
				Return Result
			End If
		End Function

		Public Function GetResults(Optional adv As Boolean = False) As List(Of Long)
			Return Expressions.Select(Function(x) Evaluate(x, adv)).ToList
		End Function

	End Class
End Class