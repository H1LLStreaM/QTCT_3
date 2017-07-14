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
using WY.Common.Utility;
using WY.Library.Business;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// frmProject.xaml 的交互逻辑
    /// </summary>
    public partial class frmProject : Window
    {
        List<PTS_OBJECT_TYPE_SRC> mList;
        private TB_PROJECT mProj;
        public frmProject(TB_PROJECT _proj)
        {
            InitializeComponent();
            mList = new List<PTS_OBJECT_TYPE_SRC>();
            bindStaticData(); // 绑定初始数据
            if (_proj != null)
            {
                mProj = _proj;
                bindProjectData();

            }
            refreshControl();
        }

        public void refreshControl()
        {
            switch (Global.g_userrole)
            { 
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    txtCOMPANYNAME.IsEnabled = false;
                    txtCONTRACTNO.IsEnabled = false;
                    dtpBeginDate.IsEnabled = false;
                    dtpEndDate.IsEnabled = false;
                    txtMoney.IsEnabled = false;
                    btnLeader.IsEnabled = false;
                    btnEdit.IsEnabled = false;
                    txtMemo.IsEnabled = false;
                    chkBillStatus.IsEnabled = false;
                    txtADDRESS.IsEnabled = false;
                    btnBill.IsEnabled = false;
                    break;
                case 9:  //财务
                    this.btnSubmit.Visibility = System.Windows.Visibility.Collapsed;
                    this.btnSubmit2.Visibility = System.Windows.Visibility.Visible;
                    btnBill.IsEnabled = true;
                    chkBillStatus.IsEnabled = true;
                     TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(new EqExpression("OBJECTID", mProj.Id),new EqExpression("STATUS",1));
                     if (arr.Length > 0)
                     {
                         txtOBJECTNAME.IsEnabled = false;
                         txtCONTRACTNO.IsEnabled = false;
                         txtOBJECTNAME.IsEnabled = false;
                         txtADDRESS.IsEnabled = false;
                         dtpBeginDate.IsEnabled = false;
                         dtpEndDate.IsEnabled = false;
                         txtMoney.IsEnabled = false;
                         txtZHKOU.IsEnabled = false;
                         chkyn.IsEnabled = false;
                         chkyw.IsEnabled = false;
                         chkwl.IsEnabled = false;
                         chkzx.IsEnabled = false;
                         chkwx.IsEnabled = false;
                         chkwifi.IsEnabled = false;
                         chk3D.IsEnabled = false;
                         chk2D.IsEnabled = false;
                         chkxwj.IsEnabled = false;
                         chkqj.IsEnabled = false;
                         chkjrl.IsEnabled = false;
                         chkkfl.IsEnabled = false;
                         chkfhl.IsEnabled = false;
                         btnLeader.IsEnabled = false;
                         btnEdit.IsEnabled = false;
                         txtMemo.IsEnabled = false;
                     }
                    break;
            }
            if (mProj.BILLSTATUS == "已结算")
            {
                #region
                txtCOMPANYNAME.IsEnabled = false;
                txtCONTRACTNO.IsEnabled = false;
                dtpBeginDate.IsEnabled = false;
                dtpEndDate.IsEnabled = false;
                txtMoney.IsEnabled = false;
                btnLeader.IsEnabled = false;
                btnEdit.IsEnabled = false;
                txtMemo.IsEnabled = false;
                chkBillStatus.IsEnabled = false;
                txtADDRESS.IsEnabled = false;
                btnBill.IsEnabled = false;
                txtOBJECTNAME.IsEnabled = false;
                txtCONTRACTNO.IsEnabled = false;
                txtOBJECTNAME.IsEnabled = false;
                txtADDRESS.IsEnabled = false;
                dtpBeginDate.IsEnabled = false;
                dtpEndDate.IsEnabled = false;
                txtMoney.IsEnabled = false;
                txtZHKOU.IsEnabled = false;
                chkyn.IsEnabled = false;
                chkyw.IsEnabled = false;
                chkwl.IsEnabled = false;
                chkzx.IsEnabled = false;
                chkwx.IsEnabled = false;
                chkwifi.IsEnabled = false;
                chk3D.IsEnabled = false;
                chk2D.IsEnabled = false;
                chkxwj.IsEnabled = false;
                chkqj.IsEnabled = false;
                chkjrl.IsEnabled = false;
                chkkfl.IsEnabled = false;
                chkfhl.IsEnabled = false;
                btnLeader.IsEnabled = false;
                btnEdit.IsEnabled = false;
                txtMemo.IsEnabled = false;

                this.btnSubmit.IsEnabled = false;
                this.btnSubmit2.IsEnabled = false;
                #endregion
            }
        }

        /// <summary>
        /// 绑定初始数据 
        /// </summary>
        private void bindStaticData()
        {
           
        }

        private void bindProjectData()
        {
            try
            {
                //mProj
                this.txtOBJECTNAME.Text = mProj.OBJECTNAME;
                this.txtCONTRACTNO.Text = mProj.CONTRACTNO;
                this.txtCOMPANYNAME.Text = mProj.COMPANYNAME;
                this.txtADDRESS.Text = mProj.ADDRESS;
                this.dtpBeginDate.DateTime = mProj.BEGINDATE;
                this.dtpEndDate.DateTime = mProj.ENDDATE;
                if(mProj.OBJECTTYPENAME!=null)
                    setProjIdentity(mProj.OBJECTTYPENAME);
                //PTS_OBJECT_TYPE_SRC src = PTS_OBJECT_TYPE_SRCDAO.FindFirst(new EqExpression("ID", mProj.OBJECTTYPE));
                //if(src!=null)
                //    this.drpObjectType.Text = src.OBJECTTYPENAME;
                this.txtMoney.Text = mProj.MONEY.ToString();
                if (mProj.BILLSTATUS == "已结算")
                    this.chkBillStatus.IsChecked = true;
                if (!string.IsNullOrEmpty(mProj.TEAMLEDER))
                {
                    TB_User leader = TB_UserDao.FindFirst(new EqExpression("USER_CODE", mProj.TEAMLEDER));
                    this.txtleder.Text = leader.USER_NAME;
                    this.txtleder.Tag = leader;
                }
                //绑定组员
                string menbers = mProj.TEAMMEMBER;
                if (!string.IsNullOrEmpty(menbers))
                {
                    string[] arr = menbers.Split('|');
                    //TB_User[] arr_member = TB_UserDao.FindAll(new EqExpression("TEAMMEMBER", arr));
                    if (arr.Length > 0)
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            TB_User user = TB_UserDao.FindFirst(new EqExpression("USER_CODE", arr[i]));
                            uc_user uc = new uc_user(user);
                            uc.DelSelectUser+=uc_DelSelectUser;
                            this.warp.Children.Add(uc);
                        }
                    }                   
                    TB_BILL[] arr_bills = TB_BILLDAO.FindAll(new EqExpression("PROJECTID", mProj.Id), new EqExpression("STATUS", 1));
                    if (arr_bills.Length > 0)
                    {
                        //绑定发票信息
                        for (int i = 0; i < arr_bills.Length; i++)
                        {
                            uc_bill uc = new uc_bill(arr_bills[i]);
                            uc.DelSelectBill+=uc_DelSelectBill;
                            this.warp2.Children.Add(uc);
                        }
                    }
                    this.txtMemo.Text = mProj.MEMO;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void setProjIdentity(string identitys)
        {
            string[] arr = identitys.Split('|');
            for (int i = 0; i < arr.Length; i++)
            {
                string str = arr[i];
                if(arr[i]=="yn")
                    this.chkyn.IsChecked = true;
                if(arr[i]=="yw")
                    this.chkyw.IsChecked = true;
                if(arr[i]=="wl")
                    this.chkwl.IsChecked = true;
                if(arr[i]=="zx")
                    this.chkzx.IsChecked = true;
                if(arr[i]=="wx")
                    this.chkwx.IsChecked = true;
                if(arr[i]=="wifi")
                    this.chkwifi.IsChecked = true;
                if(arr[i]=="2D")
                    this.chk2D.IsChecked = true;
                if(arr[i]=="3D")
                    this.chk3D.IsChecked = true;
                if(arr[i]=="xwj")
                    this.chkxwj.IsChecked = true;
                if(arr[i]=="qj")
                    this.chkqj.IsChecked = true;
                if(arr[i]=="jrl")
                    this.chkjrl.IsChecked = true;
                if(arr[i]=="kfl")
                    this.chkkfl.IsChecked = true;
                if (arr[i] == "fhl")
                    this.chkfhl.IsChecked = true;
                
            }
        }

        /// <summary>
        /// 添加项目负责人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeader_Click(object sender, RoutedEventArgs e)
        {
            frmUserSearch frm = new frmUserSearch();
            frm.ShowDialog();
            if (frm.mUser != null)
            {
                TB_User user = frm.mUser[0];
                this.txtleder.Text = user.USER_NAME;
                this.txtleder.Tag = user;
            }
        }

        List<TB_User> mListMember = new List<TB_User>();
        /// <summary>
        /// 条件组员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            frmUserSearch frm = new frmUserSearch();
            frm.ShowDialog();
            List<TB_User> user = frm.mUser;
            if (user != null)
            {
                //warp.Children.Clear();
                for (int i = 0; i < user.Count; i++)
                {
                    uc_user uc = new uc_user(user[i]);
                    for (int j = 0; j < warp.Children.Count; j++)
                    {
                        if (((warp.Children[j] as uc_user).txtName.Tag as TB_User).Id == user[i].Id)
                        {
                            MessageHelper.ShowMessage(user[i].USER_NAME + "已存在,不可添加重复组员");
                            continue;
                        }
                    }
                    if (user[i].Id != (txtleder.Tag as TB_User).Id)
                    {
                        uc.txtName.Text = user[i].USER_NAME;
                        warp.Children.Add(uc);
                        uc.DelSelectUser += uc_DelSelectUser;
                    }
                    else
                    {
                        MessageHelper.ShowMessage("组员不能与负责人一致");
                        continue;
                    }
                }

            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="uc"></param>
        void uc_DelSelectUser(uc_user uc)
        {
            foreach (UserControl _uc in warp.Children)
            {
                if ((_uc as uc_user).MUser.Id == uc.MUser.Id)
                {
                    //判断是否有报销信息
                    TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(new EqExpression("STATUS", 1), new EqExpression("OPUID", uc.MUser.Id));
                    if (arr.Length == 0)
                    {
                        warp.Children.Remove(_uc);
                        break;
                    }
                    else
                        MessageHelper.ShowMessage((_uc as uc_user).MUser.USER_NAME + "已有项目报销信息，不能删除");
                }
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal zhekou = -1;
                decimal.TryParse(txtZHKOU.Text, out zhekou);
                this.txtZHKOU.Text = zhekou.ToString();
               
                //固定/可分配提成比例
                decimal ratio1 = 60;
                decimal ratio2 = 40;
                PTS_OBJECT_TYPE_SRC src = PTS_OBJECT_TYPE_SRCDAO.FindFirst(new EqExpression("STATUS", 1));
                if (src != null)
                {
                    ratio1 = src.RATIO1;
                    ratio2 = src.RATIO2;
                }
                if (mProj == null)  //新增工程信息
                {
                    // 检查OBJECTNAME(工程名称)是否重复?
                    TB_PROJECT proj = new TB_PROJECT();
                    proj.OBJECTNAME = this.txtOBJECTNAME.Text;
                    proj.CONTRACTNO = this.txtCONTRACTNO.Text;
                    proj.COMPANYNAME = this.txtCOMPANYNAME.Text;
                    proj.ADDRESS = this.txtADDRESS.Text;
                    proj.BEGINDATE = this.dtpBeginDate.DateTime;
                    proj.ENDDATE = this.dtpEndDate.DateTime;
                    proj.OBJECTTYPENAME = getProjidentity();
                    proj.MONEY = decimal.Parse(this.txtMoney.Text);
                    //折扣
                    proj.ZHEKOU = decimal.Parse(txtZHKOU.Text);
                    //if (this.chkBillStatus.IsChecked == true)
                    //{
                    //    proj.BILLSTATUS = "已结算";
                    //    proj.BILLDATE = TableManager.DBServerTime();
                    //}
                    //负责人
                    if (txtleder.Tag != null && (txtleder.Tag as TB_User).USER_NAME == txtleder.Text)
                    {
                        proj.TEAMLEDER = (txtleder.Tag as TB_User).USER_CODE.ToString();
                        proj.TEAMLEDERNAME = (txtleder.Tag as TB_User).USER_NAME;
                    }
                    //成员
                    proj.TEAMMEMBER = getTeamMember();
                    //发票
                    proj.MEMO = this.txtMemo.Text;
                    proj.CREATEUSER = Global.g_usercode;  //创建者
                    proj.RATIO1 = ratio1;
                    proj.RATIO2 = ratio2;
                    proj.Save();
                    MessageHelper.ShowMessage("保存成功!");
                }
                else  //更新工程信息
                {
                    //判断是否已有报销信息，如果有报销信息项目不能再更新
                    //TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(new EqExpression("OBJECTID", mProj.Id), new EqExpression("STATUS", 1));
                    //if (arr.Length > 0 && Global.g_userrole != 9)
                    //{
                    //    if (arr.Length == 1 && arr[0].EXPENS == "管理费")
                    //    { }
                    //    else
                    //    {
                    //        MessageHelper.ShowMessage("该项目已有报销信息，不能再更新项目信息！");
                    //        return;
                    //    }
                    //}
                    mProj.OBJECTNAME = this.txtOBJECTNAME.Text;
                    mProj.CONTRACTNO = this.txtCONTRACTNO.Text;
                    mProj.COMPANYNAME = this.txtCOMPANYNAME.Text;
                    mProj.ADDRESS = this.txtADDRESS.Text;
                    mProj.BEGINDATE = this.dtpBeginDate.DateTime;
                    mProj.ENDDATE = this.dtpEndDate.DateTime;
                    mProj.OBJECTTYPENAME = getProjidentity();
                    mProj.MONEY = decimal.Parse(this.txtMoney.Text);
                    //折扣
                    mProj.ZHEKOU = decimal.Parse(txtZHKOU.Text);
                    //负责人
                    if (txtleder.Tag != null && (txtleder.Tag as TB_User).USER_NAME == txtleder.Text)
                    {
                        mProj.TEAMLEDER = (txtleder.Tag as TB_User).USER_CODE.ToString();
                        mProj.TEAMLEDERNAME = (txtleder.Tag as TB_User).USER_NAME;
                    }
                    //成员
                    mProj.TEAMMEMBER = getTeamMember();
                    //发票
                    mProj.MEMO = this.txtMemo.Text;
                    //项目更新操作不更新全局提成比率
                    //mProj.RATIO1 = ratio1;
                    //mProj.RATIO2 = ratio2;
                    mProj.Update();

                    MessageHelper.ShowMessage("保存成功!");
                }
                #region 发票信息处理
                //先把工程对应的所有发票全部作废，再重新新增
                //TB_PROJECT temp = TB_PROJECTDAO.FindFirst(new EqExpression("OBJECTNAME", txtOBJECTNAME.Text));
                //BaseBusiness bb = new BaseBusiness();
                //bool rtn = bb.DelALLItem("TB_BILL", "PROJECTID=" + temp.Id + "");
                //List<TB_BILL> ls = getBills();
                //if (ls.Count > 0)
                //{                    
                //    if (rtn == true)
                //    {
                //        for (int i = 0; i < ls.Count; i++)
                //        {
                //            TB_BILL bill = new TB_BILL();
                //            bill.CREATEDATE = ls[i].CREATEDATE;
                //            bill.BILLNUMBER = ls[i].BILLNUMBER;
                //            bill.PROJECTID = temp.Id;
                //            bill.MONEY = ls[i].MONEY;
                //            bill.STATUS = 1;
                //            bill.Save();
                //        }
                //    }
                //    else
                //    {
                //        MessageHelper.ShowMessage("保存发票信息发生错误!");
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private string getProjidentity()
        {
            string rtn = "";
            if (this.chkyn.IsChecked == true)
                rtn += this.chkyn.Tag.ToString() + "|";

            if (this.chkyw.IsChecked == true)
                rtn += this.chkyw.Tag.ToString() + "|";

            if (this.chkwl.IsChecked == true)
                rtn += this.chkwl.Tag.ToString() + "|";

            if (this.chkzx.IsChecked == true)
                rtn += this.chkzx.Tag.ToString() + "|";

            if (this.chkwx.IsChecked == true)
                rtn += this.chkwx.Tag.ToString() + "|";

            if (this.chkwifi.IsChecked == true)
                rtn += this.chkwifi.Tag.ToString() + "|";

            if (this.chk2D.IsChecked == true)
                rtn += this.chk2D.Tag.ToString() + "|";

            if (this.chk3D.IsChecked == true)
                rtn += this.chk3D.Tag.ToString() + "|";

            if (this.chkxwj.IsChecked == true)
                rtn += this.chkxwj.Tag.ToString() + "|";

            if (this.chkqj.IsChecked == true)
                rtn += this.chkqj.Tag.ToString() + "|";

            if (this.chkjrl.IsChecked == true)
                rtn += this.chkjrl.Tag.ToString() + "|";

            if (this.chkkfl.IsChecked == true)
                rtn += this.chkkfl.Tag.ToString() + "|";

            if (this.chkfhl.IsChecked == true)
                rtn += this.chkfhl.Tag.ToString() + "|";
            return rtn;
        }

        List<TB_BILL> getBills()
        {
            List<TB_BILL> list = new List<TB_BILL>();
            foreach (UserControl _uc in warp2.Children)
            {
                list.Add((_uc as uc_bill).MBill);
            }
            return list;
        }
      
        string getTeamMember()
        {
            string memberID = "";
            foreach (UserControl _uc in warp.Children)
            {
                memberID += (_uc as uc_user).MUser.USER_CODE + "|";
            }
            if (memberID.Length > 0)
            {
                memberID = memberID.Substring(0, memberID.Length - 1);
            }
            return memberID;
        }

        private void btnBill_Click(object sender, RoutedEventArgs e)
        {
            if (mProj == null)
            {
                MessageHelper.ShowMessage("请先新建工程！");
                return;
            }            
            //填写发票信息
            frmBILL frm = new frmBILL(mProj);
            frm.ShowDialog();
            if (frm.mBill != null)
            {
                uc_bill uc = new uc_bill(frm.mBill);
                warp2.Children.Add(uc);
                uc.Tag = frm.mBill;
                uc.txt.ToolTip = "金额:" + frm.mBill.MONEY;
                uc.DelSelectBill += uc_DelSelectBill;
            }
        }

        void uc_DelSelectBill(uc_bill uc)
        {
            foreach (UserControl _uc in warp2.Children)
            {
                if ((_uc as uc_bill).MBill.Id == uc.MBill.Id)
                {
                    warp2.Children.Remove(_uc);
                    break;
                }
            }
        }


        /// <summary>
        /// 财务更新工程按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TB_PROJECT tempProj = TB_PROJECTDAO.FindFirst(new EqExpression("OBJECTNAME", txtOBJECTNAME.Text), new EqExpression("STATUS",1));
                //更新发票信息
                #region 发票信息处理
                //先把工程对应的所有发票全部作废，再重新新增                
                BaseBusiness bb = new BaseBusiness();
                bool rtn = bb.DelALLItem("TB_BILL", "PROJECTID=" + tempProj.Id + "");
                List<TB_BILL> ls = getBills();
                if (ls.Count > 0)
                {
                    if (rtn == true)
                    {
                        for (int i = 0; i < ls.Count; i++)
                        {
                            TB_BILL bill = new TB_BILL();
                            bill.CREATEDATE = ls[i].CREATEDATE;
                            bill.BILLNUMBER = ls[i].BILLNUMBER;
                            bill.PROJECTID = tempProj.Id;
                            bill.MONEY = ls[i].MONEY;
                            bill.STATUS = 1;
                            bill.Save();
                        }
                    }
                    else
                    {
                        MessageHelper.ShowMessage("保存发票信息发生错误!");
                        return;
                    }
                }
                #endregion
                //更新结算信息
                if (this.chkBillStatus.IsChecked == true)
                {
                    if (MessageBox.Show("是否确定该项目执行结算?\n如果确定结算，该项目不能再做任何操作！", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        tempProj.BILLSTATUS = "已结算";
                        tempProj.BILLDATE = TableManager.DBServerTime();
                        tempProj.Update();
                        //根据合同价计算项目管理费用
                        decimal cost = RatioBusiness.QueryProjCost(tempProj.MONEY);  //项目成本金额
                        TB_EXPENSE expense = new TB_EXPENSE();
                        expense.MONEY = cost;
                        expense.EXPENS = "项目管理成本";
                        expense.OBJECTID = tempProj.Id;
                        expense.OBJECTNAME = tempProj.OBJECTNAME;
                        expense.EXPENSTYPE = 99;
                        expense.OPUID = 99;
                        expense.OPNAME = "乾唐通信";
                        expense.RESPONSESTATUS = 2;
                        expense.CREATEDATE = TableManager.DBServerTime();
                        expense.Save();
                        //保存组员提成金额
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }
    }
}
