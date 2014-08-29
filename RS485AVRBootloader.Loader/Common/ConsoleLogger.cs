using System;

namespace SerialAVRBootloader.Loader.Common
{
    public class ConsoleLogger : BaseProgramLogger
    {
        public override void WriteLine(string rawInfo)
        {
            Console.WriteLine(rawInfo);
        }

        public override void WriteError(Exception exception)
        {
            using (new ConsoleTextColor(ConsoleColor.Red))
            {
                Console.Error.WriteLine(exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
            }
        }

        public override void ProgramOutput(string text)
        {
            using (new ConsoleTextColor(ConsoleColor.Green))
            {
                Console.WriteLine(" --> " + text);
            }
        }

        public override void ProgramInput(string text)
        {
            using (new ConsoleTextColor(ConsoleColor.Yellow))
            {
                Console.WriteLine(" <-- " + text);
            }
        }
    }
}