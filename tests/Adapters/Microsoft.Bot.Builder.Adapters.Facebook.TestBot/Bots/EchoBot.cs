﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.3.0

using System.Collections.Generic;
using System.IO;
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
                if (turnContext.Activity.Text.Contains("template"))
                {
                    var activity = MessageFactory.Attachment(CreateTemplateAttachment(Directory.GetCurrentDirectory() + @"\Resources\TemplatePayload.json"));
                    await turnContext.SendActivityAsync(activity, cancellationToken);
                }
                else
                {
                    var activity = MessageFactory.Text($"Echo: {turnContext.Activity.Text}");
                    await turnContext.SendActivityAsync(activity, cancellationToken);
                }
            }
        }

        protected override async Task OnEventActivityAsync(ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Value != null)
            {
                var inputs = (Dictionary<string, string>)turnContext.Activity.Value;
                var name = inputs["Name"];

                var activity = MessageFactory.Text($"How are you doing {name}?");
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
