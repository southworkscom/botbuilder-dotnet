// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Builder.Adapters.Slack.TestBot
{
    public class SimpleSlackAdapterOptions : SlackAdapterOptions
    {
        public SimpleSlackAdapterOptions(string verificationToken, string botToken, string signingSecret)
        {
            VerificationToken = verificationToken;
            BotToken = botToken;
            ClientSigningSecret = signingSecret;
        }
    }
}
