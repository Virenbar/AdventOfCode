Imports System.Runtime.CompilerServices

Module Extensions

	<Extension>
	Public Function IsBetween(I As Integer, min As Integer, max As Integer) As Boolean
		Return min <= I And I <= max
	End Function

End Module