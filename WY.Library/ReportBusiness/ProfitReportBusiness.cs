using System;
using System.Collections.Generic;
using System.Text;
using Aspose.Cells;
using WY.Library.Business;
using WY.Library.Model;
using Library.Model;
using System.Windows.Forms;
using System.Data;
using WY.Common.Utility;
using WY.Common.Message;
using WY.Common;

namespace WY.Library.ReportBusiness
{
    public class ProfitReportBusiness
    {
        private const int PROFITDATA_STARTLINE_INDEX = 4;
        private Workbook book;
        private Worksheet profitSheet;
        private Style cellstyle;
        public ProfitReportBusiness()
        {
            book = new Workbook();
            book.Open(Application.StartupPath + "/templet/Profit_report_templet.xls");
            profitSheet = book.Worksheets[0];
            cellstyle = profitSheet.Cells[1, 0].Style;  //表格边框的STYLE
        }

        public void exportProfitReport(string outfile, int year, int month, float ptax)
        {
            try
            {
                int line = 0;
                decimal totalreceive = 0;
                decimal totalcost = 0;
                decimal totalprofit = 0;
                decimal totalafterTax = 0;
                float tax = float.Parse(ptax.ToString()) / 100;   //税率
                string str = year.ToString() + "-" + month.ToString() + "-01";
                DateTime startdate = DateTime.Parse(str);
                DateTime enddate = DateTime.Parse(str).AddMonths(1).AddDays(-1).Date;
                Customer[] customers = CustomerBusiness.getAllCustomers();
                if (customers.Length > 0)
                {
                    for (int i = 0; i < customers.Length; i++)
                    {
                        //string condition = " and b.year=" + year + " and b.month=" + month + " and cu.customername='" + customers[i].Customername + "'";
                        DataTable tb = ScoreBusiness.makeActualScore(startdate, enddate, "", 0, customers[i].Customername);//.getActual(condition, enddate);
                        decimal tmptotal = 0;  //电路金额合计
                        decimal profit = 0;
                        if (tb.Rows.Count > 0)
                        {
                            for (int j = 0; j < tb.Rows.Count; j++)
                            {
                                tmptotal += Utils.NvDecimal(tb.Rows[j]["提成金额"].ToString()); //渠道提成金额
                            }
                        }
                        else
                        {
                            continue;
                        }
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 1].PutValue(customers[i].Customername);
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 1].Style.Copy(cellstyle);
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 2].PutValue(tb.Rows[0]["电路代码"].ToString());
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 2].Style.Copy(cellstyle);
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 3].PutValue(DateTime.Parse(tb.Rows[0]["结算起始日期"].ToString()).ToString("yyyy-MM-dd"));
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 3].Style.Copy(cellstyle);
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 4].PutValue(DateTime.Parse(tb.Rows[0]["结算截止日期"].ToString()).ToString("yyyy-MM-dd"));
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 4].Style.Copy(cellstyle);
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 5].PutValue(Utils.NvDecimal(tb.Rows[0]["销账金额"].ToString()));
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 5].Style.Copy(cellstyle);
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 6].PutValue(Utils.NvDecimal(tb.Rows[0]["代理费"].ToString()));
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 6].Style.Copy(cellstyle);
                        totalreceive += Utils.NvDecimal(tb.Rows[0]["代理费"].ToString());
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 7].PutValue(ptax + "%");
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 7].Style.Copy(cellstyle);
                        decimal afterTaxAmoun = Utils.NvDecimal(tb.Rows[0]["代理费"].ToString()) * (1 - Utils.NvDecimal(ptax) / 100);
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 8].PutValue(Math.Round(afterTaxAmoun, 2));
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 8].Style.Copy(this.cellstyle);
                        totalafterTax += afterTaxAmoun;
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 9].PutValue(Math.Round(tmptotal, 2));
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 9].Style.Copy(this.cellstyle);
                        totalcost += tmptotal;
                        profit = afterTaxAmoun - tmptotal;
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 10].PutValue(Math.Round(profit, 2));
                        profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line, 10].Style.Copy(this.cellstyle);
                        totalprofit = totalprofit + profit;
                        line++;
                    }
                    profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line + 2, 1].PutValue("合计:");

                    profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line + 2, 6].PutValue(Math.Round(totalreceive, 2));
                    profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line + 2, 8].PutValue(Math.Round(totalafterTax, 2));
                    profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line + 2, 9].PutValue(Math.Round(totalcost, 2));
                    profitSheet.Cells[PROFITDATA_STARTLINE_INDEX + line + 2, 10].PutValue(Math.Round(totalprofit, 2));
                    profitSheet.Cells[1, 0].Style = profitSheet.Cells[1, 1].Style;

                    book.Password = DES.Decode(Global.g_password, Global.DB_PWDKEY);
                    book.Save(outfile);
                    MessageHelper.ShowMessage("I007");
                }
                else
                {
                    MessageHelper.ShowMessage("I014");
                }
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("I008");
            }           
        }
    }
}
