using NHibernate.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WY.Common;
using WY.Common.Framework;
using WY.Common.Message;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// frmExpenseAler.xaml 的交互逻辑
    /// </summary>
    public partial class frmExpenseAler : Window
    {
        private string year;
        private string month;
        private Dictionary<TB_EXPENSE_SUMMERY, List<TB_EXPENSE>> dic;

        public frmExpenseAler(string _year, string _month)
        {
            InitializeComponent();
            this.dgExpense.ContextMenu = contextMenuLogs;
            //绑定数据
            rdo1.Checked -= rdo1_Checked;
            rdo2.Checked -= rdo2_Checked;
            //BindDataSource();
            rdo1.Checked += rdo1_Checked;
            rdo2.Checked += rdo2_Checked;
            rdo1.IsChecked = true;
            this.year = _year;
            this.month = _month;
            this.cmbYear.Text = this.year;
            this.cmbMonth.Text = this.month;
            if (Global.g_userrole == 8 || Global.g_userrole == 9)
                this.btnPass.Visibility = System.Windows.Visibility.Visible;
            else
                this.btnPass.Visibility = System.Windows.Visibility.Hidden;
            bindExpenseData();
        }

        /// <summary>
        /// 报销查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            bindExpenseData();
        }

        private void bindExpenseData()
        {
            List<TB_EXPENSE> _ls = new List<TB_EXPENSE>();
            //查询条件
            int expenxeType = 0;  //个人或项目
            if (this.rdo2.IsChecked == true)  //项目报销
            {
                expenxeType = 1;
            }
            _ls = Comments.Comment.QueryExpenseAlert(expenxeType, int.Parse(cmbYear.Text), int.Parse(cmbMonth.Text));
            this.dgExpense.ItemsSource = null;
            _ls = _ls.FindAll(a => a.LEADERRESPONSESTATUS == 0 || a.RESPONSESTATUS == 0);
            summeryExpenseDteail(_ls);
        }

        private void summeryExpenseDteail(List<TB_EXPENSE> detail)
        {
            dic = new Dictionary<TB_EXPENSE_SUMMERY, List<TB_EXPENSE>>();
            for (int i = 0; i < detail.Count; i++)
            {
                string opname = detail[i].OPNAME;

                bool isExit = false;
                foreach (KeyValuePair<TB_EXPENSE_SUMMERY, List<TB_EXPENSE>> kvp in dic)
                {
                    if ((kvp.Key as TB_EXPENSE_SUMMERY).opname == opname)
                    {
                        isExit = true;
                        break;
                    }
                }
                if (isExit == false)
                {
                    TB_EXPENSE_SUMMERY es = new TB_EXPENSE_SUMMERY();
                    es.opname = opname;
                    es.year = cmbYear.Text;
                    es.month = cmbMonth.Text;
                    es.id = detail[i].OBJECTID;
                    List<TB_EXPENSE> ls = detail.FindAll(a => a.OPNAME == opname);
                    decimal money = (from m in ls
                                     select m.MONEY).Sum();
                    es.money = money;
                    dic.Add(es, ls);
                }
            }
            List<TB_EXPENSE_SUMMERY> lses = new List<TB_EXPENSE_SUMMERY>();
            foreach (KeyValuePair<TB_EXPENSE_SUMMERY, List<TB_EXPENSE>> kvp in dic)
            {
                TB_EXPENSE_SUMMERY es = new TB_EXPENSE_SUMMERY();
                es = kvp.Key as TB_EXPENSE_SUMMERY;
                lses.Add(es);
            }
            this.dgSummery.ItemsSource = null;
            this.dgSummery.ItemsSource = lses;
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

        /// <summary>
        /// 数据过滤
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        private List<TB_EXPENSE> screening(List<TB_EXPENSE> ls)
        {
            List<TB_EXPENSE> rtn = null;
            //只有总经理和财务可以查看所有的报销信息，其他人员只能查看自己的报销信息
            switch (Global.g_userrole)
            {
                case 7:
                    rtn = ls;
                    break;

                case 8:
                    rtn = ls;
                    break;

                case 9:
                    rtn = ls;
                    break;

                default:
                    rtn = ls.FindAll(a => a.OPUID == Global.g_userid);  //数据过滤
                    break;
            }
            return rtn;
        }

        private void rdo1_Checked(object sender, RoutedEventArgs e)
        {
            if (rdo1.IsChecked == true)
            {
                this.dgExpense.ItemsSource = null;
            }
        }

        private void rdo2_Checked(object sender, RoutedEventArgs e)
        {
            if (rdo2.IsChecked == true)
            {
                this.dgExpense.ItemsSource = null;
            }
        }

        #region 勿删！

        private void dgExpense_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //暂时关闭双击编辑功能 （勿删！）
            //if (dgExpense.SelectedItem != null)
            //{
            //    TB_EXPENSE exp = dgExpense.SelectedItem as TB_EXPENSE;
            //    frmExpense frm = new frmExpense(exp);
            //    frm.ShowDialog();
            //    btnSearch_Click(null,null);
            //}
        }

        #endregion 勿删！



        /// <summary>
        /// 报销驳回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuEdit_Click(object sender, RoutedEventArgs e)
        {
            //判断是否是项目销售人员及报销填报人不是该项目成员
            if (this.dgExpense.SelectedItem != null)
            {
                TB_EXPENSE exp = this.dgExpense.SelectedItem as TB_EXPENSE;
                frmReponse frm = new frmReponse(exp);
                frm.ShowDialog();
                bindExpenseData();
            }
        }

        private bool isProjTeam(TB_EXPENSE expense)
        {
            bool rtn = true;
            return rtn;
        }

        private void menuPass_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgExpense.SelectedItem != null)
            {
                TB_EXPENSE expense = (this.dgExpense.SelectedItem as TB_EXPENSE);
                expense.RESPONSESTATUS = 2;
                expense.RESPONSE = "";
                expense.Update();
                bindExpenseData();
            }
        }

        private void dgExpense_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown -= Row_MouseRightButtonDown;
            e.Row.MouseRightButtonDown += Row_MouseRightButtonDown;
        }

        private void Row_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuEdit.Visibility = System.Windows.Visibility.Collapsed;
            menuPass.Visibility = System.Windows.Visibility.Collapsed;
            menuEdit2.Visibility = System.Windows.Visibility.Collapsed;
            menuPass2.Visibility = System.Windows.Visibility.Collapsed;

            TB_EXPENSE expense = (TB_EXPENSE)dgExpense.SelectedItem;
            if (expense != null)
            {
                //TB_PROJECT proj = this.txtProj.Tag as TB_PROJECT; //工程
                TB_PROJECT proj = TB_PROJECTDAO.FindFirst(new EqExpression("Id", expense.OBJECTID));
                if (proj != null)  //项目报销
                {
                    //总经理
                    if (Global.g_userrole == 8 || Global.g_usercode == proj.CREATEUSER)
                    {
                        menuEdit2.Visibility = System.Windows.Visibility.Visible;
                        menuPass2.Visibility = System.Windows.Visibility.Visible;
                    }
                    else if (Global.g_userrole == 9)
                    {
                        menuEdit.Visibility = System.Windows.Visibility.Visible;
                        menuPass.Visibility = System.Windows.Visibility.Visible;
                    }
                }
                else  //个人报销
                {
                    if (Global.g_userrole == 9)
                    {
                        menuEdit.Visibility = System.Windows.Visibility.Visible;
                        menuPass.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime time = TableManager.DBServerTime();
            this.cmbYear.Text = time.Year.ToString();
            this.cmbMonth.Text = time.Month.ToString();
        }

        private void menuEdit2_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgExpense.SelectedItem != null)
            {
                TB_EXPENSE exp = this.dgExpense.SelectedItem as TB_EXPENSE;
                frmReponse2 frm = new frmReponse2(exp);
                frm.ShowDialog();
                bindExpenseData();
            }
        }

        private void menuPass2_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgExpense.SelectedItem != null)
            {
                TB_EXPENSE expense = (this.dgExpense.SelectedItem as TB_EXPENSE);
                expense.LEADERRESPONSESTATUS = 2;
                expense.LEADERRESPONSE = "";
                expense.Update();
                bindExpenseData();
            }
        }

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            if (dgExpense.ItemsSource != null)
            {
                List<TB_EXPENSE> list = dgExpense.ItemsSource as List<TB_EXPENSE>;
                for (int i = 0; i < list.Count; i++)
                {
                    if (Global.g_userrole == 9)
                    {
                        if (list[i].RESPONSESTATUS == 0)
                            list[i].RESPONSESTATUS = 2;
                    }
                    if (Global.g_userrole == 8)
                    {
                        if (list[i].LEADERRESPONSESTATUS == 0)
                            list[i].LEADERRESPONSESTATUS = 2;
                    }
                    list[i].Update();
                }
            }
            bindExpenseData();
        }

        private void dgSummery_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (dgSummery.SelectedItem != null)
            {
                TB_EXPENSE_SUMMERY item = dgSummery.SelectedItem as TB_EXPENSE_SUMMERY;
                foreach (KeyValuePair<TB_EXPENSE_SUMMERY, List<TB_EXPENSE>> kvp in dic)
                {
                    if (kvp.Key as TB_EXPENSE_SUMMERY == item)
                    {
                        //TB_PROJECT proj = this.txtProj.Tag as TB_PROJECT; //工程类型
                        TB_PROJECT proj = TB_PROJECTDAO.FindFirst(new EqExpression("Id", item.id));
                        List<TB_EXPENSE> ls = new List<TB_EXPENSE>();
                        if (proj != null && (proj.CREATEUSER != Global.g_usercode || Global.g_userrole == 8 || Global.g_userrole == 9))
                            ls = screening(kvp.Value as List<TB_EXPENSE>);
                        else
                            ls = kvp.Value as List<TB_EXPENSE>;

                        var sortedList = from items in ls orderby items.GROUPNO, items.Id descending select items;
                        ls = sortedList.ToList(); //这个时候会排序

                        decimal totalmoney = 0;
                        int _groupno = -1;
                        List<string> kmls = new List<string>();  //科目名称
                        for (int i = 0; i < ls.Count; i++)
                        {
                            string _km = ls[i].EXPENS;
                            if (!kmls.Contains(_km))
                                kmls.Add(_km);
                            //处理成组
                            if (ls[i].GROUPNO > 0 && _groupno != ls[i].GROUPNO)
                            {
                                _groupno = ls[i].GROUPNO;
                                ls[i].STRGROUPNO = "┓";
                                ls[i].grouptotal = getGroupTotal(ls, _groupno);
                            }
                            else if (_groupno == ls[i].GROUPNO)
                            {
                                ls[i].STRGROUPNO = "┃";
                                if (i + 1 < ls.Count && _groupno != ls[i + 1].GROUPNO)
                                {
                                    ls[i].STRGROUPNO = "┛";
                                }
                                if (i + 1 == ls.Count)
                                {
                                    ls[i].STRGROUPNO = "┛";
                                }
                            }
                            if (ls[i].GROUPNO == 0)
                                ls[i].STRGROUPNO = "";

                            totalmoney += ls[i].MONEY;
                            if (ls[i].RESPONSESTATUS == 1)
                                ls[i].StrResponseStatus = "驳回";
                            else if (ls[i].RESPONSESTATUS == 0)
                                ls[i].StrResponseStatus = "待审";
                            else if (ls[i].RESPONSESTATUS == 2)
                                ls[i].StrResponseStatus = "已审";

                            if (ls[i].LEADERRESPONSESTATUS == 1)
                                ls[i].StrLeaderResponseStatus = "驳回";
                            else if (ls[i].LEADERRESPONSESTATUS == 0)
                                ls[i].StrLeaderResponseStatus = "待审";
                            else if (ls[i].LEADERRESPONSESTATUS == 2)
                                ls[i].StrLeaderResponseStatus = "已审";
                        }
                        this.dgExpense.ItemsSource = ls;
                        if (ls != null)
                        {
                            dgExpense.SelectedIndex = 0;
                        }
                        string foot = "";
                        for (int i = 0; i < kmls.Count; i++)
                        {
                            string _km = kmls[i];
                            List<TB_EXPENSE> _expls = ls.FindAll(a => a.EXPENS == _km);
                            if (_expls.Count > 0)
                            {
                                int counts = _expls.Count;
                                decimal kmtotal = 0;
                                for (int j = 0; j < _expls.Count; j++)
                                {
                                    kmtotal += _expls[j].MONEY;
                                }
                                foot += "  " + _km + ": 共" + counts.ToString() + "张 合计金额" + kmtotal.ToString() + "元；";
                            }
                        }
                        //this.labFoot.Content = foot;
                        //this.labTotal.Content = "共:" + ls.Count + "张   合计:" + totalmoney.ToString() + "元";
                    }
                }
            }
        }
    }
}