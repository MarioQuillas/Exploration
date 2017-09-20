namespace Exploration.Framework.Presentation.Views
{
    using System.Drawing;

    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Runtime.Utils;

    internal class NoInstrumentView : IView
    {
        public void Render()
        {
            Logger.Log("The instrument does not  exist", Color.Red);
        }
    }
}