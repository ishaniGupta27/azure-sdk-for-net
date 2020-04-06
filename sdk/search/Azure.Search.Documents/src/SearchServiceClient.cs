﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Search.Documents.Models;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Search.Documents
{
    /// <summary>
    /// Azure Cognitive Search client that can be used to manage and query
    /// indexes and documents, as well as manage other resources, on a Search
    /// Service.
    /// </summary>
    public class SearchServiceClient
    {
        private ServiceRestClient _serviceClient;
        private IndexesRestClient _indexesClient;

        /// <summary>
        /// Gets the URI endpoint of the Search service.  This is likely
        /// to be similar to "https://{search_service}.search.windows.net".
        /// </summary>
        public virtual Uri Endpoint { get; }

        /// <summary>
        /// The name of the Search service, lazily obtained from the
        /// <see cref="Endpoint"/>.
        /// </summary>
        private string _serviceName = null;

        /// <summary>
        /// Gets the name of the Search service.
        /// </summary>
        public virtual string ServiceName =>
            _serviceName ??= Endpoint.GetSearchServiceName();

        /// <summary>
        /// Gets the authenticated <see cref="HttpPipeline"/> used for sending
        /// requests to the Search service.
        /// </summary>
        private HttpPipeline Pipeline { get; }

        /// <summary>
        /// Gets the <see cref="Azure.Core.Pipeline.ClientDiagnostics"/> used
        /// to provide tracing support for the client library.
        /// </summary>
        private ClientDiagnostics ClientDiagnostics { get; }

        /// <summary>
        /// Gets the REST API version of the Search service to use when making
        /// requests.
        /// </summary>
        private SearchClientOptions.ServiceVersion Version { get; }

        /// <summary>
        /// Gets the generated <see cref="ServiceRestClient"/> to make requests.
        /// </summary>
        private ServiceRestClient ServiceClient => LazyInitializer.EnsureInitialized(ref _serviceClient, () => new ServiceRestClient(
            ClientDiagnostics,
            Pipeline,
            Endpoint.ToString(),
            Version.ToVersionString())
        );

        /// <summary>
        /// Gets the generated <see cref="IndexesRestClient"/> to make requests.
        /// </summary>
        private IndexesRestClient IndexesClient => LazyInitializer.EnsureInitialized(ref _indexesClient, () => new IndexesRestClient(
            ClientDiagnostics,
            Pipeline,
            Endpoint.ToString(),
            Version.ToVersionString())
        );

        /// <summary>
        /// Initializes a new instance of the SearchServiceClient class for
        /// mocking.
        /// </summary>
        protected SearchServiceClient() { }

        /// <summary>
        /// Initializes a new instance of the SearchServiceClient class.
        /// </summary>
        /// <param name="endpoint">
        /// Required.  The URI endpoint of the Search service.  This is likely
        /// to be similar to "https://{search_service}.search.windows.net".
        /// The URI must use HTTPS.
        /// </param>
        /// <param name="credential">
        /// Required.  The API key credential used to authenticate requests
        /// against the Search service.  You need to use an admin key to
        /// perform any operations on the SearchServiceClient.  See
        /// <see href="https://docs.microsoft.com/azure/search/search-security-api-keys"/>
        /// for more information about API keys in Azure Cognitive Search.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <paramref name="endpoint"/> or
        /// <paramref name="credential"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the <paramref name="endpoint"/> is not using HTTPS.
        /// </exception>
        public SearchServiceClient(Uri endpoint, AzureKeyCredential credential) :
            this(endpoint, credential, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SearchServiceClient class.
        /// </summary>
        /// <param name="endpoint">
        /// Required.  The URI endpoint of the Search service.  This is likely
        /// to be similar to "https://{search_service}.search.windows.net".
        /// The URI must use HTTPS.
        /// </param>
        /// <param name="credential">
        /// Required.  The API key credential used to authenticate requests
        /// against the Search service.  You need to use an admin key to
        /// perform any operations on the SearchServiceClient.  See
        /// <see href="https://docs.microsoft.com/azure/search/search-security-api-keys"/>
        /// for more information about API keys in Azure Cognitive Search.
        /// </param>
        /// <param name="options">
        /// Client configuration options for connecting to Azure Cognitive
        /// Search.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <paramref name="endpoint"/> or
        /// <paramref name="credential"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the <paramref name="endpoint"/> is not using HTTPS.
        /// </exception>
        public SearchServiceClient(
            Uri endpoint,
            AzureKeyCredential credential,
            SearchClientOptions options)
        {
            Argument.AssertNotNull(endpoint, nameof(endpoint));
            endpoint.AssertHttpsScheme(nameof(endpoint));
            Argument.AssertNotNull(credential, nameof(credential));

            options ??= new SearchClientOptions();
            Endpoint = endpoint;
            ClientDiagnostics = new ClientDiagnostics(options);
            Pipeline = options.Build(credential);
            Version = options.Version;
        }

        /// <summary>
        /// Get a <see cref="SearchIndexClient"/> for the given
        /// <paramref name="indexName"/> to use for document operations like
        /// querying or adding documents to a Search Index.
        /// </summary>
        /// <param name="indexName">
        /// The name of the desired Search Index.
        /// </param>
        /// <returns>
        /// A SearchIndexClient for the desired Search Index.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <paramref name="indexName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the <paramref name="indexName"/> is empty.
        /// </exception>
        /// <remarks>
        /// The same request <see cref="HttpPipeline"/> (including
        /// authentication and any other configuration) will be used for the
        /// SearchIndexClient.
        /// </remarks>
        public virtual SearchIndexClient GetSearchIndexClient(string indexName)
        {
            Argument.AssertNotNullOrEmpty(indexName, nameof(indexName));
            return new SearchIndexClient(
                Endpoint,
                indexName,
                Pipeline,
                ClientDiagnostics,
                Version);
        }

        #region Service operations
        /// <summary>
        /// <para>
        /// Gets service level statistics for a Search service.
        /// </para>
        /// <para>
        /// This operation returns the number and type of objects in your
        /// service, the maximum allowed for each object type given the service
        /// tier, actual and maximum storage, and other limits that vary by
        /// tier.  This request pulls information from the service so that you
        /// don't have to look up or calculate service limits.
        /// </para>
        /// <para>
        /// Statistics on document count and storage size are collected every
        /// few minutes, not in real time.  Therefore, the statistics returned
        /// by this API may not reflect changes caused by recent indexing
        /// operations.
        /// </para>
        /// </summary>
        /// <param name="options">
        /// Optional <see cref="SearchRequestOptions"/> to customize the operation's behavior.
        /// </param>
        /// <param name="cancellationToken">
        /// Optional <see cref="CancellationToken"/> to propagate notifications
        /// that the operation should be canceled.
        /// </param>
        /// <returns>The service level statistics.</returns>
        /// <exception cref="RequestFailedException">
        /// Thrown when a failure is returned by the Search service.
        /// </exception>
        [ForwardsClientCalls]
        public virtual Response<SearchServiceStatistics> GetServiceStatistics(
            SearchRequestOptions options = null,
            CancellationToken cancellationToken = default) =>
            ServiceClient.GetServiceStatistics(
                options?.ClientRequestId,
                cancellationToken);

        /// <summary>
        /// <para>
        /// Gets service level statistics for a Search service.
        /// </para>
        /// <para>
        /// This operation returns the number and type of objects in your
        /// service, the maximum allowed for each object type given the service
        /// tier, actual and maximum storage, and other limits that vary by
        /// tier.  This request pulls information from the service so that you
        /// don't have to look up or calculate service limits.
        /// </para>
        /// <para>
        /// Statistics on document count and storage size are collected every
        /// few minutes, not in real time.  Therefore, the statistics returned
        /// by this API may not reflect changes caused by recent indexing
        /// operations.
        /// </para>
        /// </summary>
        /// <param name="options">
        /// Optional <see cref="SearchRequestOptions"/> to customize the operation's behavior.
        /// </param>
        /// <param name="cancellationToken">
        /// Optional <see cref="CancellationToken"/> to propagate notifications
        /// that the operation should be canceled.
        /// </param>
        /// <returns>The service level statistics.</returns>
        /// <exception cref="RequestFailedException">
        /// Thrown when a failure is returned by the Search service.
        /// </exception>
        [ForwardsClientCalls]
        public virtual async Task<Response<SearchServiceStatistics>> GetServiceStatisticsAsync(
            SearchRequestOptions options = null,
            CancellationToken cancellationToken = default) =>
            await ServiceClient.GetServiceStatisticsAsync(
                options?.ClientRequestId,
                cancellationToken)
                .ConfigureAwait(false);
        #endregion

        #region Index operations
        /// <summary>
        /// Creates a new search index in the Search service.
        /// </summary>
        /// <param name="index">
        /// The <see cref="SearchIndex"/> to create.
        /// </param>
        /// <param name="options">
        /// Optional <see cref="SearchRequestOptions"/> to customize the operation's behavior.
        /// </param>
        /// <param name="cancellationToken">
        /// Optional <see cref="CancellationToken"/> to propagate notifications
        /// that the operation should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="SearchIndex"/> that was created.
        /// This may differ slightly from what was passed in since the service may return back fields set to their default values depending on the field type and other properties.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="index"/> is null.
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown when a failure is returned by the Search service.
        /// </exception>
        [ForwardsClientCalls]
        public virtual Response<SearchIndex> CreateIndex(
            SearchIndex index,
            SearchRequestOptions options = null,
            CancellationToken cancellationToken = default) =>
            IndexesClient.Create(
                index,
                options?.ClientRequestId,
                cancellationToken);

        /// <summary>
        /// Creates a new search index in the Search service.
        /// </summary>
        /// <param name="index">
        /// The <see cref="SearchIndex"/> to create.
        /// </param>
        /// <param name="options">
        /// Optional <see cref="SearchRequestOptions"/> to customize the operation's behavior.
        /// </param>
        /// <param name="cancellationToken">
        /// Optional <see cref="CancellationToken"/> to propagate notifications
        /// that the operation should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="SearchIndex"/> that was created.
        /// This may differ slightly from what was passed in since the service may return back fields set to their default values depending on the field type and other properties.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="index"/> is null.
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown when a failure is returned by the Search service.
        /// </exception>
        [ForwardsClientCalls]
        public virtual async Task<Response<SearchIndex>> CreateIndexAsync(
            SearchIndex index,
            SearchRequestOptions options = null,
            CancellationToken cancellationToken = default) =>
            await IndexesClient.CreateAsync(
                index,
                options?.ClientRequestId,
                cancellationToken)
                .ConfigureAwait(false);

        /// <summary>
        /// Gets an existing index from the Search service.
        /// </summary>
        /// <param name="indexName">
        /// The name of the index.
        /// </param>
        /// <param name="options">
        /// Optional <see cref="SearchRequestOptions"/> to customize the operation's behavior.
        /// </param>
        /// <param name="cancellationToken">
        /// Optional <see cref="CancellationToken"/> to propagate notifications
        /// that the operation should be canceled.
        /// </param>
        /// <returns>
        /// The requested <see cref="SearchIndex"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="indexName"/> is an empty string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indexName"/> is null.
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown when a failure is returned by the Search service.
        /// </exception>
        [ForwardsClientCalls]
        public virtual Response<SearchIndex> GetIndex(
            string indexName,
            SearchRequestOptions options = null,
            CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(indexName, nameof(indexName));

            return IndexesClient.Get(
                indexName,
                options?.ClientRequestId,
                cancellationToken);
        }

        /// <summary>
        /// Gets an existing index from the Search service.
        /// </summary>
        /// <param name="indexName">
        /// The name of the index.
        /// </param>
        /// <param name="options">
        /// Optional <see cref="SearchRequestOptions"/> to customize the operation's behavior.
        /// </param>
        /// <param name="cancellationToken">
        /// Optional <see cref="CancellationToken"/> to propagate notifications
        /// that the operation should be canceled.
        /// </param>
        /// <returns>
        /// The requested <see cref="SearchIndex"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="indexName"/> is an empty string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indexName"/> is null.
        /// </exception>
        /// <exception cref="RequestFailedException">
        /// Thrown when a failure is returned by the Search service.
        /// </exception>
        [ForwardsClientCalls]
        public virtual async Task<Response<SearchIndex>> GetIndexAsync(
            string indexName,
            SearchRequestOptions options = null,
            CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(indexName, nameof(indexName));

            return await IndexesClient.GetAsync(
                indexName,
                options?.ClientRequestId,
                cancellationToken)
                .ConfigureAwait(false);
        }
        #endregion
    }
}