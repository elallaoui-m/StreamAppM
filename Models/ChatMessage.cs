

namespace StreamApp.Models
{
    public class ChatMessage
    {
        public string sender { get; set; }
        public string content { get; set; }
        public string timestamp { get; set; }


        public override string ToString()
        {
            return "Message{" +
                "sender='" + sender + '\'' +
                ", content='" + content + '\'' +
                ", timestamp='" + timestamp + '\'' +
                '}';
        }
    }
}
