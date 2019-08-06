// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Security;

namespace Microsoft.Bot.Builder.Adapters.Twilio
{
    /// <summary>
    /// Contains helper functions for the TwilioAdapter class.
    /// </summary>
    public static class TwilioHelper
    {
        /// <summary>
        /// Extracts attachments (if any) from a twilio message and returns them in an Attachments array.
        /// </summary>
        /// <param name="message">The TwilioEvent message.</param>
        /// <returns>An Attachments array with the converted attachments.</returns>
        public static List<Attachment> GetMessageAttachments(TwilioEvent message)
        {
            if (message == null)
            {
                return null;
            }

            var attachments = new List<Attachment>();
            if (int.TryParse(message.NumMedia, out var numMediaResult) && numMediaResult > 0)
            {
                for (var i = 0; i < numMediaResult; i++)
                {
                    var attachment = new Attachment()
                    {
                        ContentType = message.MediaContentTypes[i],
                        ContentUrl = message.MediaUrls[i].AbsolutePath,
                    };
                    attachments.Add(attachment);
                }
            }

            return attachments;
        }

        /// <summary>
        /// Converts a query string to a dictionary with key-value pairs.
        /// </summary>
        /// <param name="query">The query string to convert.</param>
        /// <returns>A dictionary with the query values.</returns>
        public static Dictionary<string, string> QueryStringToDictionary(string query)
        {
            if (query == null)
            {
                return null;
            }

            var pairs = query.Replace("+", "%20").Split('&');
            var values = new Dictionary<string, string>();

            foreach (var p in pairs)
            {
                var pair = p.Split('=');
                var key = pair[0];
                var value = Uri.UnescapeDataString(pair[1]);

                values.Add(key, value);
            }

            return values;
        }

        /// <summary>
        /// Formats a BotBuilder activity into an outgoing Twilio SMS message.
        /// </summary>
        /// <param name="activity">A BotBuilder Activity object.</param>
        /// <param name="twilioNumber">The assigned Twilio phone number.</param>
        /// <returns>A Message's options object with {body, from, to, mediaUrl}.</returns>
        public static CreateMessageOptions ActivityToTwilio(Activity activity, string twilioNumber)
        {
            if (activity == null || string.IsNullOrWhiteSpace(twilioNumber))
            {
                return null;
            }

            List<Uri> mediaUrls = null;

            if ((activity.ChannelData as TwilioEvent)?.MediaUrls != null)
            {
                mediaUrls = new List<Uri>();
                mediaUrls = ((TwilioEvent)activity.ChannelData).MediaUrls;
            }

            var messageOptions = new CreateMessageOptions(activity.Conversation.Id)
            {
                ApplicationSid = activity.Conversation.Id,
                From = twilioNumber,
                Body = activity.Text,
                MediaUrl = mediaUrls,
            };

            return messageOptions;
        }

        /// <summary>
        /// Processes a HTTP request into an Activity.
        /// </summary>
        /// <param name="httpRequest">A httpRequest object.</param>
        /// <param name="validationUrl">The URL to check the validation against.</param>
        /// <param name="authToken">The authentication token for the Twilio app.</param>
        /// <returns>The Activity obtained from the httpRequest object.</returns>
        public static Activity ReadRequest(HttpRequest httpRequest, string validationUrl, string authToken)
        {
            if (httpRequest == null)
            {
                return null;
            }

            var signature = httpRequest.Headers["x-twilio-signature"];
            validationUrl = validationUrl ?? (httpRequest.Headers["x-forwarded-proto"][0] ?? httpRequest.Protocol + "://" + httpRequest.Host + httpRequest.Path);
            var requestValidator = new RequestValidator(authToken);
            Dictionary<string, string> body;

            using (var bodyStream = new StreamReader(httpRequest.Body))
            {
                body = QueryStringToDictionary(bodyStream.ReadToEnd());
            }

            if (!requestValidator.Validate(validationUrl, body, signature))
            {
                throw new AuthenticationException("Request does not match provided signature");
            }

            var twilioEvent = JsonConvert.DeserializeObject<TwilioEvent>(JsonConvert.SerializeObject(body));

            return new Activity()
            {
                Id = twilioEvent.MessageSid,
                Timestamp = new DateTime(),
                ChannelId = "twilio-sms",
                Conversation = new ConversationAccount()
                {
                    Id = twilioEvent.From,
                },
                From = new ChannelAccount()
                {
                    Id = twilioEvent.From,
                },
                Recipient = new ChannelAccount()
                {
                    Id = twilioEvent.To,
                },
                Text = twilioEvent.Body,
                ChannelData = twilioEvent,
                Type = ActivityTypes.Message,
                Attachments = GetMessageAttachments(twilioEvent),
            };
        }
    }
}
