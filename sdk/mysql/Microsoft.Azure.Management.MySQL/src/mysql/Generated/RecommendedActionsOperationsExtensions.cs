// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.MySQL
{
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for RecommendedActionsOperations.
    /// </summary>
    public static partial class RecommendedActionsOperationsExtensions
    {
            /// <summary>
            /// Retrieve recommended actions from the advisor.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group. The name is case insensitive.
            /// </param>
            /// <param name='serverName'>
            /// The name of the server.
            /// </param>
            /// <param name='advisorName'>
            /// The advisor name for recommendation action.
            /// </param>
            /// <param name='recommendedActionName'>
            /// The recommended action name.
            /// </param>
            public static RecommendationAction Get(this IRecommendedActionsOperations operations, string resourceGroupName, string serverName, string advisorName, string recommendedActionName)
            {
                return operations.GetAsync(resourceGroupName, serverName, advisorName, recommendedActionName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieve recommended actions from the advisor.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group. The name is case insensitive.
            /// </param>
            /// <param name='serverName'>
            /// The name of the server.
            /// </param>
            /// <param name='advisorName'>
            /// The advisor name for recommendation action.
            /// </param>
            /// <param name='recommendedActionName'>
            /// The recommended action name.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<RecommendationAction> GetAsync(this IRecommendedActionsOperations operations, string resourceGroupName, string serverName, string advisorName, string recommendedActionName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWithHttpMessagesAsync(resourceGroupName, serverName, advisorName, recommendedActionName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieve recommended actions from the advisor.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group. The name is case insensitive.
            /// </param>
            /// <param name='serverName'>
            /// The name of the server.
            /// </param>
            /// <param name='advisorName'>
            /// The advisor name for recommendation action.
            /// </param>
            /// <param name='sessionId'>
            /// The recommendation action session identifier.
            /// </param>
            public static IPage<RecommendationAction> ListByServer(this IRecommendedActionsOperations operations, string resourceGroupName, string serverName, string advisorName, string sessionId = default(string))
            {
                return operations.ListByServerAsync(resourceGroupName, serverName, advisorName, sessionId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieve recommended actions from the advisor.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group. The name is case insensitive.
            /// </param>
            /// <param name='serverName'>
            /// The name of the server.
            /// </param>
            /// <param name='advisorName'>
            /// The advisor name for recommendation action.
            /// </param>
            /// <param name='sessionId'>
            /// The recommendation action session identifier.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IPage<RecommendationAction>> ListByServerAsync(this IRecommendedActionsOperations operations, string resourceGroupName, string serverName, string advisorName, string sessionId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListByServerWithHttpMessagesAsync(resourceGroupName, serverName, advisorName, sessionId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieve recommended actions from the advisor.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='nextPageLink'>
            /// The NextLink from the previous successful call to List operation.
            /// </param>
            public static IPage<RecommendationAction> ListByServerNext(this IRecommendedActionsOperations operations, string nextPageLink)
            {
                return operations.ListByServerNextAsync(nextPageLink).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieve recommended actions from the advisor.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='nextPageLink'>
            /// The NextLink from the previous successful call to List operation.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IPage<RecommendationAction>> ListByServerNextAsync(this IRecommendedActionsOperations operations, string nextPageLink, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListByServerNextWithHttpMessagesAsync(nextPageLink, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
