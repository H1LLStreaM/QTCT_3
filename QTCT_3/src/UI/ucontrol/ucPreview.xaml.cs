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
using WY.Common;
using WY.Common.Message;
using WY.Common.Utility;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// UserExpenseList.xaml 的交互逻辑
    /// </summary>
    public partial class ucPreview : UserControl
    {
        private int mprojID;
        private TB_EXPENSE[] mExparr;
        private TB_PROJECT mproj;
        public ucPreview(int _projectID)
        {
            InitializeComponent();
            mprojID = _projectID;
            this.dgExpense.ItemsSource = null;
            Query(); //报销信息查询
            BindProjectInfo();
            QueryBillInfo();
            profileProcess(); //利润计算
            this.panel.Children.Clear();
            rationProcess(); //提成计算
        }

        private void Query()
        {
            try
            {
                if (mprojID > 0)
                {
                    mExparr = TB_EXPENSEDAO.FindAll(new EqExpression("OBJECTID", mprojID), new EqExpression("STATUS", 1));
                    if (mExparr.Length > 0)
                    {
                        List<TB_EXPENSE> list = new List<TB_EXPENSE>(mExparr);
                        this.dgExpense.ItemsSource = list;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void BindProjectInfo()
        {
            try
            {
                if (mprojID > 0)
                {
                    mproj = TB_PROJECTDAO.FindFirst(new EqExpression("Id",mprojID));
                    if (mproj != null)
                    {
                        this.labCustomer.Content = mproj.COMPANYNAME;
                        this.labLeader.Content = mproj.TEAMLEDERNAME;
                        //this.labMoney.Content = mproj.MONEY;
                        this.labProjDate.Content = mproj.BEGINDATE.ToShortDateString() + "-" + mproj.ENDDATE.ToShortDateString();
                        this.labProjName.Content = mproj.OBJECTNAME;
                        this.labProjType.Content = Comments.Comment.setProjIdentity(mproj.OBJECTTYPENAME);
                        //this.labMember.Content = mproj.TEAMMEMBER.Replace('|', ' ');
                        //this.labJS.Content = mproj.BILLSTATUS;
                        //this.labJS.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void QueryBillInfo()
        {
            try
            {
                if (mprojID > 0)
                {
                    TB_BILL[] arr = TB_BILLDAO.FindAll(new EqExpression("STATUS", 1), new EqExpression("PROJECTID", mprojID));
                    if (arr.Length > 0)
                    {
                        List<TB_BILL> list = new List<TB_BILL>(arr);
                        this.dgBill.ItemsSource = null;
                        this.dgBill.ItemsSource = list;                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);            
            }
        }

        #region 利润明细

        private decimal staticRatio1;
        private decimal staticRatio2;
        private decimal staticRatio3;
        private void profileProcess()
        {
            //获取静态提成区间配置
            getStaticRatio();
            if (mproj != null)
            {
                this.labHTZJ.Content = mproj.MONEY;  
                this.labZHJ.Content = Math.Round((mproj.MONEY * decimal.Parse("0.94")),2); //折后含税合同价
                 //报销费用合计
                decimal totalExpense=0;
                for (int i = 0; i < mExparr.Length; i++)
                {
                    totalExpense += mExparr[i].MONEY;
                }
                labExpense.Content = totalExpense.ToString();
                labMoney3.Content = Math.Round((mproj.MONEY * decimal.Parse("0.94")) - totalExpense,2);  //毛利润

                decimal mlv = 0;
                if (mproj.MONEY > 0)
                {
                    mlv = Math.Round(((((mproj.MONEY * decimal.Parse("0.94")) - totalExpense) / (mproj.MONEY * decimal.Parse("0.94"))) * 100), 2); //毛利率
                    labMLV.Content = Math.Round(((((mproj.MONEY * decimal.Parse("0.94")) - totalExpense) / (mproj.MONEY * decimal.Parse("0.94"))) * 100), 2).ToString() + "%";  //毛利率
                    labJLR.Content = Math.Round((((mproj.MONEY * decimal.Parse("0.94")) - totalExpense) * decimal.Parse("0.8")), 2);  //项目净利润
                }
                else
                { 
                    
                }
                
                if (mlv > 40)
                {
                    labRatio.Content = staticRatio1;
                }
                else if (mlv > 30 && mlv < 40)
                {
                    labRatio.Content = staticRatio2;
                }
                else if (mlv < 30)
                {
                    labRatio.Content = staticRatio3;
                }
                labTCJE.Content = Math.Round(Utils.NvDecimal(labJLR.Content)*(Utils.NvDecimal(labRatio.Content)/100),2);
                labztcje.Content = Math.Round(Utils.NvDecimal(labJLR.Content) * (Utils.NvDecimal(labRatio.Content) / 100), 2);
            }
        }

        private void getStaticRatio()
        {
            PTS_EXCEL_PROFILE_SRC[] arr = PTS_EXCEL_PROFILE_SRCDAO.FindAll(new EqExpression("STATUS",1));
            if (arr.Length > 0)
            {
                staticRatio1 = Utils.NvDecimal(arr[0].ITEMVALUE);
                staticRatio2 = Utils.NvDecimal(arr[1].ITEMVALUE);
                staticRatio3 = Utils.NvDecimal(arr[2].ITEMVALUE);
            }
        }
        #endregion

        #region 提成明细
        private void rationProcess()
        {
            if (mproj != null)
            {
                if (string.IsNullOrEmpty(mproj.BILLSTATUS))
                {
                    labMyMoney.Content = "待结算";
                    return;
                }
                profileporcess pp = new profileporcess();
                projProfileClass ppc = pp.getProfile(TB_PROJECTDAO.FindFirst(new EqExpression("Id", mprojID)));
                List<TB_PERSONAL_PROFILE> list = pp.personalProcess(ppc, mprojID);
                if (list.Count > 0)
                {
                    List<TB_PERSONAL_PROFILE> rtn = list.FindAll(a => a.USERCODE==Global.g_usercode);  //数据过滤
                    if (rtn.Count > 0)
                        labMyMoney.Content = rtn[0].PROFILE1 + rtn[0].PROFILE2 + "元";
                    else
                        labMyMoney.Content = "0元";
                }
                else
                    labMyMoney.Content = "0元";
            }
        }
        #endregion
    }
}
