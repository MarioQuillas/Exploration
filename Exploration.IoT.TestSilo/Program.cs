namespace Exploration.IoT.TestSilo
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    using Exploration.IoT.GrainClasses;

    using Orleans;
    using Orleans.Runtime.Configuration;
    using Orleans.Runtime.Host;

    using Exploration.IoT.GrainClasses;
    using Exploration.IoT.GrainInterfaces;

    using Orleans.Storage;

    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            var properties = new Dictionary<string, string>()
                                 {
                                     //["AdoInvariant"] = "System.Data.SqlClient",
                                     //["DataConnectionString"] = "Data Source=SQLLFISDEV,1460;Initial Catalog=TardisFlow;Persist Security Info=True;User ID=TardisFlowUsrRW;Password=6Sk&>3R-",
                                     //["UseJsonFormat"] = "true"

                                     ["DataConnectionString"] = "UseDevelopmentStorage=true",
                                 };

            // First, configure and start a local silo
            var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
            //siloConfig.Globals.RegisterStorageProvider<AdoNetStorageProvider>("OrleansSqlStorage", properties);


            siloConfig
                .Globals
                .RegisterStorageProvider<AzureTableStorage>("OrleansAzureTableStorage", properties);

            //var custom = new Dictionary<string, object>()
            //                 {
            //                     ["directory"] = "wdqsdf"
            //                 };

            //siloConfig
            //    .Globals
            //    .RegisterStorageProvider<Exploration.IoT.FileStorage.FileStorageProvider>("sd", custom);

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