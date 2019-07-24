using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Bot.Builder.Adapters.Facebook
{
    public class Message
    {
        public string Text { get; set; }

        public string StickerId { get; set; }

        public object Attachment { get; set; }

        public List<object> QuickReplies { get; set; }
    }
}
