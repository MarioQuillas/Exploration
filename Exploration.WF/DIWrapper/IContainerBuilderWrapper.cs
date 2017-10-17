namespace Exploration.WF.DIWrapper
{
    internal interface IContainerBuilderWrapper
    {
        void Register<TType, TInterface>();

        void Run();
    }
}