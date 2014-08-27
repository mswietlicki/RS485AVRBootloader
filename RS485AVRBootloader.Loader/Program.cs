using System;
using SerialAVRBootloader.Loader.Common;
using SerialAVRBootloader.Loader.Communicators;

namespace SerialAVRBootloader.Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            ISettingsProvider settingsProvider = new PropertiesSettingsProvider();
            ILogger logger = new MultiLogger(new ConsoleLogger(), new FileLogger());
            try
            {
                var bootloader = new BootloaderCommunicator(new SerialCommunicator(settingsProvider), logger);
                var bootloaderInfo = bootloader.GetBootloaderInfo();

                logger.WriteLine(bootloaderInfo.ToString());
            }
            catch (Exception exception)
            {
                logger.WriteError(exception);
            }

        }
    }
}
