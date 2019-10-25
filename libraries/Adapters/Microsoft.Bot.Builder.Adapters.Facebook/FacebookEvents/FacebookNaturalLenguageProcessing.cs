using System.Collections.Generic;
using Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents
{
    public class FacebookNaturalLenguageProcessing
    {
        [JsonProperty(PropertyName = "entities")]
        public FacebookEntities Entities { get; set; }

        [JsonProperty(PropertyName = "detected_locales")]
        public List<FacebookDetectedLocales> DetectedLocales { get; set; }
    }
}
