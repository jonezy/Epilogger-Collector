Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports Hammock
Imports Hammock.Authentication.OAuth
Imports Hammock.Web

Partial Public Class FourSquareCB

    <Serializable()> _
    Private Class FunctionArguments
        Public Property ConsumerKey() As String
            Get
                Return m_ConsumerKey
            End Get
            Set(ByVal value As String)
                m_ConsumerKey = value
            End Set
        End Property
        Private m_ConsumerKey As String
        Public Property ConsumerSecret() As String
            Get
                Return m_ConsumerSecret
            End Get
            Set(ByVal value As String)
                m_ConsumerSecret = value
            End Set
        End Property
        Private m_ConsumerSecret As String
        Public Property Token() As String
            Get
                Return m_Token
            End Get
            Set(ByVal value As String)
                m_Token = value
            End Set
        End Property
        Private m_Token As String
        Public Property TokenSecret() As String
            Get
                Return m_TokenSecret
            End Get
            Set(ByVal value As String)
                m_TokenSecret = value
            End Set
        End Property
        Private m_TokenSecret As String
        Public Property Verifier() As String
            Get
                Return m_Verifier
            End Get
            Set(ByVal value As String)
                m_Verifier = value
            End Set
        End Property
        Private m_Verifier As String
        Public Property Username() As String
            Get
                Return m_Username
            End Get
            Set(ByVal value As String)
                m_Username = value
            End Set
        End Property
        Private m_Username As String
        Public Property Password() As String
            Get
                Return m_Password
            End Get
            Set(ByVal value As String)
                m_Password = value
            End Set
        End Property
        Private m_Password As String
    End Class

    Private ReadOnly _requestTokenQuery As Func(Of FunctionArguments, RestRequest) = Function(args)
                                                                                         Dim request = New RestRequest() With { _
                                                                                              .Credentials = New OAuthCredentials() With { _
                                                                                           .ConsumerKey = args.ConsumerKey, _
                                                                                           .ConsumerSecret = args.ConsumerSecret, _
                                                                                           .ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader, _
                                                                                           .SignatureMethod = OAuthSignatureMethod.HmacSha1, _
                                                                                           .Type = OAuthType.RequestToken _
                                                                                          }, _
                                                                                          .Method = WebMethod.[Get], _
                                                                                          .Path = "/oauth/request_token" _
                                                                                         }
                                                                                         Return request

                                                                                     End Function


    Private ReadOnly _accessTokenQuery As Func(Of FunctionArguments, RestRequest) = Function(args)
                                                                                        Dim request = New RestRequest() With { _
                                                                                             .Credentials = New OAuthCredentials() With { _
                                                                                          .ConsumerKey = args.ConsumerKey, _
                                                                                             .ConsumerSecret = args.ConsumerSecret, _
                                                                                          .Token = args.Token, _
                                                                                          .TokenSecret = args.TokenSecret, _
                                                                                          .Verifier = args.Verifier, _
                                                                                          .ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader, _
                                                                                          .SignatureMethod = OAuthSignatureMethod.HmacSha1, _
                                                                                          .Type = OAuthType.AccessToken _
                                                                                         }, _
                                                                                         .Method = WebMethod.Post, _
                                                                                         .Path = "/oauth/access_token" _
                                                                                        }
                                                                                        Return request

                                                                                    End Function

    Private ReadOnly _protectedResourceQuery As Func(Of FunctionArguments, RestRequest) = Function(args)
                                                                                              Dim request = New RestRequest() With { _
                                                                                                .Credentials = New OAuthCredentials() With { _
                                                                                                 .Type = OAuthType.ProtectedResource, _
                                                                                                 .SignatureMethod = OAuthSignatureMethod.HmacSha1, _
                                                                                                 .ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader, _
                                                                                                 .ConsumerKey = args.ConsumerKey, _
                                                                                                 .ConsumerSecret = args.ConsumerSecret, _
                                                                                                 .Token = args.Token, _
                                                                                                 .TokenSecret = args.TokenSecret _
                                                                                               } _
                                                                                              }
                                                                                              Return request

                                                                                          End Function

    Private ReadOnly _xAuthQuery As Func(Of FunctionArguments, RestRequest) = Function(args)
                                                                                  Dim request = New RestRequest() With { _
                                                                                    .Credentials = New OAuthCredentials() With { _
                                                                                     .Type = OAuthType.ClientAuthentication, _
                                                                                     .SignatureMethod = OAuthSignatureMethod.HmacSha1, _
                                                                                     .ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader, _
                                                                                     .ConsumerKey = args.ConsumerKey, _
                                                                                     .ConsumerSecret = args.ConsumerSecret, _
                                                                                     .ClientUsername = args.Username, _
                                                                                     .ClientPassword = args.Password _
                                                                                   }, _
                                                                                    .Method = WebMethod.Post, _
                                                                                    .Path = "/oauth/access_token" _
                                                                                  }
                                                                                  Return request

                                                                              End Function

    Private ReadOnly _oauth As RestClient

    Public Overridable Sub AuthenticateWith(ByVal token As String, ByVal tokenSecret As String)
        _token = token
        _tokenSecret = tokenSecret
    End Sub

    Public Overridable Sub AuthenticateWith(ByVal consumerKey As String, ByVal consumerSecret As String, ByVal token As String, ByVal tokenSecret As String)
        _consumerKey = consumerKey
        _consumerSecret = consumerSecret
        _token = token
        _tokenSecret = tokenSecret
    End Sub

    Public Overridable Function GetAuthorizationUri(ByVal oauth As OAuthRequestToken) As Uri
        Return New Uri("https://api.twitter.com/oauth/authorize?oauth_token=" & Convert.ToString(oauth.Token))
    End Function

    Public Overridable Function GetAuthorizationUri(ByVal oauth As OAuthRequestToken, ByVal callback As String) As Uri
        Return New Uri("https://api.twitter.com/oauth/authorize?oauth_token=" & Convert.ToString(oauth.Token) & "&oauth_callback=" & callback)
    End Function

    Public Overridable Function GetAuthenticationUrl(ByVal oauth As OAuthRequestToken) As Uri
        Return New Uri("https://api.twitter.com/oauth/authenticate?oauth_token=" & Convert.ToString(oauth.Token))
    End Function

    Public Overridable Function GetAuthenticationUrl(ByVal oauth As OAuthRequestToken, ByVal callback As String) As Uri
        Return New Uri("https://api.twitter.com/oauth/authenticate?oauth_token=" & Convert.ToString(oauth.Token) & "&oauth_callback=" & callback)
    End Function

