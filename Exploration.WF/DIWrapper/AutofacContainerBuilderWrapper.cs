namespace Exploration.WF.DIWrapper
{
    using Autofac;

    internal class AutofacContainerBuilderWrapper : IContainerBuilderWrapper
    {
        private readonly ContainerBuilder containerBuilder;

        internal AutofacContainerBuilderWrapper()
        {
            this.containerBuilder = new ContainerBuilder();
        }

        public void Register<TType, TInterface>()
        {
            this.containerBuilder.RegisterType<TType>().As<TInterface>();
        }

        public void Run()
        {
            var container = this.containerBuilder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                // var app = scope.Resolve<Application>();
                // app.Run();
            }
        }
    }
}