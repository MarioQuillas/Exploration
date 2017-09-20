namespace Exploration.Framework.Runtime.Utils
{
    using System.Drawing;
    using System.Linq;

    using Colorful;

    internal static class Logger
    {
        public static void Clear()
        {
            Console.Clear();
        }

        public static void Log(string text, params string[] args)
        {
            Console.WriteLine(string.Format(text, args));
        }

        public static void Log(string text, Color color)
        {
            Console.WriteLine(text, color);
        }

        public static void Log(string text, string arg0, params Color[] colors)
        {
            Log(text, new[] { arg0 }, colors);
        }

        public static void Log(string text, string arg0, string arg1, params Color[] colors)
        {
            Log(text, new[] { arg0, arg1 }, colors);
        }

        public static void Log(string text, string arg0, string arg1, string arg2, params Color[] colors)
        {
            Log(text, new[] { arg0, arg1, arg2 }, colors);
        }

        public static void Log(string text, string arg0, string arg1, string arg2, string arg3, params Color[] colors)
        {
            Log(text, new[] { arg0, arg1, arg2, arg3 }, colors);
        }

        public static char ReadKey(bool displayKey)
        {
            return Console.ReadKey(displayKey).KeyChar;
        }

        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public static string ReadLine(Color color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            var result = Console.ReadLine();

            Console.ForegroundColor = previousColor;

            return result;
        }

        public static void Write(string text)
        {
            Console.Write(text);
        }

        public static void Write(string text, Color color)
        {
            Console.Write(text, color);
        }

        private static void Log(string text, string[] args, Color[] colors)
        {
            if (args == null || colors == null || colors.Length != args.Length)
            {
                Console.WriteLine(args != null ? string.Format(text, args) : text);
            }
            else
            {
                var formatters = args.Zip(colors, (arg, color) => new Formatter(arg, color)).ToArray();

                Console.WriteLineFormatted(text, Color.Chocolate, formatters);
            }
        }
    }
}