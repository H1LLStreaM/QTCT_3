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
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// frmUserSearch.xaml 的交互逻辑
    /// </summary>
    public partial class frmUserSearch : Window
    {
        public List<TB_User> mUser = null;
        public frmUserSearch()
        {
            InitializeComponent();
            BindUserData();
        }

        public frmUserSearch(string dept)
        {
            InitializeComponent();
            BindUserData(dept);
        }

        private void BindUserData(string str)
        {
            TB_User[] arr = TB_UserDao.FindAll(new EqExpression("STATUS", 1),new EqExpression("DEPT",str));
            if(!string.IsNullOrEmpty(txtName.Text))
                arr = TB_UserDao.FindAll(new EqExpression("STATUS", 1), new LikeExpression("USER_CODE", "%" + txtName.Text + "%"),new EqExpression("DEPT",str));
            List<TB_User> list = new List<TB_User>(arr);
            if (arr.Length > 0)
            {
                dgUser.ItemsSource = list;
                dgUser.SelectedIndex = 0;
            }
        }

        private void BindUserData()
        {
            TB_User[] arr = TB_UserDao.FindAll(new EqExpression("STATUS", 1));
           
            List<TB_User> list = new List<TB_User>(arr);
            if (arr.Length > 0)
            {
                list = list.FindAll(a => a.USER_CODE != "财务" && a.USER_CODE != "系统管理员");
                dgUser.ItemsSource = list;
                dgUser.SelectedIndex = 0;
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string str = this.txtName.Text.Trim();
                BindUserData(str);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string str = this.txtName.Text.Trim();
            BindUserData(str);
        }

        private void setSelectUser()
        {
            //mUser = dgUser.SelectedItem as TB_User;
            //this.Close();
        }

        private void dgUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //setSelectUser();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //setSelectUser();
            try
            {
                List<TB_User> ls = dgUser.ItemsSource as List<TB_User>;
                mUser = new List<TB_User>();
                for (int i = 0; i < ls.Count; i++)
                {
                    if (ls[i].IsChecked == true)
                    {
                        TB_User user = ls[i];
                        mUser.Add(user);
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            TB_User user = (TB_User)this.dgUser.SelectedItem;
            if (user != null)
            {
                user.IsChecked = !user.IsChecked;
            }
        }
    }
}
