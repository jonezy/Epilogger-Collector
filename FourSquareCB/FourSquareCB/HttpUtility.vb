#Region "Assembly Hammock.ClientProfile.dll, v4.0.30319"
' Z:\Projects\Epilogger\Epilogger Service\Trunk\CommonDLLs\Hammock.ClientProfile.dll
#End Region

Imports System.Collections.Specialized
Imports System.IO
Imports System.Text

Public NotInheritable Class HttpUtility
    Public Sub New()
    End Sub

    Public Shared Function HtmlAttributeEncode(ByVal s As String) As String
    End Function
    Public Shared Sub HtmlAttributeEncode(ByVal s As String, ByVal output As TextWriter)
    End Sub
    Public Shared Function HtmlDecode(ByVal s As String) As String
    End Function
    Public Shared Sub HtmlDecode(ByVal s As String, ByVal output As TextWriter)
    End Sub
    Public Shared Function HtmlEncode(ByVal s As String) As String
    End Function
    Public Shared Sub HtmlEncode(ByVal s As String, ByVal output As TextWriter)
    End Sub
    Public Shared Function ParseQueryString(ByVal query As String) As NameValueCollection
    End Function
    Public Shared Function ParseQueryString(ByVal query As String, ByVal encoding As Encoding) As NameValueCollection
    End Function
    Public Shared Function UrlDecode(ByVal str As String) As String
    End Function
    Public Shared Function UrlDecode(ByVal bytes As Byte(), ByVal e As Encoding) As String
    End Function
    Public Shared Function UrlDecode(ByVal s As String, ByVal e As Encoding) As String
    End Function
    Public Shared Function UrlDecode(ByVal bytes As Byte(), ByVal offset As Integer, ByVal count As Integer, ByVal e As Encoding) As String
    End Function
    Public Shared Function UrlDecodeToBytes(ByVal bytes As Byte()) As Byte()
    End Function
    Public Shared Function UrlDecodeToBytes(ByVal str As String) As Byte()
    End Function
    Public Shared Function UrlDecodeToBytes(ByVal str As String, ByVal e As Encoding) As Byte()
    End Function
    Public Shared Function UrlDecodeToBytes(ByVal bytes As Byte(), ByVal offset As Integer, ByVal count As Integer) As Byte()
    End Function
    Public Shared Function UrlEncode(ByVal bytes As Byte()) As String
    End Function
    Public Shared Function UrlEncode(ByVal str As String) As String
    End Function
    Public Shared Function UrlEncode(ByVal s As String, ByVal Enc As Encoding) As String
    End Function
    Public Shared Function UrlEncode(ByVal bytes As Byte(), ByVal offset As Integer, ByVal count As Integer) As String
    End Function
    Public Shared Function UrlEncodeToBytes(ByVal bytes As Byte()) As Byte()
    End Function
    Public Shared Function UrlEncodeToBytes(ByVal str As String) As Byte()
    End Function
    Public Shared Function UrlEncodeToBytes(ByVal str As String, ByVal e As Encoding) As Byte()
    End Function
    Public Shared Function UrlEncodeToBytes(ByVal bytes As Byte(), ByVal offset As Integer, ByVal count As Integer) As Byte()
    End Function
    Public Shared Function UrlEncodeUnicode(ByVal str As String) As String
    End Function
    Public Shared Function UrlEncodeUnicodeToBytes(ByVal str As String) As Byte()
    End Function
    Public Shared Function UrlPathEncode(ByVal s As String) As String
    End Function
End Class
