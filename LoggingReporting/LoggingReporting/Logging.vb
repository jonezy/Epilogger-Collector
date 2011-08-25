Public Class Logging

    Enum AlertType
        LogInformation = 0
        LogWarning = 1
        LogError = 2
    End Enum

    Public Sub New()
    End Sub

    Public Sub writeToLog(ByVal ProjectName As String, ByVal Message As String, ByVal AlertType As AlertType)
        Try

            '
            'Write the Event
            If Not EventLog.SourceExists(ProjectName) Then
                EventLog.CreateEventSource(ProjectName, "Application")
            End If

            Select Case AlertType
                Case 0 'Information
                    Diagnostics.EventLog.WriteEntry(ProjectName, Message, EventLogEntryType.Information)
                Case 1 'Warning
                    Diagnostics.EventLog.WriteEntry(ProjectName, Message, EventLogEntryType.Warning)
                Case 2 'Error
                    Diagnostics.EventLog.WriteEntry(ProjectName, Message, EventLogEntryType.Error)

                    Dim MyMail As New SpamSafeMail

                    MyMail.HTMLEmail = "When: " & Now.ToLongDateString & "<br />" & _
                                    "Error: " & Message

                    MyMail.TextEmail = "When: " & Now.ToLongDateString & Environment.NewLine & _
                                    "Error: " & Message

                    MyMail.ToEmailAddresses.Add("chrisb@dsm-corp.com")
                    MyMail.EmailSubject = "Production Server Error: " & ProjectName

                    MyMail.SendMail()

                    MyMail = Nothing

                    'Dim EmailMessage As String = ""
                    'EmailMessage = "When: " & Now.ToLongDateString & "<br />" & _
                    '                "Error: " & Message


                    'Dim N5REmail As New N5REmailDeployer.N5REmailDeployer
                    'N5REmail.SendEmail(My.Settings.ConnectionString, _
                    '                    My.Settings.EmailAddress, _
                    '                    My.Settings.EmailName, _
                    '                    "Error@ProductionServer.com", _
                    '                    "Error@ProductionServer.com", _
                    '                    "Production Server Error: " & ProjectName, _
                    '                    True, _
                    '                    EmailMessage)
                    'N5REmail = Nothing

            End Select

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
