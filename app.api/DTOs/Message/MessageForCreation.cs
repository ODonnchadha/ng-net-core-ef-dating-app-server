using System;

namespace app.api.DTOs
{
    public class MessageForCreation
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }
        public MessageForCreation() => this.MessageSent = DateTime.Now;
    }
}
