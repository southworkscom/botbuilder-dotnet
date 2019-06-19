// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotKit.Adapters.Slack
{
    public class SlackMessage
    {
        public string type;
        public string subtype;
        public string text;
        public string ts;
        public string username;
        public string bot_id;
        public string thread_ts;
    }
}
