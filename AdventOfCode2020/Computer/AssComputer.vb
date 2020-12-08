Namespace Computer
	Public Class AssComputer
		Private _accumulator As Integer
		Private instructions As List(Of Instruction)
		Private pointer As Integer

		Public Sub New(input As List(Of Instruction))
			instructions = input
			Reset()
		End Sub

		Public Property Accumulator() As Integer
			Get
				Return _accumulator
			End Get
			Private Set(value As Integer)
				_accumulator = value
			End Set
		End Property

		Public Function RunToLoop() As Boolean
			Reset()
			Dim H = New HashSet(Of Instruction)
			Dim I = instructions(pointer)
			While Not H.Contains(I)
				ExecuteInstruction(I)
				H.Add(I)
				If pointer >= instructions.Count Then Return False
				I = instructions(pointer)
			End While
			Return True
		End Function

		Public Function TryFix() As Boolean
			Dim Check = instructions.Where(Function(x) x.Operation = Operation.jmp Or x.Operation = Operation.nop).ToList()
			For Each I In Check
				I.Exchange
				If Not RunToLoop() Then Return True
				I.Exchange
			Next
			Return False
		End Function

		Private Sub ExecuteInstruction(I As Instruction)
			Select Case I.Operation
				Case Operation.acc
					Accumulator += I.Argument
					pointer += 1
				Case Operation.jmp
					pointer += I.Argument
				Case Operation.nop
					pointer += 1
				Case Else
					Throw New NotImplementedException($"{I.Operation}")
			End Select
		End Sub

		Private Sub Reset()
			pointer = 0
			Accumulator = 0
		End Sub

	End Class
End Namespace