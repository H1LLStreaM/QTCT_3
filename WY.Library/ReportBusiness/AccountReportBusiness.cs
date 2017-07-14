using System;
using System.Collections.Generic;
using System.Text;
using Aspose.Cells;
using WY.Library.Model;
using WY.Library.Business;
using WY.Common.Message;
using WY.Common.Utility;
using System.Data;
using System.Windows.Forms;
using WY.Common;

namespace WY.Library.ReportBusiness
{
    /// <summary>
    /// 财务清单
    /// </summary>
    public class AccountReportBusiness
    {
        private const int ACCOUNTDATA_STARTLINE_INDEX = 4;
        private Workbook book;
        private Worksheet AccountSheet;
        private Style cellstyle;
        decimal Total = 0;

        public AccountReportBusiness()
        {
            book = new Workbook();
            book.Open(Application.StartupPath + "/templet/Account_report_templet.xls");
            AccountSheet = book.Worksheets[0];
            cellstyle = AccountSheet.Cells[1, 0].Style;
        }

        public void saveAccountReport(string outfile, int year, int month)
        {
            int lines = 0;
            TB_User[] sales = UserBusiness.getAllSalersAndWrite();
            if (sales != null)
            {   
                AccountSheet.Cells[2, 4].PutValue(year.ToString() + "年" + month + "月度");
                string str = year.ToString() + "-" + month.ToString() + "-01";
                DateTime startdate = DateTime.Parse(str);
                DateTime enddate = DateTime.Parse(str).AddMonths(1).AddSeconds(-1);               
                for (int i = 0; i < sales.Length; i++)
                {
                    lines = lines + 1;
                    decimal totalScore = ScoreBusiness.AccountSummaryReport(startdate, enddate, sales[i].Id, startdate.Month, startdate.Year);
                    writeAccountReport(sales[i], year, month, i, totalScore);
                }
                try
                {
                    AccountSheet.Cells[1, 0].Style = AccountSheet.Cells[1, 1].Style;
                    AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + lines + 2, 4].PutValue("合计:");
                    AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + lines + 2, 4].PutValue(Math.Round(Total, 2));
                    book.Password = DES.Decode(Global.g_password,Global.DB_PWDKEY);
                    book.Save(outfile);
                    MessageHelper.ShowMessage("I007");
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    MessageHelper.ShowMessage("E999","财务清单导出失败。");
                }
            }
        }

        private void writeAccountReport(TB_User sales, int year, int month, int line,DataTable tb)
        {
            decimal tmpTotal = 0;
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                tmpTotal += Utils.NvDecimal(tb.Rows[i]["提成金额"].ToString());
            }
            Total += tmpTotal;
            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 2].PutValue(sales.Id);
            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 2].Style.Copy(cellstyle);
            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 3].PutValue(GlobalBusiness.getUserRoleType(sales.ROLEID));
            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 3].Style.Copy(cellstyle);

            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 4].PutValue(tmpTotal);
            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 4].Style.Copy(this.cellstyle);
        }

        private void writeAccountReport(TB_User sales, int year, int month, int line, decimal totalScore)
        {
            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 2].PutValue(sales.USER_NAME);
            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 2].Style.Copy(cellstyle);
            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 3].PutValue(GlobalBusiness.getUserRoleType(sales.ROLEID));
            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 3].Style.Copy(cellstyle);

            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 4].PutValue(Math.Round(totalScore,2));
            AccountSheet.Cells[ACCOUNTDATA_STARTLINE_INDEX + line, 4].Style.Copy(this.cellstyle);
        }
    }
}
