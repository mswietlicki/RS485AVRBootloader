using System;

namespace SerialAVRBootloader.Loader.Common
{
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

        public void ProgramOutput(string text)
        {
            using (new ConsoleTextColor(ConsoleColor.Green))
            {
                Console.WriteLine(" --> " + text);
            }
        }

        public void ProgramInput(string text)
        {
            using (new ConsoleTextColor(ConsoleColor.Yellow))
            {
                Console.WriteLine(" <-- " + text);
            }
        }
    }
}