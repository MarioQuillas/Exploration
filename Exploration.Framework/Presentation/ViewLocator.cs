namespace Exploration.Framework.Presentation
{
    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Runtime;

    internal class ViewLocator : ServiceLocator<ICommandResult, IView>
    {
    }
}