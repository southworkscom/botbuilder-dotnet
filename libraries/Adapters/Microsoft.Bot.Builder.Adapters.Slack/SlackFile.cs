using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Slack
{
    public class SlackFile
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "thumb_480")]

        public string Thumb480 { get; set; }
        
        [JsonProperty(PropertyName = "idthumb_480_w")]
        public int Thumb480W { get; set; }
        
        [JsonProperty(PropertyName = "thumb_480_h")]
        public int Thumb480H { get; set; }
        
        [JsonProperty(PropertyName = "permalink")]
        public string Permalink { get; set; }
        
        [JsonProperty(PropertyName = "permalink_public")]
        public string PermalinkPublic { get; set; }
        
        [JsonProperty(PropertyName = "edit_link")]
        public string EditLink { get; set; }
        
        [JsonProperty(PropertyName = "preview")]
        public string Preview { get; set; }
        
        [JsonProperty(PropertyName = "preview_highlight")]
        public string PreviewHighlight { get; set; }
        
        [JsonProperty(PropertyName = "lines")]
        public int Lines { get; set; }
        
        [JsonProperty(PropertyName = "lines_more")]
        public int LinesMore { get; set; }
        
        [JsonProperty(PropertyName = "is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty(PropertyName = "public_url_shared")]
        public bool PublicUrlShared { get; set; }

        [JsonProperty(PropertyName = "display_as_bot")]
        public bool DisplayAsBot { get; set; }

        [JsonProperty(PropertyName = "channels")]
        public string[] Channels { get; set; }
        
        [JsonProperty(PropertyName = "groups")]
        public string[] Groups { get; set; }
        
        [JsonProperty(PropertyName = "ims")]
        public string[] Ims { get; set; }

        [JsonProperty(PropertyName = "initial_comment")]
        public SlackFileComment InitialComment { get; set; }
      
        [JsonProperty(PropertyName = "comments_count")]
        public int CommentsCount { get; set; }
        
        [JsonProperty(PropertyName = "num_stars")]
        public int NumStars { get; set; }
        
        [JsonProperty(PropertyName = "is_starred")]
        public bool IsStarred { get; set; }
        
        [JsonProperty(PropertyName = "pinned_to")]
        public string[] PinnedTo { get; set; }
        
        [JsonProperty(PropertyName = "thumb_360_h")]
        public int Thumb360H { get; set; }
        
        [JsonProperty(PropertyName = "reactions")]
        public SlackReaction[] Reactions { get; set; }
        
        [JsonProperty(PropertyName = "thumb_360_w")]
        public int Thumb360W { get; set; }
        
        [JsonProperty(PropertyName = "thumb_360")]
        public string Thumb360 { get; set; }
        
        [JsonProperty(PropertyName = "created")]
        public int Created { get; set; }

        [JsonProperty(PropertyName = "timestamp")] 
        public int Timestamp { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        
        [JsonProperty(PropertyName = "mimetype")]
        public string Mimetype { get; set; }
        
        [JsonProperty(PropertyName = "filetype")]
        public string Filetype { get; set; }
        
        [JsonProperty(PropertyName = "pretty_type")]
        public string PrettyType { get; set; }
        
        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }
        
        [JsonProperty(PropertyName = "mode")]
        public string Mode { get; set; }
        
        [JsonProperty(PropertyName = "editable")]
        public bool Editable { get; set; }
        
        [JsonProperty(PropertyName = "is_external")]
        public bool IsExternal { get; set; }
        
        [JsonProperty(PropertyName = "external_type")]
        public string ExternalType { get; set; }
        
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        
        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }
        
        [JsonProperty(PropertyName = "url")]
        public System.Uri Url { get; set; }
        
        [JsonProperty(PropertyName = "url_download")]
        public System.Uri UrlDownload { get; set; }
        
        [JsonProperty(PropertyName = "url_private")]
        public System.Uri UrlPrivate { get; set; }
        
        [JsonProperty(PropertyName = "url_private_download")]
        public System.Uri UrlPrivateDownload { get; set; }
        
        [JsonProperty(PropertyName = "thumb_64")]
        public string Thumb64 { get; set; }
        
        [JsonProperty(PropertyName = "thumb_80")]
        public string Thumb80 { get; set; }
        
        [JsonProperty(PropertyName = "thumb_160")]
        public string Thumb160 { get; set; }
        
        [JsonProperty(PropertyName = "thumb_360_gif")]
        public string Thumb360Gif { get; set; }
    }
}
