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
using System.Windows.Shapes;
using WY.Common.Message;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// frmNewProjectType.xaml 的交互逻辑
    /// </summary>
    public partial class frmNewProjectType : Window
    {
        private PTS_OBJECT_TYPE_SRC mObject = null;

        public frmNewProjectType(PTS_OBJECT_TYPE_SRC src)
        {
            InitializeComponent();
            if (src != null)
            {
                mObject = src;
                this.txtProjectType.Text = mObject.OBJECTTYPENAME;
                this.txtRatio1.Text = mObject.RATIO1.ToString();
                this.txtRatio2.Text = mObject.RATIO2.ToString();
            }
        }

        private void txtRatio1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TotalRatio();
        }

        private void txtRatio2_TextChanged(object sender, TextChangedEventArgs e)
        {
            TotalRatio();
        }

        private void TotalRatio()
        {
            decimal ratio1 = 0;
            decimal ratio2 = 0;
            try
            {
                string txtRatio1 = this.txtRatio1.Text;
                string txtRatio2 = this.txtRatio2.Text;

                decimal.TryParse(txtRatio1, out ratio1);
                decimal.TryParse(txtRatio2, out ratio2);

                decimal total = ratio1 + ratio2;
                this.labTotal.Content = "合计提成百分比:" + total.ToString() + "%";
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!checkValue())
                    return;
                if (mObject != null)  //更新数据
                {
                    mObject.OBJECTTYPENAME = txtProjectType.Text;
                    mObject.RATIO1 = Math.Round(decimal.Parse(txtRatio1.Text), 2);
                    mObject.RATIO2 = Math.Round(decimal.Parse(txtRatio2.Text), 2);
                    mObject.STATUS = 1;
                    mObject.Update();
                    MessageHelper.ShowMessage("更新成功!");                   
                }
                else  //新增数据
                {
                    PTS_OBJECT_TYPE_SRC _src = new PTS_OBJECT_TYPE_SRC();
                    _src.OBJECTTYPENAME = txtProjectType.Text;
                    _src.RATIO1 = Math.Round(decimal.Parse(txtRatio1.Text), 2);
                    _src.RATIO2 = Math.Round(decimal.Parse(txtRatio2.Text), 2);
                    _src.STATUS = 1;
                    _src.Save();
                    MessageHelper.ShowMessage("保存成功!");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private bool checkValue()
        {
            bool rtn = true;
            try
            {
                //判断项目类型是否重复
                if (String.IsNullOrEmpty(this.txtProjectType.Text))
                {
                    MessageHelper.ShowMessage("工程类型名称必填!");
                    this.txtProjectType.Focus();
                    rtn = false;
                }
                if (mObject == null)
                {
                    PTS_OBJECT_TYPE_SRC[] arr = PTS_OBJECT_TYPE_SRCDAO.FindAll(new EqExpression("STATUS", 1), new EqExpression("OBJECTTYPENAME", txtProjectType.Text));
                    if (arr.Length > 0)
                    {
                        MessageHelper.ShowMessage("有重复的工程类型名称,请确认!");
                        this.txtProjectType.Focus();
                        rtn = false;
                    }
                }
                else
                {
                    PTS_OBJECT_TYPE_SRC[] arr = PTS_OBJECT_TYPE_SRCDAO.FindAll(new EqExpression("STATUS", 1), new EqExpression("OBJECTTYPENAME", txtProjectType.Text),new NotExpression(new EqExpression("ID",mObject.ID)));
                    if (arr.Length > 0)
                    {
                        MessageHelper.ShowMessage("有重复的工程类型名称,请确认!");
                        this.txtProjectType.Focus();
                        rtn = false;
                    }
                }
                decimal d = -1;
                decimal.TryParse(txtRatio1.Text, out d);
                if (d < 0)
                {
                    MessageHelper.ShowMessage("固定提成输入格式错误!");
                    this.txtRatio1.Focus();
                    rtn = false;
                }
                d = -1;
                decimal.TryParse(txtRatio2.Text, out d);
                if (d < 0)
                {
                    MessageHelper.ShowMessage("可分配提成输入格式错误!");
                    this.txtRatio1.Focus();
                    rtn = false;
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
