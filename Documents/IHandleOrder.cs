namespace Documents
{
    public interface IHandle { }
    public interface IHandle<in T> :IHandle where T : IMessage
    {
        void Handle(T order);
    }
}