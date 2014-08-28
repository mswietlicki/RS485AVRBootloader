using SerialAVRBootloader.Loader.Common;

namespace SerialAVRBootloader.Loader
{
    public class MessagePrinter
    {
        private readonly ILogger _logger;

        public MessagePrinter(ILogger logger)
        {
            _logger = logger;
        }

        public void PrintConfig(ISettingsProvider settingsProvider)
        {
            _logger.WriteLine("Settings: ");
            foreach (var setting in settingsProvider.GetSettings())
            {
                _logger.WriteLine(string.Format("    {0}: {1}", setting.Key, setting.Value));
            }
            _logger.WriteLine(" ");
        }

        public void PrintArgs(string[] args)
        {
            _logger.WriteLine("Arguments: ");
            foreach (var arg in args)
            {
                _logger.WriteLine(string.Format("    {0}", arg));
            }
            _logger.WriteLine(" ");
        }

        public void PrintWelcome()
        {
            _logger.WriteLine("");
            _logger.WriteLine("RS485 AVR Bootloader loader");
            _logger.WriteLine("===========================");
            _logger.WriteLine("");
        }
    }
}