namespace Exploration.IoT.GrainClasses
{
    using System.Threading.Tasks;

    using Exploration.IoT.GrainInterfaces;

    using Orleans;
    using Orleans.Concurrency;

    //[StatelessWorker]
    [Reentrant]
    public class GrainDecoder : Grain, IGrainDecoder
    {
        public Task Decode(string message)
        {
            var parts = message.Split(',');
            var grain = this.GrainFactory.GetGrain<IDeviceGrain>(long.Parse(parts[0]));
            return grain.SetTemperature(double.Parse(parts[1]));
        }
    }
}