// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotKit.Conversation
{
    /// <summary>
    /// Template for defining a BotkitConversation.
    /// </summary>
    public interface IBotkitMessageTemplate
    {
        /// <summary>
        /// Gets or Sets the Text.
        /// </summary>
        string[] Text { get; set; }

        /// <summary>
        /// Gets or Sets the Action.
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// Gets or sets the Execute.
        /// </summary>
        Execute Execute { get; set; }

        /// <summary>
        /// Gets or Sets the QuickReplies array.
        /// </summary>
        object[] QuickReplies { get; set; } // TO-DO: Validate this line

        /// <summary>
        /// Gets or Sets the Attachments array.
        /// </summary>
        object[] Attachments { get; set; }

        /// <summary>
        /// Gets or Sets the ChannelData.
        /// </summary>
        object ChannelData { get; set; }

        /// <summary>
        /// Gets or Sets the Collect object.
        /// </summary>
        Collect Collect { get; set; }
    }
}
