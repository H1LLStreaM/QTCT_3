using NHibernate.Expression;
using System;
using System.Windows;
using System.Windows.Input;
using WY.Common;
using WY.Common.Message;
using WY.Common.Utility;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ServerIP;

        public MainWindow()
        {
            InitializeComponent();
            //获取ini信息
            getIniInfo();
            MyProgram.MainProgress(ServerIP); //暂时只需要初始化一次
            this.pwd.Focus();
        }

        private void getIniInfo()
        {
            this.txtIPServer.Text = ConfigManager.GetStringVal(ConfigManager.enmAppKey.MYSQL, "SERVER", "");
            ServerIP = this.txtIPServer.Text;
            this.txtLogName.Text = ConfigManager.GetStringVal(ConfigManager.enmAppKey.LOGNAME, "LOGNAME", "");
        }

        private void writeIniInfo()
        {
            ConfigManager.WriteIniVal(ConfigManager.enmAppKey.MYSQL, "SERVER", this.txtIPServer.Text);
            ConfigManager.WriteIniVal(ConfigManager.enmAppKey.LOGNAME, "LOGNAME", this.txtLogName.Text);
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string logName = this.txtLogName.Text.Trim();  //用户工号
                string pwd = DES.Encode(this.pwd.Password, Global.DB_PWDKEY);  //Utils.MD5(txtPwd.Text);
                string dcodePwd = DES.Decode("G2ZaEl9zO9xeS+77fmGKow==", Global.DB_PWDKEY);
                TB_User user = TB_UserDao.FindFirst(new EqExpression("USER_CODE", logName), new EqExpression("PASSWORD", pwd), new EqExpression("STATUS", 1));
                if (user != null)
                {
                    Global.g_username = user.USER_NAME;
                    Global.g_userid = user.Id;
                    Global.g_userrole = user.ROLEID;
                    Global.g_usercode = user.USER_CODE;
                    Global.g_password = user.PASSWORD;  //已加密
                    Global.g_dept = user.DEPT;
                    //获取默认提成比例
                    PTS_OBJECT_TYPE_SRC[] arr = PTS_OBJECT_TYPE_SRCDAO.FindAll(new EqExpression("STATUS", 1));
                    if (arr.Length > 0)
                    {
                        Global.g_ratio1 = arr[0].RATIO1;
                        Global.g_ratio2 = arr[0].RATIO2;
                    }
                    writeSystemIni();
                    writeLogName();
                    this.Cursor = Cursors.Arrow;
                    frmMain Main = new frmMain();
                    Main.Show();
                    this.Close();
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageHelper.ShowMessage("E008");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        //private void loadLogName()
        //{
        //    txtLogName.Text = ConfigManager.GetStringVal(ConfigManager.enmAppKey.LOGNAME, "LOGNAME", "");
        //}

        private void writeSystemIni()
        {
            ConfigManager.WriteIniVal(ConfigManager.enmAppKey.MYSQL, "SERVER", txtIPServer.Text.Trim());
        }

        private void writeLogName()
        {
            ConfigManager.WriteIniVal(ConfigManager.enmAppKey.LOGNAME, "LOGNAME", txtLogName.Text.Trim());
        }

        private void btn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_Click(null, null);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}