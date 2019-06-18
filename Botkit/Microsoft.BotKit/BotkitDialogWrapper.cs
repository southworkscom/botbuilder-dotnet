// <copyright file="BotkitDialogWrapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.BotKit
{
    using System;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.BotKit.Conversation;

    /// <summary>
    ///  BotkitDialogWrapper class.
    /// </summary>
    public class BotkitDialogWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BotkitDialogWrapper"/> class.
        /// </summary>
        /// <param name="dialogContext">dialogContext for the BotkitDialogWrapper.</param>
        /// <param name="botkitconvoStep">botkitconvoStep for the BotkitDialogWrapper.</param>
        public BotkitDialogWrapper(DialogContext dialogContext, IBotkitConversationStep botkitconvoStep)
        {
        }

        /// <summary>
        /// Gets or sets an object containing variables and user responses from this conversation.
        /// </summary>
        public Tuple<string, object> Vars { get; set; }

        /// <summary>
        /// Jump immediately to the first message in a different thread.
        /// </summary>
        /// <param name="thread">Name of a thread.</param>
        public void GotoThread(string thread)
        {
        }

        /// <summary>
        ///  Repeat the last message sent on the next turn.
        /// </summary>
        public void Repeat()
        {
        }

        /// <summary>
        /// Set the value of a variable that will be available to messages in the conversation.
        /// Equivalent to convo.vars.key = val;
        /// Results in {{vars.key}} being replaced with the value in val.
        /// </summary>
        /// <param name="key">The name of the variable.</param>
        /// <param name="val">The value for the variable.</param>
        public void SetVar(object key, object val)
        {
        }
    }
}
