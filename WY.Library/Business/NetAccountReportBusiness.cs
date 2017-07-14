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
namespace WY.Library.Business
{ 
    /// <summary>
    /// 智能网财务清单
    /// </summary>
    public class NetAccountReportBusiness
    {
        private const int ACCOUNTDATA_STARTLINE_INDEX = 4;
        private Workbook book;
        private Worksheet AccountSheet;
        private Style cellstyle;
        decimal Total = 0;

        public NetAccountReportBusiness()
        {
            book = new Workbook();
            book.Open(Application.StartupPath + "/templet/NETAccount_report_templet.xls");
            AccountSheet = book.Worksheets[0];
            cellstyle = AccountSheet.Cells[1, 0].Style;
        }

        public void saveAccountReport(string outfile, int year, int month, List<netMoney> list)
        {
            int lines = 0;
            //TB_User[] sales = UserBusiness.getAllSalersAndWrite();
            if (true) //(sales != null)
            {   
                AccountSheet.Cells[2, 4].PutValue(year.ToString() + "年" + month + "月度");
                string str = year.ToString() + "-" + month.ToString() + "-01";
                DateTime startdate = DateTime.Parse(str);
                DateTime enddate = DateTime.Parse(str).AddMonths(1).AddDays(-1);
                for (int i = 0; i < list.Count; i++)
                {                    
                    //出账	长途出账金额	长途佣金金额	本地出账金额	本地佣金金额	佣金总额
                    decimal CZ = 0;
                    decimal CT = 0;
                    decimal CTYJ = 0;
                    decimal BD = 0;
                    decimal BDYJ = 0;
                    decimal TOTAL = 0;
                    AccountMoney(list[i],ref CZ, ref CT, ref CTYJ, ref BD, ref BDYJ, ref TOTAL);
                    writeAccountReport(list[i], CZ, CT, CTYJ, BD, BDYJ, list[i].money, lines);
                    lines = lines + 1;
                }
                try
                {
                    AccountSheet.Cells[1, 0].Style = AccountSheet.Cells[1, 1].Style;
                    AccountSheet.Cells[4 + lines + 2, 8].PutValue("合计:");
                    //lines += 2;
                    AccountSheet.Cells[4 + lines + 2, 9].R1C1Formula = "=SUM(R[-" + lines + "]C:R[-1]C)";
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

        private void writeAccountReport(netMoney m,decimal CZ,decimal CT,decimal CTYJ,decimal BD,decimal BDYJ,decimal TOTAL,int line)
        {
            AccountSheet.Cells[4 + line, 2].PutValue(line + 1);  //序号
            AccountSheet.Cells[4 + line, 2].Style.Copy(this.cellstyle);

            AccountSheet.Cells[4 + line, 3].PutValue(m.Logname); //渠道
            AccountSheet.Cells[4 + line, 3].Style.Copy(this.cellstyle);

            AccountSheet.Cells[4 + line, 4].PutValue(CZ); //结算日期
            AccountSheet.Cells[4 + line, 4].Style.Copy(this.cellstyle);

            AccountSheet.Cells[4 + line, 5].PutValue(CT);  //长途出账金额
            AccountSheet.Cells[4 + line, 5].Style.Copy(this.cellstyle);

            AccountSheet.Cells[4 + line, 6].PutValue(CTYJ); //长途佣金
            AccountSheet.Cells[4 + line, 6].Style.Copy(this.cellstyle);

            AccountSheet.Cells[4 + line, 7].PutValue(BD);  //本地出账金额
            AccountSheet.Cells[4 + line, 7].Style.Copy(this.cellstyle);

            AccountSheet.Cells[4 + line, 8].PutValue(BDYJ);  //本地佣金
            AccountSheet.Cells[4 + line, 8].Style.Copy(this.cellstyle);

            AccountSheet.Cells[4 + line, 9].PutValue(TOTAL);  //佣金总额
            AccountSheet.Cells[4 + line, 9].Style.Copy(this.cellstyle);
            
        }

        private void AccountMoney(netMoney m,ref decimal CZ, ref decimal CT, ref decimal CTYJ, ref decimal BD, ref decimal BDYJ, ref decimal TOTAL)
        {
            for (int i = 0; i < m.list1.Count; i++)
            {
                CZ += Utils.NvDecimal(m.list1[i].chuzhang);
                CT += Utils.NvDecimal(m.list1[i].kaihuqi2006);
                CTYJ += Utils.NvDecimal(m.list1[i].CtMoney);
                BD += Utils.NvDecimal(m.list1[i].kaihuqian2006);
                BDYJ += Utils.NvDecimal(m.list1[i].DbMoney);
                TOTAL += Utils.NvDecimal(m.list1[i].Pay);
            }
        }
    }
}
