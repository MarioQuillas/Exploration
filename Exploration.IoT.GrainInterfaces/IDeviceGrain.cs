namespace Exploration.IoT.GrainInterfaces
{
    using System.Threading.Tasks;

    using Orleans;

    /// <summary>
    /// Grain interface IDeviceGrain
    /// </summary>
    public interface IDeviceGrain : IGrainWithIntegerKey
    {
        Task SetTemperature(double value);

        //Task JoinSystem(string name);
    }
}
