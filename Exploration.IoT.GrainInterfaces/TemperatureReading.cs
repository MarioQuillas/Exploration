namespace Exploration.IoT.GrainInterfaces
{
    using System;

    using Orleans.Concurrency;

    [Immutable]
    public class TemperatureReading
    {
        public double Value { get; set; }

        public long DeviceId { get; set; }

        public DateTime Time { get; set; }
    }
}