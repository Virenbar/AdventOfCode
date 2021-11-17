Imports System.Text.RegularExpressions

Public Class Day14
	Inherits BaseDay
	Private Shared MR As New Regex("mask = (.+)")
	Private Shared AR As New Regex("mem\[(\d+)\] = (\d+)")
	Const ML = 36

	Public Sub New()
		MyBase.New(14)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim P = New DocProg(StringListTest)
		P.Run()
		Dim S = P.Sum
		Return S = 165
	End Function

	Protected Overrides Function TestPart2() As Boolean
		Dim P = New DocProgV2(StringListTestA)
		P.Run()
		Dim S = P.Sum
		Return S = 208
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim P = New DocProg(StringList)
		P.Run()
		Dim S = P.Sum
		Return S
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim P = New DocProgV2(StringList)
		P.Run()
		Dim S = P.Sum
		Return S
	End Function

	Private Class DocProg
		Private mem As New DefaultDictionary(Of Integer, Long)
		Private mask As Dictionary(Of Integer, Boolean)
		Private instrs As List(Of Instruction)

		Public Sub New(strs As List(Of String))
			instrs = ParseInstr(strs)
		End Sub

		Public ReadOnly Property Sum As Long
			Get
				Return mem.Values.Sum
			End Get
		End Property

		Public Sub Run()
			instrs.ForEach(
				Sub(i)
					If i.Mask IsNot Nothing Then
						mask = i.Mask
						Exit Sub
					End If
					mem(i.Address) = ApplyMask(i.Value)
				End Sub)
		End Sub

		Private Function ApplyMask(value As Long) As Long
			For Each m In mask
				Dim i = 35 - m.Key
				If m.Value Then
					value = (value Or CLng(1) << i)
				Else
					value = value And Long.MaxValue - (CLng(1) << i)
				End If
			Next
			Return value
		End Function

		Private Class Instruction
			Public Property Mask As Dictionary(Of Integer, Boolean) = Nothing
			Public Property Address As Integer = 0
			Public Property Value As Integer = 0
		End Class

		Private Shared Function ParseInstr(strs As List(Of String)) As List(Of Instruction)
			Return strs.Select(
				Function(x)
					Dim MA = AR.Match(x)
					If MA.Success Then
						Return New Instruction With {.Address = CInt(MA.Groups(1).Value), .Value = CInt(MA.Groups(2).Value)}
					End If
					Dim Mask = MR.Match(x).Groups(1).Value
					Dim D = Mask.Select(Function(c, i) (c, i)).Where(Function(m) m.c <> "X"c).ToDictionary(Function(k) k.i, Function(v) v.c = "1"c)
					Return New Instruction With {.Mask = D}
				End Function).ToList
		End Function

	End Class
	Private Class DocProgV2
		Private mem As New DefaultDictionary(Of String, Long)
		Private mask As String
		Private instrs As List(Of Instruction)

		Public Sub New(strs As List(Of String))
			instrs = ParseInstr(strs)
		End Sub

		Public ReadOnly Property Sum As Long
			Get
				Return mem.Values.Sum
			End Get
		End Property

		Public Sub Run()
			instrs.ForEach(
				Sub(i)
					If i.Mask IsNot Nothing Then
						mask = i.Mask
						Exit Sub
					End If
					Dim Address = String.Concat(mask.Zip(i.Address, Function(M, A) (M, A)).Select(Function(x) If(x.M <> "0"c, x.M, x.A)))
					Dim AL0 = GetAddressList(Address)
					For Each A In AL0
						mem(A) = i.Value
					Next
				End Sub)
		End Sub

		Private Shared Function GetAddressList(value As String) As List(Of String)
			If value.Any(Function(c) c = "X"c) Then
				Dim Mask0 = value, Mask1 = value
				Dim i = value.IndexOf("X"c)
				If i >= 0 Then
					Mask0 = Mask0.Remove(i, 1).Insert(i, "0"c)
					Mask1 = Mask1.Remove(i, 1).Insert(i, "1"c)
					Return GetAddressList(Mask0).Concat(GetAddressList(Mask1)).ToList()
				Else
					Return {Mask0, Mask1}.ToList()
				End If
			Else
				Return New List(Of String)({value})
			End If
		End Function

		Private Class Instruction
			Public Property Mask As String
			Public Property Address As String
			Public Property Value As Integer = 0
		End Class

		Private Shared Function ParseInstr(strs As List(Of String)) As List(Of Instruction)
			Return strs.Select(
				Function(x)
					Dim MA = AR.Match(x)
					If MA.Success Then
						Return New Instruction With {.Address = Convert.ToString(Long.Parse(MA.Groups(1).Value), 2).PadLeft(ML, "0"c), .Value = CInt(MA.Groups(2).Value)}
					End If
					Dim Mask = MR.Match(x).Groups(1).Value
					Return New Instruction With {.Mask = Mask}
				End Function).ToList
		End Function

	End Class
End Class