// Copyright (c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Microsoft.Bot.Builder.Adapters.Twilio
{
    public class TwilioAPI : ITwilioApi
    {
        public void LogIn(string username, string password)
        {
            TwilioClient.Init(username, password);
        }

        public async Task<object> CreateMessageResourceAsync(object messageOptions)
        {
            return await MessageResource.CreateAsync((CreateMessageOptions)messageOptions).ConfigureAwait(false);
        }
    }
}
