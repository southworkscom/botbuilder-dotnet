﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder.Adapters.Facebook.FacebookEvents;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook
{
    public class FacebookAdapter : BotAdapter, IBotFrameworkHttpAdapter
    {
        private const string HubModeSubscribe = "subscribe";

        /// <summary>
        /// The constant ID representing the page inbox.
        /// </summary>
        private const string PageInboxId = "263902037430900";

        private readonly FacebookClientWrapper _facebookClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdapter"/> class using configuration settings.
        /// </summary>
        /// <param name="configuration">An <see cref="IConfiguration"/> instance.</param>
        /// <remarks>
        /// The configuration keys are:
        /// VerifyToken: The token to respond to the initial verification request.
        /// AppSecret: The secret used to validate incoming webhooks.
        /// AccessToken: An access token for the bot.
        /// </remarks>
        public FacebookAdapter(IConfiguration configuration)
            : this(new FacebookClientWrapper(new FacebookAdapterOptions(configuration["FacebookVerifyToken"], configuration["FacebookAppSecret"], configuration["FacebookAccessToken"])))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdapter"/> class.
        /// Creates a Facebook adapter.
        /// </summary>
        /// <param name="facebookClient">A Facebook API interface.</param>
        public FacebookAdapter(FacebookClientWrapper facebookClient)
        {
            _facebookClient = facebookClient ?? throw new ArgumentNullException(nameof(facebookClient));
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
            var responses = new List<ResourceResponse>();

            foreach (var activity in activities)
            {
                if (activity.Type != ActivityTypes.Message && activity.Type != ActivityTypes.Event)
                {
                    continue;
                }

                var message = FacebookHelper.ActivityToFacebook(activity);

                if (message.Message.Attachment != null)
                {
                    message.Message.Attachments = null;
                    message.Message.Text = null;
                }

                var res = await _facebookClient.SendMessageAsync("/me/messages", message, null, cancellationToken).ConfigureAwait(false);

                if (activity.Type == ActivityTypes.Event)
                {
                    if (activity.Name.Equals("pass_thread_control", StringComparison.InvariantCulture))
                    {
                        var recipient = (string)activity.Value == "inbox" ? PageInboxId : (string)activity.Value;
                        await _facebookClient.PassThreadControlAsync(recipient, activity.Conversation.Id, "Pass thread control").ConfigureAwait(false);
                    }
                    else if (activity.Name.Equals("take_thread_control", StringComparison.InvariantCulture))
                    {
                        await _facebookClient.TakeThreadControlAsync(activity.Conversation.Id, "Take thread control from a secondary receiver").ConfigureAwait(false);
                    }
                    else if (activity.Name.Equals("request_thread_control", StringComparison.InvariantCulture))
                    {
                        await _facebookClient.RequestThreadControlAsync(activity.Conversation.Id, "Request thread control to the primary receiver").ConfigureAwait(false);
                    }
                }

                var response = new ResourceResponse()
                {
                    Id = res,
                };

                responses.Add(response);
            }

            return responses.ToArray();
        }

        /// <summary>
        /// Standard BotBuilder adapter method to update a previous message with new content.
        /// </summary>
        /// <param name="turnContext">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="activity">The updated activity in the form '{id: `id of activity to update`, ...}'.</param>
        /// <param name="cancellationToken">A cancellation token for the task.</param>
        /// <returns>A resource response with the Id of the updated activity.</returns>
        public override Task<ResourceResponse> UpdateActivityAsync(ITurnContext turnContext, Activity activity, CancellationToken cancellationToken)
        {
            return Task.FromException<ResourceResponse>(new NotImplementedException("Facebook adapter does not support updateActivity."));
        }

        /// <summary>
        /// Standard BotBuilder adapter method to delete a previous message.
        /// </summary>
        /// <param name="turnContext">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="reference">An object in the form "{activityId: `id of message to delete`, conversation: { id: `id of channel`}}".</param>
        /// <param name="cancellationToken">A cancellation token for the task.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override Task DeleteActivityAsync(ITurnContext turnContext, ConversationReference reference, CancellationToken cancellationToken)
        {
            return Task.FromException(new NotImplementedException("Facebook adapter does not support deleteActivity."));
        }

        /// <summary>
        /// Standard BotBuilder adapter method for continuing an existing conversation based on a conversation reference.
        /// </summary>
        /// <param name="reference">A conversation reference to be applied to future messages.</param>
        /// <param name="logic">A bot logic function that will perform continuing action in the form `async(context) => { ... }`.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ContinueConversationAsync(ConversationReference reference, BotCallbackHandler logic)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (logic == null)
            {
                throw new ArgumentNullException(nameof(logic));
            }

            var request = reference.GetContinuationActivity().ApplyConversationReference(reference, true);

            using (var context = new TurnContext(this, request))
            {
                await RunPipelineAsync(context, logic, default).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Accept an incoming webhook request and convert it into a TurnContext which can be processed by the bot's logic.
        /// </summary>
        /// <param name="request">A request object.</param>
        /// <param name="response">A response object.</param>
        /// <param name="bot">A bot logic function.</param>
        /// <param name="cancellationToken">A cancellation token for the task.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ProcessAsync(HttpRequest request, HttpResponse response, IBot bot, CancellationToken cancellationToken)
        {
            if (request.Query["hub.mode"] == HubModeSubscribe)
            {
                await _facebookClient.VerifyWebhookAsync(request, response, cancellationToken).ConfigureAwait(false);
                return;
            }

            string stringifiedBody;

            using (var sr = new StreamReader(request.Body))
            {
                stringifiedBody = sr.ReadToEnd();
            }

            if (!_facebookClient.VerifySignature(request, stringifiedBody))
            {
                await FacebookHelper.WriteAsync(response, HttpStatusCode.Unauthorized, string.Empty, Encoding.UTF8, cancellationToken).ConfigureAwait(false);
                throw new Exception("WARNING: Webhook received message with invalid signature. Potential malicious behavior!");
            }

            FacebookResponseEvent facebookResponseEvent = null;

            facebookResponseEvent = JsonConvert.DeserializeObject<FacebookResponseEvent>(stringifiedBody);

            foreach (var entry in facebookResponseEvent.Entry)
            {
                var payload = entry.Changes.Any() ? entry.Changes : entry.Messaging.Any() ? entry.Messaging : entry.Standby.Any() ? entry.Standby : new List<FacebookMessage>();

                foreach (var message in payload)
                {
                    message.IsStandby = entry.Standby.Any();

                    var activity = FacebookHelper.ProcessSingleMessage(message);

                    using (var context = new TurnContext(this, activity))
                    {
                        await RunPipelineAsync(context, bot.OnTurnAsync, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
