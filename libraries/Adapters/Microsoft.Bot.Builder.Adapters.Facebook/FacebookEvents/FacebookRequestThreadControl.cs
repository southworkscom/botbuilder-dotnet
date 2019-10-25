using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    /// <summary>
    /// A Facebook thread control message, including appid of requested thread owner and an optional message to send with the request
    /// <see cref="Metadata"/>.
    /// </summary>
    public class FacebookRequestThreadControl
    {
        /// <summary>
        /// Gets or Sets the app id of the requested owner.
        /// </summary>
        /// <remarks>
        /// 263902037430900 for the page inbox.
        /// </remarks>
        /// <value>
        /// the app id of the requested owner.
        /// </value>
        [JsonProperty("requested_owner_app_id")]
        public string RequestedOwnerAppId { get; set; } // 263902037430900 for page

        /// <summary>
        /// Gets or Sets the message sent from the requester.
        /// </summary>
        /// <remarks>
        /// Example: "i want the control!".
        /// </remarks>
        /// <value>
        /// the message sent from the requester.
        /// </value>
        [JsonProperty("metadata")]
        public string Metadata { get; set; }
    }
}
