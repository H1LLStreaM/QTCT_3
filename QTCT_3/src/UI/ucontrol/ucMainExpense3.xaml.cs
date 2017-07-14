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
using WY.Common;
using WY.Common.Message;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// ucMainExpense3.xaml 的交互逻辑
    /// </summary>
    public partial class ucMainExpense3 : UserControl
    {
        public ucMainExpense3()
        {
            InitializeComponent();
            BindDataSource();
            if (Global.g_userrole != 8 || Global.g_userrole != 9)
            {
                this.txtUser.Text = Global.g_usercode;
                this.txtUser.Tag = TB_UserDao.FindFirst(new EqExpression("USER_CODE", Global.g_usercode));
                this.txtUser.IsEnabled = false;
                this.imgDel2.IsEnabled = false;
            }
        }

        private void BindDataSource()
        {           
            //报销类型
            List<PTS_TABLE_SRC> src_list = null;
            PTS_TABLE_SRC[] src_arr = PTS_TABLE_SRCDAO.FindAll(new EqExpression("TABLE_ID", "PTS_TABLE_EXPENSE_ITEM"));  //没有过滤status=0
            if (src_arr.Length > 0)
            {
                src_list = new List<PTS_TABLE_SRC>(src_arr);
                PTS_TABLE_SRC tmp = new PTS_TABLE_SRC();
                tmp.TABLE_NAME = "请选择";
                tmp.ID = 0;
                src_list.Insert(0, tmp);
            }
            this.cmbExpenseType.ItemsSource = null;
            this.cmbExpenseType.ItemsSource = src_list;
            this.cmbExpenseType.DisplayMemberPath = "TABLE_NAME";
            this.cmbExpenseType.SelectedIndex = 0;
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
                if (txtProj.Tag != null)
                {
                    IClist.Add(new EqExpression("OBJECTID", (txtProj.Tag as TB_PROJECT).Id));
                }
                if (cmbExpenseType.SelectedIndex > 0)
                {
                    IClist.Add(new EqExpression("EXPENSETYPE", (cmbExpenseType.SelectedItem as PTS_TABLE_SRC).ID));
                }
                if (chk.IsChecked == true)
                {
                    BetweenExpression and1 = new BetweenExpression("CREATEDATE", dtpBeginDate.DateTime, DateTime.Parse(dtpEndDate.DateTime.ToString("yyyy-MM-dd 23:59:59")));
                    IClist.Add(and1);
                }
                if (txtUser.Tag != null)
                {
                    IClist.Add(new EqExpression("OPNAME", (txtUser.Tag as TB_User).USER_CODE));
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

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            if (chk.IsChecked == true)
            {
                this.dtpBeginDate.IsEnabled = true;
                this.dtpEndDate.IsEnabled = true;
            }
            else
            {
                this.dtpBeginDate.IsEnabled = false;
                this.dtpEndDate.IsEnabled = false;
            }
        }

        #region 工程文本框操作
        private void txtProj_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtProj.Tag == null)
                {
                    frmObjectSearch frm = new frmObjectSearch();
                    frm.ShowDialog();
                    if (frm.mProj != null && frm.mProj.Id > 0)
                    {
                        this.txtProj.Tag = frm.mProj;
                        this.txtProj.Text = frm.mProj.OBJECTNAME;
                    }
                    else
                    {
                        this.txtProj.Tag = null;
                        this.txtProj.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void txtProj_KeyDown(object sender, KeyEventArgs e)
        {
            ClearPorject();
        }

        private void ClearPorject()
        {
            this.txtProj.Tag = null;
            this.txtProj.Text = string.Empty;
        }

        private void imgDel1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearPorject();
        }
        #endregion

        #region 人员文本框操作
        private void txtUser_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtUser.Tag == null)
                {
                    frmUserSearch frm = new frmUserSearch();
                    frm.ShowDialog();
                    if (frm.mUser != null && frm.mUser[0].Id > 0)
                    {
                        this.txtUser.Tag = frm.mUser;
                        this.txtUser.Text = frm.mUser[0].USER_CODE;
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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<TB_EXPENSE> ls = new List<TB_EXPENSE>();
            TB_PROJECT proj = this.txtProj.Tag as TB_PROJECT; //工程类型
            PTS_TABLE_SRC src = this.cmbExpenseType.SelectedItem as PTS_TABLE_SRC;  //报销类型
            //查询条件
            int expenxeType = 0;
            int expenxeType2 = 0;
            int objectID = 0;
            if (txtProj.Tag != null)
                objectID = (txtProj.Tag as TB_PROJECT).Id;
            string userCode = string.Empty;
            if(txtUser.Tag!=null)
                userCode = (txtUser.Tag as TB_User).USER_CODE;
            if (src != null)
                expenxeType2 = src.ID;
            //ls = Comments.Comment.QueryExpense2(userCode, expenxeType, objectls = Comments.Comment.QueryExpense2(userCode, expenxeType, objectID);

            ls = Comments.Comment.QueryExpense(Global.g_usercode, expenxeType, objectID, expenxeType2, int.Parse(cmbYear.Text), int.Parse(cmbMonth.Text));
            this.dgExpense.ItemsSource = null;
            for (int i = 0; i < ls.Count; i++)
            {
                if (ls[i].RESPONSESTATUS == 1)
                    ls[i].StrResponseStatus = "驳回/修改";
                else if(ls[i].RESPONSESTATUS==2)
                    ls[i].StrResponseStatus = "已更正";
            }
            this.dgExpense.ItemsSource = ls;
        }

        /// <summary>
        /// 报销信息驳回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgExpense.SelectedItem != null)
            {
                TB_EXPENSE exp = this.dgExpense.SelectedItem as TB_EXPENSE;
                frmReponse frm = new frmReponse(exp);
                frm.ShowDialog();
            }
        }        
    }
}
