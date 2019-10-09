using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        public void ActivityToFacebookShouldReturnMessageOptionsWithMediaUrl()
        {
            var uri = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Files\Activities.json");
            var activity = JsonConvert.DeserializeObject<Activity>(uri);

            activity.Attachments = new List<Attachment> { new Attachment(contentUrl: "http://example.com") };
            var messageOption = FacebookHelper.ActivityToFacebook(activity);

            Assert.Equal(activity.Conversation.Id, messageOption.Recipient.Id);
            Assert.Equal(activity.Text, messageOption.Message.Text);
            Assert.Equal(new Uri(activity.Attachments[0].ContentUrl), messageOption.Message.Attachment.Payload.Url);
        }

        [Fact]
        public void ActivityToFacebookShouldThrowErrorWithTwoOrMoreAttachments()
        {
            var activity = new Activity()
            {
                Conversation = new ConversationAccount()
                {
                    Id = "testId",
                },
                Attachments = new List<Attachment> { new Attachment(contentUrl: "http://example.com"), new Attachment(contentUrl: "http://example.com") },
            };

            Assert.Throws<Exception>(() => { var messageOptions = FacebookHelper.ActivityToFacebook(activity); });
        }

        [Fact]
        public void ActivityToFacebookShouldReturnNullWithActivityNull()
        {
            Activity activity = null;

            var messageOptions = FacebookHelper.ActivityToFacebook(activity);

            Assert.Null(messageOptions);
        }

        [Fact]
        public void ProcessSingleMessageShouldReturnNullWithMessageNull()
        {
            FacebookMessage message = null;

            var nullActivity = FacebookHelper.ProcessSingleMessage(message);

            Assert.Null(nullActivity);
        }

        [Fact]
        public void ProcessSingleMessageShouldReturnAActivityWithMessageWithData()
        {
            var uri = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Files\FacebookMessages.json");
            var facebookMessage = JsonConvert.DeserializeObject<List<FacebookMessage>>(uri)[0];

            var activity = FacebookHelper.ProcessSingleMessage(facebookMessage);
            Assert.Equal(activity.Conversation.Id, facebookMessage.Recipient.Id);
        }

        [Fact]
        public void ProcessSingleMessageShouldReturnConversationIdWithoutDataOnSender()
        {
            var uri = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Files\FacebookMessages.json");
            var facebookMessage = JsonConvert.DeserializeObject<List<FacebookMessage>>(uri)[1];

            var activity = FacebookHelper.ProcessSingleMessage(facebookMessage);
            Assert.NotNull(activity.Conversation.Id);
        }

        [Fact]
        public void ProcessSingleMessageShouldReturnActivityWithMessage()
        {
            var uri = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Files\FacebookMessages.json");
            var facebookMessage = JsonConvert.DeserializeObject<List<FacebookMessage>>(uri)[2];

            var activity = FacebookHelper.ProcessSingleMessage(facebookMessage);
            Assert.NotNull(activity.Text);
            Assert.NotNull(activity.ChannelData);
        }

        [Fact]
        public void ProcessSingleMessageShouldReturnActivityWithAttachments()
        {
            var uri = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Files\FacebookMessages.json");
            var facebookMessage = JsonConvert.DeserializeObject<List<FacebookMessage>>(uri)[3];

            var activity = FacebookHelper.ProcessSingleMessage(facebookMessage);
            Assert.NotNull(activity.Attachments);
            Assert.Equal(3, activity.Attachments.Count);
        }

        [Fact]
        public void ProcessSingleMessageShouldReturnActivityWithPostBack()
        {
            var uri = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Files\FacebookMessages.json");
            var facebookMessage = JsonConvert.DeserializeObject<List<FacebookMessage>>(uri)[4];

            var activity = FacebookHelper.ProcessSingleMessage(facebookMessage);
            Assert.NotNull(activity.Text);
            Assert.Equal(facebookMessage.PostBack.Payload, activity.Text);
        }

        [Fact]
        public async Task WriteAsyncShouldFailWithNullResponse()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await FacebookHelper.WriteAsync(null, HttpStatusCode.OK, "testText", Encoding.UTF8, new CancellationToken()).ConfigureAwait(false);
            });
        }

        [Fact]
        public async Task WriteAsyncShouldFailWithNullText()
        {
            var httpResponse = new Mock<HttpResponse>();

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await FacebookHelper.WriteAsync(httpResponse.Object, HttpStatusCode.OK, null, Encoding.UTF8, new CancellationToken()).ConfigureAwait(false);
            });
        }

        [Fact]
        public async Task WriteAsyncShouldFailWithNullEncoding()
        {
            var httpResponse = new Mock<HttpResponse>();

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await FacebookHelper.WriteAsync(httpResponse.Object, HttpStatusCode.OK, "testText", null, new CancellationToken()).ConfigureAwait(false);
            });
        }
    }
}
