using NHibernate.Expression;
using QTCT_3.src.UI.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WY.Common;
using WY.Common.Framework;
using WY.Common.Message;
using WY.Library.Business;
using WY.Library.Dao;
using WY.Library.Model;
using System.Linq;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// ucNewExpense.xaml 的交互逻辑
    /// </summary>
    public partial class ucNewExpense : UserControl
    {
        private List<PTS_TABLE_SRC> mList = null;
        private List<TB_EXPENSE> mEList = null;
        private TB_EXPENSE mExp = null;
        private int MONTH, YEAR;
        public ucNewExpense()
        {          
            InitializeComponent();
            this.rdoPersinal.IsChecked = true;
            rdoPersinal.Checked += rdoPersinal_Checked;
            rdoObject.Checked += rdoObject_Checked;
            mList = new List<PTS_TABLE_SRC>();
            mEList = new List<TB_EXPENSE>();
            DateTime sysDate = TableManager.DBServerTime();
            MONTH = sysDate.Month;
            YEAR = sysDate.Year;
            BindStaticData();
            BindExpenseData();
            this.rdoPersinal.Focus();
            //if (Global.g_userrole == 8)
            //    this.chkQTCT.IsEnabled = true;
        }

        public ucNewExpense(TB_EXPENSE exp)
            : this()
        {
            mExp = exp;
            this.rdoPersinal.IsChecked = true;
            rdoPersinal.Checked += rdoPersinal_Checked;
            rdoObject.Checked += rdoObject_Checked;
            mList = new List<PTS_TABLE_SRC>();
            mEList = new List<TB_EXPENSE>();
            DateTime sysDate = TableManager.DBServerTime();
            MONTH = sysDate.Month;
            YEAR = sysDate.Year;
            BindStaticData();  //绑定静态数据
            BindExpenseData2(); //绑定报销数据
            this.rdoPersinal.Focus();
        }

        private void BindExpenseData()
        {
            mEList = new List<TB_EXPENSE>();
            this.dgExpense.ItemsSource = null;
            try
            {
                if (this.rdoPersinal.IsChecked == true)
                { 
                    //个人报销
                    TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(new EqExpression("OBJECTID", 0), new EqExpression("STATUS", 1)) ;
                    if (arr != null && arr.Length > 0)
                    {
                        mEList = new List<TB_EXPENSE>(arr);
                        mEList = mEList.FindAll(a =>a.RESPONSESTATUS != 2);
                    }
                }
                else
                { 
                    //项目报销
                    if (txtProj.Tag != null)
                    {
                        if (Global.g_userrole == 8 || (txtProj.Tag as TB_PROJECT).CREATEUSER == Global.g_usercode)
                            this.chkQTCT.IsEnabled = true;
                        else
                            this.chkQTCT.IsEnabled = false;
                        TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(new EqExpression("OBJECTID", (txtProj.Tag as TB_PROJECT).Id), new EqExpression("STATUS", 1));
                        if (arr != null && arr.Length > 0)
                        {
                            mEList = new List<TB_EXPENSE>(arr);
                            mEList = mEList.FindAll(a => a.LEADERRESPONSESTATUS != 2 || a.RESPONSESTATUS != 2);
                        }
                    }
                }
                mEList = mEList.FindAll(a => a.MONTH == MONTH && a.YEAR == YEAR);
                this.dgExpense.ItemsSource = screening(mEList);

                decimal totalmoney = 0;
                this.labTotal.Content = "共: 0张   合计: 0.00元";
                if (this.dgExpense.ItemsSource != null)
                {
                    List<TB_EXPENSE> _ls = this.dgExpense.ItemsSource as List<TB_EXPENSE>;
                    for (int i = 0; i < _ls.Count; i++)
                    {
                        totalmoney += _ls[i].MONEY;
                    }
                    this.labTotal.Content = "共:" + _ls.Count + "张   合计:" + totalmoney.ToString() + "元";
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void BindExpenseData2()
        {
            mEList = new List<TB_EXPENSE>();
            this.dgExpense.ItemsSource = null;
            try
            {                
                if (mExp.OBJECTID == 0)
                {
                    this.rdoPersinal.IsChecked = true;
                    //个人报销
                    TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(new EqExpression("OBJECTID", 0), new EqExpression("STATUS", 1));
                    if (arr != null && arr.Length > 0)
                    {
                        mEList = new List<TB_EXPENSE>(arr);
                    }
                }                
                else
                {
                    this.jt.IsEnabled = false;
                    this.cb.IsEnabled = false;
                    this.qt.IsEnabled = false;
                    this.tx.IsEnabled = false;
                    this.rdoObject.IsChecked = true;
                    TB_PROJECT proj = TB_PROJECTDAO.FindFirst(new EqExpression("Id", mExp.OBJECTID));                    
                    if (proj != null)
                    {
                        this.txtProj.Tag = proj;
                        this.txtProj.Text = proj.OBJECTNAME;
                        //项目报销
                        TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(new EqExpression("OBJECTID", (txtProj.Tag as TB_PROJECT).Id), new EqExpression("STATUS", 1));
                        if (arr != null && arr.Length > 0)
                        {
                            mEList = new List<TB_EXPENSE>(arr);
                        }
                    }                    
                }
                mEList = mEList.FindAll(a => a.MONTH == MONTH && a.YEAR == YEAR);
                this.dgExpense.ItemsSource = screening(mEList);
                this.txtMoney.Text = this.mExp.MONEY.ToString();
                this.txtFPHM.Text = this.mExp.BILLNO;
                this.cmbExprenseItem.Text = mExp.EXPENS;
                this.txtComments.Text = mExp.COMMENTS;
                if (mExp.ISQTCT == 1)
                {
                    this.chkQTCT.IsEnabled = true;
                    this.chkQTCT.IsChecked = true;
                    this.txtCusName.Text = mExp.OPNAME;
                }
                this.btnSubmit.Content = "修  改";
            }
            catch (Exception ex)
            {
                this.btnSubmit.Content = "新  增";
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private List<TB_EXPENSE> screening(List<TB_EXPENSE> list)
        {
            var sortedList = from items in list orderby items.GROUPNO,items.Id descending select items;

            //var sortedList = ls.OrderBy(a => a.GROUPNO);
            List<TB_EXPENSE> ls = sortedList.ToList(); //这个时候会排序


            List<TB_EXPENSE> rtn = null;
            switch (Global.g_userrole)
            {
                case 7:
                    rtn = ls.FindAll(a => a.OPUID == Global.g_userid);
                    break;
                case 8:
                    rtn = ls.FindAll(a => a.OPUID == Global.g_userid);
                    break;
                case 9: //财务
                    rtn = ls.FindAll(a => a.OPUID == Global.g_userid);
                    break;
                case 10: //销售 只能看到自己的工程
                    rtn = ls.FindAll(a => a.OPUID == Global.g_userid);
                    break;
                case 11:
                    rtn = ls.FindAll(a => a.OPUID == Global.g_userid);
                    break;
                default:
                    break;
            }
            int _groupno = -1;
            for (int i = 0; i < rtn.Count; i++)
            {
                //处理成组
                if (rtn[i].GROUPNO > 0 && _groupno != rtn[i].GROUPNO )
                {
                    _groupno = rtn[i].GROUPNO;
                    rtn[i].STRGROUPNO = "┓";
                    rtn[i].grouptotal = getGroupTotal(rtn, _groupno);
                }
                else if (_groupno == rtn[i].GROUPNO)
                {
                    rtn[i].STRGROUPNO = "┃";
                    if (i + 1< rtn.Count && _groupno != rtn[i + 1].GROUPNO)
                    {
                        rtn[i].STRGROUPNO = "┛";
                    }
                    if (i + 1 == rtn.Count)
                    {
                        rtn[i].STRGROUPNO = "┛";
                    }
                }

                if (rtn[i].ISCOMPLETE == 0)
                    rtn[i].COMPLETE = "未提交";
                else
                    rtn[i].COMPLETE = "已提交";
                if (rtn[i].RESPONSESTATUS == 1)
                    rtn[i].StrResponseStatus = "驳回";
                else if (rtn[i].RESPONSESTATUS == 0)
                    rtn[i].StrResponseStatus = "待审";
                else if (rtn[i].RESPONSESTATUS == 2)
                    rtn[i].StrResponseStatus = "已审";

                if (rtn[i].LEADERRESPONSESTATUS == 1)
                    rtn[i].StrLeaderResponseStatus = "驳回";
                else if (rtn[i].LEADERRESPONSESTATUS == 0)
                    rtn[i].StrLeaderResponseStatus = "待审";
                else if (rtn[i].LEADERRESPONSESTATUS == 2)
                    rtn[i].StrLeaderResponseStatus = "已审";
            }
            return rtn;
        }

        private string getGroupTotal(List<TB_EXPENSE> list, int groupno)
        {
            string rtn = "";
            try
            {               
                int counts = 0;
                decimal total = 0;
                List<TB_EXPENSE> tmpls = list.FindAll(a => a.GROUPNO == groupno);
                if (tmpls.Count > 0)
                {
                    counts = tmpls.Count;
                    for (int i = 0; i < tmpls.Count; i++)
                    {
                        total += tmpls[i].MONEY;
                    }
                }
                rtn = "共:" + counts.ToString() + "张 合计金额:" + total.ToString() + "元";
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
            return rtn;
        }
       
        private void BindStaticData()
        {           
            //获取静态报销类型数据
            this.cmbExprenseItem.ItemsSource = null;
            PTS_TABLE_SRC tmp = new PTS_TABLE_SRC();
            tmp.TABLE_NAME = "请选择";
            tmp.TABLE_VALUE = "0";
            PTS_TABLE_SRC[] arr = PTS_TABLE_SRCDAO.FindAll(new EqExpression("TABLE_ID", "PTS_TABLE_EXPENSE_ITEM"), new EqExpression("STATUS", 1));
            if (arr.Length > 0)
            {
                mList = new List<PTS_TABLE_SRC>(arr);
                mList.Insert(0, tmp);
            }
           
            this.cmbExprenseItem.ItemsSource = mList;
            this.cmbExprenseItem.DisplayMemberPath = "TABLE_NAME";
            this.cmbExprenseItem.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmObjectSearch frm = new frmObjectSearch(0);
            frm.ShowDialog();
            if (frm.mProj != null)
            {
                TB_PROJECT proj = frm.mProj;
                this.txtProj.Text = proj.OBJECTNAME;
                this.txtProj.Tag = proj;
            }
            BindExpenseData();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (this.rdoObject.IsChecked == false && this.rdoPersinal.IsChecked == false)
            {
                MessageHelper.ShowMessage("请选择项目报销或个人报销");
                if (this.rdoObject.IsChecked==true && this.txtProj.Tag==null)
                {
                    MessageHelper.ShowMessage("请选择报销所对应的工程项目");
                }
            }
            else
            {
                try
                {
                    int ISMEMBER = 0;
                    int RESPONSESTATUS = 0;  //默认审核通过
                    //int ISCOMPLETE = 0; //默认提交状态
                    PTS_TABLE_SRC src = this.cmbExprenseItem.SelectedItem as PTS_TABLE_SRC;
                    DateTime sysDate = TableManager.DBServerTime();
                    DateTime expenseDate = this.dtpDate.DateTime;
                    if (expenseDate < sysDate.AddMonths(-6))
                    {
                        MessageHelper.ShowMessage("发票日期不能超过半年！");
                        return;
                    }
                    if (string.IsNullOrEmpty(txtFPHM.Text))
                    {
                        MessageHelper.ShowMessage("请填写发票号码！");
                        return;
                    }
                    if (src.ID == 0)
                    {
                        MessageHelper.ShowMessage("请选择报销类型!");
                        return;
                    }
                    else
                    {
                        if (this.rdoObject.IsChecked == true)
                        {
                            if (chkQTCT.IsChecked == true && String.IsNullOrEmpty(txtCusName.Text))
                            {
                                MessageHelper.ShowMessage("公司账户填报必须填写客户名称！");
                                txtCusName.Focus();
                                return;
                            }                        
                            //判断报销填报人员是否是项目内的成员                           
                            if (this.chkQTCT.IsChecked == false && Global.g_usercode != (txtProj.Tag as TB_PROJECT).TEAMLEDER && ((txtProj.Tag as TB_PROJECT).TEAMMEMBER).IndexOf(Global.g_usercode) < 0)
                            {
                                if (MessageBox.Show("您不属于" + txtProj.Text + "项目的成员，是否需要保存报销信息？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                    return;
                                else
                                {
                                    ISMEMBER = 1;
                                    RESPONSESTATUS = 0;
                                }
                            }
                        }
                        else
                        { RESPONSESTATUS = 0; }
                        decimal money = -1;
                        if (!String.IsNullOrEmpty(txtMoney.Text))
                        {
                            decimal.TryParse(txtMoney.Text, out money);
                        }
                        if (money <= 0)
                        {
                            MessageHelper.ShowMessage("报销填报金额格式错误!");
                            return;
                        }
                        if (mExp == null)
                        {
                            DateTime date = DateTime.Parse(new BaseBusiness().date());
                            {
                                int day = date.Day;
                                if (day > 26)
                                {
                                    MessageHelper.ShowMessage("每月报销25日截止！");
                                    return;
                                }
                            }
                            TB_EXPENSE expense = new TB_EXPENSE();
                            expense.OBJECTID = 0;
                            if (txtProj.Tag != null)
                            {
                                expense.OBJECTID = (txtProj.Tag as TB_PROJECT).Id;
                                expense.OBJECTNAME = txtProj.Text;
                            }
                            expense.OPUID = Global.g_userid;
                            expense.OPNAME = Global.g_usercode;  //工号
                            expense.EXPENS = src.TABLE_NAME;  //报销类型名称
                            expense.EXPENSTYPE = src.ID;  //报销类型ID
                            expense.MONEY = money;
                            expense.ISMEMBER = ISMEMBER;
                            expense.CREATEDATE = this.dtpDate.DateTime; //当前系统时间
                            expense.BILLNO = txtFPHM.Text;
                            expense.COMMENTS = txtComments.Text;
                            expense.RESPONSESTATUS = RESPONSESTATUS;
                            expense.ISCOMPLETE = 0;  //提交状态 默认未提交
                            expense.STATUS = 1;
                            BaseBusiness bs = new BaseBusiness();
                            string strTime = bs.date();
                            DateTime time = DateTime.Parse(strTime);
                            expense.YEAR = time.Year;
                            expense.MONTH = time.Month;
                            expense.LEADERRESPONSESTATUS = RESPONSESTATUS;
                            if (chkQTCT.IsChecked == true && !String.IsNullOrEmpty(txtCusName.Text))
                            {
                                expense.OPNAME = txtCusName.Text;
                                expense.ISQTCT = 1;
                                expense.RESPONSESTATUS = 0;
                                expense.LEADERRESPONSESTATUS = 0;
                            }
                            expense.Save(); //新增报销
                        }
                        else //update
                        {
                            mExp.OBJECTID = 0;
                            if (txtProj.Tag != null)
                            {
                                mExp.OBJECTID = (txtProj.Tag as TB_PROJECT).Id;
                                mExp.OBJECTNAME = txtProj.Text;
                            }
                            mExp.OPUID = Global.g_userid;
                            mExp.OPNAME = Global.g_usercode;  //工号
                            mExp.EXPENS = src.TABLE_NAME;  //报销类型名称
                            mExp.EXPENSTYPE = src.ID;  //报销类型ID
                            mExp.MONEY = money;
                            mExp.ISMEMBER = ISMEMBER;
                            mExp.CREATEDATE = this.dtpDate.DateTime;  //当前系统时间
                            mExp.BILLNO = txtFPHM.Text;
                            mExp.COMMENTS = txtComments.Text;
                            mExp.STATUS = 1;                            
                            if(mExp.RESPONSESTATUS==1)
                                mExp.RESPONSESTATUS = 0;  //待审
                            if (mExp.LEADERRESPONSESTATUS == 1)
                                mExp.LEADERRESPONSESTATUS = 0;  //待审
                            if (chkQTCT.IsChecked == true && !String.IsNullOrEmpty(txtCusName.Text))
                            {
                                mExp.OPNAME = txtCusName.Text;
                                mExp.ISQTCT = 1;
                                mExp.RESPONSESTATUS = 2;
                                mExp.LEADERRESPONSESTATUS = 2;
                            }
                            mExp.Update();
                            this.btnSubmit.Content = "新  增";
                            mExp = null;
                        }
                        refreshForm();
                    }
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowMessage(ex.Message); 
                }
            }
            BindExpenseData();
        }

        /// <summary>
        /// 刷新界面控件
        /// </summary>
        private void refreshForm()
        {
            this.txtFPHM.Text = string.Empty;
            this.txtMoney.Text = "0.00";
            this.txtComments.Text = string.Empty;
            this.cb.IsChecked = false;
            this.tx.IsChecked = false;
            this.jt.IsChecked = false;
            this.qt.IsChecked = false;
            this.chkQTCT.IsChecked = false;
            this.txtCusName.Text = "";           
        }

        #region 报销
        /// <summary>
        /// 项目报销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoObject_Checked(object sender, RoutedEventArgs e)
        {
            if (rdoObject.IsChecked == true)
            {
                this.txtProj.IsEnabled = true;
                this.btnSearch.IsEnabled = true;
                this.jt.IsEnabled = false;
                this.cb.IsEnabled = false;
                this.qt.IsEnabled = false;
                this.tx.IsEnabled = false;
                //this.col6.Visibility = System.Windows.Visibility.Visible;
                //this.col7.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.txtProj.Text = string.Empty;
                this.txtProj.IsEnabled = false;
                this.txtProj.Tag = null;
                this.btnSearch.IsEnabled = false;
                this.jt.IsEnabled = true;
                this.cb.IsEnabled = true;
                this.qt.IsEnabled = true;
                this.tx.IsEnabled = true;
                //this.col6.Visibility = System.Windows.Visibility.Hidden;
                //this.col7.Visibility = System.Windows.Visibility.Hidden;
            }
            BindStaticData();
            BindExpenseData();
        }
        /// <summary>
        /// 个人报销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoPersinal_Checked(object sender, RoutedEventArgs e)
        {
            if (rdoPersinal.IsChecked == true)
            {
                this.txtProj.Text = string.Empty;
                this.txtProj.IsEnabled = false;
                this.txtProj.Tag = null;
                this.btnSearch.IsEnabled = false;

                this.jt.IsEnabled = true;
                this.cb.IsEnabled = true;
                this.qt.IsEnabled = true;
                this.tx.IsEnabled = true;
                this.col6.Visibility = System.Windows.Visibility.Hidden;
                this.col7.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                this.txtProj.IsEnabled = true;
                this.btnSearch.IsEnabled = true;
                this.jt.IsEnabled = false;
                this.cb.IsEnabled = false;
                this.qt.IsEnabled = false;
                this.tx.IsEnabled = false;
                this.col6.Visibility = System.Windows.Visibility.Visible;
                this.col7.Visibility = System.Windows.Visibility.Visible;
            }
            BindStaticData();
            BindExpenseData();
        }
        #endregion

        private void rdio_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked == true)
            {
                this.txtComments.Text = (sender as RadioButton).Name;
            }
        }

        private void contorl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {

                    Control control = sender as Control;
                    if (control.TabIndex == 13)
                    {
                        btnSubmit_Click(null, null);
                    }
                    //else
                        //contorl_KeyDown(sender, Key.Tab);
                }
            }
            catch(Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void dgExpense_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgExpense.SelectedItem != null)
            {
                mExp = dgExpense.SelectedItem as TB_EXPENSE;
                BindExpenseData2();
            }
        }

        /// <summary>
        /// 提交报销信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTJ_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgExpense.ItemsSource != null)
            {
                List<TB_EXPENSE> list = this.dgExpense.ItemsSource as List<TB_EXPENSE>;
                for (int i = 0; i < list.Count; i++)
                {
                    TB_EXPENSE expense = list[i];
                    expense.ISCOMPLETE = 1;
                    expense.Update();
                }
            }
            BindExpenseData();
        }

        private void chkQTCT_Checked(object sender, RoutedEventArgs e)
        {
            if (chkQTCT.IsChecked == true)
                this.txtCusName.IsEnabled = true;
            else
                this.txtCusName.IsEnabled = false;
        }

        private void menuDel_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgExpense.SelectedItem != null)
            {
                TB_EXPENSE _exp = this.dgExpense.SelectedItem as TB_EXPENSE;
                
                if (mExp != null && mExp.Id == _exp.Id)
                {
                    MessageHelper.ShowMessage("请先保存当前修改报销信息！");
                    return;
                }
                else
                {
                    if (_exp.RESPONSESTATUS == 2 && _exp.LEADERRESPONSESTATUS == 2)
                    {
                        if (!string.IsNullOrEmpty(_exp.OBJECTNAME) && _exp.ISMEMBER == 1)
                        {
                            MessageBox.Show("该报销已审核通过不能删除");
                            return;
                        }
                    }
                   _exp.STATUS = 0;
                   _exp.Update();
                }
            }
            BindExpenseData();
        }

        private void btnGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int GROUPNO = new BaseBusiness().getMaxGroupNo(Global.g_userid) + 1;
                List<TB_EXPENSE> ls = dgExpense.ItemsSource as List<TB_EXPENSE>;
                List<TB_EXPENSE> checkList = new List<TB_EXPENSE>();
                for (int i = 0; i < ls.Count; i++)
                {
                    if (ls[i].IsChecked == true)
                    {
                        checkList.Add(ls[i]);
                    }
                }
                if (checkList.Count > 1)
                {
                    for (int i = 0; i < checkList.Count; i++)
                    {
                        checkList[i].GROUPNO = GROUPNO;
                        checkList[i].Update();
                    }
                    BindExpenseData();
                }
                else
                {
                    MessageHelper.ShowMessage("单条报销记录不能成组！");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            TB_EXPENSE expen = (TB_EXPENSE)this.dgExpense.SelectedItem;
            if (expen != null)
            {
                expen.IsChecked = !expen.IsChecked;
            }
        }

        private void btnGroupCancle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<TB_EXPENSE> ls = dgExpense.ItemsSource as List<TB_EXPENSE>;
                for (int i = 0; i < ls.Count; i++)
                {
                    if (ls[i].IsChecked == true)
                    {
                        ls[i].GROUPNO = 0;
                        ls[i].Update();
                    }
                }
                BindExpenseData();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }
    }
}
