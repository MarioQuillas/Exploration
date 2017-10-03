namespace Exploration.IoT.GrainClasses
{
    using System;
    using System.Threading.Tasks;

    using Exploration.IoT.GrainInterfaces;

    using Orleans;
    using Orleans.Providers;

    /// <summary>
    /// Grain implementation class DeviceGrain.
    /// </summary>
    [StorageProvider(ProviderName = "OrleansAzureTableStorage")]
    public class DeviceGrain : Grain<DeviceGrainState>, IDeviceGrain
    {
        //private double lastValue;

        public override Task OnActivateAsync()
        {
            var id = this.GetPrimaryKeyLong();
            Console.WriteLine("Activated {0}", id);
            Console.WriteLine("Activated state = {0}", this.State.LastValue);
            return base.OnActivateAsync();
        }

        public async Task SetTemperature(double value)
        {
            if (this.State.LastValue < 100 && value >= 100)
                Console.WriteLine("High temperature recorded {0}", value);

            if (this.State.LastValue != value)
            {
                this.State.LastValue = value;
                await this.WriteStateAsync();
            }
        }
    }
}
