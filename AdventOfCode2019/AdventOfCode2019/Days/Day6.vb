Imports AdventOfCode2019.ComLib
Namespace Days
    Module Day6
        Private ReadOnly Orbits = LoadLines("Day6")
        Public Sub DayRun()
            Dim Map = New UOM(Orbits)

            Dim res1 = Map.GetOrbitsCount
            Console.WriteLine($"Day 6-1: {res1}")

            Dim res2 = Map.GetMinimalTransfers()
            Console.WriteLine($"Day 6-2: {res2}")
        End Sub
        Public Sub DayTest()
            Dim TestMap = New UOM({"COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L"})
            Dim res = TestMap.GetOrbitsCount
            Console.WriteLine($"Day 6 Test: {res}")
        End Sub
        Private Class UOM
            Private ReadOnly Orbits As Dictionary(Of String, String)
            Public Sub New(_orbits As String())
                Orbits = _orbits.ToDictionary(Function(orb) orb.Split(")")(1), Function(orb) orb.Split(")")(0))
            End Sub
            Public Function GetOrbitsCount() As Integer
                Dim Count = 0
                For Each obj In Orbits.Keys
                    Count += GetParentObjects(obj).Count
                Next
                Return Count
            End Function
            Public Function GetMinimalTransfers() As Integer
                Dim YOU = GetParentObjects("YOU")
                Dim SAN = GetParentObjects("SAN")
                Dim Common = 0 'YOU.Intersect(SAN).Count
                While YOU(Common) = SAN(Common)
                    Common += 1
                End While
                Return YOU.Count + SAN.Count - Common * 2
            End Function
            Private Function GetParentObjects(obj As String) As List(Of String)
                Dim parents As New List(Of String)
                Do
                    parents.Insert(0, Orbits(obj))
                    obj = Orbits(obj)
                Loop While obj <> "COM"
                Return parents
            End Function
            Private Function GetParentObjectsRec(obj As String) As List(Of String)
                If obj = "COM" Then
                    Return New List(Of String)()
                Else
                    Dim parent = Orbits(obj)
                    Dim Parents = GetParentObjectsRec(parent)
                    Parents.Add(parent)
                    Return Parents
                End If
            End Function
        End Class
    End Module
End Namespace
