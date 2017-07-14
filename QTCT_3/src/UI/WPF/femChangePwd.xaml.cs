using NHibernate.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WY.Common;
using WY.Common.Message;
using WY.Common.Utility;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// femChangePwd.xaml 的交互逻辑
    /// </summary>
    public partial class femChangePwd : Window
    {
        public femChangePwd()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //判断当前密码的有效性            
            if (DES.Encode(this.txtpwd.Password, Global.DB_PWDKEY) == Global.g_password)
            {
                string newpwd = this.txtnewpwd.Password;
                string newpwd2 = this.txtnewpwd2.Password;
                if (newpwd == newpwd2)
                {
                    if (newpwd.Length < 6)
                    {
                        MessageHelper.ShowMessage("新密码不能少于6位！");
                        return;
                    }
                    TB_User user = TB_UserDao.FindFirst(new EqExpression("STATUS", 1), new EqExpression("USER_CODE", Global.g_usercode));
                    if (user != null)
                    {
                        user.PASSWORD = DES.Encode(this.txtnewpwd.Password, Global.DB_PWDKEY);
                        user.Update();
                        MessageHelper.ShowMessage("密码更新成功!");
                        Global.g_password = DES.Encode(this.txtnewpwd.Password, Global.DB_PWDKEY);
                        this.Close();
                    }
                    else
                    {
                        MessageHelper.ShowMessage("当前用户信息已失效，请与管理员确认！");
                    }
                }
                else
                {
                    MessageHelper.ShowMessage("新密码不一致，请重新确认!");
                }
            }
            else
            {
                MessageHelper.ShowMessage("原密码与登陆密码不一致，请重新输入!");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
