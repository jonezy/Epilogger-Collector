' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class Location

		Private __jobject As JObject
		Public Sub New(obj As JObject)
			Me.__jobject = obj
		End Sub

		Public ReadOnly Property Address() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "address"))
			End Get
		End Property

		Public ReadOnly Property CrossStreet() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "crossStreet"))
			End Get
		End Property

		Public ReadOnly Property City() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "city"))
			End Get
		End Property

		Public ReadOnly Property State() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "state"))
			End Get
		End Property

		Public ReadOnly Property PostalCode() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "postalCode"))
			End Get
		End Property

		Public ReadOnly Property Country() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "country"))
			End Get
		End Property

		Public ReadOnly Property Lat() As Double
			Get
				Return JsonClassHelper.ReadFloat(JsonClassHelper.GetJToken(Of JValue)(__jobject, "lat"))
			End Get
		End Property

		Public ReadOnly Property Lng() As Double
			Get
				Return JsonClassHelper.ReadFloat(JsonClassHelper.GetJToken(Of JValue)(__jobject, "lng"))
			End Get
		End Property

	End Class
End Namespace
