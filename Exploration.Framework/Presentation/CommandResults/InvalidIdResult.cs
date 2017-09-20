namespace Exploration.Framework.Presentation.CommandResults
{
    using Exploration.Framework.Presentation.Abstractions;

    internal class InvalidIdResult : ICommandResult
    {
        public InvalidIdResult(string enteredId)
        {
            this.EnteredId = enteredId;
        }

        public string EnteredId { get; }
    }
}