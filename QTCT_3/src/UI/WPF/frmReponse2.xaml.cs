using System;
using System.Windows;
using WY.Common.Message;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// frmReponse2.xaml 的交互逻辑
    /// </summary>
    public partial class frmReponse2 : Window
    {
        private TB_EXPENSE mExpense;
        public frmReponse2(TB_EXPENSE exp)
        {
            InitializeComponent();
            mExpense = exp;
            labMoney.Content = mExpense.MONEY;
            labType.Content = mExpense.EXPENS;
            labDate.Content = mExpense.CREATEDATE.ToString("yyyy-MM-dd");
            if (mExpense.OBJECTID == 0)
                lab.Content = "个人报销";
            else
                lab.Content = mExpense.OBJECTNAME;
            this.txtResponse.Text = mExpense.LEADERRESPONSE;
        }        

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtResponse.Text))
                {
                    mExpense.LEADERRESPONSE = this.txtResponse.Text;
                    mExpense.LEADERRESPONSESTATUS = 1;
                    mExpense.Update();
                    //MessageHelper.ShowMessage("保存成功");
                    this.Close();
                }
                else
                {
                    MessageHelper.ShowMessage("请填写驳回/修改原因");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }
    }
}
