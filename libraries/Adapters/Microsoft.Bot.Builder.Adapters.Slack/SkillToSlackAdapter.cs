using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.Bot.Builder.Adapters.Slack
{
    public class SkillToSlackAdapter : BotFrameworkHttpAdapter
    {
        private const string SlackVerificationTokenKey = "SlackVerificationToken";
        private const string SlackBotTokenKey = "SlackBotToken";
        private const string SlackClientSigningSecretKey = "SlackClientSigningSecret";

        private readonly SlackClientWrapper _slackClient;
        private readonly ILogger _logger;
        private readonly SlackAdapterOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillToSlackAdapter"/> class using configuration settings.
        /// </summary>
        /// <param name="configuration">An <see cref="IConfiguration"/> instance.</param>
        /// <remarks>
        /// The configuration keys are:
        /// SlackVerificationToken: A token for validating the origin of incoming webhooks.
        /// SlackBotToken: A token for a bot to work on a single workspace.
        /// SlackClientSigningSecret: The token used to validate that incoming webhooks are originated from Slack.
        /// </remarks>
        /// <param name="options">An instance of <see cref="SlackAdapterOptions"/>.</param>
        /// <param name="logger">The ILogger implementation this adapter should use.</param>
        public SkillToSlackAdapter(IConfiguration configuration, SlackAdapterOptions options = null, ILogger logger = null)
            : this(new SlackClientWrapper(new SlackClientWrapperOptions(configuration[SlackVerificationTokenKey], configuration[SlackBotTokenKey], configuration[SlackClientSigningSecretKey])), options, configuration, logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillToSlackAdapter"/> class.
        /// Creates a Slack adapter.
        /// </summary>
        /// <param name="adapterOptions">The adapter options to be used when connecting to the Slack API.</param>
        /// <param name="configuration">TODO.</param>
        /// <param name="logger">The ILogger implementation this adapter should use.</param>
        /// <param name="slackClient">The SlackClientWrapper used to connect to the Slack API.</param>
        public SkillToSlackAdapter(SlackClientWrapper slackClient, SlackAdapterOptions adapterOptions, IConfiguration configuration, ILogger logger = null)
            : base(configuration)
        {
            _slackClient = slackClient ?? throw new ArgumentNullException(nameof(adapterOptions));
            _logger = logger ?? NullLogger.Instance;
            _options = adapterOptions ?? new SlackAdapterOptions();
        }

        /// <summary>
        /// Standard BotBuilder adapter method to send a message from the bot to the messaging API.
        /// </summary>
        /// <param name="turnContext">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="activities">An array of outgoing activities to be sent back to the messaging API.</param>
        /// <param name="cancellationToken">A cancellation token for the task.</param>
        /// <returns>An array of <see cref="ResourceResponse"/> objects containing the IDs that Slack assigned to the sent messages.</returns>
        public override async Task<ResourceResponse[]> SendActivitiesAsync(ITurnContext turnContext, Activity[] activities, CancellationToken cancellationToken)
        {
            if (activities[0].ChannelId != "slack")
            {
                return await base.SendActivitiesAsync(turnContext, activities, cancellationToken).ConfigureAwait(false);
            }

            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (activities == null)
            {
                throw new ArgumentNullException(nameof(activities));
            }

            var responses = new List<ResourceResponse>();

            foreach (var activity in activities)
            {
                if (activity.Type != ActivityTypes.Message)
                {
                    _logger.LogTrace($"Unsupported Activity Type: '{activity.Type}'. Only Activities of type 'Message' are supported.");
                }
                else
                {
                    var message = SlackHelper.ActivityToSlack(activity);

                    var slackResponse = await _slackClient.PostMessageAsync(message, cancellationToken)
                        .ConfigureAwait(false);

                    if (slackResponse != null && slackResponse.Ok)
                    {
                        var resourceResponse = new ActivityResourceResponse()
                        {
                            Id = slackResponse.Ts,
                            ActivityId = slackResponse.Ts,
                            Conversation = new ConversationAccount() { Id = slackResponse.Channel, },
                        };
                        responses.Add(resourceResponse);
                    }
                }
            }

            return responses.ToArray();
        }
    }
}
