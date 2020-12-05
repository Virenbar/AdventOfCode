Public Class Result

	Public Sub New()

	End Sub

	Public Sub New(_p1 As Object, _p2 As Object)
		P1 = _p1.ToString()
		P2 = _p2.ToString()
	End Sub

	Public Property P1 As String

	Public Property P2 As String

End Class