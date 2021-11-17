Imports AdventOfCode2019.ComLib
Imports LayerType = System.Collections.Generic.Dictionary(Of (Integer, Integer), Integer)

Namespace Days
    Module Day8
        Private Image As Integer() = Array.ConvertAll(LoadLine("Day8").ToArray(), AddressOf Integer.Parse)
        Private W As Integer = 25 : Private H As Integer = 6
        Public Sub DayRun()
            Dim LCount As Integer = Image.Length / (W * H)
            Dim ColorCount = New LayerType
            Dim Layers = New List(Of LayerType)(LCount)
            For l = 0 To LCount - 1
                Dim Layer = New LayerType
                For i = 0 To W * H - 1
                    Dim Color = Image(l * W * H + i)
                    Layer.Add((i Mod W, i \ W), Color)
                    'CC(l, Color) += 1
                Next
                Layers.Add(Layer)
            Next
            Dim LayerLess0 = Layers.Aggregate(
                Function(x, y)
                    If x.Where(Function(p) p.Value = 0).Count < y.Where(Function(p) p.Value = 0).Count Then
                        Return x
                    Else
                        Return y
                    End If
                End Function)
            Dim n1 = LayerLess0.Where(Function(p) p.Value = 1).Count
            Dim n2 = LayerLess0.Where(Function(p) p.Value = 2).Count
            Dim res1 = n1 * n2
            Console.WriteLine($"Day 8-1: {res1}")

            Dim res2 = New LayerType(Layers(0))
            For Each key In Layers(0).Keys
                For Each l In Layers
                    Dim color = l(key)
                    If color <> 2 Then
                        res2(key) = color
                        Exit For
                    End If
                Next
            Next
            Console.WriteLine($"Day 8-2:")
            For iH = 0 To H - 1
                For iW = 0 To W - 1
                    Console.Write(If(res2((iW, iH)) = 0, " ", "#"))
                Next
                Console.WriteLine()
            Next
        End Sub
        Public Sub DayTest()

        End Sub
    End Module
End Namespace
