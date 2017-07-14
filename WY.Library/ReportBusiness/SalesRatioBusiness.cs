using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WY.Common.Data;
using System.Data.Common;
using WY.Common.Framework;
using WY.Common.Message;

namespace WY.Library.ReportBusiness
{
    public class SalesRatioBusiness
    {
        public static DataTable getSalesRatio(int startYear, int startMonth, int endYear, int endMonth)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "select sum(receivable) as salespercentage, name as salesname " +
                                            " from salebills as s left join tb_user as u on s.salerid=u.id " +
                                            " where (year>@syear or (year=@syear and month>=@smonth)) " +
                                            " and (year<@eyear or (year=@eyear and month<=@emonth))" +
                                            " and u.isDeleted=@del  and s.isDeleted=@del group by name";

                    DbParameter[] paramlist = { db.CreateParameter("@syear", startYear), db.CreateParameter("@smonth", startMonth),
                                            db.CreateParameter("@del",(int)EnmIsdeleted.使用中),
                                            db.CreateParameter("@eyear",endYear),db.CreateParameter("@emonth",endMonth)};

                    return db.GetDataSet(sql, paramlist).Tables[0];
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowMessage("E999", "业绩百分比报表导出失败。");
                    return new DataTable();
                }
            }
        }
    }
}
