Imports System.Text.RegularExpressions

Public Class Day19
	Inherits BaseDay

	Public Sub New()
		MyBase.New(19)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim MV = New MessageValidator(RawTest)
		Dim S = MV.CountValid
		Return S = 2
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim MV = New MessageValidator(RawTestA)
		Dim S1 = MV.CountValid
		MV.FixRules()
		Dim S2 = MV.CountValid
		Return S1 = 3 And S2 = 12
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim MV = New MessageValidator(Raw)
		Dim S = MV.CountValid
		Return S
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim MV = New MessageValidator(Raw)
		MV.FixRules()
		Dim S = MV.CountValid
		Return S
	End Function

	Private Class MessageValidator
		Private ReadOnly messages As List(Of String)
		Private ReadOnly rules As Dictionary(Of String, String)

		Public Sub New(str As String)
			Dim RandM = str.Split(vbNewLine + vbNewLine).Select(Function(s) s.Split(vbNewLine).ToList).ToList
			rules = RandM(0).Select(Function(x) x.Split(": ")).ToDictionary(Function(r) r(0),
				Function(r)
					Dim v = r(1).Replace("""", "")
					If v.Contains("|") Then
						Dim vals = v.Split(" | ")
						v = $"( {vals(0)} | {vals(1)} )"
					End If
					Return v
				End Function)
			messages = RandM(1)
		End Sub

		Public Sub FixRules()
			rules("8") = "( 42 | 42 8 )"
			rules("11") = "( 42 31 | 42 11 31 )"
		End Sub

		''' <summary>
		''' Nooooooooooooo, you can't just convert rules to one regular expression
		''' haha Regex go brrr
		''' </summary>
		''' <returns></returns>
		Private Function ToRegex() As Regex
			Dim regex = rules("0").Split(" ").ToList()
			While (regex.Any(Function(x) x.Any(Function(y) Char.IsDigit(y))) And regex.Count() < 100000)
				regex = regex.Select(Function(x) If(rules.ContainsKey(x), rules(x), x)).SelectMany(Function(x) x.Split(" ")).ToList()
			End While
			regex.Remove("8")
			regex.Remove("11")
			Return New Regex($"^{String.Join("", regex)}$")
		End Function

		Public Function CountValid() As Integer
			Dim Regex = ToRegex()
			Return messages.Where(Function(m) Regex.IsMatch(m)).Count
		End Function

	End Class

End Class