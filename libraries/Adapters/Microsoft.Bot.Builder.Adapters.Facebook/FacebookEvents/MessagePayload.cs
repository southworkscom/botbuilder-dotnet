// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
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
        public long? StickerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attachment is reusable or not. Default false.
        /// </summary>
        /// <value>Indicates the reusable condition.</value>
        [JsonProperty(PropertyName = "is_reusable")]
        public bool IsReusable { get; set; }

        /// <summary>
        /// Gets or sets the Id of the attachment (for reusable attachments).
        /// </summary>
        /// <value>The id of the saved attachment.</value>
        [JsonProperty(PropertyName = "attachment_id")]
        public string AttachmentId { get; set; }

        /// <summary>
        /// Gets or sets the type of the template attached.
        /// </summary>
        /// <value>The type of template attached.</value>
        [JsonProperty(PropertyName = "template_type")]
        public string TemplateType { get; set; }

        /// <summary>
        /// Gets or sets the text of the template.
        /// </summary>
        /// <value>The text of the template.</value>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a list of buttons of the template.
        /// </summary>
        /// <value>The list of buttons of the template.</value>
        [JsonProperty(PropertyName = "buttons")]
        public List<TemplateButton> Buttons { get; set; }

        public bool ShouldSerializeIsReusable()
        {
            return IsReusable;
        }
    }
}
