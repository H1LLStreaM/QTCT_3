using System;
using System.Collections.Generic;
using System.Text;

using WY.Common.Utility;
using WY.Common.Message;
using NHibernate.Expression;
using WY.Common.Data;
using System.Data.Common;
using System.Data;
using WY.Common;


namespace WY.Library.Business
{
    public class BaseBusiness
    {
        public bool del(int year, int month, int salerId)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    db.TrnStart();
                    string sql = "delete from dt_salermoney where year=@year and month=@month and salerid=@salerId ";
                    //DateTime endtime = TableManager.DBServerTime().Date;
                    DbParameter[] parmar2 = { db.CreateParameter("@year", year), db.CreateParameter("@month", month), db.CreateParameter("@salerId", salerId) };
                    db.ExecuteNonQuery(sql, parmar2);
                    db.TrnCommit();
                    return true;
                }
                catch (Exception ex)
                {
                    db.TrnRollBack();
                    Log.Error(ex.Message);
                    MessageHelper.ShowMessage("E999", "±£´æ·¢Éú´íÎó£¡");
                    return false;
                }
            }
        }

        public string date()
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "select now()";
                    DataSet ds = db.GetDataSet(sql);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        return Utils.NvStr(ds.Tables[0].Rows[0][0]);
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public bool DelALLItem(string tablename, string where)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    db.TrnStart();
                    string sql = "Update " + tablename + "  SET STATUS=0  WHERE " + where + "";
                    int result = db.ExecuteNonQuery(sql);
                    db.TrnCommit();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowMessage(ex.Message);
                    return false;
                }
            }
        }

        public int getMaxGroupNo(int opId)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    db.TrnStart();
                    string sql = "SELECT MAX(GROUPNO) FROM TB_EXPENSE WHERE OPUID="+opId;
                    DataSet ds = db.GetDataSet(sql);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        return Utils.NvInt(ds.Tables[0].Rows[0][0]);
                    }
                    else { return -1; }
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowMessage(ex.Message);
                    return -1;
                }
            }
        }
    }
}
