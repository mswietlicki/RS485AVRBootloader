using SerialAVRBootloader.Loader.Properties;

namespace SerialAVRBootloader.Loader.Common
{
    public class PropertiesSettingsProvider : ISettingsProvider
    {
        public string GetSetting(string name)
        {
            Settings.Default.Reload();

            return Settings.Default[name].ToString();
        }
    }
}