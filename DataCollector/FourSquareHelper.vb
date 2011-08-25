Imports Microsoft.VisualBasic
Imports System.Linq
Imports System.Net
Imports Igloo.SharpSquare.Core
Imports Igloo.SharpSquare.Entities
Imports Igloo.SharpSquare

'Imports FoursquareNET
'Imports FourSquareAPI
'Imports System.IO
'Imports System.Web.Script.Serialization

Public Class FourSquareHelper

    Private _MyLogging As LoggingReporting.Logging

    Private _ClienID As String = "GWTLBG1OYTHMGFJDF31SBDVJY5GTWANC2VNMNX1DU1TZ0OAK"
    Private _Secret As String = "PQPC10UR1L2JJGUM5OCQ4EYXJ41RDLBNQPXAAKKREGQNZLBI"

    '
    'Access Token is currently for Chris Brooker - Change to Epilogger Account.
    Private _AccessToken As String = "KXYKL2KU0NG2FPDP1YY0OAE0IDKHF2WKWBCQHUHN5MUXDSVQ"


    Public Property EventID As Integer
    Public Property VenueID As Integer
    Public Property CollectionMode As Integer

    '
    'This is a function a wrote that will clean up the Venue data in our DB. It will query the FS venue and populate our DB with the Venue data. Used cuz
    'ours is currently empty.
    Public Sub CleanUpVenueData()

        Dim db As New TweetsDataContext

        Dim TheVenues = From v In db.Venues
                          Select v

        For Each DBVenue As Venue In TheVenues
            Dim FSVenues As Entities.Venue = GetVenue(DBVenue.VenueID)
            DBVenue.Address = FSVenues.location.address
            DBVenue.City = FSVenues.location.city
            DBVenue.CrossStreet = FSVenues.location.crossStreet
            DBVenue.Geolat = FSVenues.location.lat
            DBVenue.Geolong = FSVenues.location.lng
            DBVenue.Name = FSVenues.name
            DBVenue.Phone = FSVenues.contact.phone
            DBVenue.State = FSVenues.location.state
            DBVenue.Zip = FSVenues.location.postalCode

            db.SubmitChanges()
        Next

    End Sub

    '
    'Get the Venue with the FourSquare Venue ID.
    Public Function GetVenue(ByVal VenueID As Integer) As Entities.Venue

        Dim sharpSquare As New SharpSquare(_ClienID, _Secret)

        sharpSquare.SetAccessToken(_AccessToken)

        Dim TheVenue As Entities.Venue = sharpSquare.GetVenue(VenueID)

        Return TheVenue

    End Function

    Public Sub GetFourSquareCheckins()

        If Me.EventID = 0 Then
            Throw New SystemException("This method requires an EventID")
        End If
        If Me.VenueID = 0 Then
            Throw New SystemException("This method requires a VenueID")
        End If

        Dim sharpSquare As New SharpSquare(_ClienID, _Secret)
        sharpSquare.SetAccessToken(_AccessToken)

        '
        'Get the Venue data from FourSquare, this will contain the checkins.
        Dim FSVenue As Entities.Venue = GetVenue(Me.VenueID)
        If FSVenue IsNot Nothing Then
            Dim hi As String = FSVenue.hereNow.count

            For Each Users As FourSquareEntityItems(Of User) In FSVenue.hereNow.groups
                For Each User As User In Users.items
                    '
                    'This gets the users checkin, but doesn't seem to give any information, except userID. Argh.

                    Dim User2 As Entities.User = sharpSquare.GetUser(User.id)

                    Dim test As String = User.lastName
                    Dim star As String = ""
                Next

            Next


            For Each g As Object In FSVenue.hereNow.groups
                Dim h2i As String = g.ToString
            Next

        End If





        '
        'This URL will get all the data I need
        'Using FourSquare V2 Venues system, currently in Beta. Should be pimp.
        'https://api.foursquare.com/v2/venues/search?ll=40.7,-74&client_id=GWTLBG1OYTHMGFJDF31SBDVJY5GTWANC2VNMNX1DU1TZ0OAK&client_secret=PQPC10UR1L2JJGUM5OCQ4EYXJ41RDLBNQPXAAKKREGQNZLBI

        'This will get info on bnotions
        'https://api.foursquare.com/v2/venues/2367584?client_id=GWTLBG1OYTHMGFJDF31SBDVJY5GTWANC2VNMNX1DU1TZ0OAK&client_secret=PQPC10UR1L2JJGUM5OCQ4EYXJ41RDLBNQPXAAKKREGQNZLBI



        '
        'WORKING
        'Dim TheURL As String = "https://api.foursquare.com/v2/venues/" & VenueID & "?client_id=" & ClienID & "&client_secret=" & Secret

        'Dim req As HttpWebRequest = DirectCast(System.Net.WebRequest.Create(TheURL), HttpWebRequest)

        'Dim response As HttpWebResponse = DirectCast(req.GetResponse(), HttpWebResponse)

        'Dim result As String = ""
        'Using sr As New StreamReader(response.GetResponseStream())
        '    result = sr.ReadToEnd()
        'End Using

        'Dim JS As New JavaScriptSerializer
        'Dim test = JS.Deserialize(Of Schema.Venue)(result)

        'Dim hi As String = ""








        'https://foursquare.com/oauth2/access_token
        '  ?client_id=YOUR_CLIENT_ID
        '  &client_secret=YOUR_CLIENT_SECRET
        '  &grant_type=authorization_code
        '  &redirect_uri=YOUR_REGISTERED_REDIRECT_URI
        '  &code=CODE

        'Dim AuthToken As String = ""
        'Dim AuthURL As String = "https://foursquare.com/oauth2/access_token" '?client_id=" & ClienID & "&client_secret=" & Secret & "&grant_type=authorization_code&redirect_uri=epilogger.com&code=CODE"
        'Dim req As HttpWebRequest = DirectCast(System.Net.WebRequest.Create(AuthURL), HttpWebRequest)

        'req.Headers.Add("client_id", ClienID)
        'req.Headers.Add("client_secret", Secret)
        ''req.Headers.Add("client_id", "cbrooker@gmail.com")
        ''req.Headers.Add("client_secret", "xea,79,19c")
        'req.Headers.Add("grant_type", "authorization_code")
        'req.Headers.Add("redirect_uri", "Epilogger.com")
        'req.Headers.Add("code", "CODE")

        'Dim response As HttpWebResponse = DirectCast(req.GetResponse(), HttpWebResponse)

        'Using sr As New StreamReader(response.GetResponseStream())
        '    AuthToken = sr.ReadToEnd()
        'End Using




        '
        'This MAY not work property. FourSquare qill only give you a list of people who checked in if you sign in with Crudentials.
        'If that user is a Friend it will display full details, if not. It will only show Firstname and Last Initial.

        '
        'This will get all the details for a Venue when the VenueID is passed in.

        '
        'This should return the Chackins for that Venue for the last 3 hours.
        'Dim credential = New FoursquareNET.Credential.OAuth("cbrooker@gmail.com", "xea,79,19c", "GWTLBG1OYTHMGFJDF31SBDVJY5GTWANC2VNMNX1DU1TZ0OAK", "PQPC10UR1L2JJGUM5OCQ4EYXJ41RDLBNQPXAAKKREGQNZLBI")


        'This was the CODE
        'Dim credential = New FoursquareNET.Credential.OAuth("cbrooker@gmail.com", "xea,79,19c")
        'Dim TheVenue As New FoursquareNET.Venue.GetDetail
        'TheVenue.VenueId = VenueID
        'Dim VenueDetail = TheVenue.Execute(credential)


        '
        'This works
        'Dim TheURL As String = "https://api.foursquare.com/v2/venues/" & VenueID & "?client_id=" & ClienID & "&client_secret=" & Secret
        'Dim TheVenue As New FoursquareNET.Venue.GetDetail
        'TheVenue.VenueId = VenueID
        'Dim VenueDetail = TheVenue.Executev2(TheURL)


        'Dim hi As String = ""


        'Dim history = New FoursquareNET.Checkins.History()
        'Dim checkins = history.Execute(credential)

        'Dim hi As String = ""





    End Sub


End Class
