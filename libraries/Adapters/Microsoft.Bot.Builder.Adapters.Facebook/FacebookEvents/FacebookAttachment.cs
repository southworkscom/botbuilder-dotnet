// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    [ExcludeFromCodeCoverage]
    public class FacebookAttachment
    {
        /// <summary>
        /// Gets or sets the type of the attachment.
        /// </summary>
        /// <value>Type of attachment, may be image, audio, video, file or template.</value>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the payload of the attachment.
        /// </summary>
        /// <value>Payload of the attachment.</value>
        [JsonProperty(PropertyName = "payload")]
        public AttachmentPayload Payload { get; set; }
    }
}
