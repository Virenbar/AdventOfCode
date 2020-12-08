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

End Module