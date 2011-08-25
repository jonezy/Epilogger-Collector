' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class User

		Private __jobject As JObject
		Public Sub New(obj As JObject)
			Me.__jobject = obj
		End Sub

		Public ReadOnly Property Id() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "id"))
			End Get
		End Property

		Public ReadOnly Property FirstName() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "firstName"))
			End Get
		End Property

		Public ReadOnly Property LastName() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "lastName"))
			End Get
		End Property

		Public ReadOnly Property Photo() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "photo"))
			End Get
		End Property

		Public ReadOnly Property Gender() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "gender"))
			End Get
		End Property

		Public ReadOnly Property HomeCity() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "homeCity"))
			End Get
		End Property

	End Class
End Namespace
