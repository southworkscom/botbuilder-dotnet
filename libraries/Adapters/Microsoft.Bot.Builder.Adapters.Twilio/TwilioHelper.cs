using System.Collections.Generic;
using Microsoft.Bot.Schema;

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
                var value = System.Uri.UnescapeDataString(pair[1]);

                values.Add(key, value);
            }

            return values;
        }
    }
}
