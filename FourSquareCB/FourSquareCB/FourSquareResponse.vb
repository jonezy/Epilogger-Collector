Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Net
Imports Hammock
Imports System.Collections.Specialized


<Serializable()> _
Public Class FourSquareResponse
    Private ReadOnly _response As RestResponseBase

    Friend Sub New(ByVal response As RestResponseBase)
        _response = response
    End Sub

    'Public Overridable ReadOnly Property RateLimitStatus() As TwitterRateLimitStatus
    '    Get
    '        Dim limit = Headers("X-RateLimit-Limit")
    '        Dim remaining = Headers("X-RateLimit-Remaining")
    '        Dim reset = Headers("X-RateLimit-Reset")

    '        limit = If(IsStringANumber(If(Not String.IsNullOrEmpty(limit), limit.Trim(), "-1")), limit, "-1")
    '        remaining = If(IsStringANumber(If(Not String.IsNullOrEmpty(remaining), remaining.Trim(), "-1")), remaining, "-1")
    '        reset = If(IsStringANumber(If(Not String.IsNullOrEmpty(reset), reset.Trim(), "-1")), reset, "0")

    'Return If(Not (New () {limit, remaining, reset}).AreNullOrBlank(), New TwitterRateLimitStatus() With { _
    '	Key .HourlyLimit = Convert.ToInt32(limit, CultureInfo.InvariantCulture), _
    '	Key .RemainingHits = Convert.ToInt32(remaining, CultureInfo.InvariantCulture), _
    '	Key .ResetTimeInSeconds = Convert.ToInt64(reset, CultureInfo.InvariantCulture), _
    '	Key .ResetTime = Convert.ToInt64(reset, CultureInfo.InvariantCulture).FromUnixTime() _
    '}, Nothing)
    '    End Get
    'End Property

    Private Shared Function IsStringANumber(ByVal limit As IEnumerable(Of Char)) As Boolean
        Return limit.All(Function(c) Char.IsNumber(c))
    End Function

    Public Overridable ReadOnly Property Headers() As NameValueCollection
        Get
            Return _response.Headers
        End Get
    End Property

    Public Overridable Property StatusCode() As HttpStatusCode
        Get
            Return _response.StatusCode
        End Get
        Set(ByVal value As HttpStatusCode)
            _response.StatusCode = value
        End Set
    End Property

    'Public Overridable Property SkippedDueToRateLimitingRule() As Boolean
    '    Get
    '        Return _response.SkippedDueToRateLimitingRule
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _response.SkippedDueToRateLimitingRule = value
    '    End Set
    'End Property

    Public Overridable Property StatusDescription() As String
        Get
            Return _response.StatusDescription
        End Get
        Set(ByVal value As String)
            _response.StatusDescription = value
        End Set
    End Property

    Public Overridable ReadOnly Property Response() As String
        Get
            Return _response.Content
        End Get
    End Property

    Public Overridable Property RequestMethod() As String
        Get
            Return _response.RequestMethod
        End Get
        Set(ByVal value As String)
            _response.RequestMethod = value
        End Set
    End Property

    Public Overridable Property RequestUri() As Uri
        Get
            Return _response.RequestUri
        End Get
        Set(ByVal value As Uri)
            _response.RequestUri = value
        End Set
    End Property

    Public Overridable Property ResponseDate() As System.Nullable(Of DateTime)
        Get
            Return _response.ResponseDate
        End Get
        Set(ByVal value As System.Nullable(Of DateTime))
            _response.ResponseDate = value
        End Set
    End Property

    Public Overridable Property RequestDate() As System.Nullable(Of DateTime)
        Get
            Return _response.RequestDate
        End Get
        Set(ByVal value As System.Nullable(Of DateTime))
            _response.RequestDate = value
        End Set
    End Property

    Public Overridable Property InnerException() As WebException
        Get
            Return _response.InnerException
        End Get
        Set(ByVal value As WebException)
            _response.InnerException = value
        End Set
    End Property
End Class

