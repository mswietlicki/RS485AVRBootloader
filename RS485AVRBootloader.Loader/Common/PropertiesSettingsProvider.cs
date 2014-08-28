using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

        public IEnumerable<KeyValuePair<string, string>> GetSettings()
        {
            return Settings.Default.Properties
                .OfType<SettingsProperty>()
                .Select(p => p.Name)
                .Select(property =>
                    new KeyValuePair<string, string>(
                        property,
                        Settings.Default[property].ToString()));
        }
    }
}