namespace Exploration.Framework.Presentation
{
    using System.Collections.Generic;
    using System.Linq;

    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Presentation.Commands;
    using Exploration.Framework.Presentation.MenuSelectionHandlers;
    using Exploration.Framework.Runtime.Utils;

    internal class UserInterface : IUserInterface
    {
        private readonly IEnumerable<MenuItem> menu;

        private readonly ViewLocator viewLocator;

        private IMenuSelectionHandler menuSelectionHandler;

        public UserInterface(ViewLocator viewLocator)
        {
            this.menuSelectionHandler = new EmptyMenuSelectionHandler();
            this.viewLocator = viewLocator;
            this.menu = new[]
                            {
                                MenuItem.CreateNonTerminal(
                                    "Get Intrument by Id",
                                    'I',
                                    new GetInstrumentCommand(),
                                    () => true),
                                MenuItem.CreateNonTerminal(
                                    "Get Position by Id",
                                    'P',
                                    new GetPositionCommand(),
                                    () => true),
                                MenuItem.CreateTerminal("Quit", 'Q')
                            };
        }

        public void ExecuteCommand()
        {
            this.menuSelectionHandler.DisplayContent();
        }

        public bool ReadCommand()
        {
            this.RefreshDisplay();
            this.ShowMenu();

            var keyChar = Logger.ReadKey(true);

            var selectedMenuItem = this.menu.SingleOrDefault(item => item.MatchesKey(keyChar));

            if (selectedMenuItem == null)
            {
                this.menuSelectionHandler = new InvalidMenuSelectionHandler();
                return true;
            }

            if (selectedMenuItem.IsTerminalCommand) return false;

            this.menuSelectionHandler = new ValidMenuSelectionHandler(
                selectedMenuItem.Command,
                this.viewLocator,
                this.Render);

            return true;
        }

        private void RefreshDisplay()
        {
            Logger.Clear();
        }

        private void Render(IView view)
        {
            var message = $"Rendering {view.GetType().Name}";
            var delimiter = new string('-', message.Length);

            Logger.Log("\n{0}\n{1}\n{0}\n", delimiter, message);

            view.Render();

            Logger.Log("\n{0}", delimiter);
        }

        private void ShowMenu()
        {
            Logger.Log("Select an operation:");

            foreach (var menuItem in this.menu) menuItem.Display();

            Logger.Log(string.Empty);
        }
    }
}