#If Not SILVERLIGHT Then
    Public Overridable Function GetRequestToken(ByVal callback As String) As OAuthRequestToken
        Dim args = New FunctionArguments() With { _
          .ConsumerKey = _consumerKey, _
          .ConsumerSecret = _consumerSecret _
        }

        Dim request = _requestTokenQuery.Invoke(args)
        If Not callback Is Nothing Then
            request.AddParameter("oauth_callback", callback)
        End If

        Dim response = _oauth.Request(request)

        SetResponse(response)

        Dim query = HttpUtility.ParseQueryString(response.Content)
        Dim oauth = New OAuthRequestToken() With { _
          .Token = If(query("oauth_token"), "?"), _
          .TokenSecret = If(query("oauth_token_secret"), "?"), _
          .OAuthCallbackConfirmed = Convert.ToBoolean(If(query("oauth_callback_confirmed"), "false")) _
        }

        Return oauth
    End Function

    Public Overridable Function GetRequestToken() As OAuthRequestToken
        Return GetRequestToken(Nothing)
    End Function

    Public Overridable Function GetAccessTokenWithXAuth(ByVal username As String, ByVal password As String) As OAuthAccessToken
        Dim args = New FunctionArguments() With { _
          .ConsumerKey = _consumerKey, _
          .ConsumerSecret = _consumerSecret, _
          .Username = username, _
          .Password = password _
        }

        Dim request = _xAuthQuery.Invoke(args)
        Dim response = _oauth.Request(request)

        SetResponse(response)

        Dim query = HttpUtility.ParseQueryString(response.Content)
        Dim accessToken = New OAuthAccessToken() With { _
          .Token = If(query("oauth_token"), "?"), _
          .TokenSecret = If(query("oauth_token_secret"), "?"), _
          .UserId = Convert.ToInt32(If(query("user_id"), "0")), _
          .ScreenName = If(query("screen_name"), "?") _
        }

        Return accessToken
    End Function

    Public Overridable Function GetAccessToken(ByVal requestToken As OAuthRequestToken) As OAuthAccessToken
        Return GetAccessToken(requestToken, Nothing)
    End Function

    Public Overridable Function GetAccessToken(ByVal requestToken As OAuthRequestToken, ByVal verifier As String) As OAuthAccessToken
        Dim args = New FunctionArguments() With { _
          .ConsumerKey = _consumerKey, _
          .ConsumerSecret = _consumerSecret, _
          .Token = requestToken.Token, _
          .TokenSecret = requestToken.TokenSecret, _
          .Verifier = verifier _
        }

        Dim request = _accessTokenQuery.Invoke(args)
        Dim response = _oauth.Request(request)

        SetResponse(response)

        Dim query = HttpUtility.ParseQueryString(response.Content)
        Dim accessToken = New OAuthAccessToken() With { _
          .Token = If(query("oauth_token"), "?"), _
          .TokenSecret = If(query("oauth_token_secret"), "?"), _
          .UserId = Convert.ToInt32(If(query("user_id"), "0")), _
          .ScreenName = If(query("screen_name"), "?") _
        }

        Return accessToken
    End Function

    Public Overridable Function GetEchoRequest(ByVal url As String) As String
        Dim client = New RestClient() With { _
          .Authority = "" _
        }
        Dim request = PrepareEchoRequest()
        request.Path = url
        Dim response = client.Request(request)
        Return If(response.Content, "")
    End Function
#End If

    Public Overridable Function PrepareEchoRequest() As RestRequest
        Dim args = New FunctionArguments() With { _
          .ConsumerKey = _consumerKey, _
          .ConsumerSecret = _consumerSecret, _
          .Token = _token, _
          .TokenSecret = _tokenSecret _
        }

        Dim request = _protectedResourceQuery.Invoke(args)
        request.Method = WebMethod.[Get]
        request.Path = String.Concat("account/verify_credentials", FormatAsString)

        Dim credentials = DirectCast(request.Credentials, OAuthCredentials)
        Dim url = request.BuildEndpoint(_client).ToString()
        Dim workflow = New OAuthWorkflow(credentials)

        Dim info = workflow.BuildProtectedResourceInfo(request.Method.Value, request.GetAllHeaders(), url)
        Dim query = credentials.GetQueryFor(url, request, info, request.Method.Value)
        DirectCast(query, OAuthWebQuery).Realm = "http://api.twitter.com"
        Dim auth = query.GetAuthorizationContent()

        Dim echo = New RestRequest()
        echo.AddHeader("X-Auth-Service-Provider", url)
        echo.AddHeader("X-Verify-Credentials-Authorization", auth)
        Return echo
    End Function
End Class
