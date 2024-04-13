using System.Runtime.InteropServices;

namespace Trans2QuikNet.Models
{

    [StructLayout(LayoutKind.Sequential)]
    public struct FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;

        public DateTime ToDateTime()
        {
            long fileTime = dwHighDateTime;
            fileTime <<= 32;
            fileTime |= dwLowDateTime;
            return DateTime.FromFileTime(fileTime);
        }

        public static FILETIME FromDateTime(DateTime dateTime)
        {
            long fileTime = dateTime.ToFileTime();
            FILETIME ft = new FILETIME
            {
                dwLowDateTime = (uint)(fileTime & 0xFFFFFFFF),
                dwHighDateTime = (uint)(fileTime >> 32)
            };
            return ft;
        }
    }

}
