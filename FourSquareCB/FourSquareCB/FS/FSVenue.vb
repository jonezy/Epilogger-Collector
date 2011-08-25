' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class FSVenue

		Public Sub New(json As String)
			Me.New(JObject.Parse(json))
		End Sub

		Private __jobject As JObject
		Public Sub New(obj As JObject)
			Me.__jobject = obj
		End Sub

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _meta As Meta
		Public ReadOnly Property Meta() As Meta
			Get
				If _meta Is Nothing Then
					_meta = JsonClassHelper.ReadStronglyTypedObject(Of Meta)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "meta"))
				End If
				Return _meta
			End Get
		End Property

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _response As Response
		Public ReadOnly Property Response() As Response
			Get
				If _response Is Nothing Then
					_response = JsonClassHelper.ReadStronglyTypedObject(Of Response)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "response"))
				End If
				Return _response
			End Get
		End Property

	End Class
End Namespace
