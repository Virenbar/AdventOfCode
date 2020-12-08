Imports AdventOfCode2020.Computer

Public Class Day8
	Inherits BaseDay

	Public Sub New()
		MyBase.New(8)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim Com = New AssComputer(ParseInstructions(StringListTest))
		Com.RunToLoop()
		Return Com.Accumulator = 5
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim Com = New AssComputer(ParseInstructions(StringListTest))
		Com.TryFix()
		Return Com.Accumulator = 8
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim Com = New AssComputer(ParseInstructions(StringList))
		Com.RunToLoop()
		Return Com.Accumulator
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim Com = New AssComputer(ParseInstructions(StringList))
		Com.TryFix()
		Return Com.Accumulator
	End Function

	Private Function ParseInstructions(strs As List(Of String)) As List(Of Instruction)
		Return strs.Select(
			Function(x)
				Return New Instruction With {
					.Operation = DirectCast([Enum].Parse(GetType(Operation), x.Substring(0, 3)), Operation),
					.Argument = CInt(x.Substring(4))}
			End Function).ToList()
	End Function

End Class