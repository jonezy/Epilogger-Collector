' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class Contact

		Private __jobject As JObject
		Public Sub New(obj As JObject)
			Me.__jobject = obj
		End Sub

		Public ReadOnly Property Phone() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "phone"))
			End Get
		End Property

	End Class
End Namespace
