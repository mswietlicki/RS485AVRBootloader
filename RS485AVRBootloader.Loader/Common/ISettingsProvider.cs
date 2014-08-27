namespace SerialAVRBootloader.Loader.Common
{
    public interface ISettingsProvider
    {
        string GetSetting(string name);
    }
}