using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WY.Common.Utility;

namespace WY.Common
{
    public class ConfigManager
    {
        public const string INI_FILENAME = "system.ini";

        public enum enmAppKey
        {
            MYSQL,
            LOGNAME
        }

        private static string config_file;

        static ConfigManager()
        {
            config_file = Application.StartupPath + "\\" + INI_FILENAME;
        }

        #region 读配置信息

        public static string GetStringVal(enmAppKey app, string key, string defaultval)
        {
            return Utils.GetIniSetting(app.ToString(), key, defaultval, config_file);
        }

        public static int GetIntVal(enmAppKey app, string key, int defaultval)
        {
            return Utils.NvInt(GetStringVal(app, key, defaultval.ToString()));
        }

        public static bool GetBoolVal(enmAppKey app, string key, bool defaultval)
        {
            return Utils.NvBool(GetIntVal(app, key, defaultval ? 1 : 0));
        }

        #endregion

        #region 写配置信息

        public static void WriteIniVal(enmAppKey app, string key, string val)
        {
            Utils.WriteIniSetting(app.ToString(), key, val, config_file);
        }

        public static void WriteIniVal(enmAppKey app, string key, int val)
        {
            WriteIniVal(app, key, val.ToString());
        }

        public static void WriteIniVal(enmAppKey app, string key, bool val)
        {
            WriteIniVal(app, key, val ? "1" : "0");
        }

        #endregion

    }
}
