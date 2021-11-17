Imports AdventOfCode2019.ComLib

Namespace Days
    Module Day10
        Private AsteroidMap As String() = LoadLines("Day10")
        Public Sub DayRun()
            Dim Asteroids = New AsteroidField(AsteroidMap)
            Dim Station = Asteroids.FindBestAsteroid
            Dim res1 = Station.Visible.Count
            Console.WriteLine($"Day 10-1: {res1}")

            Dim n200 = Station.Visible.OrderBy(Function(x) If(x.Key < 0, x.Key + 360, x.Key))(199).Value.Asteroid
            Dim res2 = n200.x * 100 + n200.y
            Console.WriteLine($"Day 10-2: {res2}")
        End Sub
        Public Sub DayTest()

        End Sub
        Private Class AsteroidField
            Private Asteroids As New List(Of Asteroid)
            Public Sub New(map As String())
                For Y = 0 To map.GetUpperBound(0)
                    For X = 0 To map(Y).Length - 1
                        If map(Y)(X) = "#" Then
                            Dim astr = New Asteroid With {.X = X, .Y = Y}
                            Asteroids.Add(astr)
                        End If
                    Next
                Next
            End Sub
            Public Function FindBestAsteroid() As Asteroid
                Dim best As New Asteroid
                For Each astrSt In Asteroids
                    For Each astr In Asteroids
                        If astr.Equals(astrSt) Then Continue For
                        Dim xDiff = astr.X - astrSt.X : Dim yDiff = astr.Y - astrSt.Y
                        Dim angle = Math.Atan2(xDiff, -yDiff) * 180.0 / Math.PI
                        Dim distanse = Math.Sqrt(xDiff ^ 2 + yDiff ^ 2)
                        If astrSt.Visible.ContainsKey(angle) Then
                            If distanse < astrSt.Visible(angle).Distance Then
                                astrSt.Visible(angle) = New VisibleAsteroid With {.Distance = distanse, .Asteroid = astr}
                            End If
                        Else
                            astrSt.Visible.Add(angle, New VisibleAsteroid With {.Distance = distanse, .Asteroid = astr})
                        End If
                    Next
                    If astrSt.Visible.Count > best.Visible.Count Then best = astrSt
                Next
                Return best
            End Function
            Class Asteroid
                Public X As Integer
                Public Y As Integer
                Public Visible As New Dictionary(Of Double, VisibleAsteroid)
            End Class
            Structure VisibleAsteroid
                Public Asteroid As Asteroid
                Public Distance As Double
                'Public Angle As Double
            End Structure
        End Class
    End Module
End Namespace
