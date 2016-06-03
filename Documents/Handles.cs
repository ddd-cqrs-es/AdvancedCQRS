namespace Documents
{
    public interface Handles { }
    public interface Handles<in T> :Handles where T : IMessage
    {
        void Handle(T message);
    }
}