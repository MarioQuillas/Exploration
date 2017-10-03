namespace Exploration.IoT.GrainClasses
{
    using System;
    using System.Threading.Tasks;

    using Exploration.IoT.GrainInterfaces;

    using Orleans;
    using Orleans.Concurrency;
    using Orleans.Providers;

    /// <summary>
    /// Grain implementation class DeviceGrain.
    /// </summary>
    //[StorageProvider(ProviderName = "OrleansAzureTableStorage")]
    [Reentrant]
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

            var systemGrain = this.GrainFactory.GetGrain<ISystemGrain>(0);//, this.State.System);
            var reading = new TemperatureReading()
                              {
                                  DeviceId =  this.GetPrimaryKeyLong(),
                                  Time = DateTime.UtcNow,
                                  Value = value
                              };

            await systemGrain.SetTemperature(reading);
        }

        public Task JoinSystem(string name)
        {
            this.State.System = name;
            return this.WriteStateAsync();
        }
    }
}
