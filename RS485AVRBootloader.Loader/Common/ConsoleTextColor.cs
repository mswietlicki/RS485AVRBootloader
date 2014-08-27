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
}