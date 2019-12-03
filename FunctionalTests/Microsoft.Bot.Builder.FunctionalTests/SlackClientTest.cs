using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.FunctionalTests.Payloads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.FunctionalTests
{
    [TestClass]
    public class SlackClientTest
    {
        private HttpClient client;

        [TestMethod]
        public async Task SendSlackMessage()
        {
            try
            {
                await SendMessage();
                var response = await ReceiveMessage();
                Assert.AreEqual("Echo: Test6", response);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<string> ReceiveMessage()
        {
            client = new HttpClient();
            var requestUri = "https://slack.com/api/conversations.history?token=XXXXX&channel=XXXXX";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri),
            };

            var httpResponse = await client.SendAsync(request).ConfigureAwait(false);

            var response = httpResponse.Content.ReadAsStringAsync().Result;
            var chatHistory = JsonConvert.DeserializeObject<SlackHistoryRetrieve>(response).Messages[0].Text;
            
            return chatHistory;
        }

        private async Task<string> SendMessage()
        {
            var requestUri = "XXXX";
            var json = File.ReadAllText(Directory.GetCurrentDirectory() + @"/Payloads/SlackSendGreets.json").Replace(" ", string.Empty);
            var timestamp = "fakeStamp";

            var slackSignature = string.Empty;

            object[] signature = { "v0", timestamp.ToString(), json };
            var baseString = string.Join(":", signature);
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes("XXXXXX")))
            {
                var hashArray = hmac.ComputeHash(Encoding.UTF8.GetBytes(baseString));

                var hash = string.Concat("v0=", BitConverter.ToString(hashArray).Replace("-", string.Empty)).ToUpperInvariant();
                slackSignature = hash;
            }

            client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Slack-Request-Timestamp", "fakeStamp");
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Slack-Signature", slackSignature);

            var requestMessage = new HttpRequestMessage(new HttpMethod("POST"), requestUri);

            requestMessage.Content = new StringContent(json);
            var result = await client.PostAsync(requestUri, requestMessage.Content);
            return result.StatusCode.ToString();
        }
    }
}
