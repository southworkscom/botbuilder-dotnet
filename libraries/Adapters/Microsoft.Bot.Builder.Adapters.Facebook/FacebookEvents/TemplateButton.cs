using System;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    public class TemplateButton
    {
        /// <summary>
        /// Gets or sets the type of button.
        /// </summary>
        /// <value>The type of the button.</value>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the url of the button.
        /// </summary>
        /// <value>Url of the button.</value>
        [JsonProperty(PropertyName = "url")]
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets the title of the button.
        /// </summary>
        /// <value>The title of the button.</value>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
