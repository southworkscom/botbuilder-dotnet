using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Bot.Builder.Adapters.Twilio
{
    public interface ITwilioApi
    {
        void LogIn(string username, string password);

        Task<object> CreateMessageResourceAsync(object messageOptions);
    }
}
