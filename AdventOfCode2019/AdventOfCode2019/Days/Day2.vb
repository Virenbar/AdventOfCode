Imports AdventOfCode2019.ComLib

Namespace Days
    Module Day2
        Private ReadOnly opcodes = Array.ConvertAll(LoadLineList("Day2"), AddressOf Integer.Parse)
        Public Sub DayRun()
            Dim res1 = RunProgram(opcodes.Clone(), 12, 2)
            Console.WriteLine($"Day 2-1: {res1}")

            Dim res2 = 0
            For n = 0 To 99
                For v = 0 To 99
                    If RunProgram(opcodes.Clone(), n, v) = 19690720 Then res2 = n * 100 + v
                Next
            Next
            Console.WriteLine($"Day 2-2: {res2}")
        End Sub
        Private Function RunProgram(opcodes As Integer(), op1 As Integer, op2 As Integer) As Integer
            Dim pos = 0
            opcodes(1) = op1
            opcodes(2) = op2
            While True
                Dim opcode = opcodes(pos)
                Select Case opcode
                    Case 1
                        opcodes(opcodes(pos + 3)) = opcodes(opcodes(pos + 1)) + opcodes(opcodes(pos + 2))
                        pos += 4
                    Case 2
                        opcodes(opcodes(pos + 3)) = opcodes(opcodes(pos + 1)) * opcodes(opcodes(pos + 2))
                        pos += 4
                    Case 99 : Exit While
                    Case Else : Throw New NotImplementedException()
                End Select
            End While
            Return opcodes(0)
        End Function
    End Module
End Namespace
