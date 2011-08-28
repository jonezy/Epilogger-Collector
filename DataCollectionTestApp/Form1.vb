Imports System.IO
Imports System.Text
''Imports FoursquareNET
Imports Igloo.SharpSquare.Core
Imports Igloo.SharpSquare.Entities

Public Class Form1

    Public Count As String = 0
    Dim DataCollector As New DataCollector.DataCollector

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        BeforeEventTimer.Enabled = False
        ActiveEventTimer.Enabled = False
        AfterEventWiki.Enabled = False
        Timer1.Enabled = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim DataCollector As New DataCollector.DataCollector
        DataCollector.UpdateCollectionModes()

        '
        'This is my code to Manually fill an event it also back fills with a slight code adjustment to the TweetCollector code

        Dim TH As New DataCollector.TwitterHelper

        TH.EventID = 186
        TH.SearchTerms = "girl geek"
        TH.CollectionMode = 2

        TH.GetNewTweets()



        '
        'Test FourSquare

        '
        'Gets Chris Brooker's last 20 Checkins.
        'Dim credential = New FoursquareNET.Credential.Basic("cbrooker@gmail.com", "xea,79,19c")
        'Dim history = New FoursquareNET.Checkins.History()
        'Dim checkins = history.Execute(credential)

        '
        'Get Checkins for a Venue.
        'Get Venu but no check ins.
        'Dim credential = New FoursquareNET.Credential.Basic("cbrooker@gmail.com", "xea,79,19c")
        'Dim credential = New FoursquareNET.Credential.OAuth("cbrooker@gmail.com", "xea,79,19c", "GWTLBG1OYTHMGFJDF31SBDVJY5GTWANC2VNMNX1DU1TZ0OAK", "PQPC10UR1L2JJGUM5OCQ4EYXJ41RDLBNQPXAAKKREGQNZLBI")


        'Dim GVenue = New FoursquareNET.Venue.GetDetail()
        'GVenue.VenueId = 248577
        'Dim VenueDetails = GVenue.Executev2()

        'Dim Test As String = ""





        'Dim AccessCode As String = ""

        ''
        ''Try to get the venue fucking information

        'Dim clientId As String = "GWTLBG1OYTHMGFJDF31SBDVJY5GTWANC2VNMNX1DU1TZ0OAK"
        'Dim clientSecret As String = "PQPC10UR1L2JJGUM5OCQ4EYXJ41RDLBNQPXAAKKREGQNZLBI"
        'Dim redirectUri As String = "http://localhost:63072/FSTest/Default.aspx"
        'Dim sharpSquare As New SharpSquare(clientId, clientSecret)
        'Dim AccessToken As String = ""

        'If AccessCode <> "" Then
        '    AccessToken = sharpSquare.GetAccessToken(redirectUri, AccessCode)
        '    ' Here, you can do something
        'Else
        '    Try
        '        '
        '        'This will unshorten any URL
        '        Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(sharpSquare.GetAuthenticateUrl(redirectUri))
        '        Dim resp As System.Net.WebResponse = req.GetResponse()
        '        Dim receiveStream As Stream = resp.GetResponseStream()
        '        Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
        '        Dim ReturnValue As String = readStream.ReadToEnd()
        '        'If ReturnValue <> "ERROR:1" And ReturnValue <> "ERROR:2" Then
        '        '    Return New Uri(ReturnValue)
        '        'Else
        '        '    Return ShortURL
        '        'End If
        '        resp.Close()
        '        readStream.Close()
        '    Catch ex As Exception
        '        'Return ShortURL
        '    End Try



        '    sharpSquare.GetAuthenticateUrl(redirectUri)
        'End If

        ''AccessToken = sharpSquare.GetAccessToken("http://localhost:65464/GetFuckingFSToken/", "P00LDBZBGYHR2EAODKSXAJGGHQE0PLV1IHGTDNHAVWW2VSVV")

        'For Each check As Igloo.SharpSquare.Entities.Checkin In sharpSquare.GetVenueHereNow("18504481")

        'Next











        '
        'This really only works from a web site. It get's the initial token. We should have that now!
        '
        'Test SharpSquare, which is supposed to support v2
        'Dim clientId As String = "GWTLBG1OYTHMGFJDF31SBDVJY5GTWANC2VNMNX1DU1TZ0OAK"
        'Dim clientSecret As String = "PQPC10UR1L2JJGUM5OCQ4EYXJ41RDLBNQPXAAKKREGQNZLBI"
        'Dim redirectUri As String = "http://localhost:63072/FSTest/Default.aspx"
        'Dim sharpSquare As New SharpSquare(clientId, clientSecret)

        'If Request("code") IsNot Nothing Then
        '    sharpSquare.GetAccessToken(redirectUri, Request("code"))
        '    ' Here, you can do something
        'Else
        '    HyperLink.NavigateUrl = sharpSquare.GetAuthenticateUrl(redirectUri)
        'End If

        'Dim parameters As New Dictionary(Of String, String)()
        'parameters.Add("venueId", "18504481")
        'parameters.Add("broadcast", "public")
        'Dim checkin As Checkin = sharpSquare.AddCheckin(parameters)












        'Dim Test As New DataCollector.TwitterHelper
        'Test.TestStoringToAzure()



        'Dim FS As New DataCollector.FourSquareHelper
        ''FS.CleanUpVenueData()
        'FS.EventID = 35
        'FS.VenueID = 2367584
        'FS.GetFourSquareCheckins()


        'FS.VenueID = 2367584
        'FS.EventID = 2 'Doesn't matter for test.
        'FS.GetVenue(2367584)





        'Dim Test As New FourSquareCB.FourSquareCB
        'Dim Venue As FourSquareCB. = Test.GetVenue(2367584)


        'Dim hi As String = ""





        '
        'Quick test of URL unshortener API.
        'Dim TheURL As New Uri("http://t.co/lQGkw2a")
        'Dim Pic As Uri
        'Pic = test(TheURL)




        'Dim req As System.Net.WebRequest = System.Net.WebRequest.Create("http://api.unfwd4.me/?url=" & TheURL)
        'Dim resp As System.Net.WebResponse = req.GetResponse()
        'Dim receiveStream As Stream = resp.GetResponseStream()
        'Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
        'Console.WriteLine(readStream.ReadToEnd())
        'resp.Close()
        'readStream.Close()


        '
        'Test why this didn't resolve
        'Made it to real sports! #jacketofshame  http://t.co/lQGkw2a

        'Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(TheURL)
        'Dim resp As System.Net.WebResponse = req.GetResponse()
        'TheURL = resp.ResponseUri.ToString




        'Dim FourSQ As New DataCollector.FourSquareHelper

        'FourSQ.EventID = 32
        'FourSQ.VenueID = 2367584

        'FourSQ.GetFourSquareCheckins()
        '167318 - Gladstone

        'DataCollector.CollectData(DataCollector.CollectionModes.AfterEventWiki)

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        '
        'Pops every 1 minute
        Dim DataCollector As New DataCollector.DataCollector
        DataCollector.UpdateCollectionModes()
        Count += 1
        Label1.Text = Count

    End Sub

    Private Sub BeforeEventTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BeforeEventTimer.Tick
        '
        'Pops every 5 minutes
        DataCollector.CollectData(DataCollector.CollectionModes.BeforeEvent)
    End Sub

    Private Sub ActiveEventTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActiveEventTimer.Tick
        '
        'Pops every 15 seconds
        DataCollector.CollectData(DataCollector.CollectionModes.ActiveEvent)
    End Sub

    Private Sub AfterEventWiki_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AfterEventWiki.Tick
        '
        'Pops every 5 minutes
        DataCollector.CollectData(DataCollector.CollectionModes.AfterEventWiki)
    End Sub
End Class
