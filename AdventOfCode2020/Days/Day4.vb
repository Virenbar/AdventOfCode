Imports System.Text.RegularExpressions

Public Class Day4
	Inherits BaseDay
	Private ReadOnly R As New Regex("(?'key'\w{3}):(?'value'[\w#]+)", RegexOptions.ExplicitCapture)

	Public Sub New()
		MyBase.New(4)
	End Sub

	Protected Overrides Function TestPart1() As Boolean
		Dim P = ParsePassports(RawTest.Split(vbNewLine + vbNewLine).ToList())
		Dim V = P.Where(Function(x) x.IsValid).ToList
		Return V.Count = 2
	End Function

	Protected Overrides Function SolvePart1() As Object
		Dim P = ParsePassports(Raw.Split(vbNewLine + vbNewLine).ToList())
		Dim V = P.Where(Function(x) x.IsValid).ToList
		Return V.Count
	End Function

	Protected Overrides Function SolvePart2() As Object
		Dim P = ParsePassports(Raw.Split(vbNewLine + vbNewLine).ToList())
		Dim V = P.Where(Function(x) x.IsValidValues).ToList
		Return V.Count
	End Function

	Private Function ParsePassports(strs As List(Of String)) As List(Of Passport)
		Return strs.Select(
			Function(S)
				Dim P = S.Replace(vbNewLine, " ")
				Dim M = R.Matches(P)
				Dim D = M.ToDictionary((Function(x) x.Groups.Item("key").Value), (Function(x) x.Groups.Item("value").Value))
				Return New Passport(D)
			End Function).ToList()
	End Function

	Private Class Passport
		Private Shared ReadOnly reqFields As New List(Of String) From {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"}
		Private Shared ReadOnly colors As String() = {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}
		Private Shared ReadOnly RH As New Regex("#[0-9a-f]{6}")
		Private _fields As Dictionary(Of String, String)

		Public Sub New(fields As Dictionary(Of String, String))
			_fields = fields
		End Sub

		Public ReadOnly Property IsValid() As Boolean
			Get
				Return Not reqFields.Except(_fields.Keys).Count > 0
			End Get
		End Property

		Public Function IsValidValues() As Boolean
			If Not IsValid Then Return False
			Dim byr = CInt(_fields("byr"))
			Dim iyr = CInt(_fields("iyr"))
			Dim eyr = CInt(_fields("eyr"))
			Dim hgt = _fields("hgt")
			Dim hcl = _fields("hcl")
			Dim ecl = _fields("ecl")
			Dim pid = _fields("pid")

			If Not (byr.IsBetween(1920, 2002) AndAlso iyr.IsBetween(2010, 2020) AndAlso eyr.IsBetween(2020, 2030)) Then Return False
			If hgt.EndsWith("cm") And hgt.Length = 5 Then
				If Not CInt(hgt.Substring(0, 3)).IsBetween(150, 193) Then Return False
			ElseIf hgt.EndsWith("in") And hgt.Length = 4 Then
				If Not CInt(hgt.Substring(0, 2)).IsBetween(59, 76) Then Return False
			Else
				Return False
			End If
			If Not RH.Match(hcl).Success Then Return False
			If Not (colors.Contains(ecl) And pid.Length = 9) Then Return False
			Return True
		End Function

	End Class
End Class