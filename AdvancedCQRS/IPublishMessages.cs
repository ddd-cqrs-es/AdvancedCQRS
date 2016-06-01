namespace AdvancedCQRS
{
    public interface IPublishMessages
    {
        void Publish(Message message);
    }
}