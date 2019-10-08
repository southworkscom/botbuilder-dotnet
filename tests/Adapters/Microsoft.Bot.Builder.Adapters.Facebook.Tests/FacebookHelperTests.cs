using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Schema;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.Bot.Builder.Adapters.Facebook.Tests
{
    public class FacebookHelperTests
    {
        private const string AuthTokenString = "authToken";
        private readonly Uri _validationUrlString = new Uri("http://contoso.com");

        [Fact]
        public void ActivityToFacebook_Should_Return_MessageOptions_With_MediaUrl()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(File.ReadAllText(Directory.GetCurrentDirectory() + @"\files\Activities.json"));
            activity.Attachments = new List<Attachment> { new Attachment(contentUrl: "http://example.com") };
            var messageOption = FacebookHelper.ActivityToFacebook(activity);

            Assert.Equal(activity.Conversation.Id, messageOption.Sender.Id);
            Assert.Equal(activity.Text, messageOption.Message.Text);
            Assert.Equal(new Uri(activity.Attachments[0].ContentUrl), messageOption.Message.Attachment.Payload);
        }
    }
}
