Class IntComputer
    'Private ReadOnly OCLength = 2, ModeLength = 3
    Private IntProgram As Integer()
    Private OpCode As String
    Private Pos As Integer
    Private Input As New Queue(Of Integer)
    Private Output As New List(Of Integer)
    Private PauseMode, RunDone As Boolean
    Public Sub New(_opcodes As Integer(), Optional pm As Boolean = False)
        IntProgram = _opcodes : Pos = 0
        RunDone = False
        PauseMode = pm
    End Sub
    Public Function RunProgram(Optional _input As Integer() = Nothing) As List(Of Integer)
        Input = New Queue(Of Integer)(_input)
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
                    SetValue(GetParam(1), GetInput())
                    Pos += 2
                Case 4
                    Dim out = GetParamValue(1)
                    Pos += 2
                    If PauseMode Then Return out Else Console.WriteLine(out) : Output.Add(out)
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
        RunDone = True
        Return Output
    End Function
    Public Function GetInput() As Integer
        If Input.Count > 0 Then Return Input.Dequeue Else Return CInt(Console.ReadLine())
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