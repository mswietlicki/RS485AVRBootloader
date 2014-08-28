using System.Linq;
using System.Text.RegularExpressions;

namespace SerialAVRBootloader.Loader.Model
{
    [ToString]
    public struct BootloaderInfo
    {
        public string SPM_PAGESIZE { get; set; }
        public string BLS_START { get; set; }
        public string MCU { get; set; }
        public string XTAL { get; set; }
        public string BOOTLOADER_VERSION { get; set; }

        public static BootloaderInfo Parse(string rawData)
        {
            var regex = new Regex(@"&(\d+?),(.+?),(.+?),(\d+?),(.+?)\*");
            var data = regex.Match(rawData);
            return new BootloaderInfo
            {
                SPM_PAGESIZE = data.Groups[1].Value,
                BLS_START = data.Groups[2].Value,
                MCU = data.Groups[3].Value,
                XTAL = data.Groups[4].Value,
                BOOTLOADER_VERSION = data.Groups[5].Value
            };
        }
    }
}