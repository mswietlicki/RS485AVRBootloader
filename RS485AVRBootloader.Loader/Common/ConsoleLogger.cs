using System;

namespace SerialAVRBootloader.Loader.Common
{
    public class ConsoleTextColor : IDisposable
    {
        private readonly ConsoleColor _oldColor;

        public ConsoleTextColor(ConsoleColor color)
        {
            _oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
        }

        public void Dispose()
        {
            Console.ForegroundColor = _oldColor;
        }
    }


    public class ConsoleLogger : ILogger
    {
        public void WriteLine(string rawInfo)
        {
            Console.WriteLine(rawInfo);
        }

        public void WriteError(Exception exception)
        {
            using (new ConsoleTextColor(ConsoleColor.Red))
            {
                Console.Error.WriteLine(exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
            }
        }
    }
}