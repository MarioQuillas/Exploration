namespace Exploration.IoT.TestSilo
{
    using System;

    using Exploration.IoT.GrainInterfaces;

    public class SystemObserver : ISystemObserver
    {
        public void HighTemperature(double value)
        {
            Console.WriteLine("Observed a high system temperature {0}", value);
        }
    }
}