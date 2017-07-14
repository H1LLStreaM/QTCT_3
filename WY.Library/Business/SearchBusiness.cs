using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WY.Common.Data;
using System.Data.Common;
using WY.Common.Utility;
using WY.Common.Message;

namespace WY.Library.Business
{
    public class SearchBusiness
    {
        #region 模糊查询销售渠道
        public static DataTable getSalersByCondition(string name, string number)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string condition = "";
                    if (!string.IsNullOrEmpty(name))
                    {
                        name = "%" + name + "%";
                        condition += " and Name like '%" + name + "%'";
                    }
                    if (!string.IsNullOrEmpty(number))
                    {
                        number = "%" + number + "%";
                        condition += " and logname like '%"+number+"%'";
                    }
                    condition += " order by logname";
                    string sql = "select id, Name as 姓名,logname as 工号 from tb_user where Isdeleted=@isDeleted and role=@role" + condition;
                    DbParameter[] paramlist = { db.CreateParameter("@isDeleted", (int)EnmIsdeleted.使用中), db.CreateParameter("@role", (int)EnmUserRole.销售渠道)};
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //销售渠道数据
                    if (tb.Rows.Count == 0)
                    {
                        MessageHelper.ShowMessage("I005");
                    }
                    return tb;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return new DataTable();
            }

        }

        public static DataTable getAllUsersByCondition(string name, string number)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string condition = "";
                    if (!string.IsNullOrEmpty(name))
                    {
                        name = "%" + name + "%";
                        condition += " and Name like '%" + name + "%'";
                    }
                    if (!string.IsNullOrEmpty(number))
                    {
                        number = "%" + number + "%";
                        condition += " and logname like '%" + number + "%'";
                    }
                    condition += " Order by logname";    
                    string sql = "select id, Name as 姓名,logname as 工号 from tb_user where  Isdeleted=@isDeleted and role=@role or role=@role2" + condition;
                    DbParameter[] paramlist = { db.CreateParameter("@isDeleted", (int)EnmIsdeleted.使用中), db.CreateParameter("@role", (int)EnmUserRole.销售渠道),db.CreateParameter("@role2",(int)EnmUserRole.录入人员) };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //销售渠道数据
                    if (tb.Rows.Count == 0)
                    {
                        MessageHelper.ShowMessage("I005");
                    }
                    return tb;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return new DataTable();
            }

        }
        #endregion
    }
}
