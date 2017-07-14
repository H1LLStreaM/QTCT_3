using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WY.Common.Message;
using System.Collections;
using WY.Common.Utility;

namespace QianTang_2
{
    public partial class UserScoreSearch : UserControl
    {
        public UserScoreSearch()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!GlobalBusiness.isConnServer())
            {
                MessageHelper.ShowMessage("E034");
                return;
            }
            resultGrid.Rows.Clear();
            Hashtable hs = new Hashtable();
            //��ѯ����

            if (!string.IsNullOrEmpty(txtCusName.Text.Trim()))
            {
                hs.Add("�ͻ�", txtCusName.Text.Trim());
            }
            DateTime Start = completeStart.Value.Date; //�깤��ʼ����
            DateTime End = completeEnd.Value.Date;   //�깤��������

            if (chkIsEnable.Checked)
            {
                if (Start.CompareTo(End) > 1)
                {
                    MessageHelper.ShowMessage("W010");
                    return;
                }
                hs.Add("�깤��ʼ", Start);
                hs.Add("�깤��ֹ", End);
            }
            if (cmbStatus.SelectedIndex < 3)
            {
                hs.Add("���㷽ʽ", cmbStatus.SelectedIndex);
            }
            int imYearstart = Utils.NvInt(numStartYear.Value);  //������ʼ���
            int imMonthstart = Utils.NvInt(numStartMonth.Value);
            int imYearend = Utils.NvInt(numEndYear.Value);
            int imMonthend = Utils.NvInt(numEndMonth.Value);
            if (imYearstart > imYearend)
            {
                MessageHelper.ShowMessage("E022");
                return;
            }
            else if (imYearstart == imYearend)
            {
                if (imMonthstart > imMonthend)
                {
                    MessageHelper.ShowMessage("E022");
                    return;
                }
            }
            hs.Add("������ʼ���", imYearstart);
            hs.Add("������ʼ�¶�", imMonthstart);
            hs.Add("�����ֹ���", imYearend);
            hs.Add("�����ֹ�¶�", imMonthend);
            if (hs.Count > 0)
            {
                DataTable tb = ScoreBusiness.getScoreByCondition(hs);
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            resultGrid.Rows.Add();
                            resultGrid.Rows[i].Cells["No"].Value = i + 1;
                            resultGrid.Rows[i].Cells["customer"].Value = Utils.NvStr(tb.Rows[i]["�ͻ�����"].ToString());
                            resultGrid.Rows[i].Cells["cablenumber"].Value = Utils.NvStr(tb.Rows[i]["��·����"].ToString());
                            resultGrid.Rows[i].Cells["saler"].Value = Utils.NvStr(tb.Rows[i]["����������"].ToString());
                            resultGrid.Rows[i].Cells["complete"].Value = DateTime.Parse(tb.Rows[i]["�깤����"].ToString());
                            resultGrid.Rows[i].Cells["startdate"].Value = DateTime.Parse(tb.Rows[i]["������ʼ����"].ToString()).ToString("yyyy-MM-dd");
                            resultGrid.Rows[i].Cells["enddate"].Value = DateTime.Parse(tb.Rows[i]["�����ֹ����"].ToString()).ToString("yyyy-MM-dd");
                            resultGrid.Rows[i].Cells["money"].Value = Utils.NvStr(tb.Rows[i]["��ͬ���"].ToString());
                            resultGrid.Rows[i].Cells["paytype"].Value = Utils.NvStr(tb.Rows[i]["��������"].ToString());
                            resultGrid.Rows[i].Cells["receivable"].Value = Utils.NvStr(tb.Rows[i]["���˽��"].ToString());
                            resultGrid.Rows[i].Cells["inputdata"].Value = Utils.NvStr(tb.Rows[i]["���"].ToString()) + "-" + Utils.NvStr(tb.Rows[i]["�¶�"].ToString());
                            resultGrid.Rows[i].Cells["remove"].Value = Utils.NvStr(tb.Rows[i]["�������"].ToString());
                            resultGrid.Rows[i].Cells["contractType"].Value = Utils.NvStr(tb.Rows[i]["��·����"].ToString());
                        }
                    }
                    else
                    {
                        MessageHelper.ShowMessage("I005");
                    }
                }
            }
        }

        private void UserScoreSearch_Load(object sender, EventArgs e)
        {
            decimal year = Decimal.Parse(DateTime.Now.Year.ToString());
            decimal month = Decimal.Parse(DateTime.Now.Month.ToString());
            this.numEndYear.Value = year;
            this.numStartYear.Value = year;
            this.numStartMonth.Value = month;
            this.numEndMonth.Value = month;
            this.cmbStatus.SelectedIndex = 3;
        }

        private void chkIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsEnable.Checked)
            {
                completeStart.Enabled = true;
                completeEnd.Enabled = true;
            }
            else
            {
                completeStart.Enabled = false;
                completeEnd.Enabled = false;
            }
        }

    }
}
