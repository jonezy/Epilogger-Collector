' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class Tips

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
		Private _groups As Group2()
		Public ReadOnly Property Groups() As Group2()
			Get
				If _groups Is Nothing Then
					_groups = DirectCast(JsonClassHelper.ReadArray(Of Group2)(JsonClassHelper.GetJToken(Of JArray)(__jobject, "groups"), JsonClassHelper.ReadStronglyTypedObject(Of Group2), GetType(Group2())), Group2())
				End If
				Return _groups
			End Get
		End Property

	End Class
End Namespace
