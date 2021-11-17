Imports AdventOfCode2019.ComLib
Imports System.Runtime.CompilerServices

Namespace Days
    Module Day7
        Private ReadOnly opcodes As Integer() = Array.ConvertAll(LoadLineList("Day7"), AddressOf Integer.Parse)
        Public Sub DayRun()
            Dim PhaseSettings As Integer() = {0, 1, 2, 3, 4}
            Dim Settings = PhaseSettings.GetPermutations(5)
            Dim res1 = 0
            For Each S In Settings
                Dim A1, A2, A3, A4, A5 As New IntComputer(opcodes.Clone)
                Dim R1, R2, R3, R4, R5 As Integer
                R1 = A1.RunProgram({S(0), 0})(0)
                R2 = A2.RunProgram({S(1), R1})(0)
                R3 = A3.RunProgram({S(2), R2})(0)
                R4 = A4.RunProgram({S(3), R3})(0)
                R5 = A5.RunProgram({S(4), R4})(0)
                If R5 > res1 Then res1 = R5
            Next
            Console.WriteLine($"Day 7-1: {res1}")

            PhaseSettings = {5, 6, 7, 8, 9}
            Settings = PhaseSettings.GetPermutations(5)
            For Each S In Settings
                Dim A1, A2, A3, A4, A5 As New IntComputer(opcodes.Clone)
                Dim R1, R2, R3, R4, R5 As Integer
                R1 = A1.RunProgram({S(0), R5})(0)
                R2 = A2.RunProgram({S(1), R1})(0)
                R3 = A3.RunProgram({S(2), R2})(0)
                R4 = A4.RunProgram({S(3), R3})(0)
                R5 = A5.RunProgram({S(4), R4})(0)
                If R5 > res1 Then res1 = R5
            Next
            Console.WriteLine($"Day 7-2: {0}")
        End Sub
        Public Sub DayTest()

        End Sub
        <Extension>
        Private Function GetPermutations(Of T)(List As IEnumerable(Of T), length As Integer) As IEnumerable(Of IEnumerable(Of T))
            If (length = 1) Then Return List.Select(Function(val) New T() {val})

            Return GetPermutations(List, length - 1).SelectMany(
                Function(l) List.Where(
                    Function(e) Not l.Contains(e)
                ),
                Function(t1, t2) t1.Concat(New T() {t2})
            )
        End Function
    End Module
End Namespace
