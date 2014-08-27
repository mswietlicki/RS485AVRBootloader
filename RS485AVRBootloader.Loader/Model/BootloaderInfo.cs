using System.Linq;

namespace SerialAVRBootloader.Loader.Model
{
    public struct BootloaderInfo
    {
        public string SPM_PAGESIZE { get; set; }
        public string BLS_START { get; set; }
        public string MCU { get; set; }
        public string XTAL { get; set; }
        public string BOOTLOADER_VERSION { get; set; }

        public static BootloaderInfo Parse(string rawData)
        {
            var data = rawData.Split(',').Select(_ => _.Trim()).ToArray();
            return new BootloaderInfo
            {
                SPM_PAGESIZE = data[0],
                BLS_START = data[1],
                MCU = data[2],
                XTAL = data[3],
                BOOTLOADER_VERSION = data[4]
            };
        }
    }
}