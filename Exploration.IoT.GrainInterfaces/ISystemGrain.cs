namespace Exploration.IoT.GrainInterfaces
{
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using Orleans;

    
    public interface ISystemGrain : IGrainWithIntegerKey
    {
        Task SetTemperature(TemperatureReading reading);

        Task Subscribe(ISystemObserver observer);

        Task Unsubscribe(ISystemObserver observer);
    }
}