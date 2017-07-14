using NHibernate.Expression;
using QTCT_3.Comments;
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
using WY.Library.Business;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// ucQTCTProfile.xaml 的交互逻辑
    /// </summary>
    public partial class ucQTCTProfile : UserControl
    {
        public ucQTCTProfile()
        {
            InitializeComponent();
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            this.dgProfile.ItemsSource = null;
            Decimal TotalMoney = 0;//合同金额总计
            Decimal TotalCost = 0; //成本金额总计
            Decimal TotalProfile = 0; //净利润金额合计
            //项目查询
            string year1 = this.cmbYear.Text;
            string month1 = this.cmbMonth.Text;
            string year2 = this.cmbYear2.Text;
            string month2 = this.cmbMonth2.Text;
            string start = year1 + "-" + month1 + "-1 00:00:00";
            string end = year2 + "-" + month2 + "-1 23:59:59";
            DateTime startDate = DateTime.Parse(start);
            DateTime endDate = DateTime.Parse(end).AddMonths(1).AddDays(-1);
            List<TB_PROJECT> list = Comments.Comment.QueryProject(0,"","",startDate, endDate);
            if (list.Count > 0)
            {
                List<tmpProfile> ls = new List<tmpProfile>();
                for (int i = 0; i < list.Count; i++)
                {
                    tmpProfile tmp = new tmpProfile();
                    tmp.INDEX = i + 1;
                    tmp.projName = list[i].OBJECTNAME;
                    tmp.projDate = list[i].BEGINDATE.ToShortDateString();
                    tmp.projAddress = list[i].ADDRESS;
                    tmp.LEADER = list[i].TEAMLEDER;
                    tmp.SALER = list[i].CREATEUSER;
                    tmp.MONEY = list[i].MONEY;
                    TotalMoney += tmp.MONEY;
                    tmp.COST = ExpenseBusiness.getTotalExpense(list[i].Id);
                    TotalCost += tmp.COST;
                    profileporcess pp = new profileporcess();
                    projProfileClass ppc = pp.getProfile(list[i]);
                    if (ppc != null)
                    {
                        tmp.JLR = ppc.xmjlr; //净利润
                        tmp.JLV = ppc.mlv;  //净利率
                    }
                    TB_BILL bill = getBillInfo(list[i].Id);
                    if (bill != null)
                        tmp.BILLDATE = bill.CREATEDATE.ToShortDateString();
                    if (list[i].BILLDATE != null && list[i].BILLSTATUS == "已结算")
                        tmp.COMPLETEDATE = list[i].BILLDATE.Year.ToString() + "年" + list[i].BILLDATE.Month.ToString() + "月";
                    ls.Add(tmp);
                }
                this.dgProfile.ItemsSource = ls;
                this.lab1.Content = "合计发票金额:" + TotalMoney.ToString();
                this.lab2.Content = "合计成本金额:" + TotalCost.ToString();
                this.lab3.Content = "合计净利润金额:" + TotalProfile.ToString();
            }
        }

        private TB_BILL getBillInfo(int projId)
        {
            TB_BILL rtn = null;
            try
            {
                TB_BILL[] arr = TB_BILLDAO.FindAll(new EqExpression("PROJECTID", projId), new EqExpression("STATUS", 1));
                if (arr.Length > 0)
                {
                    rtn = arr[0];
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return rtn;
        }

        private void menuEdit_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class tmpProfile 
    {
        public int INDEX { get; set; }
        public string projName { get; set; }
        public string projDate { get; set; }
        public string projAddress { get; set; }
        public string LEADER { get; set; }
        public string SALER { get; set; }
        public decimal MONEY { get; set; }
        public decimal COST { get; set; }
        public decimal JLR { get; set; }
        public decimal JLV { get; set; }
        public string BILLDATE { get; set; }
        public string COMPLETEDATE { get; set; }
    }
}
