' JSON C# Class Generator
' http://at-my-window.blogspot.com/?page=json-class-generator

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Newtonsoft.Json.Linq

Namespace JsonCSharpClassGenerator
	Friend NotInheritable Class JsonClassHelper
		Private Sub New()
		End Sub

		Public Shared Function GetJToken(Of T As JToken)(obj As JObject, field As String) As T
			Dim value As JToken
			If obj.TryGetValue(field, value) Then
				Return GetJToken(Of T)(value)
			Else
				Return Nothing
			End If
		End Function

		Private Shared Function GetJToken(Of T As JToken)(token As JToken) As T
			If token Is Nothing Then
				Return Nothing
			End If
			If token.Type = JTokenType.Null Then
				Return Nothing
			End If
			If token.Type = JTokenType.Undefined Then
				Return Nothing
			End If
			Return DirectCast(token, T)
		End Function

		Public Shared Function ReadString(token As JToken) As String
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Return Nothing
			End If
			Return DirectCast(value.Value, String)
		End Function


		Public Shared Function ReadBoolean(token As JToken) As Boolean
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Throw New Newtonsoft.Json.JsonSerializationException()
			End If
			Return Convert.ToBoolean(value.Value)

		End Function

		Public Shared Function ReadNullableBoolean(token As JToken) As System.Nullable(Of Boolean)
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Return Nothing
			End If
			Return Convert.ToBoolean(value.Value)
		End Function


		Public Shared Function ReadInteger(token As JToken) As Integer
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Throw New Newtonsoft.Json.JsonSerializationException()
			End If
			Return Convert.ToInt32(CLng(value.Value))

		End Function

		Public Shared Function ReadNullableInteger(token As JToken) As System.Nullable(Of Integer)
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Return Nothing
			End If
			Return Convert.ToInt32(CLng(value.Value))

		End Function



		Public Shared Function ReadLong(token As JToken) As Long
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Throw New Newtonsoft.Json.JsonSerializationException()
			End If
			Return Convert.ToInt64(value.Value)

		End Function

		Public Shared Function ReadNullableLong(token As JToken) As System.Nullable(Of Long)
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Return Nothing
			End If
			Return Convert.ToInt64(value.Value)
		End Function


		Public Shared Function ReadFloat(token As JToken) As Double
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Throw New Newtonsoft.Json.JsonSerializationException()
			End If
			Return Convert.ToDouble(value.Value)

		End Function

		Public Shared Function ReadNullableFloat(token As JToken) As System.Nullable(Of Double)
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Return Nothing
			End If
			Return Convert.ToDouble(value.Value)

		End Function




		Public Shared Function ReadDate(token As JToken) As DateTime
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Throw New Newtonsoft.Json.JsonSerializationException()
			End If
			Return Convert.ToDateTime(value.Value)

		End Function

		Public Shared Function ReadNullableDate(token As JToken) As System.Nullable(Of DateTime)
			Dim value = GetJToken(Of JValue)(token)
			If value Is Nothing Then
				Return Nothing
			End If
			Return Convert.ToDateTime(value.Value)

		End Function

		Public Shared Function ReadObject(token As JToken) As Object
			Dim value = GetJToken(Of JToken)(token)
			If value Is Nothing Then
				Return Nothing
			End If
			If value.Type = JTokenType.[Object] Then
				Return value
			End If
			If value.Type = JTokenType.Array Then
				Return ReadArray(Of Object)(value, AddressOf ReadObject)
			End If

			Dim jvalue = TryCast(value, JValue)
			If jvalue IsNot Nothing Then
				Return jvalue.Value
			End If

			Return value
		End Function

		Public Shared Function ReadStronglyTypedObject(Of T As Class)(token As JToken) As T
			Dim value = GetJToken(Of JObject)(token)
			If value Is Nothing Then
				Return Nothing
			End If
			Return DirectCast(Activator.CreateInstance(GetType(T), New Object() {token}), T)

		End Function


		Public Delegate Function ValueReader(Of T)(token As JToken) As T



		Public Shared Function ReadArray(Of T)(token As JToken, reader As ValueReader(Of T)) As T()
			Dim value = GetJToken(Of JArray)(token)
			If value Is Nothing Then
				Return Nothing
			End If

			Dim array = New T(value.Count - 1) {}
			For i As Integer = 0 To array.Length - 1
				array(i) = reader(value(i))
			Next
			Return array

		End Function



		Public Shared Function ReadDictionary(Of T)(token As JToken) As Dictionary(Of String, T)
			Dim value = GetJToken(Of JObject)(token)
			If value Is Nothing Then
				Return Nothing
			End If

			Dim dict = New Dictionary(Of String, T)()

			Return dict
		End Function

		Public Shared Function ReadArray(Of K)(jArray As JArray, reader As ValueReader(Of K), type As Type) As Array
			If jArray Is Nothing Then
				Return Nothing
			End If

			Dim elemType = type.GetElementType()

			Dim array__1 = Array.CreateInstance(elemType, jArray.Count)
			For i As Integer = 0 To array__1.Length - 1
				If elemType.IsArray Then
					array__1.SetValue(ReadArray(Of K)(GetJToken(Of JArray)(jArray(i)), reader, elemType), i)
				Else
					array__1.SetValue(reader(jArray(i)), i)

				End If
			Next
			Return array__1

		End Function
	End Class
End Namespace
