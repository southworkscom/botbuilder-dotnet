// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Bot.Builder.Adapters.Facebook
{
    public class FacebookAPI
    {
        private string token;
        private string secret;
        private string apiHost;
        private string apiVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAPI"/> class.
        /// </summary>
        /// <param name="token">A page access token.</param>
        /// <param name="secret">An app secret.</param>
        /// <param name="apiHost">Optional root hostname for constructing api calls, defaults to graph.facebook.com.</param>
        /// <param name="apiVersion">Optional api version used when constructing api calls, defaults to v3.2.</param>
        public FacebookAPI(string token, string secret, string apiHost = "graph.facebook.com", string apiVersion = "v3.2")
        {
        }

        /// <summary>
        /// Call one of the Facebook APIs.
        /// </summary>
        /// <param name="path">Path to the API endpoint, for example `/me/messages`.</param>
        /// <param name="payload">An object to be sent as parameters to the API call..</param>
        /// <param name="method">HTTP method, for example POST, GET, DELETE or PUT.</param>
        /// <returns>A task representing the async operation.</returns>
        public async Task CallAPIAsync(string path, string payload, string method = "POST")
        {
            await Task.FromException(new NotImplementedException());
        }

        /// <summary>
        /// Generate the app secret proof used to increase security on calls to the graph API.
        /// </summary>
        /// <param name="accessToken">A page access token.</param>
        /// <param name="appSecret">An app secret.</param>
        /// <returns>The app secret proof.</returns>
        private string GetAppSecretProof(string accessToken, string appSecret)
        {
            return string.Empty;
        }
    }
}
