using System.Collections.Generic;
using Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    public class FacebookEntities
    {
        [JsonProperty(PropertyName = "sentiment")]
        public List<FacebookSentiments> Sentiment { get; set; }

        [JsonProperty(PropertyName = "greetings")]
        public List<FacebookGreetings> Greetings { get; set; }
    }
}
