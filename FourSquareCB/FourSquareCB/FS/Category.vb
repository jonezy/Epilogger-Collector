' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class Category

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

		Public ReadOnly Property PluralName() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "pluralName"))
			End Get
		End Property

		Public ReadOnly Property Icon() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "icon"))
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _parents As String()
		Public ReadOnly Property Parents() As String()
			Get
				If _parents Is Nothing Then
                    '_parents = DirectCast(JsonClassHelper.ReadArray(Of String)(JsonClassHelper.GetJToken(Of JArray)(__jobject, "parents"), JsonClassHelper.ReadString, GetType(String())), String())
				End If
				Return _parents
			End Get
		End Property

		Public ReadOnly Property Primary() As Boolean
			Get
				Return JsonClassHelper.ReadBoolean(JsonClassHelper.GetJToken(Of JValue)(__jobject, "primary"))
			End Get
		End Property

	End Class
End Namespace
