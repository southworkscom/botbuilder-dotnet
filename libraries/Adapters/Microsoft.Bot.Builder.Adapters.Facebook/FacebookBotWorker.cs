// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.BotKit;
using Microsoft.Bot.Builder.BotKit.Core;
using Microsoft.Bot.Schema;

namespace Microsoft.Bot.Builder.Adapters.Facebook
{
    public class FacebookBotWorker : BotWorker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookBotWorker"/> class.
        /// Reserved for use internally by Botkit's `controller.spawn()`, this class is used to create a BotWorker instance that can send messages, replies, and make other API calls.
        /// When used with the FacebookAdapter's multi-tenancy mode, it is possible to spawn a bot instance by passing in the Facebook page ID representing the appropriate bot identity.
        /// Use this in concert with [startConversationWithUser()](#startConversationWithUser) and [changeContext()](core.md#changecontext) to start conversations.
        /// or send proactive alerts to users on a schedule or in response to external events.
        /// </summary>
        /// <param name="botkit">The Botkit controller object responsible for spawning this bot worker.</param>
        /// <param name="config">Normally, a DialogContext object.</param>
        public FacebookBotWorker(Botkit botkit, BotWorkerConfiguration config)
            : base(botkit, config)
        {
        }

        /// <summary>
        /// Gets or sets an instance of the [webex api client].
        /// </summary>
        /// <value>A copy of the FacebookAPI client giving access to `let res = await bot.api.callAPI(path, method, parameters);`.</value>
        public object Api { get; set; }

        /// <summary>
        /// Change the operating context of the worker to begin a conversation with a specific user.
        /// After calling this method, any calls to `bot.say()` or `bot.beginDialog()` will occur in this new context.
        /// This method can be used to send users scheduled messages or messages triggered by external events.
        /// </summary>
        /// <param name="userID">the PSID of a user the bot has previously interacted with.</param>
        /// <returns>A task representing the async operation.</returns>
        public async Task StartConversationWithUserAsync(string userID)
        {
            var userChannelAccount = new ChannelAccount(userID);
            var botChannelAccount = new ChannelAccount(this.Config.Activity.Recipient.ToString());
            var conversation = new ConversationAccount(null, null, userID);
            var conversationReference = new ConversationReference(null, userChannelAccount, botChannelAccount, conversation, "facebook");

            await this.ChangeContextAsync(conversationReference);
        }
    }
}
