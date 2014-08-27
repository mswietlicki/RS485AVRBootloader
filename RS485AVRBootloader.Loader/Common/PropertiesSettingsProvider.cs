namespace SerialAVRBootloader.Loader.Common
{
    public class PropertiesSettingsProvider : ISettingsProvider
    {
        public string GetSetting(string name)
        {
            Properties.Settings.Default.Reload();

            return Properties.Settings.Default[name].ToString();
        }
    }
}