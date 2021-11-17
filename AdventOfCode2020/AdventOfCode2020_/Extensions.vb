Imports System.Runtime.CompilerServices
Imports AdventOfCode2020.Computer

Module Extensions

	<Extension>
	Public Function IsBetween(I As Integer, min As Integer, max As Integer) As Boolean
		Return min <= I And I <= max
	End Function

	<Extension>
	Public Sub Exchange(I As Instruction)
		I.Operation = If(I.Operation = Operation.jmp, Operation.nop, Operation.jmp)
	End Sub
	<Extension>
	Public Function Multiply(E As IEnumerable(Of Integer)) As Long
		Return E.Aggregate(Of Long)(1, Function(agr, i) agr * i)
	End Function

	<Extension>
	Public Function Multiply(E As IEnumerable(Of Long)) As Long
		Return E.Aggregate(Of Long)(1, Function(agr, i) agr * i)
	End Function

	<Extension>
	Public Function Multiply(Of T)(E As IEnumerable(Of T), selector As Func(Of T, Long)) As Long
		Return E.Aggregate(Of Long)(1, Function(agr, i) agr * selector(i))
	End Function

End Module