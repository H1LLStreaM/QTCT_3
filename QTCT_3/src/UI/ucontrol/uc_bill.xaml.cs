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
using WY.Library.Model;

namespace QTCT_3.src
{
    /// <summary>
    /// uc_bill.xaml 的交互逻辑
    /// </summary>
    public partial class uc_bill : UserControl
    {
        private TB_BILL mBill;

        public TB_BILL MBill
        {
            get { return mBill; }
            set { mBill = value; }
        }

        public delegate void DelBill(uc_bill uc);
        public event DelBill DelSelectBill;

        public uc_bill(TB_BILL _bill)
        {
            InitializeComponent();
            mBill = _bill;
            this.txt.Text = _bill.BILLNUMBER + "[" + _bill.CREATEDATE.ToShortDateString() + "]";
        }

        private void img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DelSelectBill != null)
            {
                DelSelectBill(this);
            }
        }
    }
}
