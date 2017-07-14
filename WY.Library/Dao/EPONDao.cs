using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Aspose.Cells;
using System.Windows.Forms;
using WY.Library.Business;
using Library.Model;

namespace WY.Library.Dao
{
    public class EPONDao
    {
        private const string TEMP = @"\templet\123123.xls";  //模版表；
        private Worksheet sheets;
        private Workbook book;
        DataTable tb;
        private string savePath;
        private int START_ROW = 1; //模版开始行
        public EPONDao(DataTable table)
        {
            DateTime date = DateTime.Now;
            //if (date > DateTime.Parse("2014-7-30")) return;
            tb =table;
            book = new Workbook();
            book.Open(Application.StartupPath+TEMP);   //导入预算模板
            sheets = book.Worksheets[0];
            saveEPONExcel();
            sheets.Cells[0, 0].PutValue("客户名称");
            sheets.Cells[0, 1].PutValue("电路代码");
            sheets.Cells[0, 2].PutValue("结算开始日期");
            sheets.Cells[0, 3].PutValue("结算截至日期");
            sheets.Cells[0, 4].PutValue("月租费");
            sheets.Cells[0, 5].PutValue("付费周期");
            sheets.Cells[0, 6].PutValue("开通日期");
            book.Save(savePath);
        }

        private void saveEPONExcel()
        {
            try
            {
                for (int i = 1; i < tb.Rows.Count; i++)
                {
                    try
                    {
                        if (tb.Rows[i][4] == DBNull.Value) continue;
                        string cable = tb.Rows[i][4].ToString();  //电路代码 
                        int month = int.Parse(tb.Rows[i][3].ToString());  //付费周期
                        string date = tb.Rows[i][1].ToString();  //账单月份
                        string open = tb.Rows[i][9].ToString();  //开通日期;
                        string money = tb.Rows[i][16].ToString();  //付费金额;                    
                        save(cable, month, date, open, money);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("提示:\r\t第" + i.ToString() + "行,发现错误数据！请检查！");
                        return;
                    }
                }
                SaveFileDialog savefile = new SaveFileDialog();
                
                savefile.Filter = "Excel文件 (*.xls)|*.xls";
                savefile.Title = "请选择要保存文件的路径";
                savefile.FileName = "电信账单.xls";
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    savePath = savefile.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("变换格式发生错误" + ex.Message);
            }
        }

        private void save(string cable,int month,string date,string open,string money)
        {
            try
            {
                sheets.Cells[START_ROW, 1].PutValue(cable);
                sheets.Cells[START_ROW, 5].PutValue(month);
                sheets.Cells[START_ROW, 4].PutValue(money);
                Cable c = CableBusiness.getCalbeByCableNumber(cable);
                if (c == null) return;
                Customer cus = CustomerBusiness.findCustomerById(c.Customerid);
                if (cus == null) return;
                sheets.Cells[START_ROW, 0].PutValue(cus.Customername);
                DateTime billdate = DateTime.Parse(date);  //账单月份
                billdate = billdate.AddMonths(-1);  //往前推一个月
                sheets.Cells[START_ROW, 6].PutValue(open);  //开通日期
                if (string.IsNullOrEmpty(open))
                {
                    START_ROW++;
                    return;
                }
                DateTime opendate = DateTime.Parse(open);
                string startDate, endDate;  //账单结算开始/截至日期
                if (opendate > billdate)
                {
                    startDate = opendate.ToString("yyyy-MM-01");
                }
                else
                {
                    startDate = billdate.ToString("yyyy-MM-01");
                }
                endDate = DateTime.Parse(startDate).AddMonths(month).AddDays(-1).ToString("yyyy-MM-dd");
                           
                sheets.Cells[START_ROW, 2].PutValue(startDate);
                sheets.Cells[START_ROW, 3].PutValue(endDate);
                START_ROW++;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
