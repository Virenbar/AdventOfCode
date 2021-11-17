Imports AdventOfCode2019.ComLib
Imports System.Text.RegularExpressions
Namespace Days
    Module Day4
        Private ReadOnly Range As String = LoadLine("Day4")
        Public Sub DayRun()
            Dim res1 = 0 : Dim res2 = 0
            For psw As Integer = Range.Substring(0, 6) To Range.Substring(7, 6)
                If IsMeetCriteria(psw) Then res1 += 1
                If IsMeetCriteria(psw, True) Then res2 += 1
            Next
            Console.WriteLine($"Day 4-1: {res1}")
            Console.WriteLine($"Day 4-2: {res2}")
        End Sub
        Private Function IsMeetCriteria(psw As String, Optional strict As Boolean = False) As Boolean
            Dim DoubleDigit = False
            Dim PrevDigit = 0
            For Each chr In psw
                Dim Digit = CInt(chr.ToString)
                If Digit < PrevDigit Then Return False
                PrevDigit = Digit
            Next
            For i = 0 To 9
                For Each match As Match In Regex.Matches(psw, $"[{i}]+")
                    If Not strict AndAlso match.Length >= 2 Then DoubleDigit = True
                    If strict AndAlso match.Length = 2 Then DoubleDigit = True
                Next
            Next
            Return DoubleDigit
        End Function
    End Module
End Namespace
