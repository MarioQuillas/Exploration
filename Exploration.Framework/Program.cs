namespace Exploration.Framework
{
    using Exploration.Framework.Presentation;
    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Presentation.CommandResults;
    using Exploration.Framework.Presentation.Views;

    internal class Program
    {
        public const int MdsTimeOut = 5 * 60 * 1000;

        private static void Main(string[] args)
        {
            var viewLocator = SetupViewLocator();
            var ui = new UserInterface(viewLocator);

            while (ui.ReadCommand())
            {
                ui.ExecuteCommand();
            }
        }

        private static bool Selector<TCommand>(ICommandResult cmdResult)
            where TCommand : ICommandResult
        {
            return cmdResult != null && cmdResult.GetType() == typeof(TCommand);
        }

        private static ViewLocator SetupViewLocator()
        {
            var viewLocator = new ViewLocator();

            viewLocator.RegisterService(Selector<InvalidIdResult>, obj => new InvalidIdView((InvalidIdResult)obj));

            viewLocator.RegisterService(
                Selector<InvalidDateResult>,
                obj => new InvalidDateView((InvalidDateResult)obj));

            viewLocator.RegisterService(Selector<EmptyResult>, obj => new EmptyView());

            viewLocator.RegisterService(Selector<InstrumentResult>, obj => new InstrumentView((InstrumentResult)obj));

            viewLocator.RegisterService(Selector<NoResult>, obj => new NoInstrumentView());

            viewLocator.RegisterService(Selector<PositionResult>, obj => new PositionView((PositionResult)obj));

            return viewLocator;
        }
    }
}