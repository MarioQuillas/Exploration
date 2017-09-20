namespace Exploration.Framework.Presentation.Commands
{
    using System;
    using System.Drawing;

    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Presentation.CommandResults;
    using Exploration.Framework.Runtime.Utils;

    public class GetPositionCommand : ICommand
    {
        public ICommandResult Execute()
        {
            Logger.Write("Enter an position id: ");
            var askedId = Logger.ReadLine(Color.LawnGreen);

            if (!int.TryParse(askedId, out var tryParseInputPosition)) return new InvalidIdResult(askedId);

            Logger.Write("Enter as of date: ");
            var askedDate = Logger.ReadLine(Color.LawnGreen);

            return !DateTime.TryParse(askedDate, out var tryParseInputDate)
                       ? new InvalidDateResult(askedDate)
                       : this.GetPosition(tryParseInputPosition, tryParseInputDate);
        }

        private ICommandResult GetPosition(int positionId, DateTime asOfDate)
        {
            //// 15751, 189727
            //var dataRetriever = new DataRetriever();

            //Folio folio = null;

            //// TODO : this is writing to the console
            //using (var timeBenchMarker = new TimeBenchmarker("Time : "))
            //{
            //    folio = dataRetriever.GetFolio(positionId, asOfDate);
            //}

            //if (folio != null) return new PositionResult(folio.Name);
            return new NoResult();
        }
    }
}