// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Schema;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.Bot.Builder.Adapters.Twilio.Tests
{
    public class TwilioAdapterTests
    {
        [Fact]
        public void Constructor_Should_Fail_With_Null_Options()
        {
            var twilioApi = new TwilioApi();
            Assert.Throws<ArgumentNullException>(() => { new TwilioAdapter(null, twilioApi); });
        }

        [Fact]
        public void Constructor_Should_Fail_With_Null_TwilioNumber()
        {
            ITwilioAdapterOptions options = new MockTwilioOptions
            {
                TwilioNumber = null,
                AccountSid = "Test",
                AuthToken = "Test",
            };

            var twilioApi = new TwilioApi();
            Assert.Throws<Exception>(() => { new TwilioAdapter(options, twilioApi); });
        }

        [Fact]
        public void Constructor_Should_Fail_With_Null_AccountSid()
        {
            ITwilioAdapterOptions options = new MockTwilioOptions
            {
                TwilioNumber = "Test",
                AccountSid = null,
                AuthToken = "Test",
            };

            var twilioApi = new TwilioApi();
            Assert.Throws<Exception>(() => { new TwilioAdapter(options, twilioApi); });
        }

        [Fact]
        public void Constructor_Should_Fail_With_Null_AuthToken()
        {
            ITwilioAdapterOptions options = new MockTwilioOptions
            {
                TwilioNumber = "Test",
                AccountSid = "Test",
                AuthToken = null,
            };

            var twilioApi = new TwilioApi();
            Assert.Throws<Exception>(() => { new TwilioAdapter(options, twilioApi); });
        }

        [Fact]
        public void Constructor_WithArguments_Succeeds()
        {
            ITwilioAdapterOptions options = new MockTwilioOptions
            {
                TwilioNumber = "Test",
                AccountSid = "Test",
                AuthToken = "Test",
            };

            var twilioApi = new TwilioApi();
            Assert.NotNull(new TwilioAdapter(options, twilioApi));
        }

        [Fact]
        public void ActivityToTwilio_Should_Return_MessageOptions_With_MediaUrl()
        {
            ITwilioAdapterOptions options = new MockTwilioOptions
            {
                TwilioNumber = "Test",
                AccountSid = "Test",
                AuthToken = "Test",
            };
            var twilioApi = new TwilioApi();
            var adapter = new TwilioAdapter(options, twilioApi);
            var activity = JsonConvert.DeserializeObject<Activity>(File.ReadAllText(Directory.GetCurrentDirectory() + @"\files\Activities.json"));
            var activities = new Activity[] { activity };
            activity.Attachments = new List<Attachment> { new Attachment(contentUrl: "http://example.com") };
            var messageOption = TwilioHelper.ActivityToTwilio(activity, "123456789");

            Assert.Equal(activity.Conversation.Id, messageOption.ApplicationSid);
            Assert.Equal("123456789", messageOption.From.ToString());
            Assert.Equal(activity.Text, messageOption.Body);
            Assert.Equal(new Uri(activity.Attachments[0].ContentUrl), messageOption.MediaUrl[0]);
        }

        [Fact]
        public async void SendActivitiesAsync_Should_Fail_With_ActivityType_Not_Message()
        {
            ITwilioAdapterOptions options = new MockTwilioOptions
            {
                TwilioNumber = "Test", AccountSid = "Test", AuthToken = "Test",
            };

            var twilioApi = new TwilioApi();
            var twilioAdapter = new TwilioAdapter(options, twilioApi);

            var activity = new Activity()
            {
                Type = ActivityTypes.Event,
            };

            Activity[] activities = { activity };

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await twilioAdapter.SendActivitiesAsync(new TurnContext(twilioAdapter, activity), activities, default);
            });
        }

        [Fact]
        public async void ProcessAsync_Should_Fail_With_Null_HttpRequest()
        {
            ITwilioAdapterOptions options = new MockTwilioOptions
            {
                TwilioNumber = "Test", AccountSid = "Test", AuthToken = "Test",
            };

            var twilioApi = new TwilioApi();
            var twilioAdapter = new TwilioAdapter(options, twilioApi);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await twilioAdapter.ProcessAsync(null, default(HttpResponse), default(IBot), default(CancellationToken));
            });
        }

        [Fact]
        public async void ProcessAsync_Should_Fail_With_Null_HttpResponse()
        {
            ITwilioAdapterOptions options = new MockTwilioOptions
            {
                TwilioNumber = "Test", AccountSid = "Test", AuthToken = "Test",
            };

            var twilioApi = new TwilioApi();
            var twilioAdapter = new TwilioAdapter(options, twilioApi);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await twilioAdapter.ProcessAsync(default(HttpRequest), null, default(IBot), default(CancellationToken));
            });
        }

        [Fact]
        public async void ProcessAsync_Should_Fail_With_Null_Bot()
        {
            ITwilioAdapterOptions options = new MockTwilioOptions
            {
                TwilioNumber = "Test", AccountSid = "Test", AuthToken = "Test",
            };

            var twilioApi = new TwilioApi();
            var twilioAdapter = new TwilioAdapter(options, twilioApi);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await twilioAdapter.ProcessAsync(default(HttpRequest), default(HttpResponse), null, default(CancellationToken));
            });
        }

        [Fact]
        public async void SendActivitiesAsync_Should_Succeed()
        {
            ITwilioAdapterOptions options = new MockTwilioOptions
            {
                TwilioNumber = "Test",
                AccountSid = "Test",
                AuthToken = "Test",
            };

            // Setup mocked Activity and get the message option
            var activity = new Mock<Activity>().SetupAllProperties();
            activity.Object.Type = "message";
            activity.Object.Attachments = new List<Attachment> { new Attachment(contentUrl: "http://example.com") };
            activity.Object.Conversation = new ConversationAccount(id: "MockId");
            activity.Object.Text = "Hello, Bot!";
            var messageOption = TwilioHelper.ActivityToTwilio(activity.Object, "123456789");

            // Setup mocked Twilio API client
            const string resourceIdentifier = "Mocked Resource Identifier";
            var twilioApi = new Mock<ITwilioClient>();
            twilioApi.Setup(x => x.GetResourceIdentifier(It.IsAny<object>())).Returns(Task.FromResult(resourceIdentifier));

            // Create a new Twilio Adapter with the mocked classes and get the responses
            var twilioAdapter = new TwilioAdapter(options, twilioApi.Object);
            ResourceResponse[] resourceResponses = await twilioAdapter.SendActivitiesAsync(null, new Activity[] { activity.Object }, default).ConfigureAwait(false);

            // Assert the result
            Assert.True(resourceResponses[0].Id == resourceIdentifier);
        }

        private class MockTwilioOptions : ITwilioAdapterOptions
        {
            public string TwilioNumber { get; set; }

            public string AccountSid { get; set; }

            public string AuthToken { get; set; }

            public string ValidationUrl { get; set; }
        }
    }
}
