using NHibernate.Expression;
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
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// ucRatio2.xaml 的交互逻辑
    /// </summary>
    public partial class ucRatio2 : UserControl
    {
        private string _mUserCode;

        public string mUserCode
        {
            get { return _mUserCode; }
            set { _mUserCode = value; }
        }
        public ucRatio2()
        {
            InitializeComponent();
        }

        public ucRatio2(string usercode,int projId)
        {
            InitializeComponent();
            mUserCode = usercode;
            this.labName.Content = usercode;
            //查询提成份额
            TB_RATIO ratio = TB_RATIODAO.FindFirst(new EqExpression("STATUS", 1), new EqExpression("PROJECTID", projId), new EqExpression("USERCODE", usercode));
            if (ratio != null)
            {
                this.txtRatio.Text = ratio.RATIO.ToString();
            }
            else 
            {
                this.txtRatio.Text = "0";
            }
        }

        public delegate void DelRatio(ucRatio2 uc);
        public event DelRatio DelSelectRatio;

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DelSelectRatio != null)
            {
                DelSelectRatio(this);
            }
        }
    }
}
