' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class Item

		Private __jobject As JObject
		Public Sub New(obj As JObject)
			Me.__jobject = obj
		End Sub

		Public ReadOnly Property Id() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "id"))
			End Get
		End Property

		Public ReadOnly Property CreatedAt() As Integer
			Get
				Return JsonClassHelper.ReadInteger(JsonClassHelper.GetJToken(Of JValue)(__jobject, "createdAt"))
			End Get
		End Property

		Public ReadOnly Property Text() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "text"))
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _todo As Todo
		Public ReadOnly Property Todo() As Todo
			Get
				If _todo Is Nothing Then
					_todo = JsonClassHelper.ReadStronglyTypedObject(Of Todo)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "todo"))
				End If
				Return _todo
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _done As Done
		Public ReadOnly Property Done() As Done
			Get
				If _done Is Nothing Then
					_done = JsonClassHelper.ReadStronglyTypedObject(Of Done)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "done"))
				End If
				Return _done
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _user As User2
		Public ReadOnly Property User() As User2
			Get
				If _user Is Nothing Then
					_user = JsonClassHelper.ReadStronglyTypedObject(Of User2)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "user"))
				End If
				Return _user
			End Get
		End Property

		Public ReadOnly Property Url() As String
			Get
				Return JsonClassHelper.ReadString(JsonClassHelper.GetJToken(Of JValue)(__jobject, "url"))
			End Get
		End Property

	End Class
End Namespace
