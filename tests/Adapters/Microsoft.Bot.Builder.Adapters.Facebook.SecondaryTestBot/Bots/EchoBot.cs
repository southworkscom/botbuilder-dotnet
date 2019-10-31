﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.3.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.SecondaryTestBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        /// <summary>
        /// This option passes thread control from the secondary receiver to a primary receiver.
        /// </summary>
        private const string OptionPassPrimaryBot = "Pass to primary";

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = MessageFactory.Text("Hello and Welcome!");
            await turnContext.SendActivityAsync(activity, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Attachments != null)
            {
                foreach (var attachment in turnContext.Activity.Attachments)
                {
                    var activity = MessageFactory.Text($" I got {turnContext.Activity.Attachments.Count} attachments");

                    var image = new Attachment(
                       attachment.ContentType,
                       content: attachment.Content);

                    activity.Attachments.Add(image);
                    await turnContext.SendActivityAsync(activity, cancellationToken);
                }
            }
            else
            {
                IActivity activity;

                switch (turnContext.Activity.Text)
                {
                    case OptionPassPrimaryBot:
                        activity = MessageFactory.Text("Redirecting to the primary bot...");
                        activity.Type = ActivityTypes.Event;
                        (activity as IEventActivity).Name = "pass_thread_control";
                        (activity as IEventActivity).Value = "An app id of the primary receiver";
                        break;

                    case "Redirected to the bot":
                        activity = MessageFactory.Text("Hello Human, I'm the secondary bot to help you!");
                        break;
                    case "Little":
                        activity = MessageFactory.Text($"You have spoken the forbidden word!");
                        break;
                    default:
                        activity = MessageFactory.Text($"Echo Secondary: {turnContext.Activity.Text}");
                        break;
                }

                await turnContext.SendActivityAsync(activity, cancellationToken);
            }
        }

        protected override async Task OnEventActivityAsync(ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Value != null)
            {
                Type type = turnContext.Activity.Value.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

                foreach (var prop in props)
                {
                    if (prop.Name == "Metadata")
                    {
                        var metadataValue = prop.GetValue(turnContext.Activity.Value, null);
                        if (metadataValue.ToString().Trim() == "Pass thread control")
                        {
                            var activity = MessageFactory.Text("Hello Human, I'm the secondary bot to help you!");
                            await turnContext.SendActivityAsync(activity, cancellationToken);
                        }
                    }
                }
            }

            if ((turnContext.Activity as Activity)?.Text == "Other Bot")
            {
                var activity = new Activity();
                activity.Type = ActivityTypes.Event;

                //Action
                (activity as IEventActivity).Name = "request_thread_control";
                await turnContext.SendActivityAsync(activity, cancellationToken);
            }
        }

        private static Attachment CreateTemplateAttachment(string filePath)
        {
            var templateAttachmentJson = File.ReadAllText(filePath);
            var templateAttachment = new Attachment()
            {
                ContentType = "template",
                Content = JsonConvert.DeserializeObject(templateAttachmentJson),
            };
            return templateAttachment;
        }
    }
}
