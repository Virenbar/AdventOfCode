Imports System.IO

Namespace ComLib
    Module ComIO
        Public Function LoadLines(name As String) As String()
            Return File.ReadAllLines(Path.Combine("Input", name + ".txt"))
        End Function
        Public Function LoadLine(name As String) As String
            Return File.ReadAllText(Path.Combine("Input", name + ".txt"))
        End Function
        Public Function LoadLineList(name As String) As String()
            Return File.ReadAllText(Path.Combine("Input", name + ".txt")).Split(",")
        End Function
    End Module
End Namespace