// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    public class MessagePayload
    {
        /// <summary>
        /// Gets or sets the url of the attachment.
        /// </summary>
        /// <value>Url of the attachment.</value>
        [JsonProperty(PropertyName = "url")]
        public Uri Url { get; set; }
    }
}
