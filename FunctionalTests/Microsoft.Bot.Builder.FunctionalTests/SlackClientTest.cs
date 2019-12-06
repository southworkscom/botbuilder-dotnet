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
        public async Task SendAndReceiveSlackMessageShouldSucceed()
        {
            try
            {
                GetEnvironmentVars();
                var echoGuid = Guid.NewGuid().ToString();
                await SendMessageAsync(echoGuid);

                var response = string.Empty;

                response = await ReceiveMessageAsync();

                Assert.AreEqual($"Echo: {echoGuid}", response);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<string> ReceiveMessageAsync()
        {
            var res = string.Empty;
            var i = 0;
            while (!res.Contains("Echo") && i < 5)
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
                res = JsonConvert.DeserializeObject<SlackHistoryRetrieve>(response).Messages[0].Text;

                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

                i++;
            }

            return res;
        }

        private async Task<string> SendMessageAsync(string echoGuid)
        {
            var data = new NameValueCollection
            {
                ["token"] = _slackToken,
                ["channel"] = _slackChannel,
                ["text"] = echoGuid,
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
