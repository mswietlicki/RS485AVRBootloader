using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialAVRBootloader.Loader.Common
{
    public static class ByteExtensions
    {
        public static string ToHexString(this byte b)
        {
            return b.ToString("X2");
        }
        public static string ToHexString(this byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0} ", b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
