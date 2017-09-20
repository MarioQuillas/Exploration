namespace Exploration.Framework.Runtime.Utils
{
    using System;
    using System.Diagnostics;
    using System.Drawing;

    internal class TimeBenchmarker : IDisposable
    {
        private readonly Color messageConsoleColor;

        private readonly string messageName;

        private readonly Color timeConsoleColor;

        private readonly Stopwatch timer = new Stopwatch();

        public TimeBenchmarker(string messageName)
        {
            this.messageName = messageName;
            this.timer.Start();
            this.messageConsoleColor = Color.Cyan;
            this.timeConsoleColor = Color.Yellow;
        }

        public void Dispose()
        {
            this.timer.Stop();
            Logger.Log(
                "{0} {1}",
                this.messageName,
                this.timer.Elapsed.ToString(),
                this.messageConsoleColor,
                this.timeConsoleColor);
        }
    }
}