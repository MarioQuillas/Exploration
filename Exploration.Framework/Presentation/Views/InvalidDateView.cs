namespace Exploration.Framework.Presentation.Views
{
    using System.Drawing;

    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Presentation.CommandResults;
    using Exploration.Framework.Runtime.Utils;

    internal class InvalidDateView : IView
    {
        public InvalidDateView(InvalidDateResult invalidDateResult)
        {
            this.InvalidDateResult = invalidDateResult;
        }

        public InvalidDateResult InvalidDateResult { get; }

        public void Render()
        {
            Logger.Log("{0} is an invalid id.", this.InvalidDateResult.AskedDate, Color.DarkRed);
            Logger.Log("Please enter a date in the following format : {0}", "dd/mm/yyyy", Color.DarkOrange);
        }
    }
}