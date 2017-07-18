using NHibernate.Expression;
using QTCT_3.src.UI.ucontrol;
using QTCT_3.src.UI.WPF;
using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using WY.Common;
using WY.Common.Message;
using WY.Library.Business;
using WY.Library.Dao;

namespace QTCT_3
{
    /// <summary>
    /// frmMain.xaml 的交互逻辑
    /// </summary>
    public partial class frmMain : Window
    {
        public frmMain()
        {
            InitializeComponent();
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Start();
            timer.Tick += timer_Tick;
            this.barUser.Content = Global.g_usercode;
            refreshControl();
            refreshMenu();
            autoAlert();
        }
        
        /// <summary>
        /// 项目到期提醒 完工日期后30天
        /// </summary>
        private void autoAlert()
        {
            DateTime date = DateTime.Parse(new BaseBusiness().date());
            frmExpenseAler frm = new frmExpenseAler();
            frm.Title = date.Year.ToString() + "-" + date.AddMonths(-1).Month.ToString() + "报销提醒";
            frm.ShowDialog();
        }

        /// <summary>
        /// 根据用户权限显示初始界面(默认全部是报销填报界面)
        /// </summary>
        private void refreshControl()
        {
            refreshMenuColor(menu_addexpense);
            ucNewExpense ucExpense = new ucNewExpense();
            ucExpense.Width = this.wpanel.Width;
            ucExpense.Height = this.wpanel.Height;
            this.wpanel.Children.Add(ucExpense);            
        }


        /// <summary>
        /// 根据用户权限管理显示菜单
        /// </summary>
        private void refreshMenu()
        {
            menu_add_project.IsEnabled = false;
            menu_profile_search.IsEnabled = false;
            menu_user.IsEnabled = false;
            menu_mange.IsEnabled = false;
            menu_profile_proj_search.IsEnabled = false;
            switch (Global.g_userrole)
            {
                case 7:  //管理员
                    break;
                case 8:  //总经理
                    menu_profile_search.IsEnabled = true;
                    menu_add_project.IsEnabled = true;
                    menu_user.IsEnabled = true;
                    menu_mange.IsEnabled = true;
                    menu_profile_proj_search.IsEnabled = true;
                    break;
                case 9: //财务
                    break;
                case 10: //销售
                    menu_add_project.IsEnabled = true;
                    break;
                case 11: //员工
                    break;
                default:
                    break;
            }
        }

        private void menu_project_window_Click(object sender, RoutedEventArgs e)
        {
            refreshMenuColor(sender);
            this.wpanel.Children.Clear();
            ucMainProj uc = new ucMainProj();
            uc.Width = this.wpanel.Width;
            uc.Height = this.wpanel.Height;
            this.wpanel.Children.Add(uc);
        }

        private void menu_expense_window_Click(object sender, RoutedEventArgs e)
        {
            refreshMenuColor(sender);
            this.wpanel.Children.Clear();
            ucMainExprense uc = new ucMainExprense();
            uc.Width = this.wpanel.Width;
            uc.Height = this.wpanel.Height;
            this.wpanel.Children.Add(uc);
        }
        
        void timer_Tick(object sender, System.EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            this.barTimer.Content = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void menu_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //人员管理
                frmUser frm = new frmUser();
                frm.ShowDialog();
            }
            catch (Exception ex)
            { 
            
            }
        }

        private void menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            if(System.Windows.Forms.MessageBox.Show("是否确认退出？","关闭",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== System.Windows.Forms.DialogResult.Yes)
                this.Close();
        }

        private void menu_newproject_Click(object sender, RoutedEventArgs e)
        {
            //新增项目
            frmProject frm = new frmProject(null);
            frm.ShowDialog();
        }

        /// <summary>
        /// 报销填报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_newexpense_Click(object sender, RoutedEventArgs e)
        {
            refreshMenuColor(sender);
            this.wpanel.Children.Clear();
            ucNewExpense uc = new ucNewExpense();
            uc.Width = this.wpanel.Width;
            uc.Height = this.wpanel.Height;
            this.wpanel.Children.Add(uc);
        }

