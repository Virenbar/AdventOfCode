Imports AdventOfCode2019.ComLib
Imports System.Text.RegularExpressions

Namespace Days
    Module Day12
        Private MoonCoords As String() = LoadLines("Day12")
        Private MoonCoordsTest As String() = LoadLines("Day12Test")
        Public Sub DayRun()
            Dim Simulation As New MoonsSim(MoonCoords)
            For i = 1 To 1000
                Simulation.StepSim()
            Next
            Dim res1 = Simulation.GetEnergy
            Console.WriteLine($"Day 12-1: {res1}")

            Simulation = New MoonsSim(MoonCoords)
            Dim res2 = Simulation.GetRepeat
            Console.WriteLine($"Day 12-2: {res2}")
        End Sub
        Public Sub DayTest()
            Dim Simulation As New MoonsSim(MoonCoordsTest)
            For i = 1 To 10
                Simulation.StepSim()
            Next
            Dim resT = Simulation.GetEnergy
            Console.WriteLine($"Day 12 Test: {resT}")
        End Sub
        Private Class MoonsSim
            Private [Step] As Integer
            Private Moons As New List(Of Moon)
            Public Sub New(_moons As String())
                For Each moon In _moons
                    Dim r = Regex.Match(moon, "x=(?<X>[\+\-\d]+).*y=(?<Y>[\+\-\d]+).*z=(?<Z>[\+\-\d]+)")
                    Moons.Add(New Moon(r.Groups("X").Value, r.Groups("Y").Value, r.Groups("Z").Value))
                Next
            End Sub
            Public Sub StepSim()
                Dim pairs As New List(Of Tuple(Of Moon, Moon))
                For i = 0 To Moons.Count - 2
                    Dim newPairs = Moons.Zip(Moons.Skip(i + 1), Function(a, b) Tuple.Create(a, b))
                    pairs.AddRange(newPairs)
                Next
                For Each pair In pairs
                    CalcVel(pair.Item1, pair.Item2)
                Next
                For Each moon In Moons
                    moon.StepPos()
                Next
            End Sub
            Public Function GetEnergy() As Integer
                GetEnergy = 0
                For Each moon In Moons
                    Dim e = 0
                    With moon.Pos
                        e += Math.Abs(.X) + Math.Abs(.Y) + Math.Abs(.Z)
                    End With
                    With moon.Vel
                        e *= Math.Abs(.X) + Math.Abs(.Y) + Math.Abs(.Z)
                    End With
                    GetEnergy += e
                Next
            End Function
            Public Function GetRepeat() As ULong
                Dim steps As ULong = 0
                Dim vx As ULong = 0 : Dim vy As ULong = 0 : Dim vz As ULong = 0
                While True
                    StepSim()
                    steps += 1
                    If vx = 0 AndAlso Moons.Sum(Function(x) Math.Abs(x.Vel.X)) = 0 Then vx = steps * 2
                    If vy = 0 AndAlso Moons.Sum(Function(x) Math.Abs(x.Vel.Y)) = 0 Then vy = steps * 2
                    If vz = 0 AndAlso Moons.Sum(Function(x) Math.Abs(x.Vel.Z)) = 0 Then vz = steps * 2
                    If vx <> 0 AndAlso vy <> 0 AndAlso vz <> 0 Then Exit While
                End While
                Return lcm(vx, lcm(vy, vz))
            End Function
            Private Sub CalcVel(m1 As Moon, m2 As Moon)
                Dim dx = Math.Sign(m1.Pos.X - m2.Pos.X)
                If dx <> 0 Then m1.Vel.X += -dx : m2.Vel.X += dx
                Dim dy = Math.Sign(m1.Pos.Y - m2.Pos.Y)
                If dy <> 0 Then m1.Vel.Y += -dy : m2.Vel.Y += dy
                Dim dz = Math.Sign(m1.Pos.Z - m2.Pos.Z)
                If dz <> 0 Then m1.Vel.Z += -dz : m2.Vel.Z += dz
            End Sub
            Class Moon
                Public Pos As Point3D
                Public Vel As Point3D
                Public Sub New(x As Integer, y As Integer, z As Integer)
                    Pos.X = x : Pos.Y = y : Pos.Z = z
                End Sub
                Public Sub StepPos()
                    Pos.X += Vel.X : Pos.Y += Vel.Y : Pos.Z += Vel.Z
                End Sub
            End Class
            Structure Point3D
                Public X As Integer
                Public Y As Integer
                Public Z As Integer
                Sub New(_x As Integer, _y As Integer, _z As Integer)
                    X = _x : Y = _y : Z = _z
                End Sub
            End Structure
            Private Function gcd(a, b)
                ' Euclidean algorithm
                Dim t
                While (b <> 0)
                    t = b
                    b = a Mod b
                    a = t
                End While
                Return a
            End Function

            Private Function lcm(a, b)
                Return (a * b / gcd(a, b))
            End Function
        End Class
    End Module
End Namespace
