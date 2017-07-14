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
using System.Windows.Shapes;
using WY.Common.Message;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// frmBILL.xaml 的交互逻辑
    /// </summary>
    public partial class frmBILL : Window
    {
        private TB_PROJECT mProj=null;
        public frmBILL(TB_PROJECT proj)
        {
            InitializeComponent();
            mProj = proj;
        }
        public TB_BILL mBill = null;

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TB_BILL bill = new TB_BILL();
                bill.PROJECTID = mProj.Id;
                bill.BILLNUMBER = txtBillNumber.Text;
                bill.CREATEDATE = this.dtpBeginDate.DateTime;
                bill.STATUS = 1;
                decimal money = 0;
                decimal.TryParse(txtMoney.Text, out money);
                if (money > 0)
                {
                    bill.MONEY = money;
                }
                else
                {
                    MessageHelper.ShowMessage("发票金额格式错误！");
                    return;
                }
                mBill = bill;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }        
    }
}
