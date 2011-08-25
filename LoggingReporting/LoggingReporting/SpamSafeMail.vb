Imports Microsoft.VisualBasic
Imports System.Net.Mail

Public Class SpamSafeMail

    Public ToEmailAddresses As New MailAddressCollection
    Public BCCEmailAddresses As New MailAddressCollection
    Private _HTMLEmail As String
    Private _TextEmail As String
    Private _EmailSubject As String

    Public Property HTMLEmail() As String
        Get
            HTMLEmail = _HTMLEmail
        End Get
        Set(ByVal value As String)
            _HTMLEmail = value
        End Set
    End Property
    Public Property TextEmail() As String
        Get
            TextEmail = _TextEmail
        End Get
        Set(ByVal value As String)
            _TextEmail = value
        End Set
    End Property
    Public Property EmailSubject() As String
        Get
            EmailSubject = _EmailSubject
        End Get
        Set(ByVal value As String)
            _EmailSubject = value
        End Set
    End Property

    Public Sub SendMail()
        '*************************************************
        'Chris Brooker - Anti-Spam Email Code Aug 19, 2009
        '*************************************************
        Try

            Dim objEMail As New MailMessage

            '
            'From
            objEMail.Headers.Add("Message-ID", "<Error@dsm-corp.com>")
            objEMail.From = New MailAddress("Error@dsm-corp.com")
            objEMail.Sender = New MailAddress("Error@dsm-corp.com")

            '
            'To
            For Each MyMailAddress As MailAddress In ToEmailAddresses
                objEMail.To.Add(MyMailAddress)
            Next

            '
            'BCC
            For Each MyMailAddress As MailAddress In BCCEmailAddresses
                objEMail.Bcc.Add(MyMailAddress)
            Next

            '
            'Properly Formatted Subject
            objEMail.SubjectEncoding = System.Text.Encoding.UTF8
            objEMail.Subject = _EmailSubject

            '
            'Define HTML Message
            Dim htmlView As System.Net.Mail.AlternateView = _
            System.Net.Mail.AlternateView.CreateAlternateViewFromString( _
            _HTMLEmail)

            htmlView.ContentType = New System.Net.Mime.ContentType("text/html")
            htmlView.TransferEncoding = Net.Mime.TransferEncoding.SevenBit

            '
            'Define Txt Message
            Dim textView As System.Net.Mail.AlternateView = _
            System.Net.Mail.AlternateView.CreateAlternateViewFromString( _
            _TextEmail)
            textView.ContentType = New System.Net.Mime.ContentType("text/plain")
            textView.TransferEncoding = Net.Mime.TransferEncoding.SevenBit

            '
            'Define the Main Message Types
            objEMail.IsBodyHtml = True
            objEMail.BodyEncoding = System.Text.Encoding.UTF8

            objEMail.AlternateViews.Add(htmlView)
            objEMail.AlternateViews.Add(textView)

            '
            'Define the SMTP Server Information
            Dim SMTPClient As New SmtpClient("linux.dsmhosting.net")
            SMTPClient.DeliveryMethod = SmtpDeliveryMethod.Network
            SMTPClient.UseDefaultCredentials = False

            '
            'Set the SMPT Crudentials
            Dim NCrudentials As New Net.NetworkCredential
            NCrudentials.UserName = "daniel@dsmhosting.net"
            NCrudentials.Password = "password"
            SMTPClient.Credentials = NCrudentials

            '
            'Finally, Send the Email
            SMTPClient.Send(objEMail)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

End Class
