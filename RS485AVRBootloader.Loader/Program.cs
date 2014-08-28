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
            Logger.WriteLine("RS485 AVR Bootloader loader");
            Logger.WriteLine("===========================");
            Logger.WriteLine("");

            ISettingsProvider settingsProvider = new PropertiesSettingsProvider();
            ISerialDevice serialDevice = new SerialPortDevice(settingsProvider);
            try
            {
                var bootloader = new BootloaderCommunicator(new SerialCommunicator(serialDevice), Logger);
                var bootloaderInfo = bootloader.GetBootloaderInfo();

                Logger.WriteLine("Bootloader info: " + bootloaderInfo);

                if (args.Any())
                    SaveProgram(args[0], bootloader);
            }
            catch (Exception exception)
            {
                Logger.WriteError(exception);
            }
        }

        private static void SaveProgram(string file, BootloaderCommunicator bootloader)
        {
            Logger.WriteLine("Writing file: " + file);
            if (!File.Exists(file))
                throw new FileNotFoundException(string.Format("File {0} not found!", file), file);
            using (var dataStream = File.OpenRead(file))
            {
                bootloader.WriteProgram(dataStream);
            }
        }
    }
}