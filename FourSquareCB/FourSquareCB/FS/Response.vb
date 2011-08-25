' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports Newtonsoft.Json.Linq
Imports FourSquareCB.JsonCSharpClassGenerator

Namespace FourSquareCB

	Friend Class Response

		Private __jobject As JObject
		Public Sub New(obj As JObject)
			Me.__jobject = obj
		End Sub

		<System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)> _
		Private _venue As Venue
		Public ReadOnly Property Venue() As Venue
			Get
				If _venue Is Nothing Then
					_venue = JsonClassHelper.ReadStronglyTypedObject(Of Venue)(JsonClassHelper.GetJToken(Of JObject)(__jobject, "venue"))
				End If
				Return _venue
			End Get
		End Property

	End Class
End Namespace
