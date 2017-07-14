using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WY.Common.Data;
using System.Data.Common;

namespace WY.Library.Business
{
    public static class UltraChartBusiness
    {
        public static DataTable getTopN(int top,DateTime startDate,DateTime endDate)
        {
            DataTable result= null;
            int startYear = startDate.Year;
            int startMonth = startDate.Month;
            int endYear = endDate.Year;
            int endMonth = endDate.Month;
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string strSql = @"Select  Sum(writeoff) as `value`,u.name as `key` From salebills as s
                                    Left join tb_user as u on u.id=s.salerid 
                                    Where s.isdeleted=@del 
                                    And (s.year>=@startYear or s.year<=@endYear) And (s.month>=@startMonth and s.month<=@endMonth) 
                                    Group by salerid,u.name
                                    Limit 0,@topn
                                    Union 
                                    Select sum(writeoff) as `value`,'其他渠道' as `key` From salebills Where salerid not in (
                                    Select v.id from(
                                    Select Sum(writeoff) as writeoff,u.id as id From salebills as s
                                    Left Join tb_user as u on u.id=s.salerid 
                                    Where s.isdeleted=@del 
                                    And (s.year>=@startYear or s.year<=@endYear) And (s.month>=@startMonth and s.month<=@endMonth) 
                                    Group By salerid,u.id 
                                    Order By writeoff desc
                                    Limit 0,@topn) as v)";
                    DbParameter[] param = { db.CreateParameter("topn", top),db.CreateParameter("del",(int)EnmIsdeleted.使用中),
                                            db.CreateParameter("startYear", startYear),db.CreateParameter("endYear", endYear),
                                            db.CreateParameter("startMonth", startMonth),db.CreateParameter("endMonth", endMonth) };
                    result = db.GetDataSet(strSql, param).Tables[0];                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static DataTable getSalerChart(DateTime startDate, DateTime endDate, int userId)
        {
            DataTable result = null;
            try
            {
                int startYear = startDate.Year;
                int startMonth = startDate.Month;
                int endYear = endDate.Year;
                int endMonth = endDate.Month;

                using (DbHelper db = new DbHelper())
                {
                    string strSql = @"Select v.date,sum(v.writeoff) From (
                                    Select sum(writeoff) as writeoff,concat(year,'-',month) as date,id
                                    From salebills 
                                    Where salerid=@userId and isdeleted=@del 
                                    And (year>=@year and month>=@month) And (year<=@year2 and month<=@month2)
                                    Group By year,month,id) v
                                    Group By v.date
                                    Order By date asc";
                    DbParameter[] param = { db.CreateParameter("year", startYear),db.CreateParameter("month",startMonth),
                                            db.CreateParameter("year2", endYear),db.CreateParameter("month2",endMonth),
                                            db.CreateParameter("del", (int)EnmIsdeleted.使用中),db.CreateParameter("userId", userId) };
                    result = db.GetDataSet(strSql, param).Tables[0];
                    

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static DataTable test()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("key", typeof(string));
            dt.Columns.Add("value", typeof(decimal));

            DataRow row = dt.NewRow();
            row[0] = "张三";
            row[1] = 68000.43;

            DataRow row2 = dt.NewRow();
            row2[0] = "李四test2";
            row2[1] = 9870984.55;

            DataRow row3 = dt.NewRow();
            row3[0] = "王wu";
            row3[1] = 918011.05;

            dt.Rows.Add(row);
            dt.Rows.Add(row2);
            dt.Rows.Add(row3);
            return dt;
        }
    }
}
