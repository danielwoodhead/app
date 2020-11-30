using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;

namespace MyHealth.Integrations.Tests.Mocks
{
    public class MockFhirClient : IFhirClient
    {
        public bool PreferCompressedResponses { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CompressRequestBody { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Uri Endpoint => throw new NotImplementedException();

        public ParserSettings ParserSettings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ResourceFormat PreferredFormat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ReturnFullResource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Timeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool UseFormatParam { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool VerifyFhirVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] LastBody => throw new NotImplementedException();

        public Hl7.Fhir.Model.Resource LastBodyAsResource => throw new NotImplementedException();

        public string LastBodyAsText => throw new NotImplementedException();

        public Hl7.Fhir.Model.Bundle.ResponseComponent LastResult => throw new NotImplementedException();

        public HttpWebRequest LastRequest => throw new NotImplementedException();

        public HttpWebResponse LastResponse => throw new NotImplementedException();

        public event EventHandler<AfterResponseEventArgs> OnAfterResponse
        {
            add { }
            remove { }
        }

        public event EventHandler<BeforeRequestEventArgs> OnBeforeRequest
        {
            add { }
            remove { }
        }

        public Hl7.Fhir.Model.CapabilityStatement CapabilityStatement(SummaryType? summary = null) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.CapabilityStatement> CapabilityStatementAsync(SummaryType? summary = null) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle Continue(Hl7.Fhir.Model.Bundle current, PageDirection direction = PageDirection.Next) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> ContinueAsync(Hl7.Fhir.Model.Bundle current, PageDirection direction = PageDirection.Next) => throw new NotImplementedException();
        public TResource Create<TResource>(TResource resource) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public TResource Create<TResource>(TResource resource, SearchParams condition) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Task<TResource> CreateAsync<TResource>(TResource resource) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Task<TResource> CreateAsync<TResource>(TResource resource, SearchParams condition) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public void Delete(Hl7.Fhir.Model.Resource resource) => throw new NotImplementedException();
        public void Delete(string location) => throw new NotImplementedException();
        public void Delete(string resourceType, SearchParams condition) => throw new NotImplementedException();
        public void Delete(Uri location) => throw new NotImplementedException();
        public Task DeleteAsync(Hl7.Fhir.Model.Resource resource) => throw new NotImplementedException();
        public Task DeleteAsync(string location) => throw new NotImplementedException();
        public Task DeleteAsync(string resourceType, SearchParams condition) => throw new NotImplementedException();
        public Task DeleteAsync(Uri location) => throw new NotImplementedException();
        public Task<TResource> executeAsync<TResource>(Hl7.Fhir.Model.Bundle tx, HttpStatusCode expect) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Hl7.Fhir.Model.Resource Get(string url) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Resource Get(Uri url) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Resource> GetAsync(string url) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Resource> GetAsync(Uri url) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle History(string location, DateTimeOffset? since = null, int? pageSize = null, SummaryType summary = SummaryType.False) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle History(Uri location, DateTimeOffset? since = null, int? pageSize = null, SummaryType summary = SummaryType.False) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> HistoryAsync(string location, DateTimeOffset? since = null, int? pageSize = null, SummaryType summary = SummaryType.False) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> HistoryAsync(Uri location, DateTimeOffset? since = null, int? pageSize = null, SummaryType summary = SummaryType.False) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Resource InstanceOperation(Uri location, string operationName, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Resource> InstanceOperationAsync(Uri location, string operationName, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Resource Operation(Uri operation, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Resource Operation(Uri location, string operationName, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Resource> OperationAsync(Uri operation, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Resource> OperationAsync(Uri location, string operationName, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) => throw new NotImplementedException();
        public TResource Read<TResource>(string location, string ifNoneMatch = null, DateTimeOffset? ifModifiedSince = null) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public TResource Read<TResource>(Uri location, string ifNoneMatch = null, DateTimeOffset? ifModifiedSince = null) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Task<TResource> ReadAsync<TResource>(string location, string ifNoneMatch = null, DateTimeOffset? ifModifiedSince = null) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Task<TResource> ReadAsync<TResource>(Uri location, string ifNoneMatch = null, DateTimeOffset? ifModifiedSince = null) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public TResource Refresh<TResource>(TResource current) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Task<TResource> RefreshAsync<TResource>(TResource current) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle Search(SearchParams q, string resourceType = null) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle Search(string resource, string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle Search<TResource>(string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) where TResource : Hl7.Fhir.Model.Resource, new() => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle Search<TResource>(SearchParams q) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchAsync(SearchParams q, string resourceType = null) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchAsync(string resource, string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchAsync<TResource>(string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) where TResource : Hl7.Fhir.Model.Resource, new() => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchAsync<TResource>(SearchParams q) where TResource : Hl7.Fhir.Model.Resource => Task.FromResult(new Hl7.Fhir.Model.Bundle { Entry = new List<Hl7.Fhir.Model.Bundle.EntryComponent>() });
        public Hl7.Fhir.Model.Bundle SearchById(string resource, string id, string[] includes = null, int? pageSize = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle SearchById<TResource>(string id, string[] includes = null, int? pageSize = null, string[] revIncludes = null) where TResource : Hl7.Fhir.Model.Resource, new() => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchByIdAsync(string resource, string id, string[] includes = null, int? pageSize = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchByIdAsync<TResource>(string id, string[] includes = null, int? pageSize = null, string[] revIncludes = null) where TResource : Hl7.Fhir.Model.Resource, new() => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle SearchByIdUsingPost(string resource, string id, string[] includes = null, int? pageSize = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle SearchByIdUsingPost<TResource>(string id, string[] includes = null, int? pageSize = null, string[] revIncludes = null) where TResource : Hl7.Fhir.Model.Resource, new() => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchByIdUsingPostAsync(string resource, string id, string[] includes = null, int? pageSize = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchByIdUsingPostAsync<TResource>(string id, string[] includes = null, int? pageSize = null, string[] revIncludes = null) where TResource : Hl7.Fhir.Model.Resource, new() => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle SearchUsingPost(SearchParams q, string resourceType = null) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle SearchUsingPost<TResource>(SearchParams q) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle SearchUsingPost<TResource>(string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) where TResource : Hl7.Fhir.Model.Resource, new() => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle SearchUsingPost(string resource, string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchUsingPostAsync(SearchParams q, string resourceType = null) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchUsingPostAsync<TResource>(SearchParams q) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchUsingPostAsync<TResource>(string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) where TResource : Hl7.Fhir.Model.Resource, new() => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> SearchUsingPostAsync(string resource, string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle Transaction(Hl7.Fhir.Model.Bundle bundle) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> TransactionAsync(Hl7.Fhir.Model.Bundle bundle) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle TypeHistory(string resourceType, DateTimeOffset? since = null, int? pageSize = null, SummaryType summary = SummaryType.False) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle TypeHistory<TResource>(DateTimeOffset? since = null, int? pageSize = null, SummaryType summary = SummaryType.False) where TResource : Hl7.Fhir.Model.Resource, new() => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> TypeHistoryAsync(string resourceType, DateTimeOffset? since = null, int? pageSize = null, SummaryType summary = SummaryType.False) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> TypeHistoryAsync<TResource>(DateTimeOffset? since = null, int? pageSize = null, SummaryType summary = SummaryType.False) where TResource : Hl7.Fhir.Model.Resource, new() => throw new NotImplementedException();
        public Hl7.Fhir.Model.Resource TypeOperation(string operationName, string typeName, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Resource TypeOperation<TResource>(string operationName, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Resource> TypeOperationAsync(string operationName, string typeName, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Resource> TypeOperationAsync<TResource>(string operationName, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public TResource Update<TResource>(TResource resource, bool versionAware = false) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public TResource Update<TResource>(TResource resource, SearchParams condition, bool versionAware = false) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Task<TResource> UpdateAsync<TResource>(TResource resource, bool versionAware = false) where TResource : Hl7.Fhir.Model.Resource => Task.FromResult((TResource)null);
        public Task<TResource> UpdateAsync<TResource>(TResource resource, SearchParams condition, bool versionAware = false) where TResource : Hl7.Fhir.Model.Resource => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle WholeSystemHistory(DateTimeOffset? since = null, int? pageSize = null, SummaryType summary = SummaryType.False) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> WholeSystemHistoryAsync(DateTimeOffset? since = null, int? pageSize = null, SummaryType summary = SummaryType.False) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Resource WholeSystemOperation(string operationName, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Resource> WholeSystemOperationAsync(string operationName, Hl7.Fhir.Model.Parameters parameters = null, bool useGet = false) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle WholeSystemSearch(string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> WholeSystemSearchAsync(string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Hl7.Fhir.Model.Bundle WholeSystemSearchUsingPost(string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) => throw new NotImplementedException();
        public Task<Hl7.Fhir.Model.Bundle> WholeSystemSearchUsingPostAsync(string[] criteria = null, string[] includes = null, int? pageSize = null, SummaryType? summary = null, string[] revIncludes = null) => throw new NotImplementedException();
    }
}
