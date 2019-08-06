using System;
using System.Collections.Generic;
using Microsoft.Bot.Schema;
using Twilio.Rest.Api.V2010.Account;

namespace Microsoft.Bot.Builder.Adapters.Twilio
{
    public static class TwilioHelper
    {
        /// <summary>
        /// Extracts attachments (if any) from a twilio message and returns them in an Attachments array.
        /// </summary>
        /// <param name="message">The TwilioEvent message.</param>
        /// <returns>An Attachments array with the converted attachments.</returns>
        public static List<Attachment> GetMessageAttachments(TwilioEvent message)
        {
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
        /// <param name="options">A set of params with the required values for authentication.</param>
        /// <returns>A Message's options object with {body, from, to, mediaUrl}.</returns>
        public static CreateMessageOptions ActivityToTwilio(Activity activity, ITwilioAdapterOptions options)
        {
            var mediaUrls = new List<Uri>();

            if ((activity.ChannelData as TwilioEvent)?.MediaUrls != null)
            {
                mediaUrls = ((TwilioEvent)activity.ChannelData).MediaUrls;
            }

            var messageOptions = new CreateMessageOptions(activity.Conversation.Id)
            {
                ApplicationSid = activity.Conversation.Id,
                From = options.TwilioNumber,
                Body = activity.Text,
                MediaUrl = mediaUrls,
            };

            return messageOptions;
        }
    }
}
