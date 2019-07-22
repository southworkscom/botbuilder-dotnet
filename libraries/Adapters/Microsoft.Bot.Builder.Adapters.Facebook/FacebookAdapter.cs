// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder.BotKit;
using Microsoft.Bot.Builder.BotKit.Core;
using Microsoft.Bot.Schema;

namespace Microsoft.Bot.Builder.Adapters.Facebook
{
    public class FacebookAdapter : BotAdapter
    {
        public const string NAME = "Facebook Adapter";

        private readonly IFacebookAdapterOptions options;

        public FacebookAdapter(IFacebookAdapterOptions options)
        {
        }

        public FacebookBotWorker BotkitWorker { get; private set; }

        /// <summary>
        /// Botkit-only: Initialization function called automatically when used with Botkit.
        /// Amends the webhook_uri with an additional behavior for responding to Facebook's webhook verification request.
        /// </summary>
        /// <param name="botkit">A botkit object.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Init(Botkit botkit)
        {
            await Task.FromException(new NotImplementedException());
        }

        /// <summary>
        /// Get a Facebook API client with the correct credentials based on the page identified in the incoming activity.
        /// This is used by many internal functions to get access to the Facebook API, and is exposed as `bot.api` on any BotWorker instances passed into Botkit handler functions.
        /// </summary>
        /// <param name="activity">An incoming message activity.</param>
        /// <returns>A Facebook API client.</returns>
        public async Task<FacebookAPI> GetAPI(Activity activity)
        {
            return await Task.FromException<FacebookAPI>(new NotImplementedException());
        }

        /// <summary>
        /// Standard BotBuilder adapter method to send a message from the bot to the messaging API.
        /// </summary>
        /// <param name="context">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="activities">An array of outgoing activities to be sent back to the messaging API.</param>
        /// <param name="cancellationToken">A cancellation token for the task.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override async Task<ResourceResponse[]> SendActivitiesAsync(ITurnContext context, Activity[] activities, CancellationToken cancellationToken)
        {
            return await Task.FromException<ResourceResponse[]>(new NotImplementedException());
        }

        /// <summary>
        /// Standard BotBuilder adapter method to update a previous message with new content.
        /// </summary>
        /// <param name="turnContext">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="activity">The updated activity in the form '{id: `id of activity to update`, ...}'.</param>
        /// <param name="cancellationToken">A cancellation token for the task.</param>
        /// <returns>A resource response with the Id of the updated activity.</returns>
        public override async Task<ResourceResponse> UpdateActivityAsync(ITurnContext turnContext, Activity activity, CancellationToken cancellationToken)
        {
            // Twilio adapter does not support updateActivity.
            return await Task.FromException<ResourceResponse>(new NotImplementedException("TFacebook adapter does not support updateActivity."));
        }

        /// <summary>
        /// Standard BotBuilder adapter method to delete a previous message.
        /// </summary>
        /// <param name="turnContext">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="reference">An object in the form "{activityId: `id of message to delete`, conversation: { id: `id of channel`}}".</param>
        /// <param name="cancellationToken">A cancellation token for the task.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override async Task DeleteActivityAsync(ITurnContext turnContext, ConversationReference reference, CancellationToken cancellationToken)
        {
            // Twilio adapter does not support deleteActivity.
            await Task.FromException<ResourceResponse>(new NotImplementedException("Facebook adapter does not support deleteActivity."));
        }

        /// <summary>
        /// Standard BotBuilder adapter method for continuing an existing conversation based on a conversation reference.
        /// </summary>
        /// <param name="reference">A conversation reference to be applied to future messages.</param>
        /// <param name="logic">A bot logic function that will perform continuing action in the form `async(context) => { ... }`.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ContinueConversationAsync(ConversationReference reference, BotCallbackHandler logic)
        {
            await Task.FromException(new NotImplementedException());
        }

        /// <summary>
        /// Accept an incoming webhook request and convert it into a TurnContext which can be processed by the bot's logic.
        /// </summary>
        /// <param name="request">A request object.</param>
        /// <param name="response">A response object.</param>
        /// <param name="bot">A bot logic function.</param>
        /// <param name="cancellationToken">A cancellation token for the task.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ProcessAsync(HttpRequest request, HttpResponse response, IBot bot, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.FromException(new NotImplementedException());
        }

        /// <summary>
        /// Converts an Activity object to a Facebook messenger outbound message ready for the API.
        /// </summary>
        /// <param name="activity">The activity to be converted to Facebook message.</param>
        /// <returns>The resulting message.</returns>
        private object ActivityToFacebook(Activity activity)
        {
            return new NotImplementedException();
        }

        /// <summary>
        /// Handles each individual message inside a webhook payload (webhook may deliver more than one message at a time).
        /// </summary>
        /// <param name="message">The message to be processed.</param>
        /// <param name="logic">The callback logic to call upon the message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task ProcessSingleMessageAsync(object message, object logic)
        {
            await Task.FromException<bool>(new NotImplementedException());
        }

        /// <summary>
        /// Verifies the SHA1 signature of the raw request payload before bodyParser parses it will abort parsing if signature is invalid, and pass a generic error to response.
        /// </summary>
        /// <param name="request">An Http request object.</param>
        /// <param name="response">An Http response object.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<bool> VerifySignatureAsync(HttpRequest request, HttpResponse response)
        {
            return await Task.FromException<bool>(new NotImplementedException());
        }
    }
}
