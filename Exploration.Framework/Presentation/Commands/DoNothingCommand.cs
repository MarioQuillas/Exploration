namespace Exploration.Framework.Presentation.Commands
{
    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Presentation.CommandResults;

    internal class DoNothingCommand : ICommand
    {
        public ICommandResult Execute()
        {
            return new EmptyResult();
        }
    }
}