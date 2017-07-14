using NHibernate.Expression;
using System.Collections.Generic;
using WY.Common;
using WY.Library.Dao;
using WY.Library.Model;
using FastReport;
using cis.pub;
using cis.pub.Src.Report;
using WY.Common.Message;

namespace WY.Library.ReportBusiness
{
    public class ExpensePrint
    {
        private List<TB_EXPENSE> mList = null;
        public ExpensePrint(List<TB_EXPENSE> _ls,string start,string end,string foot)
        {
            mList = _ls;
            int projectId = mList[0].OBJECTID;
            titletable titletable = new titletable();
            if (projectId > 0)  //项目报销
            {
                TB_PROJECT proj = TB_PROJECTDAO.FindFirst(new EqExpression("Id", projectId));
                titletable.title = proj.OBJECTNAME + "报销单";
                titletable.name = Global.g_username;
                
            }
            else  //个人报销
            {
                titletable.title = "个人报销单";
                titletable.name = Global.g_username;
            }
            titletable.dept = Global.g_dept;
            titletable.date = start + "年" + end + "月";
            int count=0;
            decimal totalmoney=0;
            for (int i = 0; i < mList.Count; i++)
            {
                totalmoney += mList[i].MONEY;
                count++;
            }
            string[] arr = foot.Split('；');

            
            List<foot1> fs1 = new List<foot1>();
            List<foot2> fs2 = new List<foot2>();
            List<foot3> fs3 = new List<foot3>();
            if (arr.Length > 0)
            {
                int idx = 1;
                for (int i = 0; i < arr.Length; i++)
                {                    
                    if (idx == 1)
                    {
                        foot1 f1 = new foot1();
                        f1.strfoot1 = arr[i];
                        fs1.Add(f1);
                        idx++;
                    }
                    else if (idx == 2)
                    {
                        foot2 f2 = new foot2();
                        f2.strfoot2 = arr[i];
                        fs2.Add(f2);
                        idx++;
                    }
                    else if (idx == 3)
                    {
                        idx = 1;
                        foot3 f3 = new foot3();
                        f3.strfoot3 = arr[i];
                        fs3.Add(f3);
                    }
                }
            }
            List<foots> fs = new List<foots>();
            for (int i = 0; i < fs1.Count; i++)
            {
                foots f = new foots();
                f.strfoot1 = fs1[i].strfoot1;

                if (i < fs2.Count)
                    f.strfoot2 = fs2[i].strfoot2;
                else
                    f.strfoot2 = "";
                if (i < fs3.Count)
                    f.strfoot3 = fs3[i].strfoot3;
                else
                    f.strfoot3 = "";

                fs.Add(f);
            }

            titletable.summary = "合计发票共:" + count.ToString() + "张   合计金额:" + totalmoney.ToString();
            List<titletable> title = new List<ReportBusiness.titletable>();
            title.Add(titletable);
            for (int i = 0; i < mList.Count; i++)
            {
                mList[i].Index = i + 1;
            }
            reportPrint(title, mList, fs);
        }

        private void reportPrint(List<titletable> title, List<TB_EXPENSE> detail,List<foots> fs)
        {
            try
            {
                ReportTools myReportTool;
                TfrxReportClass report;
                myReportTool = new ReportTools();
                report = new TfrxReportClass();
                report.ClearDatasets();
                FrxDataTable titleable = new FrxDataTable("dtWorkTitle");  //概要
                myReportTool.ListToFrxTable(title, titleable);
                FrxDataTable detailtable = new FrxDataTable("dtWorkDetail");  //明细
                myReportTool.ListToFrxTable(detail, detailtable);
                FrxDataTable detailtable2 = new FrxDataTable("dtWorkDetail2");  //明细2
                myReportTool.ListToFrxTable(fs, detailtable2);
                report.LoadReportFromFile("ExpenseReport.fr3");
                
                detailtable.AssignToReport(true, report);
                titleable.AssignToReport(true, report);
                report.ShowReport();
            }
            catch(System.Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }
    }

    public class titletable
    {
        public string title { get; set; }
        public string date { get; set; }
        public string name { get; set; }
        public string summary { get; set; }
        public string dept { get; set; }
        public string foot { get; set; }
    }

    public class foots
    {        
        public string strfoot1 { get; set; }
        public string strfoot2 { get; set; }
        public string strfoot3 { get; set; }
    }

    public class foot1
    {
        public string strfoot1 { get; set; }}

    public class foot2
    {
        public string strfoot2 { get; set; }       
    }

    public class foot3
    {
        public string strfoot3 { get; set; }
    }
}
