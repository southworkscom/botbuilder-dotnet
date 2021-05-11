using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Bot.Builder.Adapters.Slack
{
    public class SkillToSlackAdapter : BotFrameworkHttpAdapter
    {
        private SlackAdapter _slackAdapter;

        public SkillToSlackAdapter(IConfiguration configuration, SlackAdapter slackAdapter)
            : base(configuration)
        {
            _slackAdapter = slackAdapter;
        }

        public override async Task<ResourceResponse[]> SendActivitiesAsync(ITurnContext turnContext, Activity[] activities, CancellationToken cancellationToken)
        {
            if (activities[0].ChannelId != "slack")
            {
                return await base.SendActivitiesAsync(turnContext, activities, cancellationToken).ConfigureAwait(false);
            }

            return await _slackAdapter.SendActivitiesAsync(turnContext, activities, cancellationToken).ConfigureAwait(false);
        }

        public override Task<ResourceResponse> UpdateActivityAsync(ITurnContext turnContext, Activity activity, CancellationToken cancellationToken)
        {
            if (activity.ChannelId != "slack")
            {
                return base.UpdateActivityAsync(turnContext, activity, cancellationToken);
            }            

            return _slackAdapter.UpdateActivityAsync(turnContext, activity, cancellationToken);
        }
    }
}
