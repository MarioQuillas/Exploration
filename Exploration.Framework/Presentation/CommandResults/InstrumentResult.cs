namespace Exploration.Framework.Presentation.CommandResults
{
    using Exploration.Framework.Presentation.Abstractions;

    internal class InstrumentResult : ICommandResult
    {
        public InstrumentResult(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}