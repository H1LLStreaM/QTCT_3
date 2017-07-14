using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Aspose.Cells;
using System.Windows.Forms;
using WY.Common.Message;

namespace WY.Library.ReportBusiness
{
    public class AlertReportBusiness
    {
        private const int LOSTDATA_STARTLINE_INDEX = 2;
        private Workbook book;
        private Worksheet LostSheet;
        //private Style cellstyle;

        public AlertReportBusiness()
        {
            book = new Workbook();
            book.Open(Application.StartupPath + "/templet/lostcustomer_report_templet.xls");
            LostSheet = book.Worksheets[0];
        }

        public void exportReport(DataGridView view,string outpath)
        {
            try
            {
                for (int i = 0; i < view.Rows.Count; i++)
                {
                    LostSheet.Cells[LOSTDATA_STARTLINE_INDEX + i, 0].PutValue(view.Rows[i].Cells["customer"].Value.ToString());
                    LostSheet.Cells[LOSTDATA_STARTLINE_INDEX + i, 1].PutValue(view.Rows[i].Cells["cablenumber"].Value.ToString());
                    LostSheet.Cells[LOSTDATA_STARTLINE_INDEX + i, 2].PutValue(view.Rows[i].Cells["limitDate"].Value.ToString());
                }
                book.Save(outpath);
                MessageHelper.ShowMessage("I007");
            }
            catch
            {
                MessageHelper.ShowMessage("E999", "到期提醒报表导出失败。");
            }
        }
    }
}
