namespace Exploration.Framework.Presentation.CommandResults
{
    using Exploration.Framework.Presentation.Abstractions;

    public class PositionResult : ICommandResult
    {
        public PositionResult(string folioName)
        {
            this.FolioName = folioName;
        }

        public string FolioName { get; }
    }
}