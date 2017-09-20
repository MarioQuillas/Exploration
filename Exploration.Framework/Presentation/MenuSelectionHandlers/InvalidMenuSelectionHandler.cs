namespace Exploration.Framework.Presentation.MenuSelectionHandlers
{
    using System.Drawing;

    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Runtime.Utils;

    internal class InvalidMenuSelectionHandler : IMenuSelectionHandler
    {
        public void DisplayContent()
        {
            Logger.Log("Invalid selected menu item.", Color.Bisque);
            Logger.Log("Press ENTER to try again...");
            Logger.ReadLine();
        }
    }
}