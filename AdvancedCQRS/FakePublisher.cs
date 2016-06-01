using System.Collections.Generic;
using System.Linq;

namespace AdvancedCQRS
{
    public class FakePublisher : IPublishMessages
    {
        public readonly List<Message> PublishedMessages = new List<Message>();

        public T FindMessage<T>() where T : Message
        {
            return PublishedMessages.OfType<T>().FirstOrDefault();
        }
        
        public void Publish(Message message)
        {
            PublishedMessages.Add(message);
        }
        
    }
}