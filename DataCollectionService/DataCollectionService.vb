Imports System.Threading

Public Class DataCollectionService

    Private UpdateModesTimer As System.Timers.Timer
    Private BeforeEventTimer As System.Timers.Timer
    Private ActiveEventTimer As System.Timers.Timer
    Private AfterEventWiki As System.Timers.Timer

    Private MyLogging As LoggingReporting.Logging

    Protected Overrides Sub OnStart(ByVal args() As String)
        StartService()
    End Sub
    Protected Overrides Sub OnStop()
        StopTimer()
    End Sub

    Private Sub StartService()

        MyLogging = New LoggingReporting.Logging

        Try
            Dim autoEvent As New AutoResetEvent(False)

            '
            'Timer for Every 60 seconds that updates the Event Modes
            UpdateModesTimer = New System.Timers.Timer(60000)
            AddHandler UpdateModesTimer.Elapsed, AddressOf UpdateModesTimer_Tick
            UpdateModesTimer.AutoReset = True
            UpdateModesTimer.Enabled = True

            '
            'Every 5 mins - Before Event logging
            BeforeEventTimer = New System.Timers.Timer(300000)
            AddHandler BeforeEventTimer.Elapsed, AddressOf BeforeEventTimer_Tick
            BeforeEventTimer.AutoReset = True
            BeforeEventTimer.Enabled = True

            '
            'Every 60 seconds - During Event
            ActiveEventTimer = New System.Timers.Timer(60000)
            AddHandler ActiveEventTimer.Elapsed, AddressOf ActiveEventTimer_Tick
            ActiveEventTimer.AutoReset = True
            ActiveEventTimer.Enabled = True

            '
            'Every 5 mins - After Event
            AfterEventWiki = New System.Timers.Timer(300000)
            AddHandler AfterEventWiki.Elapsed, AddressOf AfterEventWiki_Tick
            AfterEventWiki.AutoReset = True
            AfterEventWiki.Enabled = True


        Catch ex As Exception
            MyLogging.writeToLog(My.Application.Info.ProductName, ex.Message.ToString & " - " & ex.InnerException.ToString, LoggingReporting.Logging.AlertType.LogError)
        End Try
    End Sub

    Private Sub UpdateModesTimer_Tick(ByVal source As Object, ByVal e As System.Timers.ElapsedEventArgs)

        UpdateModesTimer.Enabled = False

        Try
            Dim DataCollector As New DataCollector.DataCollector
            DataCollector.UpdateCollectionModes()
        Catch ex As Exception
            Dim MyLogging As New LoggingReporting.Logging
            MyLogging.writeToLog(My.Application.Info.ProductName, "DataCollectorService.vb/UpdateModesTimer_Tick - " & ex.Message.ToString, LoggingReporting.Logging.AlertType.LogError)
        End Try

        UpdateModesTimer.Enabled = True

    End Sub

    Private Sub BeforeEventTimer_Tick(ByVal source As Object, ByVal e As System.Timers.ElapsedEventArgs)

        BeforeEventTimer.Enabled = False

        Try
            Dim DataCollector As New DataCollector.DataCollector
            DataCollector.CollectData(DataCollector.CollectionModes.BeforeEvent)
        Catch ex As Exception
            Dim MyLogging As New LoggingReporting.Logging
            MyLogging.writeToLog(My.Application.Info.ProductName, "DataCollectorService.vb/BeforeEventTimer_Tick - " & ex.Message.ToString, LoggingReporting.Logging.AlertType.LogError)
        End Try

        BeforeEventTimer.Enabled = True

    End Sub

    Private Sub ActiveEventTimer_Tick(ByVal source As Object, ByVal e As System.Timers.ElapsedEventArgs)

        ActiveEventTimer.Enabled = False

        Try
            Dim DataCollector As New DataCollector.DataCollector
            DataCollector.CollectData(DataCollector.CollectionModes.ActiveEvent)
        Catch ex As Exception
            Dim MyLogging As New LoggingReporting.Logging
            MyLogging.writeToLog(My.Application.Info.ProductName, "DataCollectorService.vb/ActiveEventTimer - " & ex.Message.ToString, LoggingReporting.Logging.AlertType.LogError)
        End Try

        ActiveEventTimer.Enabled = True

    End Sub

    Private Sub AfterEventWiki_Tick(ByVal source As Object, ByVal e As System.Timers.ElapsedEventArgs)

        AfterEventWiki.Enabled = False

        Try
            Dim DataCollector As New DataCollector.DataCollector
            DataCollector.CollectData(DataCollector.CollectionModes.AfterEventWiki)
        Catch ex As Exception
            Dim MyLogging As New LoggingReporting.Logging
            MyLogging.writeToLog(My.Application.Info.ProductName, "DataCollectorService.vb/AfterEventWiki - " & ex.Message.ToString, LoggingReporting.Logging.AlertType.LogError)
        End Try

        AfterEventWiki.Enabled = True

    End Sub

    Private Sub StopTimer()
        UpdateModesTimer.Dispose()
        BeforeEventTimer.Dispose()
        ActiveEventTimer.Dispose()
        AfterEventWiki.Dispose()
    End Sub

End Class
