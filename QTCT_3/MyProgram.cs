using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WY.Common;
using WY.Common.Utility;
using WY.Library.Model;

namespace QTCT_3
{
    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    public static class MyProgram
    {
        

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void MainProgress(string serverIP)
        {
            string server = serverIP;
            string port = "3306";
            string logId = "root";
            string password = "root";
            string database = "myobject";
            server = ConfigManager.GetStringVal(ConfigManager.enmAppKey.MYSQL, "SERVER", "");
            database = ConfigManager.GetStringVal(ConfigManager.enmAppKey.MYSQL, "DATABASE", "myobject");
            logId = DES.Decode(ConfigManager.GetStringVal(ConfigManager.enmAppKey.MYSQL, "NAME", "FmKfww+xL4U="), Global.DB_PWDKEY);
            password = DES.Decode(ConfigManager.GetStringVal(ConfigManager.enmAppKey.MYSQL, "PWD", "FmKfww+xL4U="), Global.DB_PWDKEY);
            port = ConfigManager.GetStringVal(ConfigManager.enmAppKey.MYSQL, "PORT", "3306");

            string connstr = "server=" + server + ";port=" + port + ";user id=" + logId + ";password=" + password + ";database=" + database + ";charset=utf8;";
            InPlaceConfigurationSource source1 = new InPlaceConfigurationSource();
            Hashtable properties = new Hashtable();
            properties.Add("hibernate.connection.driver class", "NHibernate.Driver.MySqlDataDriver");
            properties.Add("hibernate.dialect", "NHibernate.Dialect.MySQLDialect");
            properties.Add("hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            properties.Add("hibernate.connection.connection_string", connstr);
            source1.Add(typeof(ActiveRecordBase), properties);
            Type[] types = { typeof(TB_User),
                             typeof(PTS_TABLE_SRC),
                             typeof(TB_PROJECT),
                             typeof(TB_EXPENSE),
                             typeof(TB_BILL),
                             typeof(PTS_OBJECT_TYPE_SRC),
                             typeof(PTS_EXCEL_PROFILE_SRC),
                             typeof(TB_RATIO),
                             typeof(pts_proj_ratio),
                             typeof(PTS_PROJ_COST),
                        };
            Castle.ActiveRecord.ActiveRecordStarter.Initialize(source1, types);
            WY.Common.Data.DbHelper.Initialize("MySql.Data.MySqlClient", connstr);
        }        
    }
}
