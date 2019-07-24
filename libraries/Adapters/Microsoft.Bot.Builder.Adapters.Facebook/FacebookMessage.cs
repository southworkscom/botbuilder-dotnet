using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Bot.Builder.Adapters.Facebook
{
    public class FacebookMessage
    {
        public FacebookMessage(string recipientId, Message message, string messagingtype, string tag = null, string notificationType = null, string personalId = null, string senderAction = null, string senderId = null)
        {
            this.RecipientId = recipientId;
            this.Message = message;
            this.MessagingType = messagingtype;
            this.Tag = tag;
            this.NotificationType = notificationType;
            this.PersonaId = personalId;
            this.SenderAction = senderAction;
            this.SenderId = senderId;
        }

        public string RecipientId { get; set; }

        public string SenderId { get; set; }

        public Message Message { get; set; }

        public string MessagingType { get; set; }

        public string Tag { get; set; }

        public string NotificationType { get; set; }

        public string PersonaId { get; set; }

        public string SenderAction { get; set; }
    }
}
