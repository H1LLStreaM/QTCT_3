using NHibernate.Expression;
using QTCT_3.src.UI.WPF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WY.Common.Message;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// ucMainExpense2.xaml 的交互逻辑
    /// </summary>
    public partial class ucMainExpense2 : UserControl
    {
        public ucMainExpense2()
        {
            try
            {
                InitializeComponent();
                //this.dtpBeginDate.DateTime = this.dtpEndDate.DateTime.AddDays(-5);
            }
            catch (Exception ex)
            { }
        }

        
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<TB_EXPENSE> list = Query();
        }

        private void rdo1_Checked(object sender, RoutedEventArgs e)
        {
            List<TB_EXPENSE> list = this.dgExpense.ItemsSource as List<TB_EXPENSE>;
        }

        private void rdo2_Checked(object sender, RoutedEventArgs e)
        {
            List<TB_EXPENSE> list = this.dgExpense.ItemsSource as List<TB_EXPENSE>;
        }

        private List<TB_EXPENSE> Query()
        {
            List<TB_EXPENSE> rtn = null;
            try
            {
                List<ICriterion> IClist = new List<ICriterion>();
                //IClist.Add(new EqExpression("OPNAME", usercode));
                IClist.Add(new EqExpression("STATUS", 1));
                //IClist.Add(new EqExpression("OBJECTID", objectId));
                if(txtProject.Tag!=null)
                {
                    IClist.Add(new EqExpression("OBJECTID",(txtProject.Tag as TB_PROJECT).Id));
                }
                if (cmbExpenseType.SelectedIndex > 0)
                {
                    IClist.Add(new EqExpression("EXPENSETYPE", (cmbExpenseType.SelectedItem as PTS_TABLE_SRC).ID));
                }
                if (chk.IsChecked==true)
                {
                    //BetweenExpression and1 = new BetweenExpression("CREATEDATE", dtpBeginDate.DateTime, DateTime.Parse(dtpEndDate.DateTime.ToString("yyyy-MM-dd 23:59:59")));
                    //IClist.Add(and1);
                }
                if(txtUser.Tag!=null)
                {
                     IClist.Add(new EqExpression("OPNAME",(txtUser.Tag as TB_User).USER_CODE));
                }
                TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(IClist.ToArray());
                if (arr != null && arr.Length > 0)
                {
                    rtn = new List<TB_EXPENSE>(arr);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
            return rtn;
        }

        #region 清除项目选项
        private void imgDel1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearPorject();
        }

        private void txtProject_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                ClearPorject();
        }

        private void ClearPorject()
        {
            this.txtProject.Tag = null;
            this.txtProject.Text = string.Empty;
        }
        #endregion

        #region 清除人员选项
        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            ClearUser();
        }

        private void imgDel2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearUser();
        }

        private void ClearUser()
        {
            this.txtUser.Tag = null;
            this.txtUser.Text = string.Empty;
        }
        #endregion

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            if (chk.IsChecked == true)
            {
                //this.dtpBeginDate.IsEnabled = true;
                //this.dtpEndDate.IsEnabled = true;
            }
            else
            {
                //this.dtpBeginDate.IsEnabled = false;
                //this.dtpEndDate.IsEnabled = false;
            }
        }

        private void txtProject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtProject.Tag == null)
                {
                    frmObjectSearch frm = new frmObjectSearch();
                    frm.ShowDialog();
                    if (frm.mProj != null && frm.mProj.Id > 0)
                    {
                        this.txtProject.Tag = frm.mProj;
                        this.txtProject.Text = frm.mProj.OBJECTNAME;
                    }
                    else
                    {
                        this.txtProject.Tag = null;
                        this.txtProject.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void txtUser_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtUser.Tag == null)
                {
                    frmUserSearch frm = new frmUserSearch();
                    frm.ShowDialog();
                    if (frm.mUser != null && frm.mUser.Id > 0)
                    {
                        this.txtUser.Tag = frm.mUser;
                        this.txtUser.Text = frm.mUser.USER_CODE;
                    }
                    else
                    {
                        this.txtUser.Tag = null;
                        this.txtUser.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }
    }
}
