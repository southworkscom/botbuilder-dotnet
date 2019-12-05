using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Adapters.Slack;
using Microsoft.Bot.Builder.FunctionalTests.Payloads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.FunctionalTests
{
    [TestClass]
    #if !FUNCTIONALTESTS
    [Ignore("These integration tests run only when FUNCTIONALTESTS is defined")]
    #endif
    public class SlackClientTest
    {
        private HttpClient client;
        private string _slackChannel = null;
        private string _slackToken = null;
        private string _slackUrlBase = "https://slack.com/api";

        [TestMethod]
        public async Task SendAndReceiveSlackMessageShouldSucced()
        {
            try
            {
                GetEnvironmentVars();
                await SendMessageAsync();

                var response = string.Empty;

                while (!response.Contains("Echo"))
                {
                    response = await ReceiveMessageAsync();
                }
                
                Assert.AreEqual("Echo: Hello bot", response);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<string> ReceiveMessageAsync()
        {
            client = new HttpClient();
            var requestUri = $"{_slackUrlBase}/conversations.history?token={_slackToken}&channel={_slackChannel}";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri),
            };

            var httpResponse = await client.SendAsync(request);

            var response = httpResponse.Content.ReadAsStringAsync().Result;
            var chatHistory = JsonConvert.DeserializeObject<SlackHistoryRetrieve>(response).Messages[0].Text;
            
            return chatHistory;
        }

        private async Task<string> SendMessageAsync()
        {
            var data = new NameValueCollection
            {
                ["token"] = _slackToken,
                ["channel"] = _slackChannel,
                ["text"] = "Hello bot",
                ["as_user"] = "true",
            };
            byte[] response;
            using (var client = new WebClient())
            {
                response = await client.UploadValuesTaskAsync($"{_slackUrlBase}/chat.postMessage", "POST", data);
            }

            var stringResponse = JsonConvert.DeserializeObject<SlackResponse>(Encoding.UTF8.GetString(response));
            return stringResponse.Message.Text;
        }

        private void GetEnvironmentVars()
        {
            if (string.IsNullOrWhiteSpace(_slackChannel) || string.IsNullOrWhiteSpace(_slackToken))
            {
                _slackChannel = Environment.GetEnvironmentVariable("SLACK_CHANNEL");
                if (string.IsNullOrWhiteSpace(_slackChannel))
                {
                    throw new Exception("Environment variable 'SLACK_CHANNEL' not found.");
                }

                _slackToken = Environment.GetEnvironmentVariable("SLACK_TOKEN");
                if (string.IsNullOrWhiteSpace(_slackToken))
                {
                    throw new Exception("Environment variable 'SLACK_TOKEN' not found.");
                }
            }
        }
    }
}
