using Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook
{
    /// <summary>
    /// Simple version of the payload received from the Facebook channel.
    /// </summary>
    public class FacebookPayload
    {
        /// <summary>
        /// Gets or sets the sender of the message.
        /// </summary>
        /// <value>
        /// The sender of the message.
        /// </value>
        [JsonProperty("sender")]
        public FacebookRequestThreadControl Sender { get; set; }

        /// <summary>
        /// Gets or sets the recipient of the message.
        /// </summary>
        /// <value>
        /// The recipient of the message.
        /// </value>
        [JsonProperty("recipient")]
        public FacebookRequestThreadControl Recipient { get; set; }

        /// <summary>
        /// Gets or sets the request_thread_control of the control request.
        /// </summary>
        /// <value>
        /// The request_thread_control of the control request.
        /// </value>
        [JsonProperty("request_thread_control")]
        public FacebookRequestThreadControl RequestThreadControl { get; set; }

        /// <summary>
        /// Gets or sets the pass_thread_control of the control request.
        /// </summary>
        /// <value>
        /// The pass_thread_control of the control request.
        /// </value>
        [JsonProperty("pass_thread_control")]
        public FacebookPassThreadControl PassThreadControl { get; set; }

        /// <summary>
        /// Gets or sets the take_thread_control of the control request.
        /// </summary>
        /// <value>
        /// The take_thread_control of the control request.
        /// </value>
        [JsonProperty("take_thread_control")]
        public FacebookTakeThreadControl TakeThreadControl { get; set; }
    }
}
