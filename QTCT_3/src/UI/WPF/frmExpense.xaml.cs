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
using WY.Common.Framework;
using WY.Common.Message;
using WY.Library.Business;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// 报销填报界面
    /// </summary>
    public partial class frmExpense : Window
    {
        private List<PTS_TABLE_SRC> mList = null;
        private List<TB_EXPENSE> mEList = null;
        private TB_EXPENSE mExp = null;
        public frmExpense()
        {          
            InitializeComponent();
            this.rdoPersinal.IsChecked = true;
            rdoPersinal.Checked += rdoPersinal_Checked;
            rdoObject.Checked += rdoObject_Checked;
            mList = new List<PTS_TABLE_SRC>();
            mEList = new List<TB_EXPENSE>();
            BindStaticData();
            BindExpenseData();
            this.rdoPersinal.Focus();
        }

        public frmExpense(TB_EXPENSE exp):this()
        {
            mExp = exp;
            this.rdoPersinal.IsChecked = true;
            rdoPersinal.Checked += rdoPersinal_Checked;
            rdoObject.Checked += rdoObject_Checked;
            mList = new List<PTS_TABLE_SRC>();
            mEList = new List<TB_EXPENSE>();
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
                    TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(new EqExpression("OBJECTID", 0), new EqExpression("STATUS", 1));
                    if (arr != null && arr.Length > 0)
                    {
                        mEList = new List<TB_EXPENSE>(arr);
                    }
                }
                else
                { 
                    //项目报销
                    if (txtProj.Tag != null)
                    {
                        TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(new EqExpression("OBJECTID", (txtProj.Tag as TB_PROJECT).Id), new EqExpression("STATUS", 1));
                        if (arr != null && arr.Length > 0)
                        {
                            mEList = new List<TB_EXPENSE>(arr);
                        }
                    }
                }
                this.dgExpense.ItemsSource = screening(mEList);
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
                this.dtpDate.DateTime = mExp.CREATEDATE;
                this.dgExpense.ItemsSource = screening(mEList);
                this.txtMoney.Text = this.mExp.MONEY.ToString();
                this.txtFPHM.Text = this.mExp.BILLNO;
                this.cmbExprenseItem.Text = mExp.EXPENS;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private List<TB_EXPENSE> screening(List<TB_EXPENSE> ls)
        {
            List<TB_EXPENSE> rtn = null;
            switch (Global.g_userrole)
            {
                case 7:
                    rtn = ls.FindAll(a => a.OPNAME == Global.g_usercode);
                    break;
                case 8:
                    rtn = ls.FindAll(a => a.OPNAME == Global.g_usercode);
                    break;
                case 9:
                    rtn = ls.FindAll(a => a.OPNAME == Global.g_usercode);
                    break;
                case 10: //销售 只能看到自己的工程
                    rtn = ls.FindAll(a => a.OPNAME == Global.g_usercode);
                    break;
                case 11:
                    rtn = ls.FindAll(a => a.OPNAME == Global.g_usercode);
                    break;
                default:
                    break;
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
            frmObjectSearch frm = new frmObjectSearch();
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
                    PTS_TABLE_SRC src = this.cmbExprenseItem.SelectedItem as PTS_TABLE_SRC;
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
                            if (txtFPHM.Text.IndexOf(';') > -1)
                            {
                                MessageHelper.ShowMessage("项目报销不允许多发票填报");
                                return;
                            }
                            //判断报销填报人员是否是项目内的成员                           
                            if (Global.g_usercode != (txtProj.Tag as TB_PROJECT).TEAMLEDER && ((txtProj.Tag as TB_PROJECT).TEAMMEMBER).IndexOf(Global.g_usercode) < 0)
                            {
                                if (MessageBox.Show("您不属于" + txtProj.Text + "项目的成员，是否需要保存报销信息？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                    return;
                                else
                                    ISMEMBER = 1;
                            }
                        }
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
                            expense.STATUS = 1;
                            BaseBusiness bs = new BaseBusiness();
                            string strTime = bs.date();
                            DateTime time = DateTime.Parse(strTime);
                            expense.YEAR = time.Year;
                            expense.MONTH = time.Month;
                            expense.Save();
                        }
                        else
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
                            //mExp.RESPONSESTATUS = 2;
                            mExp.Update();
                        }
                        MessageHelper.ShowMessage("保存成功!");
                        //refreshForm();
                        this.Close();
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
            }
            else
            {
                this.txtProj.IsEnabled = true;
                this.btnSearch.IsEnabled = true;
                this.jt.IsEnabled = false;
                this.cb.IsEnabled = false;
                this.qt.IsEnabled = false;
                this.tx.IsEnabled = false;
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
                BindExpenseData();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
