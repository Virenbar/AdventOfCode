Imports AdventOfCode2019.ComLib

Namespace Days
    Module Day1
        Public Sub Day1_1()
            Dim Modules = LoadLines("Day1")
            Dim TotalFuel = 0
            For Each [module] In Modules
                Dim fuel As Integer = Math.Floor(CInt([module]) / 3) - 2
                TotalFuel += fuel
            Next
            Console.WriteLine($"Day 1-1: {TotalFuel.ToString}")
        End Sub
        Public Sub Day1_2()
            Dim Modules = LoadLines("Day1")
            Dim TotalFuel = 0
            For Each [module] In Modules
                TotalFuel += FuelNeeded(CInt([module]))
            Next
            Console.WriteLine($"Day 1-2: {TotalFuel.ToString}")
        End Sub
        Private Function FuelNeeded(mass As Integer) As Integer
            Dim fuel As Integer = Math.Floor(mass / 3) - 2
            If fuel < 1 Then Return 0
            Return fuel + FuelNeeded(fuel)
        End Function
    End Module
End Namespace
