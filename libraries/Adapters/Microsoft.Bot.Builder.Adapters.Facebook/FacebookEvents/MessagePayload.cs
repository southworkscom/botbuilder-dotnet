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

        /// <summary>
        /// Gets or sets the id of the sticker attached.
        /// </summary>
        /// <value>The Id of the sticker.</value>
        [JsonProperty(PropertyName = "sticker_id")]
        public long StickerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attachment is reusable or not. Default false.
        /// </summary>
        /// <value>Indicates the reusable condition.</value>
        [JsonProperty(PropertyName = "is_reusable")]
        public bool IsReusable { get; set; }
    }
}
