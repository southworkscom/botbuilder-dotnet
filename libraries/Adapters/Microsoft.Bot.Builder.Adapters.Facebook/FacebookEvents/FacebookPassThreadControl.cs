using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    public class FacebookPassThreadControl
    {
        /// <summary>
        /// Gets or Sets the app id of the new owner.
        /// </summary>
        /// <remarks>
        /// 263902037430900 for the page inbox.
        /// </remarks>
        /// <value>
        /// The app id of the new owner.
        /// </value>
        [JsonProperty("new_owner_app_id")]
        public string NewOwnerAppId { get; set; }

        [JsonProperty("previous_owner_app_id")]
        public string PreviousOwnerAppId { get; set; }

        [JsonProperty("requested_owner_app_id")]
        public string RequestedOwnerAppId { get; set; }

        /// <summary>
        /// Gets or Sets the message sent from the requester.
        /// </summary>
        /// <remarks>
        /// Example: "i want the control!".
        /// </remarks>
        /// <value>
        /// The message sent from the requester.
        /// </value>
        [JsonProperty("metadata")]
        public string Metadata { get; set; }
    }
}
