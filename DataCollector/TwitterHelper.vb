Imports Microsoft.VisualBasic
Imports System.Linq
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports TweetSharp

Imports Microsoft.WindowsAzure
Imports Microsoft.WindowsAzure.StorageClient

Public Class AzureStoreTweet
    Inherits TableServiceEntity
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal partitionKey As String, ByVal rowKey As String)
        MyBase.New(partitionKey, rowKey)
    End Sub

    Public Property TwitterID As Long
    Public Property EventID As Long
    Public Property Text As String
    Public Property TextAsHTML As String
    Public Property Source As String
    Public Property CreatedDate As DateTime
    Public Property FromUserScreenName As String
    Public Property ToUserScreenName As String
    Public Property IsoLanguageCode As String
    Public Property ProfileImageURL As String
    Public Property SinceID As Long
    Public Property Location As String
    Public Property RawSource As String
End Class





Public Class TwitterHelper

    Private _MyLogging As LoggingReporting.Logging
    Private _helpers As New Helpers

    Public Property EventID As Integer
    Public Property SearchTerms As String
    Public Property CollectionMode As Integer
    Public Property UseLastTweetID As Boolean

    Public Function ExecQueryReturnDR(ByVal Query As String, ByVal conn As SqlConnection) As SqlDataReader

        Try
            Dim mycommand As New Data.SqlClient.SqlCommand(Query, conn)

            mycommand.CommandTimeout = 500
            mycommand.CommandType = Data.CommandType.Text

            Dim myReader As Data.SqlClient.SqlDataReader
            myReader = mycommand.ExecuteReader()
            Return myReader
        Catch ex As Exception
            _MyLogging.writeToLog(My.Application.Info.ProductName, "TwitterHelper.vb/ExecQuery - " & ex.Message.ToString & " - " & ex.InnerException.ToString, LoggingReporting.Logging.AlertType.LogError)
            Throw ex
        End Try

    End Function

    Public Function ExecQuery(ByVal Query As String, ByVal conn As SqlConnection) As Boolean

        Try
            Dim mycommand As New Data.SqlClient.SqlCommand(Query, conn)

            mycommand.CommandTimeout = 500
            mycommand.CommandType = Data.CommandType.Text

            mycommand.ExecuteScalar()
            Return (True)
        Catch ex As Exception
            _MyLogging.writeToLog(My.Application.Info.ProductName, "TwitterHelper.vb/ExecQuery - " & ex.Message.ToString & " - " & ex.InnerException.ToString, LoggingReporting.Logging.AlertType.LogError)
            Return False
        End Try

    End Function

    Public Sub GetNewTweets()
        'ByVal EventID As Integer, ByVal SearchTerms As String, ByVal CollectionMode As Integer
        If Me.EventID = 0 Then
            Throw New Exception("This method requires an EventID")
        End If
        If Me.SearchTerms.Length = 0 Then
            Throw New Exception("This method requires Search Terms")
        End If
        

        '
        'There is a mix of LINQ and ADO.net in this sub. Need to determine if there was performance issues with the Linq queries
        Dim MyConnection As New Data.SqlClient.SqlConnection(My.Settings.EpiloggerConnectionString)
        MyConnection.Open()

        Dim db As New TweetsDataContext
        Dim service As New TwitterService()

        'Dim AzureTweetStore As New AzureTableHelper(My.Settings.AzureTableStorageConnectionString, False)

        '
        'Get the last TwitterID for the seach. This way we only get new tweets.
        Dim LastTweetID As Long = 0


        '
        'Can't use ATS to get the last record inserted. So instead of reversing the row keys (), I'm going to store the LastTwitterID in the Events Table too and query that.
        'FUCK, this isn't going to work either as the tweetIDs are not in order.
        'Going to have to store a basebones Tweets table to facilite these lookups by Id and CreatedDateTime. Fuck BS.

        '
        'UNDEPRICATED - CB Aug 26, 2011 - This code is no longer used. Tweets have been moved to Azure Table Storage. All code will now reference that.
        'This gets the lsat TwitterID so we only search from there going forward.

        If Me.UseLastTweetID Then
            Dim myreader As SqlDataReader
            myreader = ExecQueryReturnDR("Select top 1 TwitterID from Tweets Where EventID=" & EventID & " order by ID Desc", MyConnection)
            While myreader.Read()
                LastTweetID = myreader("TwitterID")
            End While
            myreader.Close()
        End If
        


        '
        'If this is the First time we're pulling tweets for an event don't get too many old tweets. This could result in info not for this event. But we do want to get some lead up.
        'If this is the first time pulling tweets for the event and it will be when the event starts. Pull 50 tweets (might need to make this smaller). Else pull 100.

        '--DEPRICATED - CB - Aug 28, 2011 - Now we want to get the max and Date filter it.
        'Dim NumberOfTweetToPull As Integer
        'Dim PageMax As Integer
        'If LastTweetID = 0 Then
        '    NumberOfTweetToPull = 50
        '    PageMax = 1
        'Else
        '    NumberOfTweetToPull = 100
        '    PageMax = 50
        'End If

        Dim NumberOfTweetToPull As Integer
        Dim PageMax As Integer
        NumberOfTweetToPull = 100
        PageMax = 50

        '
        'Now loop through all the pages 100 tweets in each.
        For page As Integer = 1 To PageMax
            '
            'Do the Search since the last tweet
            Try
                '
                'The existing implementation of TweetSharp search doesn't have enough parameters for me. Like Lang and Near. Custom implementation of those API params
                'in TweetSharp

                Dim RelaventTweetResults As TwitterSearchResult

                '
                'CB June 27, 2011 - This has been commented out as some tweets are going missed. This is to see if they're being missed because this is somehow excluding valid tweets.
                'If this event has a Venue, then we have the 4SQ venue data :), use the Lat, Long to create a tweet zone, pick up more relavent tweets.
                'If db.Events.Where(Function(e) e.ID = Me.EventID And e.VenueID > 0).Count > 0 Then
                '    '
                '    'Now we're specifying lang so we don't get chinese tweets.
                '    RelaventTweetResults = service.SearchSinceCB(LastTweetID, SearchTerms, page, NumberOfTweetToPull, "en", "", "5mi")
                'Else
                '    '
                '    'Now we're specifying lang so we don't get chinese tweets.
                '    RelaventTweetResults = service.SearchSinceCB(LastTweetID, SearchTerms, page, NumberOfTweetToPull, "en")
                'End If


                '
                'Get everything instead.
                RelaventTweetResults = service.SearchSince(LastTweetID, SearchTerms, page, NumberOfTweetToPull)

                '
                'If the object is nothing try again.
                If RelaventTweetResults.Statuses Is Nothing Then
                    RelaventTweetResults = service.SearchSince(LastTweetID, SearchTerms, page, NumberOfTweetToPull)
                    If RelaventTweetResults.Statuses Is Nothing Then
                        '
                        'Nothing again. Bail
                        Exit For
                    End If
                End If

                '
                'Store the tweets
                Dim TheEvent As [Event] = db.Events.Where(Function(e) e.ID = EventID).FirstOrDefault
                For Each STweet As TweetSharp.TwitterSearchStatus In RelaventTweetResults.Statuses
                    '
                    'Check the the tweet CreatedDate is within our Collection Start and End Dates.
                    'EndDate is nullable

                    Dim EndDateTime As DateTime
                    If TheEvent.CollectionEndDateTime.HasValue Then
                        EndDateTime = TheEvent.CollectionEndDateTime
                    Else
                        EndDateTime = DateTime.Parse("2200-01-01 00:00:00")
                    End If
                    If STweet.CreatedDate >= TheEvent.CollectionStartDateTime And STweet.CreatedDate <= EndDateTime Then

                        '
                        'Insert the Tweet into the DB - Local SQL
                        Dim CurrentTweetID As Long = STweet.Id
                        Dim ExistsCount = Aggregate Tweet In db.Tweets Into Count(Tweet.TwitterID = CurrentTweetID)

                        If ExistsCount = 0 Then

                            '
                            'Insert twitter stub record
                            Dim LTweet As Tweet
                            LTweet = New Tweet With _
                            {.TwitterID = STweet.Id, _
                            .EventID = EventID, _
                            .Text = STweet.Text, _
                            .TextAsHTML = STweet.TextAsHtml, _
                            .Source = STweet.Source, _
                            .CreatedDate = STweet.CreatedDate, _
                            .FromUserScreenName = STweet.FromUserScreenName, _
                            .ToUserScreenName = STweet.ToUserScreenName, _
                            .IsoLanguageCode = STweet.IsoLanguageCode, _
                            .ProfileImageURL = STweet.ProfileImageUrl, _
                            .SinceID = STweet.SinceId, _
                            .Location = STweet.Location, _
                            .RawSource = STweet.RawSource}

                            db.Tweets.InsertOnSubmit(LTweet)

                            'Have to submit the changes here so that we have a Tweet Record ID to insert into the Mapping Table.
                            Try
                                db.SubmitChanges()
                            Catch e As Exception
                                db.SubmitChanges()
                            End Try


                            '
                            '********** Pull out the Images stored directly on Twitter! :)
                            For Each p As TwitterMedia In STweet.Entities.Media
                                Try
                                    '
                                    'Check to see if this image has already been stored, if it has reuse the stored image and add another meta data record to it. 
                                    'If it hasn't store it.
                                    Dim TheURL As String = p.MediaURL
                                    Dim PImage As IEnumerable(Of EpiloggerImage) = db.EpiloggerImages.Where(Function(e) e.OriginalImageLink = TheURL)

                                    If PImage.Count > 0 Then
                                        For Each pI As EpiloggerImage In PImage
                                            '
                                            'Insert metadata for image
                                            Dim LIMD As New EpiloggerImageMetaData With
                                            {.ImageID = pI.ID, _
                                             .EventID = EventID, _
                                             .UserID = System.Guid.Empty, _
                                             .ImageSource = "twitter", _
                                             .TwitterID = STweet.Id, _
                                             .TwitterName = STweet.FromUserScreenName,
                                             .DateTime = STweet.CreatedDate}
                                            db.EpiloggerImageMetaDatas.InsertOnSubmit(LIMD)
                                        Next
                                    Else
                                        '
                                        'Insert image
                                        '
                                        'Get a MemStream from the Image URL, upload to Azure.
                                        Dim FullImageStream As New IO.MemoryStream(New System.Net.WebClient().DownloadData(p.MediaURL))
                                        Dim FullAzureURL As Uri = StoreImage("twitter-" & EventID & "-" & LTweet.ID & ".jpg", "twitterphotos-full", FullImageStream)

                                        'Dim ThumbImageStream As New IO.MemoryStream(New System.Net.WebClient().DownloadData(p.MediaURL))
                                        'Dim ThumbAzureURL As Uri = StoreImage("twitter-" & EventID & "-" & LTweet.ID & ".jpg", "twitterphotos-thumb", ThumbImageStream)

                                        '
                                        'Store the Info in the DB
                                        Dim LImage As New EpiloggerImage With
                                            {.EventID = EventID, _
                                             .AzureContainerPrefix = "twitterphotos", _
                                             .Fullsize = FullAzureURL.AbsoluteUri, _
                                             .Thumb = FullAzureURL.AbsoluteUri, _
                                             .OriginalImageLink = p.DisplayURL, _
                                             .DateTime = STweet.CreatedDate, _
                                             .DeleteVoteCount = 0, _
                                             .Deleted = False}
                                        db.EpiloggerImages.InsertOnSubmit(LImage)
                                        db.SubmitChanges()

                                        Dim LIMD As New EpiloggerImageMetaData With
                                            {.ImageID = LImage.ID, _
                                             .EventID = EventID, _
                                             .UserID = System.Guid.Empty, _
                                             .ImageSource = "twitter", _
                                             .TwitterID = LTweet.TwitterID, _
                                             .TwitterName = STweet.FromUserScreenName,
                                             .DateTime = STweet.CreatedDate}
                                        db.EpiloggerImageMetaDatas.InsertOnSubmit(LIMD)
                                        db.SubmitChanges()
                                    End If

                                Catch ex As Exception
                                    Console.WriteLine(ex)
                                End Try
                            Next


                            '
                            'Pull out the images from the tweets and store URLs
                            For Each l As TwitterUrl In STweet.Entities.Urls
                                Try
                                    Dim TheURL As Uri
                                    Dim UnShortenedURL As Uri
                                    Dim FullImagesURL As Uri
                                    Dim ThumbImagesURL As Uri
                                    If l.ExpandedValue IsNot Nothing Then
                                        If Not l.ExpandedValue.Contains("http://") Then
                                            l.ExpandedValue = "http://" & l.ExpandedValue
                                        End If
                                        TheURL = New Uri(l.ExpandedValue)
                                    Else
                                        If Not l.Value.Contains("http://") Then
                                            l.Value = "http://" & l.Value
                                        End If
                                        TheURL = New Uri(l.Value)
                                    End If

                                    '
                                    'Unshorten the URL so we know what's really in it.
                                    UnShortenedURL = _helpers.UnshortenURL(TheURL)


                                    '
                                    'Run the URL through image service detection and get full image URL
                                    If TheURL.Host = "plixi.com" Then
                                        _helpers.GetImageServiceImageURLs(TheURL, FullImagesURL, ThumbImagesURL)
                                    Else
                                        _helpers.GetImageServiceImageURLs(UnShortenedURL, FullImagesURL, ThumbImagesURL)
                                    End If


                                    If FullImagesURL IsNot Nothing Then
                                        '
                                        'Check to see if this image has already been stored, if it has reuse the stored image and add another meta data record to it. 
                                        'If it hasn't store it.
                                        Dim PImage As IEnumerable(Of EpiloggerImage) = db.EpiloggerImages.Where(Function(e) e.OriginalImageLink = UnShortenedURL.AbsoluteUri)

                                        If PImage.Count > 0 Then
                                            For Each pI As EpiloggerImage In PImage
                                                '
                                                'Insert metadata for image
                                                Dim LIMD As New EpiloggerImageMetaData With
                                                {.ImageID = pI.ID, _
                                                 .EventID = EventID, _
                                                 .UserID = System.Guid.Empty, _
                                                 .ImageSource = "twitter", _
                                                 .TwitterID = STweet.Id, _
                                                 .TwitterName = STweet.FromUserScreenName,
                                                 .DateTime = STweet.CreatedDate}
                                                db.EpiloggerImageMetaDatas.InsertOnSubmit(LIMD)
                                            Next
                                        Else
                                            '
                                            'Insert image
                                            '
                                            'Get a MemStream from the Image URL, upload to Azure.
                                            Dim FullImageStream As New IO.MemoryStream(New System.Net.WebClient().DownloadData(FullImagesURL))
                                            Dim FullAzureURL As Uri = StoreImage("twitter-" & EventID & "-" & LTweet.ID & ".jpg", "twitterphotos-full", FullImageStream)

                                            Dim ThumbImageStream As New IO.MemoryStream(New System.Net.WebClient().DownloadData(ThumbImagesURL))
                                            Dim ThumbAzureURL As Uri = StoreImage("twitter-" & EventID & "-" & LTweet.ID & ".jpg", "twitterphotos-thumb", ThumbImageStream)

                                            '
                                            'Store the Info in the DB
                                            Dim LImage As New EpiloggerImage With
                                                {.EventID = EventID, _
                                                 .AzureContainerPrefix = "twitterphotos", _
                                                 .Fullsize = FullAzureURL.AbsoluteUri, _
                                                 .Thumb = ThumbAzureURL.AbsoluteUri, _
                                                 .OriginalImageLink = UnShortenedURL.AbsoluteUri, _
                                                 .DateTime = STweet.CreatedDate, _
                                                 .DeleteVoteCount = 0, _
                                                 .Deleted = False}
                                            db.EpiloggerImages.InsertOnSubmit(LImage)
                                            db.SubmitChanges()

                                            Dim LIMD As New EpiloggerImageMetaData With
                                                {.ImageID = LImage.ID, _
                                                 .EventID = EventID, _
                                                 .UserID = System.Guid.Empty, _
                                                 .ImageSource = "twitter", _
                                                 .TwitterID = LTweet.TwitterID, _
                                                 .TwitterName = STweet.FromUserScreenName,
                                                 .DateTime = STweet.CreatedDate}
                                            db.EpiloggerImageMetaDatas.InsertOnSubmit(LIMD)
                                        End If
                                    Else
                                        '
                                        'Check if there is a reference to Foursquare in the URL, if there is. It's probably a check in! woohoo.
                                        If UnShortenedURL.ToString.Contains("4sq") Or TheURL.ToString.Contains("4sq") Or _
                                            UnShortenedURL.ToString.Contains("foursquare") Or TheURL.ToString.Contains("foursquare") Then
                                            If STweet.Text.Contains("(@") Then
                                                '
                                                '4sq check in
                                                Dim LCheckin As New CheckIn With {.CheckInDateTime = LTweet.CreatedDate, .EventID = EventID, .TweetID = LTweet.ID, .FourSquareCheckInURL = UnShortenedURL.ToString}
                                                db.CheckIns.InsertOnSubmit(LCheckin)
                                            End If
                                        End If

                                        '
                                        'Just a regular URL, store the url
                                        Dim LURL As New URL With
                                            {.TweetID = LTweet.ID, _
                                                .EventID = EventID, _
                                                .ShortURL = TheURL.ToString,
                                                .FullURL = UnShortenedURL.ToString,
                                                .DateTime = STweet.CreatedDate}

                                        db.URLs.InsertOnSubmit(LURL)
                                    End If
                                    db.SubmitChanges()
                                Catch ex As Exception
                                    Console.WriteLine(ex)
                                End Try
                            Next
                        End If
                    End If
                Next

                Try
                    db.SubmitChanges()
                Catch e As Exception

                End Try

                '
                'Exit if there are no more tweets on the page
                If RelaventTweetResults.Statuses.Count = 0 Then Exit For

            Catch ex As Exception
            End Try
        Next

    End Sub


    Public Function StoreImage(ByVal Filename As String, ByVal ContainerName As String, ByVal MemStream As MemoryStream) As Uri


        Try
            ' Variables for the cloud storage objects.
            Dim cloudStorageAccount1 As CloudStorageAccount
            Dim blobClient As CloudBlobClient
            Dim blobContainer As CloudBlobContainer
            Dim containerPermissions As BlobContainerPermissions
            Dim blob As CloudBlob

            cloudStorageAccount1 = CloudStorageAccount.Parse("DefaultEndpointsProtocol=http;AccountName=epiloggerphotos;AccountKey=xbSt0uQAqExzWpc60pcmP6k49Uu7raxPG1BA5+aBhrAAdNxaoiFAZ67jQmG/iiJIeFeemnp74NRuuFAXaaxGJQ==")

            ' Create the blob client, which provides
            ' authenticated access to the Blob service.
            blobClient = cloudStorageAccount1.CreateCloudBlobClient()

            ' Get the container reference.
            blobContainer = blobClient.GetContainerReference(ContainerName)
            ' Create the container if it does not exist.
            blobContainer.CreateIfNotExist()

            ' Set permissions on the container.
            containerPermissions = New BlobContainerPermissions()
            ' This sample sets the container to have public blobs. Your application
            ' needs may be different. See the documentation for BlobContainerPermissions
            ' for more information about blob container permissions.
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob
            blobContainer.SetPermissions(containerPermissions)

            ' Get a reference to the blob.
            blob = blobContainer.GetBlobReference(Filename)
            blob.Properties.ContentType = "image/jpeg"

            ' Upload a file from the local system to the blob.
            'blob.UploadFile("c:\TestAzure.jpg")
            blob.UploadFromStream(MemStream)

            Return blob.Uri

        Catch e As StorageClientException
            Return New Uri("")
        Catch e As Exception
            Return New Uri("")
        End Try


    End Function



    'Public Sub GetTweetsFromPastEvent()
    '    'ByVal EventID As Integer, ByVal SearchTerms As String, ByVal CollectionMode As Integer
    '    If Me.EventID = 0 Then
    '        Throw New Exception("This method requires an EventID")
    '    End If
    '    If Me.SearchTerms.Length = 0 Then
    '        Throw New Exception("This method requires Search Terms")
    '    End If
    '    If Me.CollectionMode = 0 Then
    '        Throw New Exception("This method requires a Collection Mode value")
    '    End If

    '    '
    '    'There is a mix of LINQ and ADO.net in this sub. Need to determine if there was performance issues with the Linq queries

    '    Dim MyConnection As New Data.SqlClient.SqlConnection(My.Settings.ConnectionString)
    '    MyConnection.Open()

    '    Dim db As New TweetsDataContext
    '    Dim service As New TwitterService()

    '    '
    '    'Get the last TwitterID for the seach. This was we only get new tweets.
    '    Dim LastTweetID As Long = 0

    '    Dim myreader As SqlDataReader
    '    myreader = ExecQueryReturnDR("Select top 1 TwitterID from Tweets Where ID in (Select top 1 TweetID from MapTweetToEvent Where EventID=" & EventID & " order by ID Desc)", MyConnection)
    '    While myreader.Read()
    '        LastTweetID = myreader("TwitterID")
    '    End While
    '    myreader.Close()

    '    '
    '    'If this is the First time we're pulling tweets for an event don't get too many old tweets. This could result in info no for this event. But we do want to get some lead up.
    '    'If this is the first time pulling tweets for the event and it will be when the event starts. Pull 50 tweets (might need to make this smaller). Else pull 100.
    '    Dim NumberOfTweetToPull As Integer
    '    NumberOfTweetToPull = 100

    '    '
    '    'Do the Search since the last tweet
    '    Dim RelaventTweetResults As TwitterSearchResult = service.SearchSince(LastTweetID, SearchTerms, NumberOfTweetToPull)

    '    For Each STweet As TweetSharp.TwitterSearchStatus In RelaventTweetResults.Statuses
    '        '
    '        'Insert the Tweet into the DB
    '        Dim CurrentTweetID As Long = STweet.Id
    '        Dim ExistsCount = Aggregate Tweet In db.Tweets Into Count(Tweet.TwitterID = CurrentTweetID)

    '        If ExistsCount = 0 Then

    '            Dim LTweet As Tweet
    '            LTweet = New Tweet With _
    '            {.TwitterID = STweet.Id, _
    '            .Text = STweet.Text, _
    '            .TextAsHTML = STweet.TextAsHtml, _
    '            .Source = STweet.Source, _
    '            .CreatedDate = STweet.CreatedDate, _
    '            .FromUserScreenName = STweet.FromUserScreenName, _
    '            .ToUserScreenName = STweet.ToUserScreenName, _
    '            .IsoLanguageCode = STweet.IsoLanguageCode, _
    '            .ProfileImageURL = STweet.ProfileImageUrl, _
    '            .SinceID = STweet.SinceId, _
    '            .Location = STweet.Location, _
    '            .RawSource = STweet.RawSource}

    '            db.Tweets.InsertOnSubmit(LTweet)

    '            '
    '            'Have to submit the changes here so that we have a Tweet Record ID to insert into the Mapping Table.
    '            Try
    '                db.SubmitChanges()
    '            Catch e As Exception
    '                db.SubmitChanges()
    '            End Try

    '            '
    '            'Insert the Mapping Record
    '            Dim TMap As New MapTweetToEvent With {.EventID = EventID, .TweetID = LTweet.ID}
    '            db.MapTweetToEvents.InsertOnSubmit(TMap)

    '            '
    '            'Check if there is a TwitPic. If so, grab the Pic URL and throw it in the Pictures Table.
    '            Dim PictureURL As String = ""
    '            If STweet.TextAsHtml.Contains("http://twitpic.com") Then
    '                '
    '                'This BS needs to be fixed, should not be hardcoded as 25
    '                PictureURL = STweet.TextAsHtml.Substring(InStr(STweet.TextAsHtml, "http://twitpic.com"), 24)
    '                PictureURL = Right(PictureURL, 6)
    '                PictureURL = "http://twitpic.com/show/large/" & PictureURL
    '            ElseIf STweet.TextAsHtml.Contains("http://yfrog.com") Then
    '                '
    '                'This BS needs to be fixed, should not be hardcoded as 25
    '                PictureURL = STweet.TextAsHtml.Substring(InStr(STweet.TextAsHtml, "http://yfrog.com") - 1, 25)
    '                PictureURL = Right(PictureURL, 8)
    '                PictureURL = "http://yfrog.com/" & PictureURL & ":iphone"
    '            ElseIf STweet.TextAsHtml.Contains("http://plixi.com") Then
    '                PictureURL = STweet.TextAsHtml.Substring(InStr(STweet.TextAsHtml, "http://plixi.com") - 1, 27)
    '                PictureURL = "http://api.plixi.com/api/tpapi.svc/imagefromurl?size=medium&url=" & PictureURL
    '            End If

    '            If PictureURL.Length > 0 Then
    '                Dim LPicture As New Picture With
    '                    {.ImageSource = "Twitter", _
    '                     .TweetID = LTweet.ID, _
    '                     .EventID = EventID, _
    '                     .ImageURL = PictureURL}

    '                db.Pictures.InsertOnSubmit(LPicture)
    '            End If

    '        End If

    '    Next

    '    Try
    '        db.SubmitChanges()
    '    Catch e As Exception
    '        Console.WriteLine(e)
    '    End Try


    'End Sub


















    'Dim InsertTweetDR As SqlDataReader
    'InsertTweetDR = ExecQueryReturnDR("Insert Into Tweets (TwitterID, Text, TextAsHTML, Source, CreatedDate, FromUserScreenName, ToUserScreenName, IsoLanguageCode, ProfileImageURL, SinceID, Location, RawSource) " & _
    '                                  "Values(" & STweet.Id & ", '" & STweet.Text & "', '" & STweet.TextAsHtml & "', '" & STweet.Source & "', '" & STweet.CreatedDate & "', '" & STweet.FromUserScreenName & "', '" _
    '                                            & STweet.ToUserScreenName & "', '" & STweet.IsoLanguageCode & "', '" & STweet.ProfileImageUrl & "', " & STweet.SinceId & ", '" & STweet.Location & "', '" & STweet.RawSource & "');select @@identity", MyConnection)
    'Dim TweetID As Long = 0
    'While InsertTweetDR.Read()
    '    TweetID = InsertTweetDR(0)
    'End While
    'InsertTweetDR.Close()

    ''
    ''Insert the Mapping Record
    'If TweetID > 0 Then
    '    If Not ExecQuery("Insert into MapTweetToEvent (EventID, TweetID) Values(" & EventID & ", " & TweetID & ")", MyConnection) Then
    '        '
    '        'Record error

    '    End If
    'End If



    'Print("Insert Into Tweets (TwitterID, Text, TextAsHTML, Source, CreatedDate, FromUserScreenName, ToUserScreenName, IsoLanguageCode, ProfileImageURL, SinceID, Location, RawSource)")
    'Print("Values(" & STweet.Id & ", " & STweet.Text & ", " & STweet.TextAsHtml & ", " & STweet.Source & ", " & STweet.CreatedDate & ", " & STweet.FromUserScreenName & ", " _
    '      & STweet.ToUserScreenName & ", " & STweet.IsoLanguageCode & ", " & STweet.ProfileImageUrl & ", " & STweet.SinceId & ", " & STweet.Location & ", " & STweet.RawSource & ");select @@identity")

    'Dim TwitterId As Long = STweet.Id

    'Dim ExistsCount = Aggregate Tweet In db.Tweets _
    '               Into Count(Tweet.TwitterID = TwitterId)

    'Dim LTweet As Tweet
    'If ExistsCount = 0 Then
    '    LTweet = New Tweet With _
    '    {.TwitterID = STweet.Id, _
    '     .Text = STweet.Text, _
    '     .TextAsHTML = STweet.TextAsHtml, _
    '     .Source = STweet.Source, _
    '     .CreatedDate = STweet.CreatedDate, _
    '     .FromUserScreenName = STweet.FromUserScreenName, _
    '     .ToUserScreenName = STweet.ToUserScreenName, _
    '     .IsoLanguageCode = STweet.IsoLanguageCode, _
    '     .ProfileImageURL = STweet.ProfileImageUrl, _
    '     .SinceID = STweet.SinceId, _
    '     .Location = STweet.Location, _
    '     .RawSource = STweet.RawSource}

    '    db.Tweets.InsertOnSubmit(LTweet)

    '    '
    '    'Have to submit the changes here so that we have a Tweet Record ID to insert into the Mapping Table.
    '    Try
    '        db.SubmitChanges()
    '    Catch e As Exception
    '        db.SubmitChanges()
    '    End Try
    'Else
    ''
    ''There is amajor problem with this query. Need to fix.
    'Dim ATweet = From t In db.Tweets Where t.TwitterID = TwitterId
    '      Select t Take 1
    'For Each t As Tweet In ATweet
    '    LTweet = t
    'Next
    'End If

    ''
    ''Insert the Linking Table Record.
    'Dim TweetInWallExistsCount = Aggregate WallToTweetMapping In db.WallToTweetMappings _
    '               Into Count(WallToTweetMapping.TweetID = LTweet.ID And WallToTweetMapping.WallID = WallID)
    'If TweetInWallExistsCount = 0 Then
    '    Dim TMap As New WallToTweetMapping With _
    '                {.WallID = WallID, .TweetID = LTweet.ID}
    '    db.WallToTweetMappings.InsertOnSubmit(TMap)
    'End If



    Public Sub New()
        Me.UseLastTweetID = True
    End Sub
End Class
