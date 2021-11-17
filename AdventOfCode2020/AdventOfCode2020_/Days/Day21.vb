Imports System.Text.RegularExpressions

Public Class Day21
	Inherits BaseDay

	Public Sub New()
		MyBase.New(21)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim AF = New AllergensFinder(StringListTest)
		AF.Process()
		Dim S = AF.CountSafe
		Return S = 5
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim AF = New AllergensFinder(StringListTest)
		AF.Process()
		Dim S = AF.GetNotSafe
		Return S = "mxmxvkd,sqjhc,fvjkl"
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim AF = New AllergensFinder(StringList)
		AF.Process()
		Dim S = AF.CountSafe
		Return S
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim AF = New AllergensFinder(StringList)
		AF.Process()
		Dim S = AF.GetNotSafe
		Return S
	End Function

	Private Class AllergensFinder
		Private Shared R As New Regex("(.+) \(contains (.+)\)")
		Private foods As List(Of (ingredients As List(Of String), allergens As List(Of String)))
		Private allergenIngredients As Dictionary(Of String, HashSet(Of String))
		Private notSafeIngredients As HashSet(Of String)

		Public Sub New(strs As List(Of String))
			foods = strs.Select(
				Function(s)
					Dim M = R.Match(s)
					Return (M.Groups(1).Value.Split(" ").ToList(), M.Groups(2).Value.Split(", ").ToList())
				End Function).ToList
		End Sub

		Public Sub Process()
			Dim Allergens = foods.SelectMany(Function(f) f.allergens).Distinct().ToArray()
			allergenIngredients = Allergens.Select(
				Function(a)
					Dim allergen = a
					Dim candidates = foods.Where(Function(f) f.allergens.Contains(a)).Select(Function(f) f.ingredients).Aggregate(Function(agr, b) agr.Intersect(b).ToList).ToHashSet()
					Return (allergen, candidates)
				End Function).ToDictionary(Function(a) a.allergen, Function(a) a.candidates)

			notSafeIngredients = New HashSet(Of String)(allergenIngredients.SelectMany(Function(kv) kv.Value))
		End Sub

		Public Function CountSafe() As Integer
			Return foods.Sum(Function(f) f.ingredients.Where(Function(i) Not notSafeIngredients.Contains(i)).Count)
		End Function

		Public Function GetNotSafe() As String
			While (allergenIngredients.Any(Function(kv) kv.Value.Count() > 1))
				Dim singles = New HashSet(Of String)(allergenIngredients.Where(Function(kv) kv.Value.Count() = 1).Select(Function(kv) kv.Value.Single()))
				For Each kv In allergenIngredients
					If (kv.Value.Count() > 1) Then kv.Value.ExceptWith(singles)
				Next
			End While
			Return String.Join(",", allergenIngredients.OrderBy(Function(kv) kv.Key).Select(Function(kv) kv.Value.Single()))
		End Function

	End Class
End Class