Imports System.Text.RegularExpressions

Public Class Day16
	Inherits BaseDay

	Public Sub New()
		MyBase.New(16)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim TV = New TicketValidator(RawTest)
		Dim S = TV.CountError
		Return S = 71
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim TV = New TicketValidator(RawTestA)
		Dim T = TV.GetMyTicket()
		Dim S = T("class") * T("row") * T("seat")
		Return S = 12 * 13 * 11
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim TV = New TicketValidator(Raw)
		Dim S = TV.CountError
		Return S
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim TV = New TicketValidator(Raw)
		Dim T = TV.GetMyTicket()
		Dim S = T.Where(Function(f) f.Key.Contains("departure")).Multiply(Function(x) x.Value)
		Return S
	End Function

	Private Class TicketValidator
		Private Shared ReadOnly RR As New Regex("([\w ]+): (\d+)-(\d+)\D+(\d+)-(\d+)")
		Private Shared ReadOnly TR As New Regex("(^[\d,?]+)", RegexOptions.Multiline)
		Private rules As New List(Of Rule)
		Private tickets As New List(Of List(Of Integer))

		Public Sub New(str As String)
			rules = RR.Matches(str).Select(Function(m) New Rule(m.Groups(1).Value, CInt(m.Groups(2).Value), CInt(m.Groups(3).Value), CInt(m.Groups(4).Value), CInt(m.Groups(5).Value))).ToList
			tickets = TR.Matches(str).Select(Function(x) x.Groups(1).Value.Split(",").Select(Function(t) CInt(t)).ToList).ToList
		End Sub

		Public Function CountError() As Integer
			Dim sum = 0
			For Each ticket In tickets
				For Each i In ticket
					If Not rules.Any(Function(x) x.IsValid(i)) Then sum += i
				Next
			Next
			Return sum
		End Function

		Public Function GetMyTicket() As Dictionary(Of String, Integer)
			Dim usedFields = New HashSet(Of Integer)
			Dim matchedFields = New Dictionary(Of Rule, HashSet(Of Integer))() 'Rules and possible fields
			Dim VT = tickets.Where(Function(t) Not t.Any(Function(i) Not rules.Any(Function(x) x.IsValid(i)))).ToList 'Valid ticket
			For Each rule In rules
				matchedFields(rule) = New HashSet(Of Integer)
				For f = 0 To VT(0).Count - 1
					Dim field = f
					If VT.All(Function(t) rule.IsValid(t(field))) Then
						matchedFields(rule).Add(f)
					End If
				Next
			Next
			While usedFields.Count < rules.Count
				Dim validRules = matchedFields.Where(Function(x) x.Value.Count = 1).ToList 'If only one matched field
				For Each vr In validRules
					Dim field = vr.Value.First
					vr.Key.Field = field
					usedFields.Add(field)
					rules.ForEach(Sub(r) matchedFields(r).Remove(field)) 'Remove from all rules
				Next
			End While
			Dim D = rules.ToDictionary(Function(k) k.Name, Function(v) VT(0)(v.Field))
			Return D
		End Function

		Private Class Rule
			Private ReadOnly R1L As Integer
			Private ReadOnly R1R As Integer
			Private ReadOnly R2L As Integer
			Private ReadOnly R2R As Integer

			Public Sub New(n As String, d1 As Integer, d2 As Integer, d3 As Integer, d4 As Integer)
				Name = n
				R1L = d1
				R1R = d2
				R2L = d3
				R2R = d4
			End Sub

			Public Property Field As Integer = -1
			Public ReadOnly Property Name As String

			Public Function IsValid(i As Integer) As Boolean
				Return (R1L <= i And i <= R1R) Or (R2L <= i And i <= R2R)
			End Function

			Public Overrides Function ToString() As String
				Return $"{Name}: {Field}"
			End Function

		End Class

	End Class
End Class