// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder.Adapters.Facebook;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Bot.Builder.Facebook.Sample
{
    public class ConfigurationFacebookAdapterOptions : SimpleFacebookAdapterOptions
    {
        public ConfigurationFacebookAdapterOptions(IConfiguration configuration)
            : base(configuration["VerifyToken"], configuration["AppSecret"], configuration["AccessToken"])
        {
        }
    }
}
