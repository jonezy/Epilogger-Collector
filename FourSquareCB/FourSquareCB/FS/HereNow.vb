' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class HereNow

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
		Private _groups As Group()
		Public ReadOnly Property Groups() As Group()
			Get
				If _groups Is Nothing Then
					_groups = DirectCast(JsonClassHelper.ReadArray(Of Group)(JsonClassHelper.GetJToken(Of JArray)(__jobject, "groups"), JsonClassHelper.ReadStronglyTypedObject(Of Group), GetType(Group())), Group())
				End If
				Return _groups
			End Get
		End Property

	End Class
End Namespace
