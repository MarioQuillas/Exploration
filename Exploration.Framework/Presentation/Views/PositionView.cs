namespace Exploration.Framework.Presentation.Views
{
    using System.Drawing;

    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Presentation.CommandResults;
    using Exploration.Framework.Runtime.Utils;

    internal class PositionView : IView
    {
        private readonly PositionResult positionResult;

        public PositionView(PositionResult positionResult)
        {
            this.positionResult = positionResult;
        }

        public void Render()
        {
            Logger.Log("The position exists");
            Logger.Log("Name : {0}", this.positionResult.FolioName, Color.DarkOrchid);
        }
    }
}