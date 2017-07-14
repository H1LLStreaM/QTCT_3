using NHibernate.Expression;
using System.Collections.Generic;
using WY.Common;
using WY.Library.Dao;
using WY.Library.Model;
using FastReport;
using cis.pub;
using cis.pub.Src.Report;
using System;
using WY.Common.Message;

namespace WY.Library.ReportBusiness
{
    public class ProfilePrint
    {
        private List<projProfileClass> list = null;
        public void Print(projProfileClass ppc,TB_PROJECT proj)
        {
            try
            {
                list = new List<projProfileClass>();
                list.Add(ppc);
                List<TB_EXPENSE> detial = new List<TB_EXPENSE>();
                detial = ppc.expens;
                for (int i = 0; i < detial.Count; i++)
                    detial[i].Index = i + 1;
                reportPrint(list, detial);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void reportPrint(List<projProfileClass> summery, List<TB_EXPENSE> detail)
        {
            try
            {
                ReportTools myReportTool;
                TfrxReportClass report;
                myReportTool = new ReportTools();
                report = new TfrxReportClass();
                report.ClearDatasets();
                FrxDataTable summeryTable = new FrxDataTable("dtWorkTitle");  //概要
                myReportTool.ListToFrxTable(summery, summeryTable);
                FrxDataTable detailtable = new FrxDataTable("dtWorkDetail");  //明细
                myReportTool.ListToFrxTable(detail, detailtable);
                report.LoadReportFromFile("profileReport.fr3");
                summeryTable.AssignToReport(true, report);
                detailtable.AssignToReport(true, report);                
                report.ShowReport();
            }
            catch (System.Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }
    }
}
