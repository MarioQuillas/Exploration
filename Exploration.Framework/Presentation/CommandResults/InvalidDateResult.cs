namespace Exploration.Framework.Presentation.CommandResults
{
    using Exploration.Framework.Presentation.Abstractions;

    internal class InvalidDateResult : ICommandResult
    {
        public InvalidDateResult(string askedDate)
        {
            this.AskedDate = askedDate;
        }

        public string AskedDate { get; }
    }
}