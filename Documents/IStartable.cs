using System.Threading.Tasks;

namespace Documents
{
    public interface IStartable
    {
        void Start();
    }

    public interface IMonitorQueue
    {
        int GetCount();
        string GetName();

        Task GetTask();
    }
}