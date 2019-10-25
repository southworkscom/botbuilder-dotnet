using System.Collections.Generic;
using Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    public class FacebookDetectedLocales
    {
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }

        [JsonProperty(PropertyName = "confidence")]
        public decimal Confidence { get; set; }
    }
}
