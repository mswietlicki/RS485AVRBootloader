using System.Collections.Generic;

namespace SerialAVRBootloader.Loader.Common
{
    public interface ISettingsProvider
    {
        string GetSetting(string name);
        IEnumerable<KeyValuePair<string, string>> GetSettings();
    }
}