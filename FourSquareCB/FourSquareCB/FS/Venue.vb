' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class Venue

		Private __jobject As JObject
		Public Sub New(obj As JObject)
			Me.__jobject = obj
		End Sub

		Public ReadOnly Property Id() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "id"))
			End Get
		End Property

		Public ReadOnly Property Name() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "name"))
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _contact As Contact
		Public ReadOnly Property Contact() As Contact
			Get
				If _contact Is Nothing Then
					_contact = JsonClassHelper.ReadStronglyTypedObject(Of Contact)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "contact"))
				End If
				Return _contact
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _location As Location
		Public ReadOnly Property Location() As Location
			Get
				If _location Is Nothing Then
					_location = JsonClassHelper.ReadStronglyTypedObject(Of Location)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "location"))
				End If
				Return _location
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _categories As Category()
		Public ReadOnly Property Categories() As Category()
			Get
				If _categories Is Nothing Then
                    _categories = DirectCast(JsonClassHelper.ReadArray(Of Category)(JsonClassHelper.GetJToken(Of JArray)(__jobject, "categories"), JsonClassHelper.ReadStronglyTypedObject(Of Category), GetType(Category())), Category())
				End If
				Return _categories
			End Get
		End Property

		Public ReadOnly Property Verified() As Boolean
			Get
				Return JsonClassHelper.ReadBoolean(JsonClassHelper.GetJToken(Of JValue)(__jobject, "verified"))
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _stats As Stats
		Public ReadOnly Property Stats() As Stats
			Get
				If _stats Is Nothing Then
					_stats = JsonClassHelper.ReadStronglyTypedObject(Of Stats)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "stats"))
				End If
				Return _stats
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _hereNow As HereNow
		Public ReadOnly Property HereNow() As HereNow
			Get
				If _hereNow Is Nothing Then
					_hereNow = JsonClassHelper.ReadStronglyTypedObject(Of HereNow)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "hereNow"))
				End If
				Return _hereNow
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _mayor As Mayor
		Public ReadOnly Property Mayor() As Mayor
			Get
				If _mayor Is Nothing Then
					_mayor = JsonClassHelper.ReadStronglyTypedObject(Of Mayor)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "mayor"))
				End If
				Return _mayor
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _tips As Tips
		Public ReadOnly Property Tips() As Tips
			Get
				If _tips Is Nothing Then
					_tips = JsonClassHelper.ReadStronglyTypedObject(Of Tips)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "tips"))
				End If
				Return _tips
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _tags As Object()
		Public ReadOnly Property Tags() As Object()
			Get
				If _tags Is Nothing Then
					_tags = DirectCast(JsonClassHelper.ReadArray(Of Object)(JsonClassHelper.GetJToken(Of JArray)(__jobject, "tags"), JsonClassHelper.ReadObject, GetType(Object())), Object())
				End If
				Return _tags
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _specials As Object()
		Public ReadOnly Property Specials() As Object()
			Get
				If _specials Is Nothing Then
					_specials = DirectCast(JsonClassHelper.ReadArray(Of Object)(JsonClassHelper.GetJToken(Of JArray)(__jobject, "specials"), JsonClassHelper.ReadObject, GetType(Object())), Object())
				End If
				Return _specials
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _specialsNearby As Object()
		Public ReadOnly Property SpecialsNearby() As Object()
			Get
				If _specialsNearby Is Nothing Then
					_specialsNearby = DirectCast(JsonClassHelper.ReadArray(Of Object)(JsonClassHelper.GetJToken(Of JArray)(__jobject, "specialsNearby"), JsonClassHelper.ReadObject, GetType(Object())), Object())
				End If
				Return _specialsNearby
			End Get
		End Property

		Public ReadOnly Property ShortUrl() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "shortUrl"))
			End Get
		End Property

		Public ReadOnly Property TimeZone() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "timeZone"))
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _photos As Photos
		Public ReadOnly Property Photos() As Photos
			Get
				If _photos Is Nothing Then
					_photos = JsonClassHelper.ReadStronglyTypedObject(Of Photos)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "photos"))
				End If
				Return _photos
			End Get
		End Property

	End Class
End Namespace
