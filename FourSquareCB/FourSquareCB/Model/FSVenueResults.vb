Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Runtime.Serialization
Imports Newtonsoft.Json

<Serializable()> _
<DataContract()> _
<DebuggerDisplay("")> _
<JsonObject(MemberSerialization.OptIn)> _
Public Class FSVenueResults

    <DataMember()> _
    <JsonProperty("venue")> _
    Public Property Venue

    Public Overridable Property Venues() As IEnumerable(Of FSVenues)
        Get
            Return m_Statuses
        End Get
        Set(ByVal value As IEnumerable(Of FSVenues))
            m_Statuses = value
        End Set
    End Property
    Private m_Statuses As IEnumerable(Of FSVenues)


End Class
