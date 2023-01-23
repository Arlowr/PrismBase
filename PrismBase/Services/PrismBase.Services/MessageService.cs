using PrismBase.Services.Interfaces;

namespace PrismBase.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
