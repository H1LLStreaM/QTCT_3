using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Reporting.WinForms;
using System.Data;
using WY.Library.Business;
using System.IO;
using Aspose.Cells;
using WY.Common.Message;
using WY.Common.Utility;
using WY.Common;

namespace WY.Library.ReportBusiness
{
    public class SaleTrendcyBusiness
    {
        public DataTable saveExcel(int StartYear, int EndYear, int StartMonth, int EndMonth, int salerId, string salerName,string savePath)
        {
            try
            {
                string tmpstartDate = StartYear.ToString() + "-" + StartMonth.ToString() + "-01";
                DateTime startDate = DateTime.Parse(tmpstartDate);
                string tmpendDate = EndYear.ToString() + "-" + EndMonth.ToString() + "-01";
                DateTime endDate = DateTime.Parse(tmpendDate).AddMonths(1).AddDays(-1);  //查询结束月最后一天

                DataTable salesTrendcy = ScoreBusiness.makeActualScore(startDate, endDate, "", salerId, "");
                DataTable tb = createCol();
                for (DateTime date = startDate; date < endDate; date = date.AddMonths(1))
                {
                    int tmpyear = date.Year;
                    int tmpmonth = date.Month;
                    decimal amount = 0;
                    string yearmonth = tmpyear.ToString() + "年" + tmpmonth + "月";

                    DataRow[] rows = salesTrendcy.Select("年度=" + tmpyear + " and 月度=" + tmpmonth + "");
                    for (int i = 0; i < rows.Length; i++)
                    {
                        amount += Utils.NvDecimal(rows[i]["应收月租费"]);
                    }
                    DataRow r = tb.NewRow();
                    r["yearmonth"] = yearmonth;
                    r["amount"] = amount;
                    tb.Rows.Add(r);
                }
                return tb;               
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return new DataTable();
                MessageHelper.ShowMessage("E999", "销售业绩走势导出失败。");

            }
        }

        private DataTable createCol()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("yearmonth", typeof(string));
            tb.Columns.Add("amount",typeof(decimal));
            return tb;
        }
    }
}
