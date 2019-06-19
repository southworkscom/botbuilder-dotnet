// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotKit.Adapters.Slack
{
    public class SlackResponse
    {
        public bool ok;
        public string channel;
        public string ts;
        public SlackMessage message;
    }
}
