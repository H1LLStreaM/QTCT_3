using System;
using System.Collections.Generic;
using System.Text;
using Aspose.Cells;
using Microsoft.Reporting.WinForms;
using System.IO;
using System.Data;
using WY.Library.Business;
using WY.Common.Message;
using WY.Common.Utility;
using System.Data.Common;
using WY.Common.Data;

namespace WY.Library.ReportBusiness
{
    public class CompanyTrendcyBusiness
    {
        public static DataTable getCustomerTrendcy(int startYear, int endYear, int startMonth, int endMonth)
        {
            try
            {
                using(DbHelper db = new DbHelper())
                {
                    string sql = "select sum(receivable) as amount,year,month " +
                                " from SaleBills " +
                                " where (year>@syear or (year=@syear and month>=@smonth)) and (year<@eyear or (year=@eyear and month<=@emonth))" +
                                " and isDeleted=@del group by year,month " +
                                " order by year,month ";
                    DbParameter[] paramlist = { db.CreateParameter("@syear", startYear), db.CreateParameter("@smonth", startMonth),db.CreateParameter("@del",(int)EnmIsdeleted.ʹ����),
                                                   db.CreateParameter("@eyear",endYear),db.CreateParameter("@emonth",endMonth)};
                    DataTable tb = createCol();
                    DataTable tmpTb = db.GetDataSet(sql, paramlist).Tables[0];
                    for (int i = 0; i < tmpTb.Rows.Count; i++)
                    {
                        DataRow row = tb.NewRow();
                        row["yearmonth"] = tmpTb.Rows[i]["year"].ToString() + "��" + tmpTb.Rows[i]["month"].ToString() + "��";
                        row["amount"] = Utils.NvDecimal(tmpTb.Rows[i]["amount"].ToString());
                        tb.Rows.Add(row);
                    }
                    return tb;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "��ҵҵ������ͼ����ʧ�ܡ�");
                return new DataTable();
            }
            
        }

        private static DataTable createCol()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("yearmonth", typeof(string));
            tb.Columns.Add("amount", typeof(decimal));
            return tb;
        }
    }
}
