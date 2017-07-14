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
using System.Drawing;

namespace WY.Library.ReportBusiness
{
    public class ExportErrorAccount
    {
        private int STARTLINE_INDEX = 1;
        private Workbook book;
        private Worksheet sheet;
        private Style cellstyle;
        decimal Total = 0;

        public ExportErrorAccount()
        {
            book = new Workbook();
            book.Open(Application.StartupPath + "/templet/error_report_templet.xls");
            sheet = book.Worksheets[0];
        }

        public void saveAccountReport(string outfile, DataGridView view)
        {
            try
            {
                for (int i = 0; i < view.Rows.Count; i++)
                {
                    if (view.Rows[i].DefaultCellStyle.BackColor == Color.Red)
                    {
                        int no = Utils.NvInt(view.Rows[i].Cells["No2"].Value) + Global.row_Startline;
                        string cablenumber = Utils.NvStr(view.Rows[i].Cells["cablenumber2"].Value);
                        string customer = Utils.NvStr(view.Rows[i].Cells["customer2"].Value);
                        string completeDate = Utils.NvStr(view.Rows[i].Cells["Completedata2"].Value);
                        string resone = Utils.NvStr(view.Rows[i].Cells["errinfo"].Value);
                        sheet.Cells[STARTLINE_INDEX, 0].PutValue(no);
                        sheet.Cells[STARTLINE_INDEX, 1].PutValue(cablenumber);
                        sheet.Cells[STARTLINE_INDEX, 2].PutValue(customer);
                        sheet.Cells[STARTLINE_INDEX, 3].PutValue(completeDate);
                        sheet.Cells[STARTLINE_INDEX, 4].PutValue(resone);
                        STARTLINE_INDEX++;
                    }
                }
                book.Save(outfile);
                MessageHelper.ShowMessage("I007");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "财务清单导出失败。");
            }
        }
    }
}
