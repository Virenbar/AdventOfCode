Imports AdventOfCode2019.ComLib
Namespace Days
    Module Day5
        Private ReadOnly opcodes As Integer() = Array.ConvertAll(LoadLineList("Day5"), AddressOf Integer.Parse)
        Public Sub DayRun()
            Dim TEST = New IntComputer(opcodes.Clone)
            Dim res1 = TEST.RunProgram()
            Console.WriteLine($"Day 5-1: {res1.Last}")

            TEST = New IntComputer(opcodes.Clone)
            Dim res2 = TEST.RunProgram()
            Console.WriteLine($"Day 5-2: {res2.Last}")
        End Sub
        Public Sub DayTest()
            Dim opcodes As Integer() = {3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99}
            Dim TEST = New IntComputer(opcodes.Clone)
            Dim res = TEST.RunProgram()
            Console.WriteLine($"Day 5 Test: {res.Last}")
        End Sub
        Private Class IntComputer
            'Private ReadOnly OCLength = 2, ModeLength = 3
            Private IntProgram As Integer()
            Private OpCode As String
            Private Pos As Integer
            Private Output = New List(Of Integer)
            Public Sub New(_opcodes As Integer())
                IntProgram = _opcodes
                Pos = 0
            End Sub
            Public Function RunProgram(Optional Input As List(Of Integer) = Nothing) As List(Of Integer)
                While True
                    OpCode = IntProgram(Pos).ToString.PadLeft(5, "0")
                    Select Case CInt(OpCode.Substring(3, 2))
                        Case 1
                            IntProgram(GetParam(3)) = GetParamValue(1) + GetParamValue(2)
                            Pos += 4
                        Case 2
                            IntProgram(GetParam(3)) = GetParamValue(1) * GetParamValue(2)
                            Pos += 4
                        Case 3
                            SetValue(GetParam(1), CInt(Console.ReadLine))
                            Pos += 2
                        Case 4
                            Dim out = GetParamValue(1)
                            Console.WriteLine(out) : Output.Add(out)
                            Pos += 2
                        Case 5
                            If GetParamValue(1) <> 0 Then Pos = GetParamValue(2) Else Pos += 3
                        Case 6
                            If GetParamValue(1) = 0 Then Pos = GetParamValue(2) Else Pos += 3
                        Case 7
                            If GetParamValue(1) < GetParamValue(2) Then SetValue(GetParam(3), 1) Else SetValue(GetParam(3), 0)
                            Pos += 4
                        Case 8
                            If GetParamValue(1) = GetParamValue(2) Then SetValue(GetParam(3), 1) Else SetValue(GetParam(3), 0)
                            Pos += 4
                        Case 99 : Exit While
                        Case Else : Throw New NotImplementedException()
                    End Select
                End While
                Return Output
            End Function
            Private Function GetParam(p As Integer)
                Return IntProgram(Pos + p)
            End Function
            Private Function GetParamValue(p As Integer)
                Dim mode = OpCode(3 - p)
                If mode = "0" Then
                    Return GetValue(GetParam(p))
                Else
                    Return GetParam(p)
                End If
            End Function
            Private Function GetValue(pos As Integer) As Integer
                Return IntProgram(pos)
            End Function
            Private Sub SetValue(pos As Integer, val As Integer)
                IntProgram(pos) = val
            End Sub
        End Class

    End Module
End Namespace
