using NHibernate.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// frmUser.xaml 的交互逻辑
    /// </summary>
    public partial class frmUser : Window
    {
        private string mStatus = "";
        public frmUser()
        {
            InitializeComponent();
            #region  获取静态角色数据
            this.cmbUserRole.ItemsSource = null;
            PTS_TABLE_SRC tmp = new PTS_TABLE_SRC();
            tmp.TABLE_NAME = "请选择";
            tmp.TABLE_VALUE = "0";
            PTS_TABLE_SRC[] arr = PTS_TABLE_SRCDAO.FindAll(new EqExpression("STATUS", 1), new EqExpression("TABLE_ID", "PTS_TABLE_USER_ROLE"));
            List<PTS_TABLE_SRC> list = new List<PTS_TABLE_SRC>();
            if (arr.Length > 0)
            {
                list = new List<PTS_TABLE_SRC>(arr);
                list.Insert(0, tmp);
            }
            this.cmbUserRole.ItemsSource = list;
            this.cmbUserRole.DisplayMemberPath = "TABLE_NAME";
            this.cmbUserRole.SelectedIndex = 0;
            #endregion
            bindUserData();
        }

        private void bindUserData()
        {
            TB_User[] arr = TB_UserDao.FindAll(new EqExpression("STATUS", 1));
            List<TB_User> list = new List<TB_User>(arr);
            if (arr.Length > 0)
            {
                dgUser.ItemsSource = list;
                //dgUser.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mStatus = "add";         
                if (!string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtCode.Text))
                {
                    if (!getUserInfo(txtCode.Text))
                    { 
                        return;
                    }
                    TB_User user = new TB_User();
                    user.USER_NAME = this.txtName.Text.Trim();
                    user.USER_CODE = this.txtCode.Text.Trim();
                    user.ROLEID = (this.cmbUserRole.SelectedValue as PTS_TABLE_SRC).ID;
                    TB_User tmp = TB_UserDao.FindFirst(new EqExpression("USER_CODE", this.txtCode.Text.Trim()));
                    user.PASSWORD = DES.Encode("111111", Global.DB_PWDKEY);
                    user.DEPT = txtDept.Text;
                    user.Save();  //保存
                    MessageHelper.ShowMessage("保存成功");
                    this.txtName.Text = "";
                    this.txtCode.Text = "";
                    this.cmbUserRole.SelectedIndex = 0;
                }
                mStatus = "";
                bindUserData();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TB_User user = this.dgUser.SelectedItem as TB_User;
                user.STATUS = 0;
                user.Update();
                MessageHelper.ShowMessage("删除成功");
                bindUserData();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.dgUser.SelectedItem != null)
                {
                    TB_User user = this.dgUser.SelectedItem as TB_User;
                    user.USER_CODE = this.txtCode.Text.Trim();
                    user.USER_NAME = this.txtName.Text.Trim();
                    user.ROLEID = (this.cmbUserRole.SelectedValue as PTS_TABLE_SRC).ID;
                    user.DEPT = txtDept.Text;
                    user.Update();
                    //MessageHelper.ShowMessage("更新成功！");
                    bindUserData();
                }
            }
            catch (Exception ex)
            {
                //MessageHelper.ShowMessage("更新失败:" + ex.Message);
            }
        }

        private void dgUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TB_User user = this.dgUser.SelectedItem as TB_User;
                this.txtName.Text = user.USER_NAME;
                this.txtCode.Text = user.USER_CODE;
                this.txtDept.Text = user.DEPT;
                PTS_TABLE_SRC role = getUserRole(user.ROLEID);
                if (role != null)
                {
                    this.cmbUserRole.Text = role.TABLE_NAME ;
                }
                else
                {
                    this.cmbUserRole.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }


        private PTS_TABLE_SRC getUserRole(int ROLEID = -1)
        {
            try
            {
                PTS_TABLE_SRC item = PTS_TABLE_SRCDAO.FindFirst(new EqExpression("ID", ROLEID));
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool getUserInfo(string usercode)
        {
            try
            {
                //新增用户工号查重
                TB_User _user = TB_UserDao.FindFirst(new EqExpression("STATUS", 1), new EqExpression("USER_CODE", usercode));
                if (_user != null)
                {
                    MessageHelper.ShowMessage("工号重复，请确认！");
                    this.txtCode.Focus();
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.dgUser.SelectedItem != null)
                {
                    TB_User user = this.dgUser.SelectedItem as TB_User;
                    user.PASSWORD = "Wv2024LOLgs=";
                    user.Update();
                    MessageHelper.ShowMessage("密码重置成功");
                    bindUserData();
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }
    }
}