        private void menu_expense_Click(object sender, RoutedEventArgs e)
        {
            refreshMenuColor(sender);
            this.wpanel.Children.Clear();
            ucMainExprense uc = new ucMainExprense();
            uc.Width = this.wpanel.Width;
            uc.Height = this.wpanel.Height;
            this.wpanel.Children.Add(uc);
        }

        private void menu_changePwd_Click(object sender, RoutedEventArgs e)
        {
            femChangePwd frm = new femChangePwd();
            frm.ShowDialog();
        }

        /// <summary>
        /// 参数管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_mange_Click(object sender, RoutedEventArgs e)
        {
            frmManageData frm = new frmManageData();
            frm.ShowDialog();
        }

        private void menu_expense2_window_Click(object sender, RoutedEventArgs e)
        {
            refreshMenuColor(sender);
            this.wpanel.Children.Clear();
            ucMainExprense uc = new ucMainExprense();
            uc.Width = this.wpanel.Width;
            uc.Height = this.wpanel.Height;
            this.wpanel.Children.Add(uc);
        }

        private void menu_projectratio_Click(object sender, RoutedEventArgs e)
        {
            frmRatioEdit frm = new frmRatioEdit();
            frm.ShowDialog();
        }

        private void menu_newproject_search_Click(object sender, RoutedEventArgs e)
        {
            refreshMenuColor(sender);
            this.wpanel.Children.Clear();
            ucMainProj uc = new ucMainProj();
            uc.Width = this.wpanel.Width;
            uc.Height = this.wpanel.Height;
            this.wpanel.Children.Add(uc);
        }

        private void menu_allexpense_Click(object sender, RoutedEventArgs e)
        {
            refreshMenuColor(sender);
            this.wpanel.Children.Clear();
            ucMainExprense uc = new ucMainExprense();
            uc.Width = this.wpanel.Width;
            uc.Height = this.wpanel.Height;
            this.wpanel.Children.Add(uc);
        }

        private void menu_add_project_Click(object sender, RoutedEventArgs e)
        {
            refreshMenuColor(sender);
            this.wpanel.Children.Clear();
            ucNewProject uc = new ucNewProject(null);
            uc.Width = this.wpanel.Width;
            uc.Height = this.wpanel.Height;
            this.wpanel.Children.Add(uc);
        }

        private void refreshMenuColor(object sender)
        {
            string itemName = ((System.Windows.Controls.HeaderedItemsControl)(sender)).Header.ToString();
            for (int i = 0; i < menu.Items.Count; i++)
            {
                string name = ((System.Windows.Controls.HeaderedItemsControl)(menu.Items[i])).Header.ToString();
                if (name == itemName)                
                    ((System.Windows.Controls.HeaderedItemsControl)menu.Items[i]).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF80C7F6"));                
                else
                    ((System.Windows.Controls.HeaderedItemsControl)menu.Items[i]).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));
            }
        }

        /// <summary>
        /// 公司业绩查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_profile_search_Click(object sender, RoutedEventArgs e)
        {
            refreshMenuColor(sender);
            this.wpanel.Children.Clear();
            ucQTCTProfile uc = new ucQTCTProfile();
            uc.Width = this.wpanel.Width;
            uc.Height = this.wpanel.Height;
            this.wpanel.Children.Add(uc);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        { }

        /// <summary>
        /// 考勤Excel导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_WorkLoad_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Excel Files|*.xls";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Path = fd.FileName;
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                string strExcel = "";
                OleDbDataAdapter myCommand = null;
                DataSet ds = null;
                strExcel = "select * from [sheet1$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");    

            }
        }

        private void menu_Work_Modify_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 项目提成查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_profile_proj_search_Click(object sender, RoutedEventArgs e)
        {
            refreshMenuColor(sender);
            this.wpanel.Children.Clear();
            ucProjProfile uc = new ucProjProfile();
            uc.Width = this.wpanel.Width;
            uc.Height = this.wpanel.Height;
            this.wpanel.Children.Add(uc);
        }
    }
}
