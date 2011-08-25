Public Class OAuthAccessToken
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
    Public Overridable Property UserId() As Integer
        Get
            Return m_UserId
        End Get
        Set(ByVal value As Integer)
            m_UserId = Value
        End Set
    End Property
    Private m_UserId As Integer
    Public Overridable Property ScreenName() As String
        Get
            Return m_ScreenName
        End Get
        Set(ByVal value As String)
            m_ScreenName = Value
        End Set
    End Property
    Private m_ScreenName As String
End Class
