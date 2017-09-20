namespace Exploration.Framework.Presentation.Commands
{
    using System.Drawing;

    using Exploration.Framework.Presentation.Abstractions;
    using Exploration.Framework.Presentation.CommandResults;
    using Exploration.Framework.Runtime.Utils;

    public class GetInstrumentCommand : ICommand
    {
        public ICommandResult Execute()
        {
            Logger.Write("Enter an instrument id: ");

            var askedId = Logger.ReadLine(Color.LawnGreen);

            long tryParseInputInstrument;

            return !long.TryParse(askedId, out tryParseInputInstrument)
                       ? new InvalidIdResult(askedId)
                       : this.GetInstrument(tryParseInputInstrument);
        }

        private ICommandResult GetInstrument(long instrumentId)
        {
            // 15751, 189727
            //var dataRetriever = new DataRetriever();

            //Instrument instrument = null;

            //// TODO : this is writing to the console
            //using (var timeBenchMarker = new TimeBenchmarker("Time : "))
            //{
            //    // using (var spinner = new ConsoleSpinner(10, 10))
            //    instrument = dataRetriever.GetInstrument(instrumentId);
            //}

            //if (instrument != null) return new InstrumentResult(instrument.Name);
            return new NoResult();

            // var titi = JsonConvert.SerializeObject(instrument);

            // var volEuroStoxx50 = dataRetriever
            // .GetVolatility(euroStoxx50, DateTime.Today, EDataSource.LightTrade);

            // using (var mdClientAdapter = new MarketDataClientAdapter(Program.MdsTimeOut))
            // {
            // var toto = mdClientAdapter.GetInstrumentList(
            // Enumerable.Repeat(15751, 1).ToList(), true);

            // var ff = JsonConvert.SerializeObject(toto[0]);
            // }
        }
    }
}