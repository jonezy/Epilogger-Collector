' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class Mayor

		Private __jobject As JObject
		Public Sub New(obj As JObject)
			Me.__jobject = obj
		End Sub

		Public ReadOnly Property Count() As Integer
			Get
				Return JsonClassHelper.ReadInteger(JsonClassHelper.GetJToken(Of JValue)(__jobject, "count"))
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _user As User
		Public ReadOnly Property User() As User
			Get
				If _user Is Nothing Then
					_user = JsonClassHelper.ReadStronglyTypedObject(Of User)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "user"))
				End If
				Return _user
			End Get
		End Property

	End Class
End Namespace
