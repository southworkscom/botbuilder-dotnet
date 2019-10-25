using System.Collections.Generic;
using Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    public class FacebookGreetings
    {
        [JsonProperty(PropertyName = "confidence")]
        public decimal Confidence { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
