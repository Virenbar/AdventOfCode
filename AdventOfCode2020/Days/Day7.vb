Imports System.Text.RegularExpressions

Public Class Day7
	Inherits BaseDay
	Private Shared ReadOnly BagR As New Regex("(\w+ \w+) bags")
	Private Shared ReadOnly InBagR As New Regex("(\d) (\w+ \w+) bag")
	Private Const MyBag = "shiny gold"

	Public Sub New()
		MyBase.New(7)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim Rules = ParseRules(StringListTest)
		Dim C = Rules(MyBag).InsideOfColors().ToHashSet.Count
		Return C = 4
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim Rules = ParseRules(StringListTestA)
		Dim C = Rules(MyBag).CountBags()
		Return C = 126
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim Rules = ParseRules(StringList)
		Dim C = Rules(MyBag).InsideOfColors().ToHashSet.Count
		Return C
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim Rules = ParseRules(StringList)
		Dim C = Rules(MyBag).CountBags()
		Return C
	End Function

	Private Shared Function ParseRules(strs As List(Of String)) As Dictionary(Of String, BagRule)
		Dim D = New Dictionary(Of String, BagRule)
		For Each str In strs
			Dim Rule = New BagRule
			Dim M1 = BagR.Match(str)
			Rule.BagColor = M1.Groups(1).Value
			Dim M2 = InBagR.Matches(str)
			If M2.Count > 0 Then M2.ToList.ForEach(Sub(x) Rule.InBagColors.Add(x.Groups(2).Value, CInt(x.Groups(1).Value)))
			D.Add(Rule.BagColor, Rule)
		Next
		For Each Rule In D.Values
			For Each C In Rule.InBagColors
				Rule.Contains.Add(D(C.Key), C.Value)
				D(C.Key).InsideOf.Add(Rule)
			Next
		Next
		Return D
	End Function

	Private Class BagRule
		Public Property BagColor As String
		Public ReadOnly Property InBagColors As New Dictionary(Of String, Integer)
		Public ReadOnly Property Contains As New Dictionary(Of BagRule, Integer)
		Public ReadOnly Property InsideOf As New HashSet(Of BagRule)

		Public Function InsideOfColors() As List(Of String)
			Return InsideOf.SelectMany(Function(x) x.InsideOfColors.Concat({x.BagColor})).ToList()
		End Function

		Public Function CountBags() As Integer
			Return Contains.Sum(Function(x) x.Value) + Contains.Sum(Function(x) x.Key.CountBags() * x.Value)
		End Function

		Public Overrides Function ToString() As String
			Return $"{BagColor} - {String.Join(" ", InBagColors.Select(Function(x) $"{x.Value} {x.Key}"))}"
		End Function

	End Class
End Class