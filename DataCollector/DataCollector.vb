Imports System.Data.SqlClient
Imports System.Threading
Imports System.Text
Imports System.Linq

Public Class DataCollector

    Public Enum CollectionModes
        BeforeEvent = 1
        ActiveEvent = 2
        AfterEventWiki = 3
        AfterEventArchived = 4
    End Enum

    Dim _MyConnection As Data.SqlClient.SqlConnection

    Private _MyLogging As LoggingReporting.Logging
    Private _count As Integer

    Public Sub New()
        _MyLogging = New LoggingReporting.Logging
    End Sub

    Public Sub UpdateCollectionModes()

        '
        'This pops off every minute to make sure all the events are in the right mode. Other timers are fired for each mode.

        '
        'Verify all the Active Events are in the Right mode. This will ensure they get polled at the right times.
        Dim db As New TweetsDataContext
        Dim Events = From e In db.Events
                     Where e.IsActive = True And e.ID = 186

        For Each e As [Event] In Events

            Dim ShouldBeInMode As CollectionModes
            Dim CompareToStart As Integer = Date.Compare(DateTime.UtcNow, e.StartDateTime)
            Dim CompareToEnd As Integer = Date.Compare(DateTime.UtcNow, e.EndDateTime)

            If CompareToStart < 0 Then
                ShouldBeInMode = CollectionModes.BeforeEvent
            ElseIf CompareToStart > 0 And CompareToEnd < 0 Then
                ShouldBeInMode = CollectionModes.ActiveEvent

            ElseIf CompareToEnd > 0 Then
                'If DateDiff(DateInterval.Day, CDate(e.StartDateTime), Now()) > 14 Then
                '    ShouldBeInMode = CollectionModes.AfterEventArchived
                'Else
                '    ShouldBeInMode = CollectionModes.AfterEventWiki
                'End If

                If DateDiff(DateInterval.Hour, CDate(e.CollectionEndDateTime), DateTime.UtcNow) <= 0 Then
                    ShouldBeInMode = CollectionModes.AfterEventWiki
                Else
                    ShouldBeInMode = CollectionModes.AfterEventArchived
                End If

            End If

            If e.CollectionMode <> ShouldBeInMode Then
                e.CollectionMode = ShouldBeInMode
            End If
        Next
        db.SubmitChanges()

    End Sub

    '
    'Kinda old
    'Public Sub CollectData()

    '    '
    '    'We need to loop through all the Events in Mode 1 and 2 - Active collection Mode.
    '    _MyConnection = New Data.SqlClient.SqlConnection(My.Settings.ConnectionString)
    '    _MyConnection.Open()

    '    ' Create command
    '    Dim myquery As String = "Select * from Events Where IsActive=1"
    '    Dim mycommand As New Data.SqlClient.SqlCommand(myquery, _MyConnection)

    '    mycommand.CommandTimeout = 500
    '    mycommand.CommandType = Data.CommandType.Text

    '    Dim myReader As Data.SqlClient.SqlDataReader
    '    myReader = mycommand.ExecuteReader()

    '    'Iterate through the results
    '    While myReader.Read()
    '        '
    '        'Current implementation only looks at Twitter. Load the Twitter Library and get the tweets.
    '        'Dim TH As New TwitterHelper
    '        'TH.GetNewTweets(myReader("ID"), myReader("SearchTerms"), myReader("CollectionMode"))
    '    End While

    'End Sub

    Public Sub CollectData(ByVal CollectionMode As CollectionModes)

        '
        'Get all the Active Events in the Mode Passed in
        Dim db As New TweetsDataContext
        Dim Events = From e In db.Events
                     Where e.IsActive = True And e.CollectionMode = CollectionMode

        For Each e As [Event] In Events
            '
            'Fire off each Event in a thread

            '
            'Tweets
            Dim TH As New TwitterHelper
            TH.EventID = e.ID
            TH.SearchTerms = e.SearchTerms
            TH.CollectionMode = CollectionMode

            Dim myThread As New Threading.Thread(AddressOf TH.GetNewTweets)
            myThread.Start()

            '
            'Foursquare
            'Dim FourSQ As New FourSquareHelper
            'FourSQ.EventID = e.ID
            'FourSQ.VenueID = e.VenueID

            'Dim FourSquareThread As New Threading.Thread(AddressOf FourSQ.GetFourSquareCheckins)
            'myThread.Start()

        Next

    End Sub

End Class