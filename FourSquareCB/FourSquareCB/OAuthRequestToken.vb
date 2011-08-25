Public Class OAuthRequestToken
    Public Overridable Property Token() As String
        Get
            Return m_Token
        End Get
        Set(ByVal value As String)
            m_Token = Value
        End Set
    End Property
    Private m_Token As String
    Public Overridable Property TokenSecret() As String
        Get
            Return m_TokenSecret
        End Get
        Set(ByVal value As String)
            m_TokenSecret = Value
        End Set
    End Property
    Private m_TokenSecret As String
    Public Overridable Property OAuthCallbackConfirmed() As Boolean
        Get
            Return m_OAuthCallbackConfirmed
        End Get
        Set(ByVal value As Boolean)
            m_OAuthCallbackConfirmed = Value
        End Set
    End Property
    Private m_OAuthCallbackConfirmed As Boolean
End Class
