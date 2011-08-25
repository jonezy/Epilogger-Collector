' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class Stats

		Private __jobject As JObject
		Public Sub New(obj As JObject)
			Me.__jobject = obj
		End Sub

		Public ReadOnly Property CheckinsCount() As Integer
			Get
				Return JsonClassHelper.ReadInteger(JsonClassHelper.GetJToken(Of JValue)(__jobject, "checkinsCount"))
			End Get
		End Property

		Public ReadOnly Property UsersCount() As Integer
			Get
				Return JsonClassHelper.ReadInteger(JsonClassHelper.GetJToken(Of JValue)(__jobject, "usersCount"))
			End Get
		End Property

	End Class
End Namespace
