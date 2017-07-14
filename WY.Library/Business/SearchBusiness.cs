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
        #region ģ����ѯ��������
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
                    string sql = "select id, Name as ����,logname as ���� from tb_user where Isdeleted=@isDeleted and role=@role" + condition;
                    DbParameter[] paramlist = { db.CreateParameter("@isDeleted", (int)EnmIsdeleted.ʹ����), db.CreateParameter("@role", (int)EnmUserRole.��������)};
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //������������
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
                    string sql = "select id, Name as ����,logname as ���� from tb_user where  Isdeleted=@isDeleted and role=@role or role=@role2" + condition;
                    DbParameter[] paramlist = { db.CreateParameter("@isDeleted", (int)EnmIsdeleted.ʹ����), db.CreateParameter("@role", (int)EnmUserRole.��������),db.CreateParameter("@role2",(int)EnmUserRole.¼����Ա) };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //������������
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
