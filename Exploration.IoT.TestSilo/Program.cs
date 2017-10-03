namespace Exploration.IoT.TestSilo
{
    using System;

    using Exploration.IoT.GrainClasses;

    using Orleans;
    using Orleans.Runtime.Configuration;
    using Orleans.Runtime.Host;

    using Exploration.IoT.GrainClasses;
    using Exploration.IoT.GrainInterfaces;

    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // First, configure and start a local silo
            var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
            var silo = new SiloHost("TestSilo", siloConfig);
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();

            Console.WriteLine("Silo started.");

            // Then configure and connect a client.
            var clientConfig = ClientConfiguration.LocalhostSilo();
            var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
            client.Connect().Wait();

            Console.WriteLine("Client connected.");

            //
            // This is the place for your test code.
            //

            var grain = client.GetGrain<IDeviceGrain>((long)0);
            while (true)
            {
                grain.SetTemperature(double.Parse(Console.ReadLine()));
            }

            Console.WriteLine("\nPress Enter to terminate...");
            Console.ReadLine();

            // Shut down
            client.Close();
            silo.ShutdownOrleansSilo();
        }
    }
}
