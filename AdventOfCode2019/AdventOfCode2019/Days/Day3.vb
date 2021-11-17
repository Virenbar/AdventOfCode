Imports AdventOfCode2019.ComLib

Namespace Days
    Module Day3
        Private ReadOnly Wires = LoadLines("Day3")
        Public Sub DayRun()
            Dim WireOne = GetPath(Wires(0))
            Dim WireTwo = GetPath(Wires(1))
            Dim Crosses = WireOne.Keys.Intersect(WireTwo.Keys
                                                 )
            Dim res1 = Crosses.Min(Function(cross) Math.Abs(cross.Item1) + Math.Abs(cross.Item2))
            Console.WriteLine($"Day 3-1: {res1}")

            Dim res2 = Crosses.Min(Function(cross) WireOne(cross) + WireTwo(cross))
            Console.WriteLine($"Day 3-2: {res2}")
        End Sub

        Private Function GetPath(wire As String) As Dictionary(Of (Integer, Integer), Integer)
            Dim x = 0, y = 0, l = 0
            Dim path = New Dictionary(Of (Integer, Integer), Integer)
            For Each p In wire.Split(",")
                Select Case p(0)
                    Case "U"
                        For i = 1 To CInt(p.Substring(1))
                            l += 1 : y += 1
                            path.TryAdd((x, y), l)
                        Next
                    Case "D"
                        For i = 1 To CInt(p.Substring(1))
                            l += 1 : y -= 1
                            path.TryAdd((x, y), l)
                        Next
                    Case "L"
                        For i = 1 To CInt(p.Substring(1))
                            l += 1 : x -= 1
                            path.TryAdd((x, y), l)
                        Next
                    Case "R"
                        For i = 1 To CInt(p.Substring(1))
                            l += 1 : x += 1
                            path.TryAdd((x, y), l)
                        Next
                End Select
            Next
            Return path
        End Function
    End Module
End Namespace
