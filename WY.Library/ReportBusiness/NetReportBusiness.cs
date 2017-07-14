using System;
using System.Collections.Generic;
using System.Text;
using WY.Library.Model;
using Aspose.Cells;
using System.Windows.Forms;
using WY.Common.Message;
using WY.Common.Utility;
using WY.Common;
using WY.Library.Business;
using System.Drawing;

namespace WY.Library.ReportBusiness
{
    /// <summary>
    /// 智能网账单导出
    /// </summary>
    public class NetReportBusiness
    {
        private const int SALESDATA_STARTLINE_INDEX = 4;
        private const int SALESDATA_STARTLINE_INDEX2 = 4;

        private Worksheet sheet;
        private Worksheet sheet2;
        private Style dataStyle;
        private netMoney money;
        private int mYear, mMonth;
        private Workbook book;
        //private string savePath;
        public NetReportBusiness(netMoney m, int year, int month)
        {
            money = m;
            mYear = year;
            mMonth = month;
            book = new Workbook();
            book.Open(Application.StartupPath + "/templet/net_salers_report_templet.xls");
            sheet = book.Worksheets[0];  //销账
            sheet2 = book.Worksheets[1];  //欠费
            dataStyle = sheet.Cells[4, 1].Style;
        }

        public void writeSalesReport(string savePath)
        {
            try
            {
                sheet = book.Worksheets[0];  //销账
                sheet2 = book.Worksheets[1];  //欠费
                //sheet.Copy(this.sheet);
                string salerLogname = money.Logname;
                #region 欠费
                if (money.list2.Count > 0)
                {
                    int i = 0;
                    for (i = 0; i < money.list2.Count; i++)
                    {
                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 1].PutValue(i + 1);
                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 1].Style.Copy(this.dataStyle);

                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 3].PutValue(money.list2[i].customername);
                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 3].Style.Copy(this.dataStyle);

                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 2].PutValue(money.list2[i].shebeihao);
                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX, 2].Style.Copy(this.dataStyle);

                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 6].PutValue(money.list2[i].money);
                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 6].Style.Copy(this.dataStyle);

                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 5].PutValue(money.list2[i].yewuleixing);
                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 5].Style.Copy(this.dataStyle);

                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 4].PutValue(money.list2[i].zhangqi);
                        sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2, 4].Style.Copy(this.dataStyle);
                    }
                    sheet2.Cells[i + SALESDATA_STARTLINE_INDEX2 + 1, 6].PutValue("欠费合计:" + money.nomoney.ToString());
                    //sheet.Cells[i + SALESDATA_STARTLINE_INDEX + 1, 10].PutValue(money.money); //支付合计
                }

                #endregion
                #region 销账
                sheet.Cells[1, 5].PutValue(mYear.ToString() + "年" + mMonth + "月度");
                sheet.Cells[1, 1].PutValue("姓名：" + salerLogname);                
                //decimal totalSalesAmount = 0;               
                if (money.list1.Count > 0)
                {
                    int i = 0;
                    for (i = 0; i < money.list1.Count; i++)
                    {
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 1].PutValue(i + 1);
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 1].Style.Copy(this.dataStyle);
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 3].PutValue(money.list1[i].customername);
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 3].Style.Copy(this.dataStyle);

                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 2].PutValue(money.list1[i].shebeihao);
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 2].Style.Copy(this.dataStyle);

                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 4].PutValue(money.list1[i].xiaozhangriqi); //结算日期
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 4].Style.Copy(this.dataStyle);

                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 5].PutValue(money.list1[i].kaihuqi2006 + money.list1[i].kaihuqian2006);
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 5].Style.Copy(this.dataStyle);

                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 6].PutValue(money.list1[i].kaihuqi2006); //长途出账金额
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 6].Style.Copy(this.dataStyle);

                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 7].PutValue(money.list1[i].CtMoney);  //长途佣金金额
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 7].Style.Copy(this.dataStyle);

                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 8].PutValue(money.list1[i].kaihuqian2006);  //本地出账金额
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 8].Style.Copy(this.dataStyle);

                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 9].PutValue(money.list1[i].DbMoney);  //本地佣金金额
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 9].Style.Copy(this.dataStyle);

                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 10].PutValue(money.list1[i].Pay);  //佣金总额
                        sheet.Cells[i + SALESDATA_STARTLINE_INDEX, 10].Style.Copy(this.dataStyle);
                    }
                    sheet.Cells[i + SALESDATA_STARTLINE_INDEX + 1, 10].PutValue("合计:" + money.money.ToString());
                    sheet.Cells[i + SALESDATA_STARTLINE_INDEX + 1, 10].Style.BackgroundColor = Color.Yellow;
                    //sheet.Cells[i + SALESDATA_STARTLINE_INDEX + 1, 10].PutValue(money.money); //支付合计
                }
                #endregion
                TB_User sales = UserBusiness.findUserById(money.Salerid);
                if (sales!=null)
                    book.Password = DES.Decode(sales.PASSWORD, Global.DB_PWDKEY);
                else
                    book.Password = DES.Decode(Global.DB_PWDKEY, Global.DB_PWDKEY);
                book.Save(savePath + "//" + salerLogname + " " + mYear + "-" + mMonth + "月度提成清单1.xls");
            }
            catch(Exception ex)
            {
                MessageHelper.ShowMessage("E999", "错误" + ex.Message + "\n此文件导出失败，其他文件仍将被导出到所选目录。");
            }
        }
    }
}