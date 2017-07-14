using NHibernate.Expression;
using QTCT_3.Comments;
using QTCT_3.src.UI.WPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WY.Common;
using WY.Common.Message;
using WY.Library.Dao;
using WY.Library.Model;
using WY.Library.ReportBusiness;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// ucMainProj.xaml 的交互逻辑
    /// </summary>
    public partial class ucMainProj : UserControl
    {
        private int mUserRole; //登录用户角色
        public ucMainProj()
        {
            InitializeComponent();
            mUserRole = Global.g_userrole;            
            BindSourceData();
            BindProjectData();//("", "", 0, null, null);
            this.dtpBeginDate.DateTime = dtpEndDate.DateTime.AddMonths(-1);
            if (Global.g_userrole == 8)
            {
                ucPreview2 uc = new ucPreview2(0);
                uc.Loaded += uc_Loaded;
                uc.Width = panel.Width;
                this.panel.Children.Add(uc);
            }
            else
            {
                ucPreview uc = new ucPreview(0);
                uc.Loaded += uc_Loaded;
                uc.Width = panel.Width;
                this.panel.Children.Add(uc);
                uc.IsEnabled = false;
            }
        }

        void uc_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// 显示概览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Global.g_userrole == 8)  //总经理可以看到所有信息
            {
                this.panel.Children.Clear();
                if (dgObject.SelectedItem == null)
                    return;
                TB_PROJECT proj = dgObject.SelectedItem as TB_PROJECT;
                ucPreview2 uc = new ucPreview2(proj.Id);
                uc.Loaded += uc_Loaded;
                uc.Width = panel.Width;
                this.panel.Children.Add(uc);
            }
            else
            {
                this.panel.Children.Clear();
                if (dgObject.SelectedItem == null)
                    return;
                TB_PROJECT proj = dgObject.SelectedItem as TB_PROJECT;
                ucPreview uc = new ucPreview(proj.Id);
                uc.Loaded += uc_Loaded;
                uc.Width = panel.Width;
                //uc.IsEnabled = false;
                this.panel.Children.Add(uc);
            }
        } 

        /// <summary>
        /// 绑定工程数据
        /// </summary>
        private void BindProjectData()//(string s1, string s2, int s3, DateTime? s4, DateTime? s5)
        {
            this.dgObject.ItemsSource = null;
            List<TB_PROJECT> list = new List<TB_PROJECT>();
            int projId =0;
            if(txtObjName.Tag!=null)
                projId = (txtObjName.Tag as TB_PROJECT).Id;
            string leader="";
            if(txtLeader.Tag!=null)
                leader = (txtLeader.Tag as TB_User).USER_CODE;
            string projidentity = (cmbObjType.SelectedItem as PTS_TABLE_SRC).TABLE_VALUE;
            list = Comments.Comment.QueryProject(this.txtObjName.Text, leader, projidentity, dtpBeginDate.DateTime, DateTime.Parse(dtpEndDate.DateTime.ToString("yyyy-MM-dd 23:59:59")));
            this.dgObject.ItemsSource = list;

            if (mUserRole == 0 || mUserRole == 1)
            {
                //List<TB_PROJECT> 过滤
            }
            else if (mUserRole == 10) //销售
            {
                list = list.FindAll(a => a.CREATEUSER == Global.g_usercode || a.TEAMLEDER == Global.g_usercode || a.TEAMMEMBER.IndexOf(Global.g_usercode) >= 0 );
            }
            else if (mUserRole == 11) //员工
            {
                list = list.FindAll(a => a.CREATEUSER == Global.g_usercode || a.TEAMLEDER == Global.g_usercode || a.TEAMMEMBER.IndexOf(Global.g_usercode) >= 0);
            }
            this.dgObject.ItemsSource = null;
            this.dgObject.ItemsSource = screening(list);
            if (this.dgObject.ItemsSource != null)
                this.dgObject.SelectedIndex = 0;
        }

        private List<TB_PROJECT> screening(List<TB_PROJECT> ls)
        {
            List<TB_PROJECT> rtn = null;
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
                case 10: //销售 只能看到自己的工程
                    rtn = ls.FindAll(a => a.CREATEUSER == Global.g_usercode);
                    break;
                case 11:
                    rtn = ls;
                    break;
                default:
                    break;
            }
            if (rtn.Count() > 0)
            {
                for (int i = 0; i < rtn.Count; i++) {
                    rtn[i].Index = i + 1;
                }
            }
            return rtn;
        }

        private void BindSourceData()
        {
            List<PTS_TABLE_SRC> list = null;
            PTS_TABLE_SRC[] arr = PTS_TABLE_SRCDAO.FindAll(new EqExpression("STATUS", 1), new EqExpression("TABLE_ID", "PTS_TABLE_OBJECT_TYPE"));
            if (arr.Length > 0)
            {
                list = new List<PTS_TABLE_SRC>(arr);
                PTS_TABLE_SRC _tmp = new PTS_TABLE_SRC();
                _tmp.TABLE_NAME = "请选择";
                list.Insert(0, _tmp);
            }
            this.cmbObjType.ItemsSource = null;
            this.cmbObjType.ItemsSource = list;
            this.cmbObjType.DisplayMemberPath = "TABLE_NAME";
            this.cmbObjType.SelectedIndex = 0;
        }

        

        private void ExpendAnimal(double k1, double k2, Border b = null)
        {
            if (dgObject.SelectedItem != null)
            {
                TB_PROJECT proj = dgObject.SelectedItem as TB_PROJECT;
                ucPreview uc = new ucPreview(proj.Id);
                this.ExpenseListPanel.Children.Clear();
                this.ExpenseListPanel.Children.Add(uc);
                if (b == null)
                {
                    if (this.spBorder.Width == k2)
                    {
                        DoubleAnimation wAnimation = new DoubleAnimation(k1, k2, new Duration(TimeSpan.FromSeconds(0.3)));
                        this.spBorder.BeginAnimation(DockPanel.WidthProperty, wAnimation, HandoffBehavior.Compose);
                    }
                }
                else
                {
                    if (b.Width == k1)
                    {
                        DoubleAnimation wAnimation = new DoubleAnimation(k1, k2, new Duration(TimeSpan.FromSeconds(0.3)));
                        b.BeginAnimation(DockPanel.WidthProperty, wAnimation, HandoffBehavior.Compose);
                    }
                    else if (this.spBorder.Width == k2)
                    {
                        DoubleAnimation wAnimation = new DoubleAnimation(k2, k1, new Duration(TimeSpan.FromSeconds(0.3)));
                        this.spBorder.BeginAnimation(DockPanel.WidthProperty, wAnimation, HandoffBehavior.Compose);
                    }
                }
            }
        }

        private void brnSearch_Click(object sender, RoutedEventArgs e)
        {
            BindProjectData();
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

        private void dgObject_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TB_PROJECT proj = dgObject.SelectedItem as TB_PROJECT;
            if (proj != null)
            {
                if (proj.CREATEUSER == Global.g_usercode || Global.g_userrole == 8 || Global.g_userrole == 9)
                {
                    frmProject frm = new frmProject(proj);
                    frm.ShowDialog();
                }
            }
            //刷新
            BindProjectData();
        }

        private void menuDel_Click(object sender, RoutedEventArgs e)
        {            
            //删除工程
            TB_PROJECT proj = dgObject.SelectedItem as TB_PROJECT;
            UserMessage msg = new UserMessage();
            msg.Button= System.Windows.Forms.MessageBoxButtons.YesNo;
            msg.Icon = System.Windows.Forms.MessageBoxIcon.Information;
            msg.Text="是否确定删除？";
            msg.Type = EnmMessageLevel.确认;
            if (proj.BILLSTATUS == "已结算")
            {
                MessageHelper.ShowMessage("项目已结算，不能删除!");
                return;
            }
            if (MessageBox.Show("是否确认删除" + proj .OBJECTNAME+ "？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                proj.STATUS = 0;
                proj.Update();
            }
            BindProjectData();
        }        

        #region
        private void txtObjName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtObjName.Tag == null)
                {
                    frmObjectSearch frm = new frmObjectSearch();
                    frm.ShowDialog();
                    if (frm.mProj != null && frm.mProj.Id > 0)
                    {
                        this.txtObjName.Tag = frm.mProj;
                        this.txtObjName.Text = frm.mProj.OBJECTNAME;
                        BindProjectData();
                    }
                    else
                    {
                        this.txtObjName.Tag = null;
                        this.txtObjName.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void imgClear_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            clearProject();
        }

        private void txtObjName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                clearProject();
        }

        private void clearProject()
        {
            this.txtObjName.Tag = null;
            this.txtObjName.Text = "";
        }
        #endregion 

        #region
        private void txtLeader_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtLeader.Tag == null)
                {
                    frmUserSearch frm = new frmUserSearch();
                    frm.ShowDialog();
                    if (frm.mUser != null && frm.mUser[0].Id>0)
                    {
                        this.txtLeader.Tag = frm.mUser;
                        this.txtLeader.Text = frm.mUser[0].USER_CODE;
                    }
                    else
                    {
                        this.txtLeader.Tag = null;
                        this.txtLeader.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void imgClear2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            clearLeader();
        }

        private void txtLeader_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                clearLeader();
        }

        private void clearLeader()
        {
            this.txtLeader.Tag = null;
            this.txtLeader.Text = "";
        }
        #endregion

        private void imgClear2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clearLeader();
        }

        private void menuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgObject.SelectedItem != null)
            {
                TB_PROJECT proj = dgObject.SelectedItem as TB_PROJECT;
                frmRatioEdit frm = new frmRatioEdit(proj);
                frm.ShowDialog();
            }
        }

        private void dgObject_LoadingRow(object sender, DataGridRowEventArgs e)
        {            
            e.Row.MouseRightButtonDown -= Row_MouseRightButtonDown;
            e.Row.MouseRightButtonDown += Row_MouseRightButtonDown;
        }

        void Row_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.menuDel.IsEnabled = false;
                if (Global.g_userrole == 8)
                    this.menuDel.IsEnabled = true;
                if (dgObject.SelectedItem != null)
                {
                    TB_PROJECT proj = dgObject.SelectedItem as TB_PROJECT;
                    if (proj.TEAMLEDER == Global.g_usercode || Global.g_userrole==8)
                        this.menuEdit.IsEnabled = true;
                    else
                        this.menuEdit.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void brnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.dgObject.SelectedItem != null)
                {
                    TB_PROJECT proj = this.dgObject.SelectedItem as TB_PROJECT;
                    profileporcess proce = new profileporcess();
                    projProfileClass ppc = proce.getProfile(proj);  //公司项目利润
                    if (ppc != null)
                    {
                        ppc.expens = ExpenseProcess(ppc.expens);  //合并
                        ProfilePrint print = new ProfilePrint();
                        print.Print(ppc, proj);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private List<TB_EXPENSE> ExpenseProcess(List<TB_EXPENSE> list)
        {
            Hashtable hs = new Hashtable();
            List<TB_EXPENSE> otherList = new List<TB_EXPENSE>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].EXPENSTYPE <95)
                {
                    if (string.IsNullOrEmpty(list[i].OPNAME))
                        continue;
                    string usercode = list[i].OPNAME;
                    
                    var sum = (from temp in list
                               where temp.OPNAME == usercode
                               select temp).Sum(t => t.MONEY);
                    if (hs.ContainsKey(usercode) == false)
                        hs.Add(usercode, sum);
                }
                else
                    otherList.Add(list[i]);
            }
            List<TB_EXPENSE> newList = new List<TB_EXPENSE>();
            foreach (DictionaryEntry item in hs)
            {
                TB_EXPENSE newExpense = new TB_EXPENSE();
                newExpense.OPNAME = item.Key.ToString();
                newExpense.EXPENS = "报销";
                newExpense.MONEY = decimal.Parse(item.Value.ToString());    
                newList.Add(newExpense);
            }
            newList.InsertRange(0,otherList);
            return newList;
        }

        private void menuEdit2_Click(object sender, RoutedEventArgs e)
        {
            if (dgObject.SelectedItem != null)
            {
                TB_PROJECT proj = dgObject.SelectedItem as TB_PROJECT;
                frmProjRatio frm = new frmProjRatio();
                frm.ShowDialog();
                if (frm.item != null)
                {
                    proj.RATIO1 = frm.item.RATIO1;
                    proj.RATIO2 = frm.item.RATIO2;
                    proj.Update();
                }
            }
        }

        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void brnModify_Click(object sender, RoutedEventArgs e)
        {
            dgObject_MouseDoubleClick(null,null);
        }

        private void brnDel_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgObject.SelectedItem != null)
                menuDel_Click(null,null);
        }        
    }
}
