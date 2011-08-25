Imports System.IO
Imports System.Text
Imports FlickrNet

Public Class Helpers

    Public Const sBase58Alphabet As String = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ"

    Public Function UnshortenURL(ByVal ShortURL As Uri) As Uri

        Try
            '
            'This will unshorten any URL
            Dim req As System.Net.WebRequest = System.Net.WebRequest.Create("http://api.unfwd4.me/?url=" & ShortURL.ToString)
            Dim resp As System.Net.WebResponse = req.GetResponse()
            Dim receiveStream As Stream = resp.GetResponseStream()
            Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
            Dim ReturnValue As String = readStream.ReadToEnd()
            If ReturnValue <> "ERROR:1" And ReturnValue <> "ERROR:2" Then
                Return New Uri(ReturnValue)
            Else
                Return ShortURL
            End If
            resp.Close()
            readStream.Close()
        Catch ex As Exception
            Return ShortURL
        End Try

    End Function


    Public Sub GetImageServiceImageURLs(ByVal TheURL As Uri, ByRef FullImageURL As Uri, ByRef ThumbImageURL As Uri)

        '
        'Get the Image URLs
        Select Case TheURL.Host
            Case "twitpic.com"
                FullImageURL = New Uri("http://twitpic.com/show/large" & TheURL.LocalPath)
                ThumbImageURL = New Uri("http://twitpic.com/show/mini" & TheURL.LocalPath)
            Case "yfrog.com"
                FullImageURL = New Uri("http://yfrog.com" & TheURL.LocalPath & ":iphone")
                ThumbImageURL = New Uri("http://yfrog.com" & TheURL.LocalPath & ":small")
            Case "plixi.com"
                FullImageURL = New Uri("http://api.plixi.com/api/tpapi.svc/imagefromurl?size=medium&url=" & TheURL.AbsoluteUri)
                ThumbImageURL = New Uri("http://api.plixi.com/api/tpapi.svc/imagefromurl?size=thumbnail&url=" & TheURL.AbsoluteUri)
            Case "lockerz.com"
                FullImageURL = New Uri("http://api.plixi.com/api/tpapi.svc/imagefromurl?size=medium&url=" & TheURL.AbsoluteUri)
                ThumbImageURL = New Uri("http://api.plixi.com/api/tpapi.svc/imagefromurl?size=thumbnail&url=" & TheURL.AbsoluteUri)
            Case "instagr.am"
                FullImageURL = New Uri(TheURL.AbsoluteUri & "media/?size=l")
                ThumbImageURL = New Uri(TheURL.AbsoluteUri & "media/?size=t")
            Case "flic.kr", "flickr.com", "www.flickr.com"
                Try
                    '
                    'Flickr is a little more complicated, need to manually unshorten the short ImageID, then we can get the URLS
                    'Query: "?short=9YQy6Z"
                    Dim TheShortcode As String = String.Empty
                    TheShortcode = TheURL.Query.Substring(InStr(TheURL.Query, "="))

                    Dim FL As New FlickrNet.Flickr("332533462d321fbb78ae8e04c691cea7")
                    Dim FPI As PhotoInfo = FL.PhotosGetInfo(DecodeBase58(TheShortcode))

                    If Not FPI Is Nothing Then
                        '
                        'Now build the URL from the parts.
                        'http://farm{farm-id}.static.flickr.com/{server-id}/{id}_{secret}_[mstzb].jpg
                        FullImageURL = New Uri("http://farm" & FPI.Farm & ".static.flickr.com/" & FPI.Server & "/" & FPI.PhotoId & "_" & FPI.Secret & "_b.jpg")
                        ThumbImageURL = New Uri("http://farm" & FPI.Farm & ".static.flickr.com/" & FPI.Server & "/" & FPI.PhotoId & "_" & FPI.Secret & "_t.jpg")
                    End If

                Catch ex As Exception
                End Try
                'Case "smugmug.com"

                'Case "posterous.com"

            Case Else
                FullImageURL = Nothing
                ThumbImageURL = Nothing
        End Select

    End Sub

    <Obsolete("Use GetImageServiceImageURLs instead")> _
    Public Function GetImageServiceFullURL(ByVal TheURL As Uri) As Uri

        Select Case TheURL.Host
            Case "twitpic.com"
                Return New Uri("http://twitpic.com/show/large" & TheURL.LocalPath)
            Case "yfrog.com"
                Return New Uri("http://yfrog.com" & TheURL.LocalPath & ":iphone")
            Case "plixi.com"
                Return New Uri("http://api.plixi.com/api/tpapi.svc/imagefromurl?size=medium&url=" & TheURL.AbsoluteUri)
            Case "lockerz.com"
                Return New Uri("http://api.plixi.com/api/tpapi.svc/imagefromurl?size=medium&url=" & TheURL.AbsoluteUri)
            Case Else
                Return Nothing
        End Select

    End Function

    <Obsolete("Use GetImageServiceImageURLs instead")> _
    Public Function GetImageServiceThumbURL(ByVal TheURL As Uri) As Uri
        Select Case TheURL.Host
            Case "twitpic.com"
                Return New Uri("http://twitpic.com/show/mini" & TheURL.LocalPath)
            Case "yfrog.com"
                Return New Uri("http://yfrog.com" & TheURL.LocalPath & ":small")
            Case "plixi.com"
                Return New Uri("http://api.plixi.com/api/tpapi.svc/imagefromurl?size=thumbnail&url=" & TheURL.AbsoluteUri)
            Case "lockerz.com"
                Return New Uri("http://api.plixi.com/api/tpapi.svc/imagefromurl?size=thumbnail&url=" & TheURL.AbsoluteUri)
            Case Else
                Return Nothing
        End Select
    End Function

    Public Shared Function DecodeBase58(base58StringToExpand As String) As Long
        Dim lConverted As Long = 0
        Dim lTemporaryNumberConverter As Long = 1

        While base58StringToExpand.Length > 0
            Dim sCurrentCharacter As [String] = base58StringToExpand.Substring(base58StringToExpand.Length - 1)
            lConverted = lConverted + (lTemporaryNumberConverter * sBase58Alphabet.IndexOf(sCurrentCharacter))
            lTemporaryNumberConverter = lTemporaryNumberConverter * sBase58Alphabet.Length
            base58StringToExpand = base58StringToExpand.Substring(0, base58StringToExpand.Length - 1)
        End While

        Return lConverted
    End Function

    Public Shared Function EncodeBase58(numberToShorten As UInt32) As [String]
        Dim sConverted As [String] = ""
        Dim iAlphabetLength As Int32 = sBase58Alphabet.Length

        While numberToShorten > 0
            Dim lNumberRemainder As Long = (numberToShorten Mod iAlphabetLength)
            numberToShorten = Convert.ToUInt32(numberToShorten / iAlphabetLength)
            sConverted = sBase58Alphabet(Convert.ToInt32(lNumberRemainder)) + sConverted
        End While

        Return sConverted
    End Function


End Class
