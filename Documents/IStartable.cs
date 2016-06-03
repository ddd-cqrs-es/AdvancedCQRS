using System.Threading.Tasks;

namespace Restaurant.ProcessManagerExample
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