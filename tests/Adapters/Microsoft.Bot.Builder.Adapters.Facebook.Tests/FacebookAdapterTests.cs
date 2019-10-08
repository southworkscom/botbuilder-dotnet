// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Schema;
using Moq;
using Xunit;

namespace Microsoft.Bot.Builder.Adapters.Facebook.Tests
{
    public class FacebookAdapterTests
    {
        private readonly FacebookAdapterOptions _testOptions = new FacebookAdapterOptions("Test", "Test", "Test");

        [Fact]
        public void ConstructorShouldFailWithNullClient()
        {
            Assert.Throws<ArgumentNullException>(() => { new FacebookAdapter((FacebookClientWrapper)null); });
        }

        [Fact]
        public void ConstructorWithArgumentsShouldSucceed()
        {
            Assert.NotNull(new FacebookAdapter(new Mock<FacebookClientWrapper>(_testOptions).Object));
        }

        [Fact]
        public async void ContinueConversationAsyncShouldFailWithNullConversationReference()
        {
            var facebookAdapter = new FacebookAdapter(new Mock<FacebookClientWrapper>(_testOptions).Object);

            Task BotsLogic(ITurnContext turnContext, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await facebookAdapter.ContinueConversationAsync(null, BotsLogic); });
        }

        [Fact]
        public async void ContinueConversationAsyncShouldFailWithNullLogic()
        {
            var facebookAdapter = new FacebookAdapter(new Mock<FacebookClientWrapper>(_testOptions).Object);
            var conversationReference = new ConversationReference();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await facebookAdapter.ContinueConversationAsync(conversationReference, null); });
        }

        [Fact]
        public async void ContinueConversationAsyncShouldSucceed()
        {
            var callbackInvoked = false;

            var facebookAdapter = new FacebookAdapter(new Mock<FacebookClientWrapper>(_testOptions).Object);
            var conversationReference = new ConversationReference();
            Task BotsLogic(ITurnContext turnContext, CancellationToken cancellationToken)
            {
                callbackInvoked = true;
                return Task.CompletedTask;
            }

            await facebookAdapter.ContinueConversationAsync(conversationReference, BotsLogic);
            Assert.True(callbackInvoked);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteActivityAsyncShouldThrowNotImplementedException()
        {
            var adapter = new FacebookAdapter(new FacebookClientWrapper(_testOptions));

            var activity = new Activity();
            var turnContext = new TurnContext(adapter, activity);

            var conversationReference = new ConversationReference();

            await Assert.ThrowsAsync<NotImplementedException>(() => adapter.DeleteActivityAsync(turnContext, conversationReference, default));
        }

        [Fact]
        public async void ProcessAsyncShouldThrowExceptionWithInvalidBody()
        {
            var clientWrapper = new Mock<FacebookClientWrapper>(_testOptions);
            clientWrapper.Setup(x => x.VerifySignature(It.IsAny<HttpRequest>(), It.IsAny<string>())).Returns(true);

            var facebookAdapter = new FacebookAdapter(clientWrapper.Object);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("wrong-formatted-json"));

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetupGet(req => req.Query[It.IsAny<string>()]).Returns("test");
            httpRequest.SetupGet(req => req.Body).Returns(stream);
            var httpResponse = new Mock<HttpResponse>();
            var bot = new Mock<IBot>();
            bot.Setup(x => x.OnTurnAsync(It.IsAny<TurnContext>(), It.IsAny<CancellationToken>())).Callback(() =>
            {
            });

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await facebookAdapter.ProcessAsync(httpRequest.Object, httpResponse.Object, bot.Object, default(CancellationToken));
            });
        }

        [Fact]
        public async void ProcessAsyncShouldSucceed()
        {
            var payload = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Files\Payload.json");

            var clientWrapper = new Mock<FacebookClientWrapper>(_testOptions);
            clientWrapper.Setup(x => x.VerifySignature(It.IsAny<HttpRequest>(), It.IsAny<string>())).Returns(true);

            var facebookAdapter = new FacebookAdapter(clientWrapper.Object);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(payload));

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetupGet(req => req.Query[It.IsAny<string>()]).Returns("test");
            httpRequest.SetupGet(req => req.Body).Returns(stream);
            var httpResponse = new Mock<HttpResponse>();
            var bot = new Mock<IBot>();
            bot.Setup(x => x.OnTurnAsync(It.IsAny<TurnContext>(), It.IsAny<CancellationToken>())).Callback(() =>
            {
            });

            await facebookAdapter.ProcessAsync(httpRequest.Object, httpResponse.Object, bot.Object, default(CancellationToken));
        }

        [Fact]
        public async void SendActivitiesAsyncShouldFailWithActivityTypeNotMessage()
        {
            var facebookAdapter = new FacebookAdapter(new Mock<FacebookClientWrapper>(_testOptions).Object);
            var activity = new Activity
            {
                Type = ActivityTypes.Event,
            };

            Activity[] activities = { activity };

            var turnContext = new TurnContext(facebookAdapter, activity);

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await facebookAdapter.SendActivitiesAsync(turnContext, activities, default);
            });
        }

        [Fact]
        public async void SendActivitiesAsyncShouldSucceedWithActivityTypeMessage()
        {
            var clientWrapper = new Mock<FacebookClientWrapper>(_testOptions);
            clientWrapper.Setup(x => x.GetApiAsync(It.IsAny<Activity>())).Returns(Task.FromResult(new FacebookClientWrapper(_testOptions)));
            clientWrapper.Setup(x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<FacebookMessage>(), It.IsAny<HttpMethod>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(string.Empty));

            var facebookAdapter = new FacebookAdapter(clientWrapper.Object);
            var activity = new Activity
            {
                Type = ActivityTypes.Message,
                Text = "Test text",
                Conversation = new ConversationAccount()
                {
                    Id = "Test id",
                },
                ChannelData = new FacebookMessage("recipientId", new Message(), "messagingtype"),
            };

            Activity[] activities = { activity };

            var turnContext = new TurnContext(facebookAdapter, activity);

            await facebookAdapter.SendActivitiesAsync(turnContext, activities, default);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateActivityAsyncShouldThrowNotImplementedException()
        {
            var adapter = new FacebookAdapter(new FacebookClientWrapper(_testOptions));

            var activity = new Activity();
            var turnContext = new TurnContext(adapter, activity);
            await Assert.ThrowsAsync<NotImplementedException>(() => adapter.UpdateActivityAsync(turnContext, activity, default));
        }
    }
}
