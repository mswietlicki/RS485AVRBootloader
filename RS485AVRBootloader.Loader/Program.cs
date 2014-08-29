using System;
using System.IO;
using System.Linq;
using SerialAVRBootloader.Loader.Common;
using SerialAVRBootloader.Loader.Communicators.Serial;

namespace SerialAVRBootloader.Loader
{
    internal class Program
    {
        public static ILogger Logger;

        private static void Main(string[] args)
        {
            Logger = new MultiLogger(new ConsoleLogger(), new FileLogger());
            var printer = new MessagePrinter(Logger);
            printer.PrintWelcome();
            printer.PrintArgs(args);

            ISettingsProvider settingsProvider = new PropertiesSettingsProvider();
            printer.PrintConfig(settingsProvider);

            using (ISerialDevice serialDevice = new LoggerSerialDevice(new SerialPortDevice(settingsProvider, Logger), Logger))
            {
                try
                {
                    var bootloader = new BootloaderCommunicator(new SerialCommunicator(serialDevice), Logger);
                    var bootloaderInfo = bootloader.GetBootloaderInfo();

                    Logger.WriteLine("Bootloader info: " + bootloaderInfo);

                    if (args.Any())
                        SaveProgram(args[0], bootloader);

                    Logger.WriteLine("Success!");
                }
                catch (Exception exception)
                {
                    Logger.WriteError(exception);
                }
            }
        }



        private static void SaveProgram(string file, BootloaderCommunicator bootloader)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(string.Format("File {0} not found!", file), file);
            if (!file.EndsWith(".bin", StringComparison.InvariantCultureIgnoreCase))
                throw new FileLoadException(string.Format("Input file must be in .BIN format!"), file);

            Logger.WriteLine("Reading file: " + file + Environment.NewLine);

            var bytes = File.ReadAllBytes(file);
            Logger.WriteLine(bytes.ToHexString());
            Logger.WriteLine("");

            Logger.WriteLine("Writing file: " + file + Environment.NewLine);

            using (var dataStream = File.OpenRead(file))
            {
                bootloader.WriteProgram(dataStream);
            }
        }
    }
}