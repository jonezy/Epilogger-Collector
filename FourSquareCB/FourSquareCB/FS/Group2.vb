' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class Group2

		Private __jobject As JObject
		Public Sub New(obj As JObject)
			Me.__jobject = obj
		End Sub

		Public ReadOnly Property TypE() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "type"))
			End Get
		End Property

		Public ReadOnly Property Name() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "name"))
			End Get
		End Property

		Public ReadOnly Property Count() As Integer
			Get
				Return JsonClassHelper.ReadInteger(JsonClassHelper.GetJToken(Of JValue)(__jobject, "count"))
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _items As Item()
		Public ReadOnly Property Items() As Item()
			Get
				If _items Is Nothing Then
					_items = DirectCast(JsonClassHelper.ReadArray(Of Item)(JsonClassHelper.GetJToken(Of JArray)(__jobject, "items"), JsonClassHelper.ReadStronglyTypedObject(Of Item), GetType(Item())), Item())
				End If
				Return _items
			End Get
		End Property

	End Class
End Namespace
