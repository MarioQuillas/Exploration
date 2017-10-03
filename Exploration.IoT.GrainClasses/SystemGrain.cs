namespace Exploration.IoT.GrainClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Exploration.IoT.GrainInterfaces;

    using Orleans;

    public class SystemGrain : Grain, ISystemGrain
    {
        private Dictionary<long, double> temperatures;

        private ObserverSubscriptionManager<ISystemObserver> observers;

        public override Task OnActivateAsync()
        {
            this.temperatures = new Dictionary<long, double>();
            this.observers = new ObserverSubscriptionManager<ISystemObserver>();

            var timer = this.RegisterTimer(this.Callback, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

            return base.OnActivateAsync();
        }

        private Task Callback(object callbackState)
        {
            var average = this.temperatures.Values.Average();
            if (average > 100)
            {
                //Console.WriteLine("System temperature is high !!!");
                this.observers.Notify(x => x.HighTemperature(average));

            }

            return Task.CompletedTask;
        }

        public Task SetTemperature(TemperatureReading reading)
        {
            this.temperatures[reading.DeviceId] = reading.Value;

            //if (this.temperatures.Keys.Contains(deviceId))
            //{
            //    this.temperatures[deviceId] = value;
            //}

            //if (this.temperatures.Values.Average() > 100)
            //    Console.WriteLine("System temperature is high !!!");

            return Task.CompletedTask;
        }

        public Task Subscribe(ISystemObserver observer)
        {
            this.observers.Subscribe(observer);
            return Task.CompletedTask;
        }

        public Task Unsubscribe(ISystemObserver observer)
        {
            this.observers.Unsubscribe(observer);
            return Task.CompletedTask;
        }
    }
}