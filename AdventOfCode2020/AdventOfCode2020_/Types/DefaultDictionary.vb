Public Class DefaultDictionary(Of TKey, TValue As New)
	Inherits Dictionary(Of TKey, TValue)

	Default Public Overloads Property Item(ByVal key As TKey) As TValue
		Get
			Dim val As TValue

			If Not TryGetValue(key, val) Then
				val = New TValue()
				Add(key, val)
			End If

			Return val
		End Get
		Set(ByVal value As TValue)
			MyBase.Item(key) = value
		End Set
	End Property

End Class