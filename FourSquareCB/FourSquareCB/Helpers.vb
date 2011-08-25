Imports System.Collections.Generic
Imports System.Linq
Imports System.Text.RegularExpressions

Friend Class PathHelpers
    Public Shared Function ReplaceUriTemplateTokens(ByVal segments As List(Of Object), ByVal path As String) As String

        Dim regexObj = New Regex("\{\w*\}", RegexOptions.Compiled)

        Dim matches = regexObj.Matches(path)
        For Each match As Match In matches
            Dim token = match.Value.Substring(1, match.Value.Length - 2)
            path = ReplacePathSegment(path, token, segments)
        Next
        Return path
    End Function

    Public Shared Function ReplacePathSegment(ByVal path As String, ByVal value As String, ByVal segments As List(Of Object)) As String
        Dim segment = segments.SingleOrDefault(Function(s) s.Equals(String.Format("?{0}=", value)) OrElse s.Equals(String.Format("&{0}=", value)))
        If segment IsNot Nothing Then
            Dim index = segments.IndexOf(segment)
            path = path.Replace(String.Format("{{{0}}}", value), String.Concat(segments(index + 1)))
            segments.RemoveRange(index, 2)

            ' Replace missing ? if the segment was first in the series (after format)
            If index = 1 AndAlso segments.Count > 1 Then
                Dim first = segments(index).ToString()
                If first.StartsWith("&") Then
                    segments(index) = String.Concat("?", first.Substring(1))
                End If
            End If
        Else
            path = path.Replace(String.Format("/{{{0}}}", value), "")
        End If
        Return path
    End Function

    Private Shared ReadOnly _escapeSegments As New Regex("\A(?:[?|&]\w*=)\Z", RegexOptions.Compiled)

    Public Shared Sub EscapeDataContainingUrlSegments(ByVal segments As IList(Of Object))
        Dim names = segments.Where(Function(s) _escapeSegments.IsMatch(s.ToString()))
        Dim indexes = names.[Select](Function(n) segments.IndexOf(n) + 1).ToList()
        For i As Integer = 0 To indexes.Count() - 1
            Dim value = segments(indexes(i)).ToString()
            segments(indexes(i)) = Uri.EscapeDataString(value)
        Next
    End Sub
End Class

Friend Class StringHelpers
    Private Const UnderscoresPattern As String = "(((?<=[a-z])[A-Z])|([A-Z](?![A-Z]|$)))"

    Public Shared Function Capitalize(ByVal upperCase As String) As String
        Dim lower = upperCase.ToLowerInvariant()
        Return Char.ToUpperInvariant(lower(0)) & lower.Substring(1)
    End Function

    Public Shared Function CamelCase(ByVal pascalCase As String) As String
        Return Char.ToLowerInvariant(pascalCase(0)) & pascalCase.Substring(1)
    End Function

    Public Shared Function PascalCase(ByVal camelCase As String) As String
        Return Char.ToUpperInvariant(camelCase(0)) & camelCase.Substring(1)
    End Function

    Public Shared Function Underscore(ByVal camelCase As String) As String
#If Not SILVERLIGHT Then
#End If
        Dim underscored = Regex.Replace(camelCase, UnderscoresPattern, New MatchEvaluator(Function(m) String.Concat("_", m.Value.ToLowerInvariant())), RegexOptions.Compiled)

        Return underscored
    End Function
End Class

