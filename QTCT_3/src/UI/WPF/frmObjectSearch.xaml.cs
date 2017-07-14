using NHibernate.Expression;
using QTCT_3.Comments;
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
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// frmObjectSearch.xaml 的交互逻辑
    /// </summary>
    public partial class frmObjectSearch : Window
    {
        public TB_PROJECT mProj;
        private List<TB_PROJECT> mList = null;
        private int mType = 0;
        public frmObjectSearch(int type=0)
        {
            InitializeComponent();
            mType = type;
            mProj = new TB_PROJECT();
            List<PTS_TABLE_SRC> srclist = new List<PTS_TABLE_SRC>();
            PTS_TABLE_SRC tmp = new PTS_TABLE_SRC();
            tmp.TABLE_NAME = "请选择";
            tmp.TABLE_VALUE = "0";
            tmp.ID = 0;
            PTS_TABLE_SRC[] arr = PTS_TABLE_SRCDAO.FindAll(new EqExpression("TABLE_ID", "PTS_TABLE_OBJECT_TYPE"));
            if (arr.Length > 0)
            {                
                srclist = new List<PTS_TABLE_SRC>(arr);
                srclist.Insert(0, tmp);
            }
            this.cmbObjType.ItemsSource = null;
            this.cmbObjType.ItemsSource = srclist;
            this.cmbObjType.DisplayMemberPath = "TABLE_NAME";
            this.cmbObjType.SelectedIndex = 0;
            TB_PROJECT[] _arr = null;
            //if(type==1)
            //    _arr = TB_PROJECTDAO.FindAll(new LikeExpression("OBJECTNAME", "%" + this.txtObjName.Text + "%"), new EqExpression("STATUS", 1), new NotExpression(new EqExpression("BILLSTATUS", "已结算")));
            //else
                _arr = TB_PROJECTDAO.FindAll(new LikeExpression("OBJECTNAME", "%" + this.txtObjName.Text + "%"), new EqExpression("STATUS", 1));
            
            if (_arr.Length > 0)
            {
                mList = new List<TB_PROJECT>(_arr);
                for (int i = 0; i < mList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(mList[i].OBJECTTYPENAME))
                    {
                        string identity = Comment.setProjIdentity(mList[i].OBJECTTYPENAME);
                        mList[i].ProjIdentity = identity;
                    }
                }
                if (type == 1)
                    mList = mList.FindAll(a => a.BILLSTATUS=="已结算");
            }
            this.dgObject.ItemsSource = screening(mList);
        }

        public frmObjectSearch(string proj)
            : this()
        {
            mProj = new TB_PROJECT();
            List<PTS_TABLE_SRC> srclist = new List<PTS_TABLE_SRC>();
            PTS_TABLE_SRC tmp = new PTS_TABLE_SRC();
            tmp.TABLE_NAME = "请选择";
            tmp.TABLE_VALUE = "0";
            tmp.ID = 0;
            PTS_TABLE_SRC[] arr = PTS_TABLE_SRCDAO.FindAll(new EqExpression("TABLE_ID", "PTS_TABLE_OBJECT_TYPE"));
            if (arr.Length > 0)
            {
                srclist = new List<PTS_TABLE_SRC>(arr);
                srclist.Insert(0, tmp);
            }
            this.cmbObjType.ItemsSource = null;
            this.cmbObjType.ItemsSource = srclist;
            this.cmbObjType.DisplayMemberPath = "TABLE_NAME";
            this.cmbObjType.SelectedIndex = 0;
            this.txtObjName.Text = proj;
            TB_PROJECT[] _arr = TB_PROJECTDAO.FindAll(new LikeExpression("OBJECTNAME", "%" + this.txtObjName.Text + "%"), new EqExpression("STATUS", 1));
            if (_arr.Length > 0)
            {
                mList = new List<TB_PROJECT>(_arr);
            }
            this.dgObject.ItemsSource = screening(mList);
        }

        private void dgObject_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            mProj = dgObject.SelectedItem as TB_PROJECT;
            this.Close();
        }

        private void brnSearch_Click(object sender, RoutedEventArgs e)
        {
            //OBJECTNAME  OBJECTTYPE  new LikeExpression("Cablenumber", "%" + cable + "%")
            string OBJECTNAME = this.txtObjName.Text;
            int objectTypeId = (this.cmbObjType.SelectionBoxItem as PTS_TABLE_SRC).ID;
            TB_PROJECT[] arr = null;
            DateTime start = dtpBeginDate.DateTime;
            DateTime end = dtpEndDate.DateTime;
            List<ICriterion> IClist = new List<ICriterion>();
            IClist.Add(new EqExpression("STATUS", 1));

            if (!string.IsNullOrEmpty(txtObjName.Text))
            {
                IClist.Add(new LikeExpression("OBJECTNAME", "%" + txtObjName.Text + "%"));
            }
            if (chk.IsChecked == true)
            {
                IClist.Add(new AndExpression(new GeExpression("BEGINDATE", start), new LeExpression("ENDDATE", end)));
            }
            if (objectTypeId > 0)
            {
                IClist.Add(new EqExpression("OBJECTTYPE", objectTypeId));
            }
            arr = TB_PROJECTDAO.FindAll(IClist.ToArray());
            if (arr != null && arr.Length > 0)
            {
                mList = new List<TB_PROJECT>(arr);
            }
            if (mType == 1)
            {
                mList = mList.FindAll(a=>a.BILLSTATUS == null);
            }
            this.dgObject.ItemsSource = null;
            if (arr != null)
            {
                mList = new List<TB_PROJECT>(arr);
                //根据角色过滤
                this.dgObject.ItemsSource = null;
                this.dgObject.ItemsSource = screening(mList);
            }
        }

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            if (chk.IsChecked == true)
            {
                this.dtpBeginDate.IsEnabled = true;
                this.dtpEndDate.IsEnabled = true;
            }
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
                    //rtn = ls.FindAll(a =>a.CREATEUSER == Global.g_usercode);
                    rtn = ls;
                    break;
                case 11:
                    rtn = ls;
                    break;
                default:
                    break;
            }
            return rtn;
        }
    }
}
