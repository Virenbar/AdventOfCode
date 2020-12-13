Public Class Day13
	Inherits BaseDay

	Public Sub New()
		MyBase.New(13)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim S = New Schedule(RawTest)
		Dim C = S.GetClosest
		Return C = 295
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim S = New Schedule(RawTest)
		Dim C = S.GetTimestamp
		Return C = 1068781
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim S = New Schedule(Raw)
		Dim C = S.GetClosest
		Return C
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim S = New Schedule(Raw)
		Dim C = S.GetTimestamp
		Return C
	End Function

	Private Class Schedule
		Private ReadOnly timestamp As Integer
		Private ReadOnly busIDs As List(Of Integer)

		Public Sub New(str As String)
			Dim s = str.Split(vbNewLine)
			timestamp = CInt(s(0))
			busIDs = s(1).Split(","c).Select(Function(x) If(x = "x", -1, Integer.Parse(x))).ToList()
		End Sub

		Public Function GetClosest() As Integer
			Dim D = busIDs.Where(Function(x) x > 0).ToDictionary(Function(k) k, Function(v) v - timestamp Mod v)
			Dim M = D.OrderBy(Function(x) x.Value).FirstOrDefault
			Return M.Key * M.Value
		End Function

		Public Function GetTimestamp() As Long
			Dim D = busIDs.Select(Function(id, i) (id, i)).Where(Function(x) x.id > 0).ToDictionary(Function(k) k.i, Function(v) v.id)
			Dim period As Long = D.First.Value
			Dim timestamp As Long = 0
			For Each busI In D.Keys.Skip(1)
				While (timestamp + busI) Mod D(busI) <> 0
					timestamp += period
				End While
				period *= D(busI)
			Next
			Return timestamp
		End Function

	End Class
End Class