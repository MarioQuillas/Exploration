namespace Exploration.Framework.Presentation.Views
{
    using System.Drawing;

    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Presentation.CommandResults;
    using Exploration.Framework.Runtime.Utils;

    internal class InvalidIdView : IView
    {
        public InvalidIdView(InvalidIdResult invalidIdResult)
        {
            this.InvalidIdResult = invalidIdResult;
        }

        public InvalidIdResult InvalidIdResult { get; }

        public void Render()
        {
            Logger.Log("{0} is an invalid id.", this.InvalidIdResult.EnteredId, Color.DarkRed);
        }
    }
}