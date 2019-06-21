// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotKit.Adapters.Slack
{
    public class SlackResponse
    {
        public bool Ok;

        public string Channel;

        public string TS;

        public SlackMessage Message;
    }
}
