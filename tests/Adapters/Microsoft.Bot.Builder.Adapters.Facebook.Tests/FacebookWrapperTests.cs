using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Schema;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.Bot.Builder.Adapters.Facebook.Tests
{
    public class FacebookWrapperTests
    {
        private readonly FacebookAdapterOptions _testOptions = new FacebookAdapterOptions("TestVerifyToken", "TestAppSecret", "TestAccessToken");

        [Fact]
        public void GetAppSecretProofShouldAlwaysReturnAStringWith64Characters()
        {
            const int SecretProofLength = 64;
            var facebookwrapper = new FacebookClientWrapper(_testOptions);
            var secretProof = facebookwrapper.GetAppSecretProof();

            Assert.NotNull(secretProof);
            Assert.Equal(SecretProofLength, secretProof.Length);
        }

        [Fact]
        public void VerifySignatureShouldThrowErrorWithNullRequest()
        {
            var facebookwrapper = new FacebookClientWrapper(_testOptions);

            Assert.Throws<ArgumentNullException>(() => { facebookwrapper.VerifySignature(null, string.Empty); });
        }

        [Fact]
        public void VerifySignatureShouldReturnTrue()
        {
            const string RequestHash = "SHA1=32ECC29689D860D68A713FF5BA8D7B787C5E8C80";

            var facebookwrapper = new FacebookClientWrapper(_testOptions);
            var request = new Mock<HttpRequest>();
            var stringifyBody = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Files\RequestResponse.json");
            request.SetupGet(req => req.Headers[It.IsAny<string>()]).Returns(RequestHash);

            Assert.True(facebookwrapper.VerifySignature(request.Object, stringifyBody));
        }

        [Fact]
        public async void ShouldReturnAnEmptyStringWithWrongPath()
        {
            var facebookMessageJson = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Files\FacebookMessages.json");
            var facebookMessage = JsonConvert.DeserializeObject<List<FacebookMessage>>(facebookMessageJson)[5];

            var cancellationTokenJson = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Files\CancellationToken.json");
            var cancellationToken = JsonConvert.DeserializeObject<CancellationToken>(cancellationTokenJson);

            var facebookwrapper = new FacebookClientWrapper(_testOptions);

            var response = await facebookwrapper.SendMessageAsync("wrongPath", facebookMessage, null, cancellationToken);

            Assert.Equal(string.Empty, response);
        }
    }
}
