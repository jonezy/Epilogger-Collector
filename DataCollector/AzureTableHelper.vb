Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Configuration
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports Microsoft.WindowsAzure.ServiceRuntime
Imports Microsoft.WindowsAzure
Imports Microsoft.WindowsAzure.StorageClient
Imports System.Data.Services.Client

'
'For the purposes of storing tweets for Epilogger, here is what we're going to do;
'- PartitionKey = EventID (From SQL)
'- RowKey = TweetID (From Twitter)



Public Class AzureTableHelper
    Public Account As CloudStorageAccount
    Public TableClient As CloudTableClient

    ' Constructor - get settings from a hosted service configuration or .NET configuration file.

    Public Sub New(ByVal configurationSettingName As String, ByVal hostedService As Boolean)
        If hostedService Then
            CloudStorageAccount.SetConfigurationSettingPublisher(Function(configName, configSettingPublisher)
                                                                     Dim connectionString = RoleEnvironment.GetConfigurationSettingValue(configName)
                                                                     Return configSettingPublisher(connectionString)
                                                                 End Function)
        Else
            'AzureTableStorage
            CloudStorageAccount.SetConfigurationSettingPublisher(Function(configName, configSettingPublisher)
                                                                     'Dim connectionString = ConfigurationManager.ConnectionStrings(configName).ConnectionString
                                                                     Dim connectionString = My.Settings.AzureTableStorageConnectionString
                                                                     Return configSettingPublisher(connectionString)
                                                                 End Function)


        End If

        Account = CloudStorageAccount.FromConfigurationSetting(configurationSettingName)

        TableClient = Account.CreateCloudTableClient()
        TableClient.RetryPolicy = RetryPolicies.Retry(4, TimeSpan.Zero)
    End Sub

    ' Constructor - pass in a storage connection string.

    Public Sub New(ByVal connectionString As String)
        Account = CloudStorageAccount.Parse(connectionString)

        TableClient = Account.CreateCloudTableClient()
        TableClient.RetryPolicy = RetryPolicies.Retry(4, TimeSpan.Zero)
    End Sub


    ' List Tables.
    ' Return true on success, false if not found, throw exception on error.

    Public Function ListTables(ByRef tableList As List(Of String)) As Boolean
        tableList = New List(Of String)()

        Try
            Dim tables As IEnumerable(Of String) = TableClient.ListTables()
            If tables IsNot Nothing Then
                tableList.AddRange(tables)
            End If
            Return True
        Catch ex As StorageClientException
            If CInt(ex.StatusCode) = 404 Then
                Return False
            End If

            Throw
        End Try
    End Function


    ' Create Table.
    ' Return true on success, false if already exists, throw exception on error.

    Public Function CreateTable(ByVal tableName As String) As Boolean
        Try
            TableClient.CreateTable(tableName)
            Return True
        Catch ex As StorageClientException
            If CInt(ex.StatusCode) = 409 Then
                Return False
            End If

            Throw
        End Try
    End Function


    ' Delete Table.
    ' Return true on success, false if not found, throw exception on error.

    Public Function DeleteTable(ByVal tableName As String) As Boolean
        Try
            TableClient.DeleteTable(tableName)
            Return True
        Catch ex As StorageClientException
            If CInt(ex.StatusCode) = 404 Then
                Return False
            End If

            Throw
        End Try
    End Function

    ' Insert entity.
    ' Return true on success, false if not found, throw exception on error.

    Public Function InsertEntity(ByVal tableName As String, ByVal obj As Object) As Boolean
        Try
            Dim tableServiceContext As TableServiceContext = TableClient.GetDataServiceContext()

            tableServiceContext.AddObject(tableName, obj)
            tableServiceContext.SaveChanges()

            Return True
        Catch generatedExceptionName As DataServiceRequestException
            Return False
        Catch ex As StorageClientException
            If CInt(ex.StatusCode) = 404 Then
                Return False
            End If

            Throw
        End Try
    End Function


    ' Retrieve an entity.
    ' Return true on success, false if not found, throw exception on error.

    Public Function GetEntity(Of T As TableServiceEntity)(ByVal tableName As String, ByVal partitionKey As String, ByVal rowKey As String, ByRef entity As T) As Boolean
        entity = Nothing

        Try
            Dim tableServiceContext As TableServiceContext = TableClient.GetDataServiceContext()
            Dim entities As IQueryable(Of T) = (From e In tableServiceContext.CreateQuery(Of T)(tableName) Where e.PartitionKey = partitionKey AndAlso e.RowKey = rowKey)

            entity = entities.FirstOrDefault()

            Return True
        Catch generatedExceptionName As DataServiceRequestException
            Return False
        Catch ex As StorageClientException
            If CInt(ex.StatusCode) = 404 Then
                Return False
            End If

            Throw
        End Try
    End Function


    ' Query entities. Use LINQ clauses to filter data.
    ' Return true on success, false if not found, throw exception on error.

    Public Function QueryEntities(Of T As TableServiceEntity)(ByVal tableName As String) As DataServiceQuery(Of T)
        Dim tableServiceContext As TableServiceContext = TableClient.GetDataServiceContext()
        Return tableServiceContext.CreateQuery(Of T)(tableName)
    End Function


    ' Replace Update entity. Completely replace previous entity with new entity.
    ' Return true on success, false if not found, throw exception on error.

    Public Function ReplaceUpdateEntity(Of T As TableServiceEntity)(ByVal tableName As String, ByVal partitionKey As String, ByVal rowKey As String, ByVal obj As T) As Boolean
        Try
            Dim tableServiceContext As TableServiceContext = TableClient.GetDataServiceContext()
            Dim entities As IQueryable(Of T) = (From e In tableServiceContext.CreateQuery(Of T)(tableName) Where e.PartitionKey = partitionKey AndAlso e.RowKey = rowKey)

            Dim entity As T = entities.FirstOrDefault()

            Dim t1 As Type = obj.[GetType]()
            Dim pi As PropertyInfo() = t1.GetProperties()

            For Each p As PropertyInfo In pi
                p.SetValue(entity, p.GetValue(obj, Nothing), Nothing)
            Next

            tableServiceContext.UpdateObject(entity)
            tableServiceContext.SaveChanges(SaveChangesOptions.ReplaceOnUpdate)

            Return True
        Catch generatedExceptionName As DataServiceRequestException
            Return False
        Catch ex As StorageClientException
            If CInt(ex.StatusCode) = 404 Then
                Return False
            End If

            Throw
        End Try
    End Function


    ' Merge update an entity (preserve previous properties not overwritten).
    ' Return true on success, false if not found, throw exception on error.

    Public Function MergeUpdateEntity(Of T As TableServiceEntity)(ByVal tableName As String, ByVal partitionKey As String, ByVal rowKey As String, ByVal obj As T) As Boolean
        Try
            Dim tableServiceContext As TableServiceContext = TableClient.GetDataServiceContext()
            Dim entities As IQueryable(Of T) = (From e In tableServiceContext.CreateQuery(Of T)(tableName) Where e.PartitionKey = partitionKey AndAlso e.RowKey = rowKey)

            Dim entity As T = entities.FirstOrDefault()

            Dim t1 As Type = obj.[GetType]()
            Dim pi As PropertyInfo() = t1.GetProperties()

            For Each p As PropertyInfo In pi
                p.SetValue(entity, p.GetValue(obj, Nothing), Nothing)
            Next

            tableServiceContext.UpdateObject(entity)
            tableServiceContext.SaveChanges()

            Return True
        Catch generatedExceptionName As DataServiceRequestException
            Return False
        Catch ex As StorageClientException
            If CInt(ex.StatusCode) = 404 Then
                Return False
            End If

            Throw
        End Try
    End Function


    ' Delete entity.
    ' Return true on success, false if not found, throw exception on error.

    Public Function DeleteEntity(Of T As TableServiceEntity)(ByVal tableName As String, ByVal partitionKey As String, ByVal rowKey As String) As Boolean
        Try
            Dim tableServiceContext As TableServiceContext = TableClient.GetDataServiceContext()
            Dim entities As IQueryable(Of T) = (From e In tableServiceContext.CreateQuery(Of T)(tableName) Where e.PartitionKey = partitionKey AndAlso e.RowKey = rowKey)

            Dim entity As T = entities.FirstOrDefault()

            If entities IsNot Nothing Then
                tableServiceContext.DeleteObject(entity)
                tableServiceContext.SaveChanges()
                Return True
            Else
                Return False
            End If
        Catch generatedExceptionName As DataServiceRequestException
            Return False
        Catch ex As StorageClientException
            If CInt(ex.StatusCode) = 404 Then
                Return False
            End If

            Throw
        End Try
    End Function

End Class


