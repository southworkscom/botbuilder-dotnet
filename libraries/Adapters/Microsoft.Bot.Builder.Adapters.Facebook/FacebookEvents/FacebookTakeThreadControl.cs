using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    public class FacebookTakeThreadControl
    {
        /// <summary>
        /// Gets or Sets the app id of the previous owner.
        /// </summary>
        /// <remarks>
        /// 263902037430900 for the page inbox.
        /// </remarks>
        /// <value>
        /// The app id of the previous owner.
        /// </value>
        [JsonProperty("previous_owner_app_id")]
        public string PreviousOwnerAppId { get; set; }

        /// <summary>
        /// Gets or Sets the message sent from the requester.
        /// </summary>
        /// <remarks>
        /// Example: "All yours!".
        /// </remarks>
        /// <value>
        /// Message sent from the requester.
        /// </value>
        [JsonProperty("metadata")]
        public string Metadata { get; set; }
    }
}
