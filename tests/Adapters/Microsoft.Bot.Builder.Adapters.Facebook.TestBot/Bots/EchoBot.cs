// Copyright (c) Microsoft Corporation. All rights reserved.
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
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Adapters.Facebook.TestBot.Bots
{
    public class EchoBot : ActivityHandler
    {
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
                    case "button template":
                        activity = MessageFactory.Attachment(CreateTemplateAttachment(Directory.GetCurrentDirectory() + @"/Resources/ButtonTemplatePayload.json"));
                        break;
                    case "media template":
                        activity = MessageFactory.Attachment(CreateTemplateAttachment(Directory.GetCurrentDirectory() + @"/Resources/MediaTemplatePayload.json"));
                        break;
                    case "generic template":
                        activity = MessageFactory.Attachment(CreateTemplateAttachment(Directory.GetCurrentDirectory() + @"/Resources/GenericTemplatePayload.json"));
                        break;
                    case "Hello button":
                        activity = MessageFactory.Text("Hello Human!");
                        break;
                    case "Goodbye button":
                        activity = MessageFactory.Text("Goodbye Human!");
                        break;
                    case "Chatting":
                        activity = MessageFactory.Text("Hello! How can I help you?");
                        break;
                    case "handover template":
                        activity = MessageFactory.Attachment(CreateTemplateAttachment(Directory.GetCurrentDirectory() + @"/Resources/HandoverTemplatePayload.json"));
                        break;
                    case "Handover":
                        activity = MessageFactory.Text("Redirecting...");
                        activity.Type = ActivityTypes.Event;
                        (activity as IEventActivity).Name = "pass_thread_control";
                        (activity as IEventActivity).Value = "inbox";
                        break;
                    case "bot template":
                        activity = MessageFactory.Attachment(CreateTemplateAttachment(Directory.GetCurrentDirectory() + @"/Resources/HandoverBotsTemplatePayload.json"));
                        break;
                    case "SecondaryBot":
                        activity = MessageFactory.Text("Redirecting to the secondary bot...");
                        activity.Type = ActivityTypes.Event;

                        //Action
                        (activity as IEventActivity).Name = "pass_thread_control";

                        //AppId
                        (activity as IEventActivity).Value = "An app id of a secondary bot";
                        break;
                    case "TakeControl":
                        activity = MessageFactory.Text("Primary Bot Taking control...");
                        activity.Type = ActivityTypes.Event;

                        //Action
                        (activity as IEventActivity).Name = "take_thread_control";
                        break;
                    default:
                        activity = MessageFactory.Text($"Echo: {turnContext.Activity.Text}");
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
                        if (metadataValue.ToString().Trim() == "Request thread control to the primary receiver")
                        {
                            var activity = new Activity();
                            activity.Type = ActivityTypes.Event;
                            (activity as IEventActivity).Name = "pass_thread_control";
                            (activity as IEventActivity).Value = "An app id";
                            await turnContext.SendActivityAsync(activity, cancellationToken);
                        }
                        else if (metadataValue.ToString().Trim() == "Pass thread control to a secondary receiver")
                        {
                            var activity = MessageFactory.Text("Hello Again Human, I'm the bot to help you!");
                            await turnContext.SendActivityAsync(activity, cancellationToken);
                        }
                    }
                }
            }

            if ((turnContext.Activity as Activity)?.Text == "Little")
            {
                var activity = MessageFactory.Text("Primary Bot Taking control... The forbidden word has been spoken");
                activity.Type = ActivityTypes.Event;

                //Action
                (activity as IEventActivity).Name = "take_thread_control";
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
