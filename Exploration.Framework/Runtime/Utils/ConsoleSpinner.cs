namespace Exploration.Framework.Runtime.Utils
{
    using System;
    using System.Drawing;
    using System.Threading;

    using Console = Colorful.Console;

    internal class ConsoleSpinner : IDisposable
    {
        private const string Sequence = @"/-\|";

        private readonly int delay;

        private readonly int left;

        private readonly Thread thread;

        private readonly int top;

        private bool active;

        private int counter;

        public ConsoleSpinner(int left, int top, int delay = 100)
        {
            this.left = left;
            this.top = top;
            this.delay = delay;
            this.thread = new Thread(this.Spin);
            this.Start();
        }

        public void Dispose()
        {
            this.Stop();
        }

        public void Start()
        {
            this.active = true;
            if (!this.thread.IsAlive) this.thread.Start();
        }

        public void Stop()
        {
            this.active = false;
            this.Draw(' ');
        }

        private void Draw(char c)
        {
            Console.SetCursorPosition(this.left, this.top);
            Console.ForegroundColor = Color.Green;
            Console.Write(c);
        }

        private void Spin()
        {
            while (this.active)
            {
                this.Turn();
                Thread.Sleep(this.delay);
            }
        }

        private void Turn()
        {
            this.Draw(Sequence[++this.counter % Sequence.Length]);
        }
    }
}