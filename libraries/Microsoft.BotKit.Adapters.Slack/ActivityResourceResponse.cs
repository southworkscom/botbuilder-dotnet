using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Bot.Schema;

namespace Microsoft.BotKit.Adapters.Slack
{
    /// <summary>
    /// Extends ResourceResponse with ActivityID and Conversation properties.
    /// </summary>
    public class ActivityResourceResponse : ResourceResponse
    {
        /// <summary>
        /// Gets or sets the Activity ID.
        /// </summary>
        public string ActivityID { get; set; }

        /// <summary>
        /// Gets or sets a Conversation Account.
        /// </summary>
        public ConversationAccount Conversation { get; set; }
    }
}
