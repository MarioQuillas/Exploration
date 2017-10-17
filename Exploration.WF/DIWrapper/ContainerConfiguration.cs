namespace Exploration.WF.DIWrapper
{
    internal class ContainerConfiguration
    {
        private readonly IContainerBuilderWrapper containerBuilderWrapper;

        internal ContainerConfiguration(IContainerBuilderWrapper containerBuilderWrapper)
        {
            this.containerBuilderWrapper = containerBuilderWrapper;
        }

        internal void Run()
        {
            this.Register();
            this.containerBuilderWrapper.Run();
        }

        private void Register()
        {
            // this.containerBuilderWrapper.Register<Application, Application>();
            // register
        }
    }
}