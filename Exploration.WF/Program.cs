namespace Exploration.WF
{
    using System;

    using Autofac;

    class Program
    {
        static void Main(string[] args)
        {
            var container = SetupDependencyInjection();
            using (var scope = container.BeginLifetimeScope())
            {
                var application = scope.Resolve<Application>();
                application.Run();
            }

            Console.WriteLine("Press any key to close the program...");
            Console.ReadKey();
        }

        static IContainer SetupDependencyInjection()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Application>(); // root of the application

            // register the types here
            builder.RegisterType<InstanceStoreSetupper>().SingleInstance();

            // end registration of types
            return builder.Build();
        }
    }
}