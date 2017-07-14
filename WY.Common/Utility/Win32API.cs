using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WY.Common.Utility
{
    public class Win32API
    {
        public const Int32 LVM_FIRST = 0x1000; //   List   messages  
        public const Int32 LVM_GETHEADER = LVM_FIRST + 31;
        public const Int32 HDM_FIRST = 0x1200;
        public const Int32 HDM_SETITEM = HDM_FIRST + 12;
        public const Int32 HDI_FORMAT = 0x0004;
        public const Int32 HDF_LEFT = 0x0000;
        public const Int32 HDF_SORTUP = 0x0400;
        public const Int32 HDF_SORTDOWN = 0x0200;
        public const Int32 HDF_STRING = 0x4000;
        public const Int32 HDF_BITMAP_ON_RIGHT = 0x1000;

        //播放声音
        public const Int32 SND_ALIAS = 0x10000;
        public const Int32 SND_ASYNC = 0x0001;
        public const Int32 SND_FILENAME = 0x20000;
        public const Int32 SND_LOOP = 0x0008;
        public const Int32 SND_SYNC = 0x0000;
        public const Int32 SND_PURGE = 0x0040;  //SND_PURGE 停止所有与调用任务有关的声音。若参数pszSound为NULL，就停止所有的声音，否则，停止pszSound指定的声音

        [StructLayout(LayoutKind.Sequential)]
        public struct HDITEM
        {
            public Int32 mask;
            public Int32 cxy;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String pszText;
            public IntPtr hbm;
            public Int32 cchTextMax;
            public Int32 fmt;
            public Int32 lParam;
            public Int32 iImage;
            public Int32 iOrder;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemTime
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMiliseconds;
        }
        /// <summary>
        /// 更新系统时间
        /// </summary>
        /// <param name="sysTime"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref   SystemTime sysTime);

        /// <summary>
        /// 取得系统时间
        /// </summary>
        /// <param name="sysTime"></param>
        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime(ref   SystemTime sysTime);

        /// <summary>
        /// 写配置文件（ini）
        /// </summary>
        /// <param name="lpApplicationName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpDefault"></param>
        /// <param name="lpReturnedString"></param>
        /// <param name="nSize"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        /// <summary>
        /// 读配置文件（ini）
        /// </summary>
        /// <param name="lpApplicationName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpString"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);


        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern IntPtr SendMessage(IntPtr hwnd, Int32 wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage2(IntPtr hwnd, Int32 wMsg, IntPtr wParam, ref HDITEM lParam);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int PlaySound(string lpszSoundName, int hModule, int dwFlags);

    }
}
