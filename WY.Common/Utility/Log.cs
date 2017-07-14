using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace WY.Common.Utility
{
    public class Log
    {
        public enum EnmLogLevel
        {
            INFORMATION = 0,
            WARNING,
            ERROR
        }

        private static string _logfilename = "_errlog.log";

        public static void Error(Exception e)
        {
            WriteLog(e.ToString(), EnmLogLevel.ERROR);
        }

        public static void Error(string message)
        {
            WriteLog(message, EnmLogLevel.ERROR);
        }

        public static void Warning(string message)
        {
            Warning(_logfilename, message);
        }

        public static void Warning(string filename, string message)
        {
            WriteLog(filename, message, EnmLogLevel.WARNING);
        }

        public static void Info(string message)
        {
            Info(_logfilename, message);
        }

        public static void Info(string filename, string message)
        {
            WriteLog(filename, message, EnmLogLevel.INFORMATION);
        }

        private static void WriteLog(string message, EnmLogLevel level)
        {
            WriteLog(_logfilename, message, level);
        }

        /// <summary>
        /// 记录出错信息
        /// </summary>
        /// <param name="message">ex中的出错内容</param>
        private static void WriteLog(string filename, string message, EnmLogLevel level)
        {
            try
            {
                filename = Application.StartupPath + "\\" + filename;

                if (File.Exists(filename))
                {
                    FileInfo info = new FileInfo(filename);
                    if (info.Length >= 1024 * 1024)
                    {
                        string newfilename = Application.StartupPath + "\\" + info.Name.Replace(info.Extension, "") + DateTime.Now.ToString("yyMMddHHmmss") + info.Extension;
                        System.IO.File.Move(filename, newfilename);
                    }
                }

                string strerror = "[" + level.ToString() + "]\t[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "]\r\n" + message + "\r\n";
                WriteLine(filename, strerror);
            }
            catch { }
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filepath">文件名</param>
        /// <param name="text">内容</param>
        /// <param name="append">True：添加；False：覆盖</param>
        /// <returns></returns>
        private static void WriteLine(String filepath, String text)
        {
            try
            {
                System.IO.StreamWriter objfile;
                objfile = new System.IO.StreamWriter(filepath, true);
                objfile.WriteLine(text);
                objfile.Close();
            }
            catch { }
        }
    }
}
