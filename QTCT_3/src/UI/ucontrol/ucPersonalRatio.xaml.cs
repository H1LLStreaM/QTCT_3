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
using WY.Common.Utility;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// ucPersonalRatio.xaml 的交互逻辑
    /// </summary>
    public partial class ucPersonalRatio : UserControl
    {
        public ucPersonalRatio(decimal profile1, decimal profile2, TB_RATIO ratio)
        {
            InitializeComponent();
            labName.Content = TB_UserDao.FindFirst(new EqExpression("USER_CODE", ratio.USERCODE)).USER_NAME;
            labtc1.Content = profile1;
            labf.Content = ratio.RATIO;
            labtc2.Content = Math.Round((profile2 * ratio.RATIO/10), 2);
            labtotal.Content = Utils.NvDecimal(labtc2.Content) + profile1;
        }
    }
}
