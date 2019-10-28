using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    public class FacebookResponseThreadControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether the request sended to the Facebook Api is success.
        /// </summary>
        /// <value>
        /// <value>A value indicating whether the request sended to the Facebook Api is success.</value>
        /// </value>
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
    }
}
