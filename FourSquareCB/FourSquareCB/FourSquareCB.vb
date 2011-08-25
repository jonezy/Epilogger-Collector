Imports System
Imports System.Collections.Generic
Imports Hammock
'Imports Hammock.Web
Imports System.Text
Imports System.Globalization
Imports FourSquareCB.FourSquareCB

Public Class FourSquareCB

    Private _consumerKey As String = "GWTLBG1OYTHMGFJDF31SBDVJY5GTWANC2VNMNX1DU1TZ0OAK"
    Private _consumerSecret As String = "PQPC10UR1L2JJGUM5OCQ4EYXJ41RDLBNQPXAAKKREGQNZLBI"
    Private _token As String
    Private _tokenSecret As String
    Private ReadOnly _client As RestClient



    'Private _response As RestResponseBase

    'Friend Sub TwitterResponse(ByVal response As RestResponseBase)
    '    _response = response
    'End Sub

    Public Property Response() As FourSquareResponse
        Get
            Return _response
        End Get
        Private Set(ByVal value As FourSquareResponse)
            _response = value
        End Set
    End Property
    Private _Response As FourSquareResponse



    Public Overridable Function GetVenue(ByVal venueID As Long) As FSVenues

        ''
        ''Pure Hammock REST implementation
        'Dim client = New RestClient() With { _
        '                                      .Authority = "https://api.foursquare.com", _
        '                                      .VersionPath = "v2" _
        '                                    }

        'Dim request = New RestRequest() With { _
        '                                      .Path = "venues/" & venueID & "?client_id=" & _consumerKey & "&client_secret=" & _consumerSecret _
        '                                    }

        '' Callback when signalled
        'Dim callback = New RestCallback(Function(restRequest, restResponse, userState)



        '                                    Return True

        '                                End Function)

        'Dim asyncResult As IAsyncResult = client.BeginRequest(request, callback)
        'Dim response As RestResponse = client.EndRequest(asyncResult)







        '
        'Scrape Everything try to get soemthing direct from the Hammock Json class

        'Dim request = PrepareHammockQuery("https://api.foursquare.com/v2/venues/" & venueID)



        'This works.
        Dim request As New RestRequest
        request.Path = "https://api.foursquare.com/v2/venues/" & venueID & "?client_id=" & _consumerKey & "&client_secret=" & _consumerSecret

        'Return WithHammockImpl(Of FSVenue)(request)

















        '
        'This will return a Venue
        'https://api.foursquare.com/v2/venues/2367584?client_id=GWTLBG1OYTHMGFJDF31SBDVJY5GTWANC2VNMNX1DU1TZ0OAK&client_secret=PQPC10UR1L2JJGUM5OCQ4EYXJ41RDLBNQPXAAKKREGQNZLBI



        'Return WithHammock(Of FSVenueResults)("https://api.foursquare.com/v2/venues/" & venueID)



        'Return WithHammock(Of FSVenueResults)("venues", "")

        'Return WithHammock(Of FSVenueResults)("venues", FormatAsString, "")

        'Return WithHammock(Of TwitterSearchResult)("search", FormatAsString, "?since_id=", since_id, "&q=", q, "&page=", page, "&rpp=", rpp, "&lang=", lang)



        'Return Hammock(Of TwitterSearchResult)("search", FormatAsString, "?since_id=", since_id, "&q=", q, _
        ' "&page=", page, "&rpp=", rpp, "&lang=", lang)
    End Function


    '
    'This will prepare the OAuth token and we need it!
    'Private Function PrepareHammockQuery(ByVal path As String) As RestRequest
    '    Dim request As RestRequest
    '    If String.IsNullOrEmpty(_token) OrElse String.IsNullOrEmpty(_tokenSecret) Then
    '        request = _noAuthQuery.Invoke()
    '    Else
    '        Dim args = New FunctionArguments() With { _
    '         .ConsumerKey = _consumerKey, _
    '         .ConsumerSecret = _consumerSecret, _
    '         .Token = _token, _
    '         .TokenSecret = _tokenSecret _
    '        }
    '        request = _protectedResourceQuery.Invoke(args)
    '    End If
    '    request.Path = path

    '    SetTwitterClientInfo(request)

    '    Return request
    'End Function

    Private Function WithHammockImpl(Of T)(ByVal request As RestRequest) As T
        'Dim response = _client.Request(Of T)(request)
        Dim _client As New RestClient
        Dim response = _client.Request(Of T)(request)

        SetResponse(response)

        Dim entity = response.ContentEntity
        Return entity
    End Function

    Private Sub SetResponse(ByVal response__1 As RestResponseBase)
        Response = New FourSquareResponse(response__1)
    End Sub




























    Private Function WithHammock(Of T)(ByVal path As String, ByVal ParamArray segments As Object()) As T
        Return WithHammock(Of T)(ResolveUrlSegments(path, segments.ToList()))
    End Function

    Private Function ResolveUrlSegments(ByVal path As String, ByVal segments As List(Of Object)) As String
        If segments Is Nothing Then
            Throw New ArgumentNullException("segments")
        End If

        ' Support alternate client authorities here

        'If path.Equals("search") Then
        '    _client.Authority = Globals.SearchAPIAuthority
        '    _client.VersionPath = Nothing
        'Else
        '    _client.Authority = Globals.RestAPIAuthority
        '    _client.VersionPath = "1"
        'End If

        '
        'Figure this OUT!
        '_client.Authority = "https://api.foursquare.com/v2/"
        '_client.VersionPath = "2"

        For i As Integer = 0 To segments.Count - 1
            ' Currently only trends takes DateTimes
            If TypeOf segments(i) Is DateTime Then
                segments(i) = DirectCast(segments(i), DateTime).ToString("yyyy-MM-dd")
            End If

            If GetType(IEnumerable).IsAssignableFrom(segments(i).[GetType]()) AndAlso Not (segments(i).[GetType]() = GetType(String)) Then
                ResolveEnumerableUrlSegments(segments, i)
            End If
        Next

        path = PathHelpers.ReplaceUriTemplateTokens(segments, path)

        PathHelpers.EscapeDataContainingUrlSegments(segments)

        'If IncludeEntities Then
        '    ' && !path.Contains("/lists"))
        '    segments.Add(If(segments.Count() > 1, "&include_entities=", "?include_entities="))
        '    segments.Add("1")
        'End If

        segments.Insert(0, path)

        Return String.Concat(segments.ToArray()).ToString(CultureInfo.InvariantCulture)
    End Function

    Private Shared Sub ResolveEnumerableUrlSegments(ByVal segments As IList(Of Object), ByVal i As Integer)
        Dim collection = DirectCast(segments(i), IEnumerable(Of Object))
        Dim total = collection.Count()
        Dim sb = New StringBuilder()
        Dim count = 0
        For Each item In collection
            sb.Append(item.ToString())
            If count < total - 1 Then
                sb.Append(",")
            End If
            count += 1
        Next
        segments(i) = sb.ToString()
    End Sub

    Friend Property FormatAsString() As String
        Get
            Return m_FormatAsString
        End Get
        Private Set(ByVal value As String)
            m_FormatAsString = Value
        End Set
    End Property
    Private m_FormatAsString As String


End Class
